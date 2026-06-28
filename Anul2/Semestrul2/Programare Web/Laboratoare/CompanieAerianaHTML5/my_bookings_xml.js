(function () {
    const ROWS_PER_PAGE = 5;

    // daca nu exista tabel, suntem pe starea goala/eroare - nu e nimic de facut
    const tbody = document.querySelector('#bookingsTable tbody');
    if (!tbody) return;

    const prev       = document.getElementById('prevPage');
    const next       = document.getElementById('nextPage');
    const totalPages = Math.ceil(window.BOOKINGS_TOTAL / ROWS_PER_PAGE);
    let currentPage  = 1;

    // curata string-urile pentru a preveni XSS
    function escapeHtml(str) {
        var div = document.createElement('div');
        div.appendChild(document.createTextNode(str != null ? String(str) : ''));
        return div.innerHTML;
    }

    // citeste continutul textual al unui tag dintr-un nod XML
    function getVal(node, tag) {
        var el = node.getElementsByTagName(tag)[0];
        return el ? el.textContent : '';
    }

    // functie care actualizeaza starea butoanelor de paginare
    function renderControls() {
        prev.disabled = currentPage === 1;
        next.disabled = currentPage === totalPages;
    }

    // preia o pagina de la server (XML) si populeaza tabelul
    function loadPage(page) {
        fetch('my_bookings.php?ajax_xml&page=' + page)
            .then(function (response) {
                // extra verificare pt raspunsul http
                // fetch nu considera raspunsurile 4xx sau 5xx ca erori, deci trebuie verificat manual
                if (!response.ok) {
                    throw new Error('HTTP ' + response.status);
                }
                return response.text();
            })
            .then(function (xmlString) {
                var parser   = new DOMParser();
                var xml      = parser.parseFromString(xmlString, 'application/xml');
                var bookings = Array.from(xml.getElementsByTagName('booking'));


                // elimina elementele vechi stocate in tabel
                tbody.innerHTML = '';
                var offset = (page - 1) * ROWS_PER_PAGE;

                // se adauga inregistrarile in tabel
                bookings.forEach(function (b, i) {
                    var tr      = document.createElement('tr');
                    var docVal  = getVal(b, 'document');

                    var docCell;
                    if (docVal) {
                        var a = document.createElement('a');
                        a.href = 'download.php?fisier=' + encodeURIComponent(docVal);
                        a.target  = '_blank';
                        a.textContent = 'Vezi document';
                        docCell = a.outerHTML;
                    } else {
                        docCell = '\u2014';
                    }

                    // se construiesc randurile tabelului folosind datele din XML
                    tr.innerHTML =
                        '<td>' + (offset + i + 1)                          + '</td>' +
                        '<td>' + escapeHtml(getVal(b, 'flight_number'))    + '</td>' +
                        '<td>' + escapeHtml(getVal(b, 'departure_date'))   + '</td>' +
                        '<td>' + escapeHtml(getVal(b, 'arrival_date'))     + '</td>' +
                        '<td>' + (getVal(b, 'special_needs') === '1' ? 'Da' : 'Nu') + '</td>' +
                        '<td>' + docCell                                   + '</td>' +
                        '<td>' + escapeHtml(getVal(b, 'created_at'))       + '</td>';

                    tbody.appendChild(tr);
                });

                currentPage = page;
                renderControls();
            })
            .catch(function (err) {
                console.error('Eroare la incarcarea rezervarilor (XML):', err);
            });
    }

    prev.addEventListener('click', function () { loadPage(currentPage - 1); });
    next.addEventListener('click', function () { loadPage(currentPage + 1); });

    loadPage(1);
})();