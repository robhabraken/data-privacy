using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing.Actions;
using Sitecore.ExperienceForms.Mvc.Models.Fields;

namespace Sitecore.Weareyou.Feature.FormsExtension.Processing.Actions
{
    public class SaveDataWithContact : SaveData
    {
        public const string TrackerIdFieldName = "xDB.Tracker";

        public SaveDataWithContact(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }

        private IViewModel CreateTrackerIdTextField()
        {
            var contactIdField = new InputViewModel<string>
            {
                Value = Analytics.Tracker.Current.Contact.ContactId.ToString().ToUpper(),
                Name = TrackerIdFieldName,
                ItemId = Guid.NewGuid().ToString(),
                AllowSave = true,
                IsTrackingEnabled = false
            };
            return contactIdField;
        }

        protected override bool SavePostedData(Guid formId, Guid sessionId, IList<IViewModel> postedFields)
        {
            if(postedFields.Any())
            {
                var trackerIdField = CreateTrackerIdTextField();
                postedFields.Add(trackerIdField);
            }

            return base.SavePostedData(formId, sessionId, postedFields);
        }

    }
}