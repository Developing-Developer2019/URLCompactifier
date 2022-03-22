namespace URLCompactifier.Models
{
    public class LinkBO
    {
        /// <summary>
        /// ID for Secondary Link
        /// </summary>
        public int Link_ID { get; set; }

        /// <summary>
        /// Name for Secondary Link
        /// </summary>
        public string Link_Name { get; set; }

        /// <summary>
        /// Compacted link
        /// </summary>
        public string Link_Token { get; set; }

        /// <summary>
        /// Created date of link
        /// </summary>
        public DateTime Link_CreatedDateT { get; set; }

        /// <summary>
        /// Expiry date of link
        /// </summary>
        public DateTime Link_ExpiryDateT { get; set; }

        public string PrimarySiteLink = "compact.com/";

        /// <summary>
        /// Original Link
        /// </summary>
        public string OriginalLink { get; set; } = string.Empty;

        /// <summary>
        /// Compact link
        /// </summary>
        public string CompactifyLink { get; set; } = string.Empty;
    }
}
