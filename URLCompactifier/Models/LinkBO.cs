namespace URLCompactifier.Models
{
    public class LinkBO
    {
        /// <summary>
        /// ID for Secondary Link
        /// </summary>
        public int? Link_ID { get; set; }

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
        public string Link_CreatedDateT { get; set; }

        /// <summary>
        /// Expiry date of link
        /// </summary>
        public string? Link_ExpiryDateT { get; set; }

        /// <summary>
        /// Primary Site Link
        /// </summary>

        public string PrimarySiteLink = "www.compact.com/";

        /// <summary>
        /// True if link already exists
        /// </summary>
        public bool LinkExists { get; set; }
    }
}
