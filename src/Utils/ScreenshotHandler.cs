using Opal.Model.AppConfiguration;
using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace Opal.src.Utils
{
    public class ScreenshotHandler : IDisposable
    {
        private Config _config = Config.Instance;
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private readonly string _screenshotsDirectory;
        private readonly Timer _timer;
        private readonly object _lock = new object();
        private bool _isDisposed = false;
        private NotifyIcon _notifyIcon;
        private readonly SynchronizationContext _syncContext;

        /// <summary>
        /// Initializes a new instance of the ScreenshotHandler class using the singleton Config instance.
        /// </summary>
        public ScreenshotHandler()
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            _screenshotsDirectory = Path.Combine(exePath, "Screenshots");

            if (!Directory.Exists(_screenshotsDirectory))
            {
                Directory.CreateDirectory(_screenshotsDirectory);
            }

            if (_config.NotificationsEnabled)
            {
                _notifyIcon = new NotifyIcon
                {
                    Visible = true,
                    Icon = Properties.Resources.screenshot1,
                    BalloonTipTitle = "Screenshot Taken"
                };
            }

            _syncContext = SynchronizationContext.Current;

            _timer = new Timer(10000); // every 10 seconds
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
            _timer.Start();
        }

        /// <summary>
        /// Captures the current screen and saves the screenshot to the Screenshots folder.
        /// </summary>
        public void Make()
        {
            lock (_lock)
            {
                try
                {
                    using (Bitmap screenshot = CaptureScreen())
                    {
                        string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
                        string filePath = Path.Combine(_screenshotsDirectory, fileName);

                        screenshot.Save(filePath, ImageFormat.Png);

                        if (_config.NotificationsEnabled)
                        {
                            ShowNotification(fileName, _screenshotsDirectory);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error capturing screenshot: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Event handler for the timer's Elapsed event. Invokes the Make method on a separate thread.
        /// </summary>
        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_config.ScreenshotsInterval > 0)
            {
                double newInterval = _config.ScreenshotsInterval * 60 * 1000;

                if (_timer.Interval != newInterval)
                {
                    _timer.Interval = newInterval;
                }

                ThreadPool.QueueUserWorkItem(state => Make());
            }
        }

        /// <summary>
        /// Captures the entire screen and returns it as a Bitmap.
        /// </summary>
        /// <returns>A Bitmap containing the screenshot.</returns>
        private Bitmap CaptureScreen()
        {
            Rectangle bounds = GetTotalScreenBounds();

            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
            }

            return bitmap;
        }

        /// <summary>
        /// Calculates the total bounds encompassing all connected screens.
        /// </summary>
        /// <returns>A Rectangle representing the total screen bounds.</returns>
        private Rectangle GetTotalScreenBounds()
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            foreach (var screen in Screen.AllScreens)
            {
                if (screen.Bounds.Left < minX)
                    minX = screen.Bounds.Left;
                if (screen.Bounds.Top < minY)
                    minY = screen.Bounds.Top;
                if (screen.Bounds.Right > maxX)
                    maxX = screen.Bounds.Right;
                if (screen.Bounds.Bottom > maxY)
                    maxY = screen.Bounds.Bottom;
            }

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Displays a notification indicating that a screenshot has been taken.
        /// </summary>
        /// <param name="fileName">The name of the screenshot file.</param>
        /// <param name="directory">The directory where the screenshot is saved.</param>
        private void ShowNotification(string fileName, string directory)
        {
            string message = $"Screenshot {fileName} was placed in {directory}.";

            if (_notifyIcon != null)
            {
                if (_syncContext != null)
                {
                    _syncContext.Post(_ =>
                    {
                        _notifyIcon.BalloonTipText = message;
                        _notifyIcon.ShowBalloonTip(3000);
                    }, null);
                }
                else
                {
                    _notifyIcon.BalloonTipText = message;
                    _notifyIcon.ShowBalloonTip(3000);
                }
            }
        }

        /// <summary>
        /// Disposes the ScreenshotHandler, stopping the timer and releasing resources.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                if (_timer != null)
                {
                    _timer.Stop();
                    _timer.Dispose();
                }

                if (_notifyIcon != null)
                {
                    _notifyIcon.Visible = false;
                    _notifyIcon.Dispose();
                }

                _isDisposed = true;
            }
        }
    }
}
