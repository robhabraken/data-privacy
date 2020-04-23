import { SingleItem} from '@sitecore/ma-core';
 
export  class EraseFormSubmissionsActivity extends SingleItem {
    getVisual(): string {
        const subTitle = 'Erase form submissions';
        const cssClass = this.isDefined ? '' : 'undefined';
        
        return `
            <div class="viewport-erase-form-submissions marketing-action ${cssClass}">
                <span class="icon">
                    <img src="/~/icon/OfficeWhite/32x32/text_field.png" />
              </span>
                <p class="text with-subtitle" title="${subTitle}">
                    ${subTitle}
                    <small class="subtitle" title="${subTitle}">${subTitle}</small>
                </p>
            </div>
        `;
    }
 
    get isDefined(): boolean {
            return true;
    }
}
 