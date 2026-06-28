(function () {
    const ROWS_PER_PAGE = 5;

    // daca nu exista tabel, suntem pe starea goala/eroare - nu e nimic de facut
    const tbody = document.querySelector('#bookingsTable tbody');
    if (!tbody) return;

    const prev = document.getElementById('prevPage');
    const next = document.getElementById('nextPage');
    const totalPages = Math.ceil(window.BOOKINGS_TOTAL / ROWS_PER_PAGE);
    let currentPage  = 1;

    // curata string-urile pentru a preveni XSS
    function escapeHtml(str) {
        var div = document.createElement('div');
        div.appendChild(document.createTextNode(str != null ? String(str) : ''));
        return div.innerHTML;
    }

    // functie care actualizeaza starea butoanelor de paginare
    function renderControls() {
        prev.disabled = currentPage === 1;
        next.disabled = currentPage === totalPages;
    }

    // preia o pagina de la server si populeaza tabelul
    function loadPage(page) {
        fetch('my_bookings.php?ajax&page=' + page)
            .then(function (r) {
                // se returneaza datele sub forma de JSON 
                return r.json(); 
            })
            .then(function (rows) {
                // se curata continutul tabelului si se adauga noile randuri
                tbody.innerHTML = '';
                var offset = (page - 1) * ROWS_PER_PAGE;

                rows.forEach(function (b, i) {
                    var tr = document.createElement('tr');

                    var docCell;
                    if (b.document) {
                        var a = document.createElement('a');
                        a.href        = 'download.php?fisier=' + encodeURIComponent(b.document);
                        a.target      = '_blank';
                        a.textContent = 'Vezi document';
                        docCell = a.outerHTML;
                    } else {
                        docCell = '\u2014';
                    }

                    // se adauga inregistrarile in tabel
                    tr.innerHTML =
                        '<td>' + (offset + i + 1)              + '</td>' +
                        '<td>' + escapeHtml(b.flight_number)   + '</td>' +
                        '<td>' + escapeHtml(b.departure_date)  + '</td>' +
                        '<td>' + escapeHtml(b.arrival_date)    + '</td>' +
                        '<td>' + (b.special_needs ? 'Da' : 'Nu') + '</td>' +
                        '<td>' + docCell                       + '</td>' +
                        '<td>' + escapeHtml(b.created_at)      + '</td>';

                    tbody.appendChild(tr);
                });

                currentPage = page;
                renderControls();
            });
    }

    prev.addEventListener('click', function () { loadPage(currentPage - 1); });
    next.addEventListener('click', function () { loadPage(currentPage + 1); });

    loadPage(1);
})();
