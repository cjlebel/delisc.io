export default function SearchLinks() {
    const frmSearch = document.querySelector('#frmSearch');

    const txtSearch = document.querySelector("#frmSearch #txt-term");
    const txtTags = document.querySelector("#frmSearch #txt-tags");
    const txtDomain = document.querySelector("#frmSearch #txt-domain");

    const chkIsActive = document.querySelector("#frmSearch #chk-is-active");
    const chkIsFlagged = document.querySelector("#frmSearch #chk-is-flagged");
    const chkIsDeleted = document.querySelector("#frmSearch #chk-is-deleted");

    const btnSearch = document.querySelector("#frmSearch #btn-search");
    const btnClear = document.querySelector("#frmSearch #btn-clear");

    if (chkIsActive) {
        chkIsActive.checked = chkIsActive.value === "true";
        //chkIsActive.value = chkIsActive.checked ? "true" : "false";

        chkIsActive.addEventListener("change", (e) => onChkChanged(e), false);
    }

    if (chkIsFlagged) {
        chkIsFlagged.value = chkIsFlagged.checked ? "true" : "false";

        chkIsFlagged.addEventListener("change", (e) => onChkChanged(e), false);
    }

    if (chkIsDeleted) {
        chkIsDeleted.value = chkIsDeleted.checked ? "true" : "false";

        chkIsDeleted.addEventListener("change", (e) => onChkChanged(e), false);
    }

    if (btnSearch) {
        btnSearch.addEventListener("click", (e) => onBtnSearchClick(e), false);
    };

    const onChkChanged = (e) => {
        if (e) {
            e.target.value = e.checked ? "true" : "false";
        }
    }

    const onBtnSearchClick = (e) => {
        e.preventDefault();
        e.stopPropagation();

        let url = '/links?term=' + txtSearch.value + '&tags=' + txtTags.value + '&domain=' + txtDomain.value + '&isActive=' + chkIsActive.value + '&isFlagged=' + chkIsFlagged.value + '&isDeleted=' + chkIsDeleted.value;

        window.location.href = url;
    };

    //btnSearch.onclick = function (e) {
    //    e.
    //    var url = "/Links?term=" + txtSearch.value + "&tags=" + txtTags.value + "&domain=" + txtDomain.value + "&isActive=" + chkIsActive.checked + "&isFlagged=" + chkIsFlagged.checked + "&isDeleted=" + chkIsDeleted.checked;
    //    window.location.href = url;
    //};
}

