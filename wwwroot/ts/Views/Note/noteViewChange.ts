import $ from 'jquery';

document.addEventListener('DOMContentLoaded', () => {
    
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
});

function loadView(view: string) {
    $.get('/Note/GetNotes?view=' + view, function(data) {
        $('#notesSection').html(data);
    })
}
