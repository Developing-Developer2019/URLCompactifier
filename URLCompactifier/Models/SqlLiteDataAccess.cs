using Dapper;
using System.Data;
using System.Data.SQLite;

namespace URLCompactifier.Models
{
    public class SqlLiteDataAccess
    {
        static string connectionString = "Data Source=.\\DemoDB.db;Version=3;";

        /// <summary>
        /// Get link details based on input
        /// </summary>
        /// <param name="link">URL to pass through</param>
        /// <returns></returns>
        public static LinkBO GetLinkDetails(string token)
        {
            string SqlQuery = $@"
SELECT 
    Link_Name
,   Link_Token
,   Link_CreatedDateT
,   Link_ExpiryDateT
FROM Link 
WHERE Link_Token = '{token}'
";

            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var output = cnn.QueryFirst<LinkBO>(SqlQuery);

                if (output.Link_ExpiryDateT < DateTime.Now)
                {

                }

                return output;
            }
        }

        /// <summary>
        /// Check if link is duplicate
        /// </summary>
        /// <param name="url">URL to check</param>
        /// <returns></returns>
        public static bool IsLinkDuplicate(string url)
        {
            string SqlQuery = $@"
SELECT 1 
FROM Link 
WHERE Link_Name = '{url}'";

            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var linkList = cnn.Query<LinkBO>(SqlQuery);
                return linkList.Any();
            }
        }

        /// <summary>
        /// Upload link
        /// </summary>
        /// <param name="link">Link details</param>
        public static void UploadLinks(LinkBO link)
        {
            string sqlQuery = @$"
INSERT INTO Link 
(
    Link_Name
,   Link_Token
,   Link_CreatedDateT
,   Link_ExpiryDateT
) 
VALUES 
(
    '{link.Link_Name}'
,   '{link.Link_Token}'
,   {DateTime.Now}
,   {DateTime.Now.AddDays(7)}
)";
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                cnn.Execute(sqlQuery);
            }
        }

        /// <summary>
        /// Delete expired links
        /// </summary>
        public void DeleteLinks()
        {
            string sqlQuery = @$"
DELETE Link
WHERE Link_ExpiryDateT < {DateTime.Now}";

            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                cnn.Execute(sqlQuery);
            }
        }
    }
}
