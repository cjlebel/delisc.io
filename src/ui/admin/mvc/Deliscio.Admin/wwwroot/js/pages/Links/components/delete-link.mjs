export default function DeleteLink() {
    const frmEdit = document.querySelector('#links-edit-container form');
    const btnDelete = document.querySelector('#links-edit-container #btnDelete');
    const divMessage = document.querySelector('#links-edit-container #message');

    const linkId = frmEdit.id.value;

    if (btnDelete) {
        btnDelete.addEventListener('click', (e) => onDelete(e), false);
    }

    const onDelete = (e) => {
        e.preventDefault();
        e.stopPropagation();

        const antiForgeInput = document.getElementsByName('__RequestVerificationToken')[0];
        const antiForgeToken = antiForgeInput ? antiForgeInput.value : null;

        const headers = new Headers();
        headers.append('Content-Type', 'application/json');
        headers.append('RequestVerificationToken', antiForgeToken);

        divMessage.classList.add('hide');
        divMessage.classList.remove('show');

        divMessage.classList.add('alert-success');
        divMessage.classList.remove('alert-danger');

        fetch(`/links/${linkId}/delete`, {
            headers: headers,
            method: 'DELETE',
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    throw new Error('Network response was not ok');
                }
            })
            .then(data => onDeleteSuccess(data))
            .catch(error => onDeleteFail(error));
    };

    const onDeleteFail = (error) => {
        divMessage.innerHTML = error ?? `Unable to delete the link`;

        divMessage.classList.remove('show');
        divMessage.classList.remove('alert-success');

        divMessage.classList.add('alert-danger');
        divMessage.classList.add('show');
    };

    const onDeleteSuccess = (response) => {
        const isSuccess = response.isSuccess ?? false;
        const message = response.message.trim() != '' ? response.message.trim() : "Link successfully deleted";

        if (isSuccess) {
            divMessage.innerHTML = message;

            divMessage.classList.remove('hide');
            divMessage.classList.remove('alert-danger');

            divMessage.classList.add('alert-success');
            divMessage.classList.add('show');
        }
        else {
            divMessage.innerHTML = message ?? "Unable to delete the link";

            divMessage.classList.remove('show');
            divMessage.classList.remove('alert-success');

            divMessage.classList.add('alert-danger');
            divMessage.classList.add('show');
        };
    };
}