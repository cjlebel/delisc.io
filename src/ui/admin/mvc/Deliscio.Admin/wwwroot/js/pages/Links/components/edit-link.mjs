export default function EditLinks() {
    console.log('Edit Links Loaded');

    const divMessage = document.querySelector('#links-edit-container #message');
    const frmEdit = document.querySelector('#links-edit-container form');

    const txtTags = document.querySelector('#tags');

    if (frmEdit) {
        frmEdit.addEventListener('submit', (e) => onSubmitForm(e), false);
    }

    if (txtTags) {
        txtTags.addEventListener('blur', (e) => onTagsBlur(e), false);
    }

    const onSubmitForm = (e) => {
        e.preventDefault();
        e.stopPropagation();

        divMessage.classList.add('hide');
        divMessage.classList.remove('show');

        divMessage.classList.add('alert-success');
        divMessage.classList.remove('alert-danger');

        const validateResult = validateForm();

        if (!validateResult.isSuccess) {
            divMessage.innerHTML = validateResult.message;

            divMessage.classList.add('alert-danger');
            divMessage.classList.add('show');
        }
        else {
            frmEdit.submit();
        }
    }

    const onTagsBlur = (e) => {

    };

    const validateForm = () => {
        const requiredElements = Array.from(frmEdit.elements)
            .filter(element => element.tagName.toLowerCase() !== 'button' && element.hasAttribute('required'));

        let hasErrors = false;
        let errorMsg = '';

        if (!requiredElements)
            return hasErrors;

        if (requiredElements) {
            requiredElements.forEach(element => {
                if (!element.checkValidity()) {
                    hasErrors = true;
                    element.classList.add('is-invalid');
                    errorMsg += `<li>${element.name} is required</li>`;
                }
                else {
                    element.classList.remove('is-invalid');
                }
            });
        };

        if (hasErrors) {
            errorMsg = `<ul>${errorMsg}</ul>`;
        }

        return { isSuccess: !hasErrors, message: errorMsg };
    }

    const createLink = (title, description, tags) => {
        return {
            title: String(title),
            description: String(description),
            tags: tags.split(',').map(tag => tag.trim())
        };
    };
}