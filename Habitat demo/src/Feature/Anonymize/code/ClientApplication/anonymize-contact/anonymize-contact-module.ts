import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AnonymizeContactComponent } from './editor/anonymize-contact.component';

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [AnonymizeContactComponent],
    entryComponents: [AnonymizeContactComponent]
})
export class AnonymizeContactModule { }   