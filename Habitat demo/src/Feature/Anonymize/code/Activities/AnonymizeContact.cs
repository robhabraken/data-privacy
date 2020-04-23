using System;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.Xdb.MarketingAutomation.Core.Activity;
using Sitecore.Xdb.MarketingAutomation.Core.Processing.Plan;

namespace Sitecore.Weareyou.Feature.Anonymize.Activities
{
    public class AnonymizeContact : IActivity
    {
        public IActivityServices Services { get; set; }

        public ActivityResult Invoke(IContactProcessingContext context)
        {
            try
            {
                Services.Collection.ExecuteRightToBeForgotten(context.Contact);
                Services.Collection.Submit();

                return new SuccessMove();
            }
            catch (Exception ex)
            {
                // in case of error, report failure; note that the contact will not move past the current step of the automation plan
                return new Failure($"Error anonymizing contact: {ex.Message}");
            }
        }
    }
}