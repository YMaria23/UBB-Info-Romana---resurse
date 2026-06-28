$(function () {
    const ROWS_PER_PAGE = 5;

    // daca nu exista tabel, suntem pe starea goala/eroare - nu e nimic de facut
    const $tbody = $('#bookingsTable tbody');
    if ($tbody.length === 0) return;

    const $prev      = $('#prevPage');
    const $next      = $('#nextPage');
    const totalPages = Math.ceil(window.BOOKINGS_TOTAL / ROWS_PER_PAGE);
    let currentPage  = 1;

    // functie care actualizeaza starea butoanelor de paginare
    function renderControls() {
        var prevDisabled = currentPage === 1;
        var nextDisabled = currentPage === totalPages;
        $prev.prop('disabled', prevDisabled).toggleClass('page-btn--disabled', prevDisabled);
        $next.prop('disabled', nextDisabled).toggleClass('page-btn--disabled', nextDisabled);
    }

    // preia o pagina de la server si populeaza tabelul
    function loadPage(page) {
        $.getJSON('my_bookings.php', { ajax: '', page: page }, function (rows) {
            // se curata continutul tabelului si se adauga noile randuri
            $tbody.empty();
            var offset = (page - 1) * ROWS_PER_PAGE;

            $.each(rows, function (i, b) {
                var $tdDoc;
                if (b.document) {
                    $tdDoc = $('<td>').append(
                        $('<a>')
                            .attr('href', 'download.php?fisier=' + encodeURIComponent(b.document))
                            .attr('target', '_blank')
                            .text('Vezi document')
                    );
                } else {
                    $tdDoc = $('<td>').text('\u2014');
                }

                // se adauga inregistrarile in tabel
                // jQuery .text() escapeaza automat HTML - nu e nevoie de escapeHtml
                $('<tr>').append(
                    $('<td>').text(offset + i + 1),
                    $('<td>').text(b.flight_number),
                    $('<td>').text(b.departure_date),
                    $('<td>').text(b.arrival_date),
                    $('<td>').text(b.special_needs ? 'Da' : 'Nu'),
                    $tdDoc,
                    $('<td>').text(b.created_at)
                ).appendTo($tbody);
            });

            currentPage = page;
            renderControls();
        });
    }

    $prev.on('click', function () { loadPage(currentPage - 1); });
    $next.on('click', function () { loadPage(currentPage + 1); });

    renderControls(); // starea initiala: suntem pe pagina 1, prev este dezactivat de la inceput
    loadPage(1);
});

