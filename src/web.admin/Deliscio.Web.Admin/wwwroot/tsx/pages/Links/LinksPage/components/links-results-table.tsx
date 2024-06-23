export class LinksResultsTable {
   table!: HTMLTableElement;

   constructor() {
      this.table =
         (document.querySelector('#links-table') as HTMLTableElement) ??
         (() => {
            throw new Error('Links results table was not found');
         });

      this.bindEvents();
   }

    //public toggleCheckboxes(): void => {

    //}

    //public get(): HTMLInputElement[] => {


    //this.selectLinkBoxes = initElements<HTMLInputElement>(this.linksTable, 'input[name="selected-link"]', { eventType: 'click', eventFunction: this.handleCheckBoxClick } as ElementEvent);
    //}

   private bindEvents(): void {
      //this.table.addEventListener('click', this.handleTableClick);
   }
}
