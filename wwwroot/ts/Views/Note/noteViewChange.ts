import $ from 'jquery';

document.addEventListener('DOMContentLoaded', () => {
    
    // TableView Pagination Handlers
    $('body').on('click', '.pagination-link', function(e) {
        e.preventDefault();

        const url: string | undefined = $(this).attr('href');

        if (url) {
            $.get(url, function (data) {
                $('#noteViewTableContainer').html(data);
            })
        }
    })
    
    
    
    // End TableView Pagination Handlers
    
    // NoteView Handlers
    const storedView: string | null = localStorage.getItem('noteView');
    
    if (storedView) {
        $('#viewSelector').val(storedView);
        loadView(storedView);
    }
    
    $('#viewSelector').change(function () {
        const selectedView: string = <string>$(this).val();
        localStorage.setItem('noteView', selectedView);
        loadView(selectedView);
    })
    // End NoteView Handlers
    
});

function loadView(view: string) {
    $.get('/Note/GetNotes?view=' + view, function(data) {
        $('#notesSection').html(data);
    })
}

