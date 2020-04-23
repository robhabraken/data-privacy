using System;
using System.Configuration;
using System.Data.SqlClient;
using Sitecore.Weareyou.Feature.FormsExtension.Processing.Actions;

namespace Sitecore.Weareyou.Feature.FormsExtension.Activities
{
    public interface IDataManagementService
    {
        void DeleteFormEntries(Guid contactId);
    }

    public class DataManagementService : IDataManagementService
    {

        private SqlConnection SqlConnection { get; set; }

        public DataManagementService()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["experienceforms"].ConnectionString;
            SqlConnection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Deletes all form entries made by a specific contact.
        /// Only works for entries saved via the custom SaveDataWithContact submit action.
        /// </summary>
        /// <remarks>
        /// It would be way nicer to extend the SqlFormDataProvider of the ExperienceForms namespace, but that would not
        /// only require adding the Sitecore.Kernel, Sitecore.ExperienceForms and Sitecore.ExperienceForms.Data.SqlServer
        /// assemblies to the xConnect deployment, but would also lack the context of the content web roles. To keep things
        /// mean and lean, I've decided to do a SQL query on the database directly via a SqlConnection object, which is,
        /// effectively, the same as what the SqlFormDataProvider does - also, the System.Data.SqlClient assembly is already
        /// present in the default xConnect deployment.
        /// </remarks>
        /// <param name="contactId">The GUID of the contact to delete the form entries for</param>
        public void DeleteFormEntries(Guid contactId)
        {
            using (SqlConnection)
            {
                SqlConnection.Open();

                const string sqlQuery =
                    "DELETE FROM [FormEntry] WHERE [ID] IN (SELECT [FormEntryID] FROM [FieldData] WHERE [FieldName]=@FieldName AND [Value]=@FieldValue)";
                var sqlCommand = new SqlCommand(sqlQuery, SqlConnection);
                sqlCommand.Parameters.AddWithValue("@FieldName", SaveDataWithContact.TrackerIdFieldName);
                sqlCommand.Parameters.AddWithValue("@FieldValue", contactId.ToString().ToUpper());
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}