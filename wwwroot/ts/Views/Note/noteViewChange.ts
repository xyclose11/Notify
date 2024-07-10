// $(document).ready(function() {
//    
//     // TableView Pagination Handlers
//     $('body').on('click', '.pagination-link', function(e) {
//         e.preventDefault();
//
//         const url: string | undefined = $(this).attr('href');
//
//         if (url) {
//             $.get(url, function (data) {
//                 $('#noteViewTableContainer').html(data);
//             })
//         }
//     })
//    
//    
//    
//     // End TableView Pagination Handlers
//    
//     // NoteView Handlers
//     const storedView: string | null = localStorage.getItem('noteView');
//     const storedCategory: string | null = localStorage.getItem('selectedCategory');
//     let storedTagIds = JSON.parse(localStorage.getItem('selectedTagIds') || '[]');
//    
//     // Repopulates view selector with users choice
//     if (storedView) {
//         $('#viewSelector').val(storedView);
//         loadView(storedView, storedCategory, storedTagIds);
//     }
//    
//     $('#viewSelector').change(function () {
//         const selectedView: string = <string>$(this).val();
//         localStorage.setItem('noteView', selectedView);
//         loadView(selectedView, storedCategory, storedTagIds);
//     })
//     // End NoteView Handlers
//    
// })
//
// function loadView(view: string, category: string | null, selectedTagIds: string[]) {
//     $.ajax({
//         url: '/Note/GetNotes/',
//         type: 'GET',
//         data: {
//             view: view,
//             category: category,
//             filterTags: selectedTagIds.join(','),
//         },
//         success: function (data) {
//             $('#notesSection').html(data);
//         }
//     })
//
// }