<?php
require_once 'my_bookings_logic.php';
?>
<!DOCTYPE html>
<html lang="ro">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Rezervările mele</title>
    <link rel="stylesheet" href="officeStyle.css">
    <link rel="stylesheet" href="booking/bookingStyle.css">
</head>
<body class="booking-page">
    <nav class="top-menu" aria-label="Navigare principală">
        <ul>
            <li><a href="index.php" class="menu-home">Acasă</a></li>
            <li><a href="information/travelerInfo.html" class="menu-services">Servicii</a></li>
            <li><a href="index.php#destinatii" class="menu-destinations">Destinații</a></li>
            <li><a href="my_bookings.php" class="menu-booking active">Rezervări</a></li>
            <li><a href="maze/maze.html" class="menu-games">✈️ AERO Kids</a></li>
        </ul>
    </nav>

    <main class="booking-shell">
        <h1>Rezervările mele</h1>
        <p class="booking-subtitle">
            Bine ai venit, <strong><?= htmlspecialchars($username) ?></strong>!
            Mai jos găsești toate zborurile rezervate.
        </p>

        <?php if ($error): ?>
            <p class="bookings-error"><?= htmlspecialchars($error) ?></p>
        <?php elseif (empty($bookings)): ?>
            <div class="bookings-empty">
                <p>Nu ai nicio rezervare încă.</p>
                <a href="booking/book.php" class="bookings-cta">Rezervă un zbor</a>
            </div>
        <?php else: ?>
            <?php if (PAGINATION_MODE === 'backend'): ?>
                <div class="bookings-table-wrap">
                    <table id="bookingsTable">
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Nr. zbor</th>
                                <th>Data plecării</th>
                                <th>Data sosirii</th>
                                <th>Nevoi speciale</th>
                                <th>Document</th>
                                <th>Rezervat la</th>
                            </tr>
                        </thead>
                        <tbody><?= $tableRowsHtml ?></tbody>
                    </table>
                </div>
                <div class="pagination"><?= $paginationHtml ?></div>
            <?php else: ?>
                <div class="bookings-table-wrap">
                    <table id="bookingsTable">
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Nr. zbor</th>
                                <th>Data plecării</th>
                                <th>Data sosirii</th>
                                <th>Nevoi speciale</th>
                                <th>Document</th>
                                <th>Rezervat la</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>
                <div class="pagination" id="pagination">
                    <button class="page-btn" id="prevPage">Previous</button>
                    <button class="page-btn" id="nextPage">Next</button>
                </div>
            <?php endif; ?>
        <?php endif; ?>

        <a href="booking/book.php" class="bookings-cta bookings-cta--bottom">+ Adaugă rezervare</a>
    </main>
<?php if (PAGINATION_MODE === 'frontend' && !$error && !empty($bookings)): ?>
<script>var BOOKINGS_TOTAL = <?= count($bookings) ?>;</script>
<?php endif; ?>
<?php if (PAGINATION_MODE === 'frontend'): ?>
<script src="https://code.jquery.com/jquery-3.7.1.min.js" defer></script>
<script src="my_bookings_xml.js" defer></script>
<?php endif; ?>
</body>
</html>
