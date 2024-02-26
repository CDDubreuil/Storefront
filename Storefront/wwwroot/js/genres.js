const successStyle = 'alert alert-success text-center';
const failStyle = 'alert alert-danger text-center';

//#region AJAX - STEP 08 - AJAX DELETE
///
//function deleteConfirmed(response) {
  
//    let rowId = '#genre-' + response.id;
//    console.log(rowId);
 
//    $('#genreTable').find(rowId).remove();

//    $('#messageContent').removeClass().addClass(successStyle).text(response.message);
//}
//function deleteFailed() {
//    $('#messageContent').removeClass().addClass(failStyle).text('Delete unsuccessful');
//}
////#endregion

//#region AJAX - STEP 15 - AJAX DETAILS
$('.detailsLink').on('click', function () {
    //grab the id from the button, because this is our category id
    let catId = $(this).attr('id');
    //load the partialviewresult into the details body.
    $('#detailsBody').load(`/Genres/Details/${catId}`);
});
//#endregion

////#region AJAX - STEP 21 - AJAX CREATE
////on page load, hide the containing div.
//$('#genreCreate').hide();
////on button click, show the div.
//$('#toggleGenreCreate').on('click', function () {
//    $('#genreCreate').toggle();
//});
////AJAX - STEP 22
//$('#createForm').on('submit', function (e) {
//    e.preventDefault();//prevents page reload. "e" is a browser event object.
//    let formData = $(this).serializeArray();

//    $.ajax({
//        url: '/Genres/Create',
//        type: 'POST',
//        data: formData,
//        dataType: 'json',
//        error: function () {
//            $('#messageContent').removeClass().addClass(failStyle).text('There was a problem...');
//        },
//        success: function (category) {
//            $('#messageContent').removeClass().addClass(successStyle).text('Category added!');
//            $('#createForm')[0].reset();
//            $(function () {
//                let row =
//                    `<tr id="category-${category.categoryId}">
//                        <td>${category.categoryName}</td>
//                        <td>${category.categoryDescription}</td>
//                        <td>Refresh Page for Options</td>
//                    </tr>`
//                $('#categoriesTable').append(row);
//                $('#categoryCreate').hide();
//            });
//        }
//    });
//});

////#endregion

////#region AJAX - STEP 27 - AJAX EDIT
////GET
//let originalContent = null;
//$('a.editLink').on('click', function (e) {
//    e.preventDefault();
//    let categoryId = $(this).attr('id');
//    let row = $(`#category-${categoryId}`).children();
//    originalContent = {
//        CatId: categoryId,
//        CatName: row[0].innerText,
//        CatDesc: row[1].innerText
//    };
//    //console.log(originalContent);
//    $.get(`/Categories/Edit/${categoryId}`, function (data) {
//        $(`#category-${categoryId}`).replaceWith(data)
//    })
//});
//// AJAX - STEP 29 (DONE!)
////POST
//$(document).on('click', '#saveUpdate', function (e) {
//    e.preventDefault();
//    let formData = $('#editForm').serializeArray();
//    //console.log(formData);
//    $('#messageContent').removeClass().addClass('alert alert-info text-center').text("Please wait...");

//    $.ajax({
//        url: '/Categories/EditPost',
//        type: 'POST',
//        data: formData,
//        dataType: 'json',
//        success: function (data) {
//            $('#messageContent').removeClass().addClass(successStyle).text('Your record was successfully updated!');
//            $('#editForm')[0].reset();
//            //console.log(data);
//            $(function () {
//                let row =
//                    `<tr id="category-${data.categoryId}">
//                        <td>${data.categoryName}</td>
//                        <td>${data.categoryDescription}</td>
//                        <td>Refresh Page for Options</td>
//                    </tr>`;
//                $(`#category-${data.categoryId}`).replaceWith(row);
//            })
//        },
//        error: function (data) {
//            console.log(data.responseText)
//            $('#messageContent').removeClass().addClass(failStyle).text('There was an error. Please try again or contact a site adminsitrator.');
//            $(function () {
//                let row =
//                    `<tr>
//                        <td>${originalContent.CatName}</td>
//                        <td>${originalContent.CatDesc}</td>
//                        <td>Refresh Page for Options</td>
//                    </tr>`;
//                $(`#category-${originalContent.CatId}`).replaceWith(row);
//            });
//        }
//    });
//});
////#endregion