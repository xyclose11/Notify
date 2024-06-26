import $ from "jquery";

$(document).ready(() => {
    $(document).on('click', "#tagFilterClearBtn", function(){
        $('input[type="checkbox"][id^="tagFCheckbox"]').prop("checked", false);

        // Clear localStorage
        localStorage.removeItem("selectedTagIds");
    })
});