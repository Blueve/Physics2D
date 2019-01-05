namespace Physics2D
{
    using Physics2D.Common;

    /// <summary>
    /// The units convert class.
    /// This is a static helper class for convert units between real world unit and graphic units.
    /// </summary>
    public static class ConvertUnits
    {
        /// <summary>
        /// The radio of display units to physical units.
        /// </summary>
        private static double displayUnitsToSimUnitsRatio = 50;

        /// <summary>
        /// The radio of physical units to display units.
        /// </summary>
        private static double simUnitsToDisplayUnitsRatio = 1 / displayUnitsToSimUnitsRatio;

        /// <summary>
        /// Set the ratio.
        /// </summary>
        /// <param name="displayUnitsPerSimUnit">The ratio of display units to physical units.</param>
        public static void SetDisplayUnitToSimUnitRatio(double displayUnitsPerSimUnit)
        {
            displayUnitsToSimUnitsRatio = displayUnitsPerSimUnit;
            simUnitsToDisplayUnitsRatio = 1 / displayUnitsPerSimUnit;
        }

        /// <summary>
        /// Convert to display size.
        /// </summary>
        /// <param name="simUnits">The double type size by physical units.</param>
        /// <returns>The size by display units.</returns>
        public static int ToDisplayUnits(this double simUnits)
        {
            return (int)(simUnits * displayUnitsToSimUnitsRatio);
        }

        /// <summary>
        /// Convert to display size.
        /// </summary>
        /// <param name="simUnits">The int type size by physical units.</param>
        /// <returns>The size by display units.</returns>
        public static int ToDisplayUnits(this int simUnits)
        {
            return (int)(simUnits * displayUnitsToSimUnitsRatio);
        }

        /// <summary>
        /// Convert to display size.
        /// </summary>
        /// <param name="simUnits">The <see cref="Vector2D"/> type size by physical units.</param>
        /// <returns>The size by display units.</returns>
        public static Vector2D ToDisplayUnits(this Vector2D simUnits)
        {
            return simUnits * displayUnitsToSimUnitsRatio;
        }

        /// <summary>
        /// Convert to physical size.
        /// </summary>
        /// <param name="displayUnits">The double type size by display units.</param>
        /// <returns>The size by physical units.</returns>
        public static double ToSimUnits(this double displayUnits)
        {
            return displayUnits / displayUnitsToSimUnitsRatio;
        }

        /// <summary>
        /// Convert to physical size.
        /// </summary>
        /// <param name="displayUnits">The int type size by display units.</param>
        /// <returns>The size by physical units.</returns>
        public static double ToSimUnits(this int displayUnits)
        {
            return displayUnits / displayUnitsToSimUnitsRatio;
        }

        /// <summary>
        /// Convert to physical size.
        /// </summary>
        /// <param name="displayUnits">The <see cref="Vector2D"/> type size by display units.</param>
        /// <returns>The size by physical units.</returns>
        public static Vector2D ToSimUnits(this Vector2D displayUnits)
        {
            return displayUnits / displayUnitsToSimUnitsRatio;
        }
    }
}