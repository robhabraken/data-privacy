using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Sitecore.Framework.Conditions;
using Sitecore.Weareyou.Feature.FormsExtension.Activities;
using Sitecore.Weareyou.Feature.FormsExtension.Processing.Actions;
using Sitecore.XConnect;
using Sitecore.XConnect.Operations;
using Sitecore.XConnect.Service.Plugins;

namespace Sitecore.Weareyou.Feature.FormsExtension.XConnectServicePlugin
{
    /// <summary>
    /// Implementation was inspired by Sitecore.EmailCampaign.XConnect.Operations.ClearSupressionListWhenExecutingRightToBeForgotten
    /// XConnect service plugin that clears forms data on executing right to be forgotten
    /// Makes Sitecore Forms to be GDPR compliant
    /// e.g. Sitecore > xProfile > Any contact > Action > Anonymize
    ///
    /// To enable it you need to copy XML configuration to sc93xconnect.dev.local\App_Data\Config\Sitecore\Collection
    /// Configuration is present in this repository Project\xConnect\code\App_data\config\sitecore\Collection\sc.XConnect.Service.Plugins.FormsAnonymize.xml
    /// </summary>
    public class EraseFormSubmissionWhenExecutingRightToBeForgotten : IXConnectServicePlugin, IDisposable
    {
        private const string PluginName = "EraseFormSubmissionWhenExecutingRightToBeForgotten";

        private XdbContextConfiguration configuration;

        private readonly ILogger logger;

        private IDataManagementService DataManagementService { get; set; }

        public EraseFormSubmissionWhenExecutingRightToBeForgotten(ILogger logger)
        {
            DataManagementService = new DataManagementService();
            this.logger = logger;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Subscribes  to events
        /// </summary>
        /// <param name="config">XdbContextConfiguration</param>
        public void Register(XdbContextConfiguration config)
        {
            configuration = Condition.Requires(config, "config").IsNotNull().Value;

            configuration.OperationAdded += OnOperationAdded;
        }

        private void OnOperationAdded(object sender, XdbOperationEventArgs xdbOperationEventArgs)
        {
            Condition.Requires(xdbOperationEventArgs, "xdbOperationEventArgs").IsNotNull();

            RightToBeForgottenOperation rightToBeForgottenOperation;
            if ((rightToBeForgottenOperation = (xdbOperationEventArgs.Operation as RightToBeForgottenOperation)) == null)
            {
                return;
            }

            if (rightToBeForgottenOperation.Status == XdbOperationStatus.Canceled)
            {
                logger.LogDebug($"[{PluginName}] Skipping operation as it has been cancelled");
            }
            else
            {
                rightToBeForgottenOperation.DependencyAdded += OnDependencyAddedToRightToBeForgottenOperation;
            }
        }

        private void OnDependencyAddedToRightToBeForgottenOperation(object sender, DependencyAddedEventArgs args)
        {
            Condition.Requires(args, "args").IsNotNull();

            GetEntityOperation<Contact> getEntityOperation;
            if ((getEntityOperation = (args.Predecessor as GetEntityOperation<Contact>)) != null)
            {
                getEntityOperation.StatusChanged += OnGetEntityOperationStatusChanged;
            }
        }

        private void OnGetEntityOperationStatusChanged(object sender, StatusChangedEventArgs args)
        {
            Condition.Requires(sender, "sender").IsNotNull();
            Condition.Requires(args, "args").IsNotNull();

            GetEntityOperation<Contact> getEntityOperation;
            if ((getEntityOperation = (sender as GetEntityOperation<Contact>)) == null ||
                args.NewStatus != XdbOperationStatus.Succeeded)
            {
                return;
            }

            var contact = getEntityOperation.Entity;
            var contactId = contact.Id.GetValueOrDefault().ToString();
            logger.LogDebug($"[{PluginName}] Starting erasing {contactId} contact records from Forms database");

            // get the tracker id to match up with the form entry bc xconnect and tracker id are not the same
            const string source = SaveDataWithContact.TrackerIdFieldName;
            var identifier = contact.Identifiers.FirstOrDefault(i => i.Source.Equals(source))?.Identifier;
                
            if (identifier != null)
            {
                var trackerId = new Guid(identifier);
                DataManagementService.DeleteFormEntries(trackerId);
            }
                    
            logger.LogDebug($"[{PluginName}] Erasing {contactId} contact records from Forms database was finished");
        }

        public void Unregister()
        {
            configuration.OperationAdded -= OnOperationAdded;
        }
    }
}