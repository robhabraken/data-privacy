using Sitecore.Xdb.MarketingAutomation.Core.Activity;
using Sitecore.Xdb.MarketingAutomation.Core.Processing.Plan;
using System;
using System.Linq;
using Sitecore.Weareyou.Feature.FormsExtension.Processing.Actions;

namespace Sitecore.Weareyou.Feature.FormsExtension.Activities
{
    public class EraseFormSubmissions : IActivity
    {
        public IActivityServices Services { get; set; }

        private IDataManagementService DataManagementService { get; set; }

        public EraseFormSubmissions()
        {
            DataManagementService = new DataManagementService();
        }

        public ActivityResult Invoke(IContactProcessingContext context)
        {
            try
            {
                // get the tracker id to match up with the form entry bc xconnect and tracker id are not the same
                const string source = SaveDataWithContact.TrackerIdFieldName;
                var identifier = context.Contact.Identifiers.FirstOrDefault(i => i.Source.Equals(source))?.Identifier;
                
                if(identifier != null) { 
                    var trackerId = new Guid(identifier);
                    DataManagementService.DeleteFormEntries(trackerId);
                }
                
                return new SuccessMove();
            }
            catch (Exception ex)
            {
                // in case of error, report failure; note that the contact will not move past the current step of the automation plan
                return new Failure($"Error erasing form submissions: {ex.Message}");
            }
        }
    }
}