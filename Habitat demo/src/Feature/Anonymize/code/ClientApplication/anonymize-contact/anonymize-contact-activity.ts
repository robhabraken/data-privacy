import { SingleItem} from '@sitecore/ma-core';
 
export  class AnonymizeContactActivity extends SingleItem {
    getVisual(): string {
        const subTitle = 'Anonymize contact';
        const cssClass = this.isDefined ? '' : 'undefined';
        
        return `
            <div class="viewport-anonymize-contact marketing-action ${cssClass}">
                <span class="icon">
                    <img src="/~/icon/OfficeWhite/32x32/erase.png" />
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
 