namespace URLCompactifier.Models.SharedExtensions
{
    /// <summary>
    /// Shared extension class
    /// </summary>
    public static class SharedExtensions
    {
        /// <summary>
        /// Input Https to allow redirects easier
        /// </summary>
        /// <param name="value">string to check</param>
        public static string HttpInput(string value)
        {
            if (!value.Contains("https://"))
            {
                value = $"https://{value}";
            }

            return value;
        }
    }
}
