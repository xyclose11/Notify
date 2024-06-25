"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const jquery_1 = __importDefault(require("jquery"));
document.addEventListener('DOMContentLoaded', () => {
    // TableView Pagination Handlers
    (0, jquery_1.default)('body').on('click', '.pagination-link', function (e) {
        e.preventDefault();
        const url = (0, jquery_1.default)(this).attr('href');
        if (url) {
            jquery_1.default.get(url, function (data) {
                (0, jquery_1.default)('#noteViewTableContainer').html(data);
            });
        }
    });
    // End TableView Pagination Handlers
    // NoteView Handlers
    const storedView = localStorage.getItem('noteView');
    const storedCategory = localStorage.getItem('selectedCategory');
    let storedTagIds = JSON.parse(localStorage.getItem('selectedTagIds') || '[]');
    // Repopulates view selector with users choice
    if (storedView) {
        (0, jquery_1.default)('#viewSelector').val(storedView);
        loadView(storedView, storedCategory, storedTagIds);
    }
    (0, jquery_1.default)(document).ready(function () {
        if (storedTagIds) {
            storedTagIds.map(function (tag) {
                console.log("tagFCheckbox+" + tag);
                (0, jquery_1.default)(`input[type="checkbox"][id="tagFCheckbox+${tag}"]`).addClass('ThisISAteest');
            });
        }
    });
    (0, jquery_1.default)('#viewSelector').change(function () {
        const selectedView = (0, jquery_1.default)(this).val();
        localStorage.setItem('noteView', selectedView);
        loadView(selectedView, storedCategory, storedTagIds);
    });
    // End NoteView Handlers
});
function loadView(view, category, selectedTagIds) {
    jquery_1.default.ajax({
        url: '/Note/GetNotes/',
        type: 'GET',
        data: {
            view: view,
            category: category,
            filterTags: selectedTagIds.join(','),
        },
        success: function (data) {
            (0, jquery_1.default)('#notesSection').html(data);
        }
    });
}
