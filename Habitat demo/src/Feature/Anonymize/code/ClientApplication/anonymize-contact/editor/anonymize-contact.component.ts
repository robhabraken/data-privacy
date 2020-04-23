import { Component, OnInit, Injector } from '@angular/core';
import {
               EditorBase
    } from '@sitecore/ma-core';
 
@Component({
    selector: 'anonymize-contact',
            template: `
        <section class="content">
            <div class="form-group">
                <div class="row anonymize-contact">
                    <label class="col-6 title">Anonymize contact</label>
                    <div class="col-6">
                                                                                
                   </div>
                </div>
            </div>
        </section>
    `,
    styles: ['']
})
 
export class AnonymizeContactComponent extends EditorBase implements OnInit {

    constructor(private injector: Injector) {
        super();
    }
 
    ngOnInit(): void { }
 
    serialize(): any {
        return {    };
    }
}
 