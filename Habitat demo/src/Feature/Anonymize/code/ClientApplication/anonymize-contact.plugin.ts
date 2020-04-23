import { Plugin } from '@sitecore/ma-core';
import { AnonymizeContactActivity } from './anonymize-contact/anonymize-contact-activity';
import { AnonymizeContactModuleNgFactory } from '../codegen/ClientApplication/anonymize-contact/anonymize-contact-module.ngfactory';
import { AnonymizeContactComponent } from '../codegen/ClientApplication/anonymize-contact/editor/anonymize-contact.component';
 
// Use the @Plugin decorator to define all the activities the module contains
@Plugin({
    activityDefinitions: [
        {
            // The ID must match the ID of the activity type description definition item in Sitecore
            id: 'eedd833d-25cd-412a-91f9-c33bfe7d56d6',
            activity: AnonymizeContactActivity,
            editorComponenet: AnonymizeContactComponent,
            editorModuleFactory: AnonymizeContactModuleNgFactory
        }
    ]
})
export default class AnonymizeContactModule {}