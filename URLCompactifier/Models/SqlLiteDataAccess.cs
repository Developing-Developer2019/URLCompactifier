using Dapper;
using System.Data;
using System.Data.SQLite;

namespace URLCompactifier.Models
{
    public class SqlLiteDataAccess
    {
        static string connectionString = "Data Source=.\\DemoDB.db;Version=3;";

        /// <summary>
        /// Get Primary links based on input
        /// </summary>
        /// <param name="link">URL to pass through</param>
        /// <returns></returns>
        public static PrimaryLinkBO GetPrimaryLink(string link)
        {
            string SqlQuery = $@"SELECT * FROM PrimaryLink WHERE PrimaryLink_Name = '{link}'";


            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var output = cnn.QueryFirst<PrimaryLinkBO>(SqlQuery);

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
            string sqlQuery = $@"SELECT * FROM SecondaryLink WHERE SecondaryLink_Name = '{link}'";

            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var output = cnn.Query<PrimaryLinkBO>(sqlQuery);

                return (SecondaryLinkBO)output;
            }
        }

        /// <summary>
        /// Check if link is duplicate
        /// </summary>
        /// <param name="primaryLink">Primary link details</param>
        /// <param name="secondaryLink">Secondary link details</param>
        /// <returns></returns>
        public static bool IsLinkDuplicate(string primaryLink, string secondaryLink)
        {
            string primarySQL = $@"SELECT * FROM SecondaryLink WHERE SecondaryLink_Name = '{secondaryLink}'";
            string secondarySQL = $@"SELECT * FROM SecondaryLink WHERE SecondaryLink_Name = '{secondaryLink}'";

            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var outputPrimary = cnn.Query<PrimaryLinkBO>(primarySQL, new DynamicParameters());
                var outputSecondary = cnn.Query<PrimaryLinkBO>(secondarySQL, new DynamicParameters());
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
            string sqlQuery = $"INSERT INTO PrimaryLink (PrimaryLink_Name) VALUES ('{primaryLink.PrimaryLink_Name}')";
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                cnn.Execute(sqlQuery, primaryLink);
            }

            var primary = GetPrimaryLink(primaryLink.PrimaryLink_Name);
            UploadSecondStageLink(secondaryLink, primary.Id);
        }

        /// <summary>
        /// Upload secondary link
        /// </summary>
        /// <param name="secondaryLink">Secondary link details</param>
        /// <param name="primaryLink_Id">Primary link ID for linking</param>
        private static void UploadSecondStageLink(SecondaryLinkBO secondaryLink, int primaryLink_Id)
        {
            secondaryLink.PrimaryLink_ID = primaryLink_Id;
            string sqlQuery = $"INSERT INTO SecondaryLink (SecondaryLink_Name, SecondaryLink_Token, PrimaryLink_ID) VALUES ('{secondaryLink.SecondaryLink_Name}','{secondaryLink.SecondaryLink_Token}', {secondaryLink.PrimaryLink_ID})";

            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                cnn.Execute(sqlQuery, secondaryLink);
            }
        }

        /// <summary>
        /// Check if token exists
        /// </summary>
        /// <param name="token">input token</param>
        /// <returns>True if exists</returns>
        public static bool DoesTokenExist(string token)
        {
            var sqlQuery = $@"SELECT 1 FROM SecondaryLink WHERE SecondaryLink_Token = '{token}'";

            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                var output = connection.Query<PrimaryLinkBO>(sqlQuery);

                return output.Any();
            }
        }
    }
}
