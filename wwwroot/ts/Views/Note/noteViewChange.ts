import $ from 'jquery';

document.addEventListener('DOMContentLoaded', () => {
    $('#viewSelector').change(function () {
        const selectedView = $(this).val();
        $.get('/Note/GetNotes?view=' + selectedView, function(data) {
            $('#notesSection').html(data);
        })
    })
});
