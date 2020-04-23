import { Plugin } from '@sitecore/ma-core';
import { EraseFormSubmissionsActivity } from './erase-form-submissions/erase-form-submissions-activity';
import { EraseFormSubmissionsModuleNgFactory } from '../codegen/ClientApplication/erase-form-submissions/erase-form-submissions-module.ngfactory';
import { EraseFormSubmissionsComponent } from '../codegen/ClientApplication/erase-form-submissions/editor/erase-form-submissions.component';
 
// Use the @Plugin decorator to define all the activities the module contains
@Plugin({
    activityDefinitions: [
        {
            // The ID must match the ID of the activity type description definition item in Sitecore
            id: '1cbacbf2-e240-4602-bea4-80df19787d7c', 
            activity: EraseFormSubmissionsActivity,
            editorComponenet: EraseFormSubmissionsComponent,
            editorModuleFactory: EraseFormSubmissionsModuleNgFactory
        }
    ]
})
export default class EraseFormSubmissionsModule {}