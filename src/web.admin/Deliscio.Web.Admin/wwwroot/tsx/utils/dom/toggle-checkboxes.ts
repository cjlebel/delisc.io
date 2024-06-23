/**
 * Toggles all checkboxes with the given selector.
 * @param checkBoxes The collection of checkboxes to toggle.
 * @returns All of the checkboxes are returned with their new state
 */
export function toggleCheckboxes(
   e: Event,
   checkBoxes: HTMLInputElement[]
): HTMLInputElement[] | null {
   e.preventDefault();
   e.stopPropagation();

   if (!checkBoxes) return null;

   // Look at the first one to determine if we should select or deselect all of them
   const doSelected = checkBoxes[0].checked;

   var toggledCheckboxes = checkBoxes.map((checkBox: HTMLInputElement) => {
      (checkBox as HTMLInputElement).checked = !doSelected;

      return checkBox;
   });

   return toggledCheckboxes;
}
