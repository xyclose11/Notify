$('.delete-user-form').off('submit').on('submit', function(event: JQuery.SubmitEvent<HTMLElement>) {
    const $form = $(this).closest('form');
    console.log($form)
    event.preventDefault();

    $('#deleteConfirmationModal').modal('show');
})

$('#confirmDeleteButton').off('click').on('click', function(e) {
    $('.delete-user-form').trigger('submit')
})

$('#cancel').off('click').on('click', function(e) {
    e.preventDefault();
    $('#deleteConfirmationModal').modal('hide');
})

