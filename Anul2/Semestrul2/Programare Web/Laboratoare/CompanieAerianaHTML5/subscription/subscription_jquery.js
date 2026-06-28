const $form           = $('#reservationForm');
const $birthDateInput = $('#dateOfBirth');
const $ageInput       = $('#age');

// Calculul varstei in functie de data nasterii
$birthDateInput.on('input', function () {
    const dateOfBirth = $birthDateInput.val();
    if (dateOfBirth) {
        $ageInput.val(calculateAge(dateOfBirth));
    } else {
        $ageInput.val('');
    }
});

function calculateAge(dateOfBirth) {
    const today     = new Date();
    const birthDate = new Date(dateOfBirth);
    let age = today.getFullYear() - birthDate.getFullYear();
    const m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age--;
    }
    return age;
}

// Validarea formularului la submit
$form.on('submit', function (event) {
    event.preventDefault();
    $form.find('.wrong-input').removeClass('wrong-input');

    const firstName = $form.find('[name="firstName"]').val().trim();
    const lastName  = $form.find('[name="name"]').val().trim();
    const date      = $form.find('[name="dateOfBirth"]').val();
    const age       = $form.find('[name="age"]').val();
    const email     = $form.find('[name="email"]').val().trim();
    const phone     = $form.find('[name="phone"]').val().trim();
    const password  = $form.find('[name="password"]').val();
    const plan      = $form.find('[name="subscriptionType"]').val();

    let hasError = false;

    if (lastName  === '')                             { $('#name').addClass('wrong-input');             hasError = true; }
    if (firstName === '')                             { $('#firstName').addClass('wrong-input');        hasError = true; }
    if (date      === '')                             { $('#dateOfBirth').addClass('wrong-input');      hasError = true; }
    if (age === '' || isNaN(age) || Number(age) < 18) {
        $('#age').addClass('wrong-input');
        $ageInput.val('Vârsta trebuie să fie un număr valid și peste 18 ani');
        hasError = true;
    }
    if (email    === '') { $('#email').addClass('wrong-input');            hasError = true; }
    if (password === '') { $('#password').addClass('wrong-input');         hasError = true; }
    if (phone    === '') { $('#phone').addClass('wrong-input');            hasError = true; }
    if (!plan)           { $('#subscriptionType').addClass('wrong-input'); hasError = true; }

    if (!hasError) {
        alert('Formular trimis cu succes!');
        $form[0].reset();
    } else {
        alert('Date incomplete sau incorecte!');
    }
});

$form.on('reset', function () {
    $form.find('.wrong-input').removeClass('wrong-input');
    $('#subscriptionSubmit').prop('disabled', true);
    $('#chosenServiceRow').hide();
    $('#chosenService').empty();
    $ageInput.val('');
});



// Cache-ul datelor incarcate din JSON
let servicesData = {};


// incarca serviciile disponibile pentru abonamente din fisierul JSON
function loadServicesFromJSON(callback) {
    $.getJSON('subscriptionServices.json')
        .done(function (data) {
            servicesData = data;
            callback(servicesData);
        })
        .fail(function (jqxhr) {
            console.error('Nu s-a putut incarca subscriptionServices.json, status:', jqxhr.status);
        });
}

function updateServices(type) {
    const $serviceContainer = $('#chosenService').empty();

    const options = servicesData[type] || [];
    options.forEach(function (opt, index) {
        const $input = $('<input>', {
            type:  'radio',
            name:  'chosenService',
            value: opt.value,
            id:    opt.id
        });
        if (index === 0) { $input.prop('checked', true); }

        const $span    = $('<span>').text(opt.label);
        const $editBtn = $('<button>', { type: 'button', text: '✎', class: 'inline-edit-btn', title: 'Editează serviciul' });

        const $labelInput = $('<input>', { type: 'text', class: 'inline-edit-label', placeholder: 'Denumire' });
        const $valueInput = $('<input>', { type: 'text', class: 'inline-edit-value', placeholder: 'Valoare' });
        const $saveBtn    = $('<button>', { type: 'button', text: 'Salvează',  class: 'inline-save-btn' });
        const $cancelBtn  = $('<button>', { type: 'button', text: 'Anulează', class: 'inline-cancel-btn' });
        const $statusSpan = $('<span>').addClass('inline-edit-status');

        const $editForm = $('<div>').addClass('inline-edit-form').hide()
            .append($labelInput, $valueInput, $saveBtn, $cancelBtn, $statusSpan);

        const $wrapper = $('<div>').addClass('chosen-service-option')
            .append($input, $span, $editBtn, $editForm);

        $serviceContainer.append($wrapper);

        $editBtn.on('click', function () {
            if ($editForm.is(':visible')) {
                $editForm.hide();
            } else {
                $labelInput.val($span.text());
                $valueInput.val($input.val());
                $statusSpan.text('');
                $editForm.show();
            }
        });

        $cancelBtn.on('click', function () {
            $editForm.hide();
            $statusSpan.text('');
        });

        $saveBtn.on('click', function () {
            const newLabel = $labelInput.val().trim();
            const newValue = $valueInput.val().trim();
            if (!newLabel || !newValue) {
                $statusSpan.text('Câmpurile nu pot fi goale.').attr('class', 'inline-edit-status error');
                return;
            }

            $.ajax({
                url:         'subscription_edit.php',
                type:        'POST',
                contentType: 'application/json',
                dataType:    'json',
                data:        JSON.stringify({ id: opt.id, label: newLabel, value: newValue }),
                success: function (resp) {
                    if (resp.success) {
                        $span.text(newLabel);
                        $input.val(newValue);
                        opt.label = newLabel;
                        opt.value = newValue;
                        $statusSpan.text('✓ Salvat').attr('class', 'inline-edit-status success');
                        setTimeout(function () { $editForm.hide(); }, 800);
                    } else {
                        $statusSpan.text('✗ ' + (resp.error || 'Eroare la salvare')).attr('class', 'inline-edit-status error');
                    }
                },
                error: function () {
                    $statusSpan.text('✗ Eroare la comunicare').attr('class', 'inline-edit-status error');
                }
            });
        });
    });
}

// La schimbarea tipului de abonament: incarca serviciile via AJAX si activeaza butonul Rezerva
$('#subscriptionType').on('change', function () {
    const plan = $(this).val();
    if (plan) {
        $('#subscriptionSubmit').prop('disabled', false);
        $('#chosenServiceRow').show();
        loadServicesFromJSON(function () {
            updateServices(plan);
        });
    } else {
        $('#chosenServiceRow').hide();
        $('#chosenService').empty();
        $('#subscriptionSubmit').prop('disabled', true);
    }
});