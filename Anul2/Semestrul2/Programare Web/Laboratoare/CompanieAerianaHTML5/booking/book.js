const $form = $('#reservationForm');

const $birthDateInput = $('#dateOfBirth');
const $ageInput = $('#age');

// adaugarea varstei in functie de data nasterii
$birthDateInput.on('input', function() {
    const dateOfBirth = $birthDateInput.val();
    if (dateOfBirth) {
        const age = calculateAge(dateOfBirth);
        $ageInput.val(age);
    } else {
        $ageInput.val('');
    }
});

function calculateAge(dateOfBirth) {
    const today = new Date();
    const birthDate = new Date(dateOfBirth);
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDifference = today.getMonth() - birthDate.getMonth();

    // se verifica daca respectivul utilizator a avut deja ziua de nastere in anul curent
    if (monthDifference < 0 || (monthDifference === 0 && today.getDate() < birthDate.getDate())) {
        age--;
    }

    return age;
}

/*
// validarea datelor din formular la apasarea butonului de submit
$form.on('submit', function(event) {
    // pentru a se impiedica trimiterea formularului si a permite validarea personalizată
    event.preventDefault();

    // pentru a elimina clasele de eroare de la validarile anteriore
    $form.find('.wrong-input').removeClass('wrong-input');

    // partea de validare
    const firstName = $form.find('[name="firstName"]').val().trim();
    const lastName = $form.find('[name="name"]').val().trim();
    const date = $form.find('[name="dateOfBirth"]').val();
    const age = $form.find('[name="age"]').val();
    const email = $form.find('[name="email"]').val().trim();
    const phone = $form.find('[name="phone"]').val().trim();

    const flightDate = $form.find('[name="date"]').val();

    let hasError = false;

    if (lastName === '') {
        $('#name').addClass('wrong-input');
        hasError = true;
    }

    if (firstName === '') {
        $('#firstName').addClass('wrong-input');
        hasError = true;
    }

    if (date === '') {
        $('#dateOfBirth').addClass('wrong-input');
        hasError = true;
    }

    if (age === '' || isNaN(age) || age < 18) {
        $('#age').addClass('wrong-input');
        $ageInput.val('Varsta trebuie sa fie un numar valid si sa fie peste 18 ani');
        hasError = true;
    }

    if (email === '') {
        $('#email').addClass('wrong-input');
        hasError = true;
    }

    if (phone === '') {
        $('#phone').addClass('wrong-input');
        hasError = true;
    }

    if (flightDate === '') {
        $('#date').addClass('wrong-input');
        hasError = true;
    }

    if (!hasError) {
        alert("Formular trimis cu succes!");
        $form[0].reset();
        $ageInput.val('');
    } else {
        alert("Date incomplete sau incorecte!");
    }

});*/