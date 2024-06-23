export function getAntiForgeryToken(parent: Element): string {
    if (!parent)
        throw new Error('Parent element is required');

   const antiForgeryInput = parent.querySelector(
      '[name="__RequestVerificationToken"]'
   ) as HTMLInputElement;

    const antiForgeryToken = antiForgeryInput ? antiForgeryInput.value : null;

    if (!antiForgeryToken) {
      console.error('RequestVerificationToken is missing from the form');
      //throw new Error('RequestVerificationToken is missing from the form');
      return '';
   }

    return antiForgeryToken;
}
