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
    const storedCategory: string | null = localStorage.getItem('selectedCategory');
    
    if (storedView) {
        $('#viewSelector').val(storedView);
        loadView(storedView, storedCategory);
    }
    
    
    $('#viewSelector').change(function () {
        const selectedView: string = <string>$(this).val();
        localStorage.setItem('noteView', selectedView);
        loadView(selectedView, storedCategory);
    })
    // End NoteView Handlers
    
});

function loadView(view: string, category: string | null) {

    $.get('/Note/GetNotes?view=' + view + "&category=" + category, function(data) {
        $('#notesSection').html(data)
    })
}

