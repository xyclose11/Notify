import $ from "jquery";

$(document).ready(() => {
    // Clear checkboxes for tag filtering
    $(document).on('click', "#tagFilterClearBtn", function(){
        $('input[type="checkbox"][id^="tagFCheckbox"]').prop("checked", false);

        // Clear localStorage
        localStorage.removeItem("selectedTagIds");
    });
    
    $(document).on('click', '#tagLoadMoreBtn', () => {
        const currentTagCount: number = $('input[type="checkbox"][id^="tagFCheckbox+"]').length;
       $.get('/Tag/LoadMoreTags/', { currentTagCount: currentTagCount }, (data) => {
           data.forEach(function (tag: {name: string, id: string}) {
               addTagToFilterCB(tag);
           })
       })
    });
    
    
});

function addTagToFilterCB(tag: {name: string, id: string}) {
    // Get previously selected tags
    let storedTagIds = JSON.parse(localStorage.getItem('selectedTagIds') || '[]');

    const containerElem = $('#tagFilterCBList');
    const divT = $('<div/>', { class: "form-check form-check-inline"}).appendTo(containerElem);
    const inputT = $('<input />', { class: "form-check-input", type: 'checkbox', name: "tagFCheckbox" , id: `tagFCheckbox+${tag.id}`, value: tag.id, 
        checked: storedTagIds.includes(tag.id)});
    const labelT = $('<label />', { class: "form-check-label", 'for': `#tagFilterCB+${tag.id}`, text: tag.name });
    
    divT.append(inputT, labelT);
    divT.appendTo(containerElem);
}