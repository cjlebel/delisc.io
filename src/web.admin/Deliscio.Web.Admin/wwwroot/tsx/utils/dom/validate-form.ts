export function validateForm(formElements: HTMLFormControlsCollection): { isSuccess: boolean; message: string } | null {
    if (!formElements) return { isSuccess: true, message: '' };
    
    const requiredElements = Array.from(formElements).filter(
        (element) => element.hasAttribute('required') && element.tagName.toLowerCase() !== 'button'
    ) as HTMLInputElement[];

    let hasErrors = false;
    let errorMsg = '';

    // No elements marked as required were in the form
    if (!requiredElements) return { isSuccess: true, message: '' };

    if (requiredElements) {
        requiredElements.forEach((element) => {
            if (!element.checkValidity()) {
                hasErrors = true;
                element.classList.add('is-invalid');
                errorMsg += `<li>${element.name} is required</li>`;
            } else {
                element.classList.remove('is-invalid');
            }
        });
    }

    if (hasErrors) {
        errorMsg = `<ul>${errorMsg}</ul>`;
    }

    return { isSuccess: !hasErrors, message: errorMsg };
}