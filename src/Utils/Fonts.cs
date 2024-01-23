using System.Drawing;

namespace ProcessDashboard.src.Utils
{
    /// <summary>
    /// Contains Font objects with default fonts for application
    /// </summary>
    internal static class Fonts
    {
        /// <summary>
        /// Contains Font objects with Sennheiser Office font of different sizes.
        /// </summary>
        internal static class Sennheiser
        {
            /// <summary>
            /// Small (size 8)
            /// </summary>
            public static Font S { get; } = new Font("Sennheiser Office", 8);

            /// <summary>
            /// Small Medium (size 10)
            /// </summary>
            public static Font SM { get; } = new Font("Sennheiser Office", 10);

            /// <summary>
            /// Medium (size 12)
            /// </summary>
            public static Font M { get; } = new Font("Sennheiser Office", 12);

            /// <summary>
            /// Medium Large (size 14)
            /// </summary>
            public static Font ML { get; } = new Font("Sennheiser Office", 14);

            /// <summary>
            /// Large (size 16)
            /// </summary>
            public static Font L { get; } = new Font("Sennheiser Office", 16);
        }

    }
}
