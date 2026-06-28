////////////////////////////////// CAROUSEL //////////////////////////////////

const carouselData = [
    {
        link: "booking/book.html",
        linkText: "Rezervă Zborul Tău",
        backgroundImage: "https://www.travelandleisure.com/thmb/Gv4G0GI2ShkZ2a3B7gM8IwS8rJs=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/TAL-Airplane-Seats-BETTERSEAT0323-c08b3e578a8b4811914efa7e68e2c531.jpg"
    },
    {
        link: "information/travelerInfo.html",
        linkText: "Vezi Informații pentru Călători",
        backgroundImage: "https://img.freepik.com/premium-photo/travel-woman-airport-luggage-vacation-break-girl-excited-smile-check-boarding-ticket-female-lady-traveler-with-suitcase-international-departure-with-passport-trip_590464-133476.jpg?w=2000"
    },
    {
        link: "#destinatii",
        linkText: "Alege o destinație",
        backgroundImage: "https://media.cntraveler.com/photos/5cf96a9dd9fb41f17ed08435/master/pass/Eiffel%20Tower_GettyImages-1005348968.jpg"
    },
    {
        link: "https://www.instagram.com/tarom.airways/",
        linkText: "Urmărește-ne pe Instagram",
        backgroundImage: "https://s1.r29static.com/bin/entry/761/0,0,2000,1050/x,80/1797096/image.jpg"
    }
];


let currentIndex = 0;
const $container = $('#carousel-container');
const $linkElement = $('#carousel-link');

function updateCarousel() {
    const currentData = carouselData[currentIndex];

    // se modifica imaginea de fundal
    $container.css('backgroundImage', `url('${currentData.backgroundImage}')`);

    // se modifica textul si linkul butonului
    $linkElement.text(currentData.linkText);
    $linkElement.attr('href', currentData.link);
}

function onNext() {
    currentIndex = (currentIndex + 1) % carouselData.length;
    updateCarousel();
}

function onPrev() {
    // previne ajungerea la un index negativ
    currentIndex = (currentIndex - 1 + carouselData.length) % carouselData.length;
    updateCarousel();
}

let timer = setInterval(onNext, 3000);

function resetTimer() {
    clearInterval(timer);
    timer = setInterval(onNext, 3000);
}

// se adauga event listeneri pentru butoanele de navigare
$('#nextBtn').on('click', () => {
    onNext();
    resetTimer();
});

$('#prevBtn').on('click', () => {
    onPrev();
    resetTimer();
});

// se adauga prima imagine la incarcarea paginii
updateCarousel();



///////////////////////////////////////// TABLE //////////////////////////////////////

// salvarea datelor din tabel intr-o variabila
const tabelVertical = [
    {Tip: "Bagaj de mână (mic)", Dimensiuni: "40 x 30 x 20 cm", Depozitare: "În compartimentul de deasupra scaunului"},
    {Tip: "Bagaj de mână (mare)", Dimensiuni: "55 x 40 x 23 cm", Depozitare: "În compartimentul de deasupra scaunului"},
    {Tip: "Bagaj de cală", Dimensiuni: "150 x 100 x 80 cm", Depozitare: "În cala avionului"}
];

const tabelOrizontal = [
    {Zbor: "AERO101", Plecare: "București (OTP)", Sosire: "Cluj-Napoca (CLJ)", Ora_Plecare: "08:00", Ora_Sosire: "08:50", Tip: "Direct"},
    {Zbor: "AERO205", Plecare: "București (OTP)", Sosire: "Paris (CDG)", Ora_Plecare: "10:00", Ora_Sosire: "12:30", Tip: "Direct"},
    {Zbor: "AERO309", Plecare: "București (OTP)", Sosire: "New York (JFK)", Ora_Plecare: "09:15", Ora_Sosire: "17:40", Tip: "1 escală"},
];

const $tabel = $('#tabelVertical');
const keys = Object.keys(tabelVertical[0]);

let lastSortedKey = "";

// popularea tabelului vertical
function genereazaTabelVertical(date) {
    $tabel.empty();

    //pentru fiecare cheie se adauga un rand in tabel
    keys.forEach(key => {
        const $tr = $('<tr>');
        const $th = $('<th>');

        // logica vizuala
        if (key === lastSortedKey) {
            $th.addClass('header-activ').text(key + " ▼");
        } else {
            $th.text(key + " ↕");
        }

        // event listener pentru sortare
        $th.on('click', () => actioneazaSortareVerticala(key));
        $tr.append($th);

        // se adauga td cu valorile corespunzatoare cheii pentru fiecare obiect
        date.forEach(item => {
            $tr.append($('<td>').text(item[key]));
        });

        $tabel.append($tr);
    });
}

const $tabelOr = $('#tabelOrizontal');
const keysOrizontal = Object.keys(tabelOrizontal[0]);

let lastSortedKeyOrizontal = "";

// popularea tabelului orizontal
function genereazaTabelOrizontal(date) {
    $tabelOr.empty();

    //se adauga un rand pentru headere (keys)
    const $tr = $('<tr>');
    keysOrizontal.forEach(key => {
        const $th = $('<th>');

        // logica vizuala
        if (key === lastSortedKeyOrizontal) {
            $th.addClass('header-activ').text(key + " ▼");
        } else {
            $th.text(key + " ↕");
        }

        // event listener pentru sortare
        $th.on('click', () => actioneazaSortareOrizontala(key));
        $tr.append($th);
    });
    $tabelOr.append($tr);

    // se adauga un rand nou pentru fiecare inregistrare din tabel
    date.forEach(item => {
        const $row = $('<tr>');

        // se adauga un td pentru fiecare valoare din inregistrare
        keysOrizontal.forEach(key => {
            $row.append($('<td>').text(item[key]));
        });

        $tabelOr.append($row);
    });
}


genereazaTabelVertical(tabelVertical);
genereazaTabelOrizontal(tabelOrizontal);

// functiile de sortare
function actioneazaSortareVerticala(key) {
    tabelVertical.sort((a, b) => {
        const valA = a[key].toString().toLowerCase();
        const valB = b[key].toString().toLowerCase();

        if (valA < valB) return  1;
        if (valA > valB) return -1;
        return 0;
    });

    lastSortedKey = key;
    genereazaTabelVertical(tabelVertical);
}


function actioneazaSortareOrizontala(key) {
    tabelOrizontal.sort((a, b) => {
        const valA = a[key].toString().toLowerCase();
        const valB = b[key].toString().toLowerCase();

        if (valA < valB) return  1;
        if (valA > valB) return -1;
        return 0;
    });

    lastSortedKeyOrizontal = key;
    genereazaTabelOrizontal(tabelOrizontal);
}





/////////////////////// LISTS //////////////////////////////////
$('.expandabil').on('click', function(e) {
    // evenimentul trebuie declansat de elementul parinte
    if (e.target !== this) return;

    // se cauta sublista asociata elementului parinte
    const $sublista = $(this).find('.sublista');

    if ($sublista.length) {
        // toggle eficientizeaza procesul: daca exista clasa, o elimina
        // daca nu exista, o adauga
        $sublista.toggleClass('hidden');
        $(this).toggleClass('activ');
    }
});
