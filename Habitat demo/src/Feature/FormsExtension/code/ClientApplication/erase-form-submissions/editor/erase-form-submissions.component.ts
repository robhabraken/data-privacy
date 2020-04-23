import { Component, OnInit, Injector } from '@angular/core';
import {
               EditorBase
    } from '@sitecore/ma-core';
 
@Component({
    selector: 'erase-form-submissions',
            template: `
        <section class="content">
            <div class="form-group">
                <div class="row erase-form-submissions">
                    <label class="col-6 title">Erase form submissions</label>
                    <div class="col-6">
                                                                                
                   </div>
                </div>
            </div>
        </section>
    `,
    styles: ['']
})
 
export class EraseFormSubmissionsComponent extends EditorBase implements OnInit {

    constructor(private injector: Injector) {
        super();
    }
 
    ngOnInit(): void { }
 
    serialize(): any {
        return {    };
    }
}
 