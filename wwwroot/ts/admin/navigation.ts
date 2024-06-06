function navLinkActive(e: Event) {
    // Get all buttons
    const buttons = document.querySelectorAll('.nav-link btn ');
    
    // Remove 'active' class from all buttons
    buttons.forEach((btn) => {
        btn.classList.remove('active');
    })
    
    // Add 'active' to the clicked button
    const clickedButton = e.target as HTMLButtonElement;
    clickedButton.classList.add('active');
}


const buttons = document.querySelectorAll('.nav-link btn');

buttons.forEach((btn) => {
    btn.addEventListener('click', navLinkActive);
})