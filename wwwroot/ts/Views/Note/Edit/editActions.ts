$(document).ready(function () {
        getCategories();

        $('#editActionSubmit').on('click', function() {
            'input[type="checkbox"][id^="tagFCheckbox+"]'
            const noteId = $('#Note_Id').val()
            const categoryId = <string | undefined>$('#categoryEditDropdown').find(':selected').val()
            
            if (noteId !== null && typeof noteId == "string") {
                updateNoteActions(noteId, categoryId);
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
            console.log(`SUCCESS`)
        },
        error: function(errorThrown) {
            console.log(`ERROR -> ${errorThrown}`)
        }
        
    })
}




