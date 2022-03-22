using Dapper;
using System.Data;
using System.Data.SQLite;

namespace URLCompactifier.Models
{
    public class SqlLiteDataAccess
    {
        static ConfigurationManager? configurationManager;

        /// <summary>
        /// Get Primary links based on input
        /// </summary>
        /// <param name="link">URL to pass through</param>
        /// <returns></returns>
        public static PrimaryLinkBO GetPrimaryLink(string link)
        {
            var dynamicParameters = new DynamicParameters();

            dynamicParameters.AddDynamicParams(link);

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PrimaryLinkBO>($@"SELECT * FROM PrimaryLink WHERE PrimaryLink_Name = '{link}'", dynamicParameters);

                return (PrimaryLinkBO)output;
            }
        }

        /// <summary>
        /// Get secondary link based on input
        /// </summary>
        /// <param name="link">URL to pass through</param>
        /// <returns></returns>
        public static SecondaryLinkBO GetSecondaryLink(string link)
        {
            var dynamicParameters = new DynamicParameters();

            dynamicParameters.AddDynamicParams(link);

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PrimaryLinkBO>($@"SELECT * FROM SecondaryLink WHERE SecondaryLink_Name = '{link}'", dynamicParameters);

                return (SecondaryLinkBO)output;
            }
        }

        /// <summary>
        /// Check if link is duplicate
        /// </summary>
        /// <param name="primaryLink">Primary link details</param>
        /// <param name="secondaryLink">Secondary link details</param>
        /// <returns></returns>
        public bool IsLinkDuplicate(string primaryLink, string secondaryLink)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var outputPrimary = cnn.Query<PrimaryLinkBO>($@"SELECT * FROM SecondaryLink WHERE SecondaryLink_Name = '{secondaryLink}'", new DynamicParameters());
                var outputSecondary = cnn.Query<PrimaryLinkBO>($@"SELECT * FROM SecondaryLink WHERE SecondaryLink_Name = '{secondaryLink}'", new DynamicParameters());
                return outputPrimary.Any() && outputSecondary.Any();
            }
        }

        /// <summary>
        /// Upload links
        /// </summary>
        /// <param name="primaryLink">Primary link details</param>
        /// <param name="secondaryLink">Secondary link details</param>
        public static void UploadLinks(PrimaryLinkBO primaryLink, SecondaryLinkBO secondaryLink)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT INTO PrimaryLink (PrimaryLink_Name) VALUES (@PrimaryLink_Name)", primaryLink);
            }

            var primary = GetPrimaryLink(primaryLink.PrimaryLink_Name);
            UploadSecondStageLink(secondaryLink, primaryLink.Id);
        }

        /// <summary>
        /// Upload secondary link
        /// </summary>
        /// <param name="secondaryLink">Secondary link details</param>
        /// <param name="primaryLink_Id">Primary link ID for linking</param>
        private static void UploadSecondStageLink(SecondaryLinkBO secondaryLink, int primaryLink_Id)
        {
            secondaryLink.PrimaryLink_ID = primaryLink_Id;

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT INTO SecondaryLink (SecondaryLink_Name, PrimaryLink_ID) VALUES (@SecondaryLink_Name, @PrimaryLink_ID)", secondaryLink);
            }
        }

        /// <summary>
        /// Load connection string
        /// </summary>
        /// <returns></returns>
        private static string LoadConnectionString()
        {
            return configurationManager.GetConnectionString("Default");
        }
    }
}
