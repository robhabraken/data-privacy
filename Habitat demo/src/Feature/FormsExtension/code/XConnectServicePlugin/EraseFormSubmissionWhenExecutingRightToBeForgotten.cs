using System;
using Microsoft.Extensions.Logging;
using Sitecore.Framework.Conditions;
using Sitecore.Weareyou.Feature.FormsExtension.Activities;
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
        private XdbContextConfiguration _configuration;
        private readonly ILogger _logger;
        private IDataManagementService DataManagementService { get; set; }
        public EraseFormSubmissionWhenExecutingRightToBeForgotten(ILogger<EraseFormSubmissionWhenExecutingRightToBeForgotten> logger)
        {
            DataManagementService = new DataManagementService();
            _logger = logger;
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
            _configuration = Condition.Requires(config, "config").IsNotNull().Value;
            _configuration.OperationAdded += OnOperationAdded;
        }

        private void OnOperationAdded(object sender, XdbOperationEventArgs xdbOperationEventArgs)
        {
            Condition.Requires(xdbOperationEventArgs, "xdbOperationEventArgs").IsNotNull();
            RightToBeForgottenOperation rightToBeForgottenOperation;
            if ((rightToBeForgottenOperation = (xdbOperationEventArgs.Operation as RightToBeForgottenOperation)) != null)
            {
                if (rightToBeForgottenOperation.Status == XdbOperationStatus.Canceled)
                {
                    _logger.LogDebug($"[{PluginName}] Skipping operation as it has been cancelled");
                }
                else
                {
                    rightToBeForgottenOperation.DependencyAdded += OnDependencyAddedToRightToBeForgottenOperation;
                }
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
            if ((getEntityOperation = (sender as GetEntityOperation<Contact>)) != null && args.NewStatus == XdbOperationStatus.Succeeded)
            {
                var entity = getEntityOperation.Entity;
                var contactId = entity.Id;
                if (contactId.HasValue)
                {
                    _logger.LogDebug($"[{PluginName}] Starting erasing {contactId.Value} contact records from Forms database");
                    DataManagementService.DeleteFormEntries(contactId.Value);
                    _logger.LogDebug($"[{PluginName}] Erasing {contactId.Value} contact records from Forms database was finished");
                }
            }
        }

        public void Unregister()
        {
            _configuration.OperationAdded -= OnOperationAdded;
        }
    }
}