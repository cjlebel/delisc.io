//import { getAntiForgeryToken } from '../../../../utils/dom/get-antiforgery-token';
///**
// * Responsible for deleting a link in the Links > Details page
// */
//export class DeleteLink {
//   formId = 'frmLinksDelete';

//   const frmEdit = document.querySelector('#links-edit-container form');
   
//   const btnDelete = document.querySelector('#links-edit-container #btnDelete');
//   const divMessage = document.querySelector('#links-edit-container #message');

//   if (!frmEdit) {
//      console.error('Unable to find the form element');
//      return;
//   }

//   if (!btnDelete) {
//      console.error('Unable to find the delete button element');
//      return;
//   }

//   if (!divMessage) {
//      console.error('Unable to find the message element');
//      return;
//   }

//   const linkId = frmEdit.id.valueOf();

//    const addDeleteButtonClick = () => {
//        btnDelete.addEventListener('click', (e: Event) => onDelete(e));
//    }

//   const onDelete = (e: Event) => {
//      e.preventDefault();
//      e.stopPropagation();

//      //   const antiForgeInput = document.getElementsByName(
//      //      '__RequestVerificationToken'
//      //   )[0] as HTMLElement;

//      //   const antiForgeToken = antiForgeInput ? antiForgeInput.value : null;

//       const antiForgeryToken = getAntiForgeryToken(frmEdit);

//      const headers = new Headers();
//      headers.append('Content-Type', 'application/json');
//       headers.append('RequestVerificationToken', antiForgeryToken);

//      divMessage.classList.add('hide');
//      divMessage.classList.remove('show');

//      divMessage.classList.add('alert-success');
//      divMessage.classList.remove('alert-danger');

//      fetch(`/links/${linkId}/delete`, {
//         headers: headers,
//         method: 'DELETE',
//      })
//         .then((response) => {
//            if (response.ok) {
//               return response.json();
//            } else {
//               throw new Error('Network response was not ok');
//            }
//         })
//         .then((data) => onDeleteSuccess(data))
//         .catch((error) => onDeleteFail(error));
//   };

//   const onDeleteFail = (error: any) => {
//      divMessage.innerHTML = error ?? `Unable to delete the link`;

//      divMessage.classList.remove('show');
//      divMessage.classList.remove('alert-success');

//      divMessage.classList.add('alert-danger');
//      divMessage.classList.add('show');
//   };

//   const onDeleteSuccess = (response: { isSuccess: boolean; message: string }) => {
//      const isSuccess = response.isSuccess ?? false;
//      const message =
//         response.message.trim() != '' ? response.message.trim() : 'Link successfully deleted';

//      if (isSuccess) {
//         divMessage.innerHTML = message;

//         divMessage.classList.remove('hide');
//         divMessage.classList.remove('alert-danger');

//         divMessage.classList.add('alert-success');
//         divMessage.classList.add('show');
//      } else {
//         divMessage.innerHTML = message ?? 'Unable to delete the link';

//         divMessage.classList.remove('show');
//         divMessage.classList.remove('alert-success');

//         divMessage.classList.add('alert-danger');
//         divMessage.classList.add('show');
//      }
//   };
//}
