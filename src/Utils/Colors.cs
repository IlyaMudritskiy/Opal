using System.Drawing;

namespace ProcessDashboard.src.Utils
{
    internal static class Colors
    {
        public static Color White { get; } = Color.FromArgb(244, 245, 247);
        public static Color Red { get; } = Color.FromArgb(200, 34, 56);
        public static Color Orange { get; } = Color.FromArgb(224, 97, 54);
        public static Color Yellow { get; } = Color.FromArgb(219, 164, 81);
        public static Color Blue { get; } = Color.FromArgb(48, 75, 122);
        public static Color Green { get; } = Color.FromArgb(95, 169, 99);
        public static Color Black { get; } = Color.FromArgb(51, 51, 51);
        public static Color Purple { get; } = Color.FromArgb(144, 58, 173);
        public static Color Cyan { get; } = Color.FromArgb(42, 161, 152);
        public static Color Grey { get; } = Color.FromArgb(151, 151, 151);


        public static Color DS11C { get; } = Orange;
        public static Color DS12C { get; } = Cyan;
        public static Color DS21C { get; } = Blue;
        public static Color DS22C { get; } = Purple;

        internal static class Default
        {
            public static Color Grey { get; } = SystemColors.Control;
        }

        internal static class Light
        {
            public static Color Grey { get; } = Color.FromArgb(20, Black);
            public static Color Green { get; } = Color.FromArgb(20, Colors.Green);
            public static Color Red { get; } = Color.FromArgb(20, Colors.Red);
            public static Color Purple { get; } = Color.FromArgb(20, Colors.Purple);
        }
    }
}
