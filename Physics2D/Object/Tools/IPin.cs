namespace Physics2D.Object.Tools
{
    using Physics2D.Common;
    using Physics2D.Core;

    public interface IPin
    {
        /// <summary>
        /// Pin self to world.
        /// </summary>
        /// <param name="world">The <see cref="World"/>.</param>
        /// <param name="position">The <see cref="Vector2D"/></param>
        /// <returns>The pined point.</returns>
        Handle Pin(World world, Vector2D position);

        /// <summary>
        /// Unpin self from world.
        /// </summary>
        /// <param name="world">The <see cref="World"/>.</param>
        void Unpin(World world);
    }
}
