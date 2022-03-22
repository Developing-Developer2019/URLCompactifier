namespace URLCompactifier.Models
{
    public class SecondaryLinkBO
    {
        /// <summary>
        /// ID for Secondary Link
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name for Secondary Link
        /// </summary>
        public string SecondaryLink_Name { get; set; }

        /// <summary>
        /// ID for Primary Link to link
        /// </summary>
        public int PrimaryLink_ID { get; set; }
    }
}
