/**
 * Handles the ability to delete multiple links on the List Index page
 */
export default function DeleteLinks() {
    const frmDeleteLinks = document.querySelector('#links-list-container #frmLinks');
    const btnSelectAll = document.querySelector('#btnSelectAll');

    if (btnSelectAll) {
        btnSelectAll.addEventListener('click', (e) => onSelectAllClick(e), false);
    }

    if (frmDeleteLinks) {
        frmDeleteLinks.addEventListener('submit', (e) => onDeletesSubmitForm(e), false);
    }

    const onSelectAllClick = (e) => {
        const links = document.querySelectorAll('#links-table input[type="checkbox"]');

        links.forEach((link) => {
            link.checked = !link.checked;
        });
    }

    const onDeletesSubmitForm = (e) => {
        e.preventDefault();
        e.stopPropagation();

        const links = [...document.querySelectorAll('input[type="checkbox"]:checked')];

        const antiForgeInput = document.getElementsByName('__RequestVerificationToken')[0];
        const antiForgeToken = antiForgeInput ? antiForgeInput.value : null;

        const headers = new Headers();
        headers.append('Content-Type', 'application/json');
        headers.append('RequestVerificationToken', antiForgeToken);

        const linkIds = links.map((link) => link.dataset.id);

        if (confirm('Are you sure you want to delete the selected links?')) {
            //document.querySelectorAll returns a nodeslist, we need an array

            const body = JSON.stringify(linkIds);

            fetch(`/links/deletes`, {
                //fetch('/links/deletes', {
                headers: headers,
                method: 'DELETE',
                body: body
            })
                .then((response) => {
                    if (response.ok) {
                        return response.json();
                    } else {
                        throw new Error('Network response was not ok');
                    }
                })
                .then(data => onDeletesSuccess(data))
                .catch(error => onDeletesFailure(error));
        }
    };

    const onDeletesSuccess = (data) => {
        if (!data)
            throw new Error('Data was expected from the server, but none was returned');

        if (!data.isSuccess)
            console.error(data.message);

        const links = data.linkIds;
        links.forEach((link) => {
            // Get all of the <tr> elements that have a data-id attribute equal to the link id
            const trs = document.querySelectorAll(`tr[data-rowid="${link}"]`);

            if (trs)
                trs.forEach(tr => {
                    // Get all of the <td> elements within the <tr> and add the class 'text-decoration-line-through'
                    const tds = tr.querySelectorAll('td');

                    // Would prefer to remove the trs, but then have to deal with repopulating if not all were deleted.
                    if (tds)
                        tds.forEach(td => {
                            td.classList.add('bg-danger-subtle');
                            td.classList.add('text-decoration-line-through');
                        });
                });
        });
    };

    const onDeletesFailure = (error) => {
        console.error('Error:', error);
    };
}