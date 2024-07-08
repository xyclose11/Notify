$(document).ready(function () {
        getCategories();

        $('#editActionSubmit').on('click', function() {
            const noteId = $('#Note_Id').val()
            const categoryId = <string | undefined>$('#categoryEditDropdown').find(':selected').val()
            // Event delegation to ensure child elements can still be selected
            let selectedTags: string[] = new Array;

            // Add each selected checkbox and add it to the Array
            $('#tagActionCBList input:checked').each(function(){
                const id = $(this).attr('id');
                if (typeof(id) == "string") {
                    selectedTags.push(id.slice(13));
                }
            })

            if (noteId !== null && typeof noteId == "string") {
                updateNoteActions(noteId, categoryId, selectedTags);
            }
        })
})


function getCategories() {
    $.ajax({
        url: "/Category/GetCategories/",
        type: 'get',
        success: function(data) {
            let dropdown: HTMLSelectElement = (document.getElementById('categoryEditDropdown') as HTMLSelectElement);
            if (dropdown) {
                // Clear any existing options
                dropdown.innerHTML = '';
                let option = document.createElement('option');
                option.value = "";
                option.text = "";
                dropdown.add(option);
                // Add an option for each category
                data.forEach(function(category: {
                    categoryId: string,
                    name: string,
                }) {
                    let option = document.createElement('option');
                    option.value = category.categoryId;
                    option.text = (category.name);
                    dropdown.add(option);
                });

                // Set the selected category to the value in localStorage, if it exists
                const selectedCategory = localStorage.getItem('selectedCategory');
                if (selectedCategory) {
                    dropdown.value = selectedCategory;
                }

            }

        },
        error: function(errorThrown) {
            console.log(`Error retrieving categories for edit page: ERROR -> ${errorThrown}`)
        }
    })
}

function updateNoteActions(noteId: string, categoryId?: string, tagIds?: string[] ) {
    $.ajax({
        url: "/Note/EditActions/",
        type: "post",
        data: {
            noteId: noteId,
            categoryId: categoryId,
            tagIds: tagIds,
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




