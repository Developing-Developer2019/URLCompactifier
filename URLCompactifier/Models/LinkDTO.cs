namespace URLCompactifier.Models
{
    public class LinkDTO
    {
        /// <summary>
        /// Original Link
        /// </summary>
        public string OriginalLink { get; set; } = string.Empty;

        /// <summary>
        /// Compact link
        /// </summary>
        public string CompactifyLink { get; set; } = string.Empty;

        /// <summary>
        /// Primary link
        /// </summary>
        public PrimaryLinkBO PrimaryLink { get; set; }

        /// <summary>
        /// Secondary link
        /// </summary>
        public SecondaryLinkBO SecondaryLink { get; set; }
    }
}
