import $, {post} from 'jquery';
import 'bootstrap';

$('.delete-user-form').on('submit', function(event: JQuery.SubmitEvent<HTMLElement>) {
    const $form = $(this).closest('form');
    console.log($form)
    event.preventDefault();

    $('#deleteConfirmationModal').modal('show').on('click', '#confirmDeleteButton', function(e) {
        $form.trigger('submit');
    });
    $('#cancel').on('click', function(e) {
        e.preventDefault();
        $('#deleteConfirmationModal').modal('hide');
    })
})

