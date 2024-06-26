"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const jquery_1 = __importDefault(require("jquery"));
document.addEventListener("DOMContentLoaded", () => {
    (0, jquery_1.default)('#createTestNotes').on('click', function (e) {
        e.preventDefault();
        jquery_1.default.ajax({
            url: (0, jquery_1.default)(this).attr('href'),
            type: 'GET',
            success: function (data) {
                (0, jquery_1.default)('#notesSection').html(data);
            },
            error: function () {
                alert('Error creating the test notes.');
            }
        });
    });
});
