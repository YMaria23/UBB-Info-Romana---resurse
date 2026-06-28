const $form = $('#reservationForm');

// validarea datelor din formular la apasarea butonului de submit
$form.on('submit', function(event) {
    // pentru a se impiedica trimiterea formularului si a permite validarea personalizată
    event.preventDefault();

    // pentru a elimina clasele de eroare de la validarile anteriore
    $form.find('.wrong-input').removeClass('wrong-input');

    // partea de validare
    const firstName = $form.find('[name="firstName"]').val().trim();
    const lastName = $form.find('[name="name"]').val().trim();
    const email = $form.find('[name="email"]').val().trim();
    const complaint = $form.find('[name="complaint"]').val().trim();

    let hasError = false;

    if (lastName === '') {
        $('#name').addClass('wrong-input');
        hasError = true;
    }

    if (firstName === '') {
        $('#firstName').addClass('wrong-input');
        hasError = true;
    }

    if (email === '') {
        $('#email').addClass('wrong-input');
        hasError = true;
    }

    if (complaint === '') {
        $('#complaint').addClass('wrong-input');
        hasError = true;
    }

    if (!hasError) {
        alert("Formular trimis cu succes!");
        $form[0].reset();
    } else {
        alert("Date incomplete sau incorecte!");
    }

});