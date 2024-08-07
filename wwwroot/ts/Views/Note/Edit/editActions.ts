$(document).ready(function () {

        $('#editActionSubmit').on('click', function() {
            const noteId = $('#Note_Id').val()
            const categoryId = <string | undefined>$('#categoryEditDropdown').find(':selected').val()
            // Event delegation to ensure child elements can still be selected
            let selectedTags: string[] = new Array;
            const isPublic = $('#isPublicCBEdit').is(':checked')
            // Add each selected checkbox and add it to the Array
            $('#tagActionCBList input:checked').each(function(){
                const id = $(this).attr('id');
                if (typeof(id) == "string") {
                    selectedTags.push(id.slice(13));
                }
            })

            if (noteId !== null && typeof noteId == "string") {
                updateNoteActions(noteId, categoryId, selectedTags, isPublic);
            }
        })

        $(document).on('click', '#tagLoadMoreBtn', () => {
            const currentTagCount: number = $('input[type="checkbox"][id^="tagACheckbox+"]').length;
            $.get('/Tag/LoadMoreTags/', { currentTagCount: currentTagCount }, (data) => {
                data.forEach(function (tag: {name: string, id: string}) {
                    addTagToActionCB(tag);
                })
            })
        });
        
        // Clear tag selection
        $(document).on('click', '#tagActionClearBtn', () => {
            $('input[type="checkbox"][id^="tagACheckbox+"]').prop("checked", false)
        })
})


function updateNoteActions(noteId: string, categoryId?: string, tagIds?: string[], isPublic?: boolean ) {
    $.ajax({
        url: "/Note/EditActions/",
        type: "post",
        data: {
            noteId: noteId,
            categoryId: categoryId,
            tagIds: tagIds,
            isPublic: isPublic
        },
        success: function(data) {
            // Display success alert box thing
            if (alertTrigger) {
                appendAlert(`Changes have been applied!`, "success")
            }

        },
        error: function(errorThrown) {
            if (alertTrigger) {
                appendAlert('Error, changes have not been saved', "danger")
            }

        }
        
    })
}

// Alert logic

const alertPlaceholder = document.getElementById('liveAlertPlaceholder')
const appendAlert = (message: string, type: string) => {
    const wrapper = document.createElement('div')
    wrapper.innerHTML = [
        `<div class="alert alert-${type} alert-dismissible" role="alert">`,
        `   <div>${message}</div>`,
        '   <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>',
        '</div>'
    ].join('')
    
    if (alertPlaceholder && alertPlaceholder.innerHTML.length < 600) {
        alertPlaceholder.append(wrapper)
        setTimeout(function() {
            alertPlaceholder.innerHTML = ''
        }, 5000)
    }
    
}

const alertTrigger = document.getElementById('editActionSubmit')


function addTagToActionCB(tag: {name: string, id: string}) {

    const containerElem = $('#tagActionCBList');
    const divT = $('<div/>', { class: "form-check form-check-inline"}).appendTo(containerElem);
    const inputT = $('<input />', { class: "form-check-input", type: 'checkbox', name: "tagACheckbox" , id: `tagACheckbox+${tag.id}`, value: tag.id,});
    const labelT = $('<label />', { class: "form-check-label", 'for': `#tagActionCB+${tag.id}`, text: tag.name });

    divT.append(inputT, labelT);
    divT.appendTo(containerElem);
}

