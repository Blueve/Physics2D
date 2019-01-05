namespace Physics2D
{
    public class Settings
    {
        /// <summary>
        /// Gets or sets the percision of double type.
        /// </summary>
        public static double Percision { get; set; } = 1e-8;

        /// <summary>
        /// Gets or sets the max contact number.
        /// </summary>
        public static int MaxContacts { get; set; } = 500;

        /// <summary>
        /// Gets or sets the iteration nuumber of contact resolver.
        /// </summary>
        public static int ContactIteration { get; set; } = 1;
    }
}