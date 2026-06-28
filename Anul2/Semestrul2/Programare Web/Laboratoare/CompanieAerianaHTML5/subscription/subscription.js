const form           = document.getElementById('reservationForm');
const birthDateInput = document.getElementById('dateOfBirth');
const ageInput       = document.getElementById('age');

// Calculul varstei in functie de data nasterii
birthDateInput.addEventListener('input', function () {
    const dateOfBirth = birthDateInput.value;
    if (dateOfBirth) {
        ageInput.value = calculateAge(dateOfBirth);
    } else {
        ageInput.value = '';
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
form.addEventListener('submit', function (event) {
    event.preventDefault();
    form.querySelectorAll('.wrong-input').forEach(function (el) {
        el.classList.remove('wrong-input');
    });

    const firstName = form.querySelector('[name="firstName"]').value.trim();
    const lastName  = form.querySelector('[name="name"]').value.trim();
    const date      = form.querySelector('[name="dateOfBirth"]').value;
    const age       = form.querySelector('[name="age"]').value;
    const email     = form.querySelector('[name="email"]').value.trim();
    const phone     = form.querySelector('[name="phone"]').value.trim();
    const password  = form.querySelector('[name="password"]').value;
    const plan      = form.querySelector('[name="subscriptionType"]').value;

    let hasError = false;

    if (lastName  === '')                             { document.getElementById('name').classList.add('wrong-input');             hasError = true; }
    if (firstName === '')                             { document.getElementById('firstName').classList.add('wrong-input');        hasError = true; }
    if (date      === '')                             { document.getElementById('dateOfBirth').classList.add('wrong-input');      hasError = true; }
    if (age === '' || isNaN(age) || Number(age) < 18) {
        document.getElementById('age').classList.add('wrong-input');
        ageInput.value = 'Vârsta trebuie să fie un număr valid și peste 18 ani';
        hasError = true;
    }
    if (email    === '') { document.getElementById('email').classList.add('wrong-input');            hasError = true; }
    if (password === '') { document.getElementById('password').classList.add('wrong-input');         hasError = true; }
    if (phone    === '') { document.getElementById('phone').classList.add('wrong-input');            hasError = true; }
    if (!plan)           { document.getElementById('subscriptionType').classList.add('wrong-input'); hasError = true; }

    if (!hasError) {
        alert('Formular trimis cu succes!');
        form.reset();
    } else {
        alert('Date incomplete sau incorecte!');
    }
});

form.addEventListener('reset', function () {
    form.querySelectorAll('.wrong-input').forEach(function (el) {
        el.classList.remove('wrong-input');
    });
    document.getElementById('subscriptionSubmit').disabled = true;
    document.getElementById('chosenServiceRow').style.display = 'none';
    document.getElementById('chosenService').innerHTML = '';
    ageInput.value = '';
});



// Cache-ul datelor incarcate din JSON
let servicesData = {};


// incarca serviciile disponibile pentru abonamente din fisierul JSON
function loadServicesFromJSON(callback) {
    const xhr = new XMLHttpRequest();
    xhr.open('GET', 'subscriptionServices.json', true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                try {
                    servicesData = JSON.parse(xhr.responseText);
                    callback(servicesData);
                } catch (e) {
                    console.error('Eroare parsare JSON servicii:', e);
                }
            } else {
                console.error('Nu s-a putut incarca subscriptionServices.json, status:', xhr.status);
            }
        }
    };
    xhr.send();
}

function updateServices(type) {
    const serviceContainer = document.getElementById('chosenService');
    serviceContainer.innerHTML = '';

    const options = servicesData[type] || [];
    options.forEach(function (opt, index) {
        const wrapper = document.createElement('div');
        wrapper.className = 'chosen-service-option';

        const input = document.createElement('input');
        input.type  = 'radio';
        input.name  = 'chosenService';
        input.value = opt.value;
        input.id    = opt.id;
        if (index === 0) { input.checked = true; }

        const span = document.createElement('span');
        span.textContent = opt.label;

        const editBtn = document.createElement('button');
        editBtn.type        = 'button';
        editBtn.textContent = '✎';
        editBtn.className   = 'inline-edit-btn';
        editBtn.title       = 'Editează serviciul';

        const editForm = document.createElement('div');
        editForm.className     = 'inline-edit-form';
        editForm.style.display = 'none';

        const labelInput = document.createElement('input');
        labelInput.type        = 'text';
        labelInput.className   = 'inline-edit-label';
        labelInput.placeholder = 'Denumire';

        const valueInput = document.createElement('input');
        valueInput.type        = 'text';
        valueInput.className   = 'inline-edit-value';
        valueInput.placeholder = 'Valoare';

        const saveBtn = document.createElement('button');
        saveBtn.type        = 'button';
        saveBtn.textContent = 'Salvează';
        saveBtn.className   = 'inline-save-btn';

        const cancelBtn = document.createElement('button');
        cancelBtn.type        = 'button';
        cancelBtn.textContent = 'Anulează';
        cancelBtn.className   = 'inline-cancel-btn';

        const statusSpan = document.createElement('span');
        statusSpan.className = 'inline-edit-status';

        editForm.appendChild(labelInput);
        editForm.appendChild(valueInput);
        editForm.appendChild(saveBtn);
        editForm.appendChild(cancelBtn);
        editForm.appendChild(statusSpan);

        wrapper.appendChild(input);
        wrapper.appendChild(span);
        wrapper.appendChild(editBtn);
        wrapper.appendChild(editForm);
        serviceContainer.appendChild(wrapper);

        editBtn.addEventListener('click', function () {
            if (editForm.style.display !== 'none') {
                editForm.style.display = 'none';
            } else {
                labelInput.value       = span.textContent;
                valueInput.value       = input.value;
                statusSpan.textContent = '';
                editForm.style.display = 'flex';
            }
        });

        cancelBtn.addEventListener('click', function () {
            editForm.style.display = 'none';
            statusSpan.textContent = '';
        });

        saveBtn.addEventListener('click', function () {
            const newLabel = labelInput.value.trim();
            const newValue = valueInput.value.trim();
            if (!newLabel || !newValue) {
                statusSpan.textContent = 'Câmpurile nu pot fi goale.';
                statusSpan.className   = 'inline-edit-status error';
                return;
            }

            const payload = JSON.stringify({ id: opt.id, label: newLabel, value: newValue });
            const xhr = new XMLHttpRequest();
            xhr.open('POST', 'subscription_edit.php', true);
            xhr.setRequestHeader('Content-Type', 'application/json');
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4) {
                    const resp = JSON.parse(xhr.responseText);
                    if (resp.success) {
                        span.textContent = newLabel;
                        input.value      = newValue;
                        opt.label        = newLabel;
                        opt.value        = newValue;
                        statusSpan.textContent = '✓ Salvat';
                        statusSpan.className   = 'inline-edit-status success';
                        setTimeout(function () { editForm.style.display = 'none'; }, 800);
                    } else {
                        statusSpan.textContent = '✗ ' + (resp.error || 'Eroare la salvare');
                        statusSpan.className   = 'inline-edit-status error';
                    }
                }
            };
            xhr.send(payload);
        });
    });
}

// La schimbarea tipului de abonament: incarca serviciile via AJAX si activeaza butonul Rezerva
document.getElementById('subscriptionType').addEventListener('change', function () {
    const plan = this.value;
    if (plan) {
        document.getElementById('subscriptionSubmit').disabled = false;
        document.getElementById('chosenServiceRow').style.display = '';
        loadServicesFromJSON(function () {
            updateServices(plan);
        });
    } else {
        document.getElementById('chosenServiceRow').style.display = 'none';
        document.getElementById('chosenService').innerHTML = '';
        document.getElementById('subscriptionSubmit').disabled = true;
    }
});