namespace WalkeDesigns.Utilities
{
    /// <summary>
    ///  Math utilities.
    /// </summary>
    public static class Math
    {
        /// <summary>
        ///  Clamp the given value to the boundary values.
        /// </summary>
        /// <param name="input">The input to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns></returns>
        public static int Clamp(int input, int min, int max)
        {
            return input < min ? min :
                   input > max ? max :
                   input;
        }
    }
}
