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
        public static LinkBO GetLinkDetailsWithTokenOrName(string input, bool isToken)
        {
            string searchInput = isToken ? "Link_Token" : "Link_Name";

            string SqlQuery = $@"
SELECT 
    Link_ID
,   Link_Name
,   Link_Token
,   Link_CreatedDateT
,   Link_ExpiryDateT
FROM Link 
WHERE {searchInput} = '{input}'
";

            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var output = cnn.QueryFirst<LinkBO>(SqlQuery);

                if (output == null)
                {
                    output = new LinkBO();
                }

                if (DateTime.Parse(output.Link_ExpiryDateT) < DateTime.Now)
                {
                    DeleteLinks();
                }

                return output;
            }
        }

        /// <summary>
        /// Check if link is duplicate
        /// </summary>
        /// <param name="link">Link to check</param>
        /// <returns>True if duplicate</returns>
        public static bool DoesLinkExist(string link)
        {
            string SqlQuery = $@"
SELECT 1
FROM Link 
WHERE Link_Name = '{link}'";

            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var linkList = cnn.Query<LinkBO>(SqlQuery);
                return linkList.Any();
            }
        }

        /// <summary>
        /// Check if token is duplicate
        /// </summary>
        /// <param name="token">Token to check</param>
        /// <returns>True if duplicate</returns>
        public static bool DoesTokenExist(string token)
        {
            string SqlQuery = $@"
SELECT 1
FROM Link 
WHERE Link_Token = '{token}'";

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
,   '{link.Link_CreatedDateT}'
,   '{link.Link_ExpiryDateT}'
)";
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                cnn.Execute(sqlQuery);
            }
        }

        /// <summary>
        /// Delete expired links
        /// </summary>
        public static void DeleteLinks()
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
