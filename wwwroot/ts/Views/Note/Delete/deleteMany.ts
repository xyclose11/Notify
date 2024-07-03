// GOAL: Allow user to select multiple checkboxes X -> onSubmit(capture list of noteIds) X -> AJAX -> noteController/Delete

document.addEventListener("DOMContentLoaded", function() {
    
    // Event delegation to ensure child elements can still be selected
    $('body').on('click','#noteDeleteSelected', function() {
        let selectedNotes: string[] = new Array;

        // Add each selected checkbox and add it to the Array
        $('#mainNoteTableBody input:checked').each(function(){
            const id = $(this).attr('id');
            if (typeof(id) == "string") {
                selectedNotes.push(id.slice(7));
            }
        })
        
        $('#submitDeleteManyNotesBtn').on('click', function() {
            AjaxDeleteMany(selectedNotes)
        })
    })
    
    // SELECT ALL NEEDS MODIFICATION
    // $('body').on('click','#noteDeleteSelected', function() {
    //     var t = $('input[type="checkbox"][id^="noteCB+"]').prop("checked", true);
    //     console.log(t)
    // })
})


function AjaxDeleteMany(noteIds: string[]) {
    $.ajax({
        url: "Note/DeleteManyConfirmation/",
        type: 'POST',
        data: {
            noteIds: noteIds
        },
        success: function() {
            $('#deleteManyNotesModal').show()
        },
        error: function(errorThrown) {
            console.log(errorThrown)
        }
    })
}