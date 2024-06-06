document.querySelectorAll('.delete-user-form').forEach(form => {
    form.addEventListener('submit', function(event: Event) {
        const confirmed = confirm('Are you sure you want to delete this user?');
        if (!confirmed) {
            event.preventDefault();
        }
    })
})