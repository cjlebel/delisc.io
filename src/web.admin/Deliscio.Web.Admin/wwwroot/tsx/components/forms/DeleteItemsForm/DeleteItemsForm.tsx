class DeleteItemsForm {
   private form!: HTMLFormElement;

   constructor(formId: string) {
      if (!document.getElementById(formId)) {
         console.error('The Delete form was not found');
         throw new Error('The Delete form was not found');
      }

      this.form = document.getElementById(formId) as HTMLFormElement;
   }
}
