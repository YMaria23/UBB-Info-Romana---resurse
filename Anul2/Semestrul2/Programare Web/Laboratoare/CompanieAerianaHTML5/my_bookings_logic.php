<?php
session_start();

$bookings = [];
$error = '';
$username = $_SESSION['username'] ?? 'Utilizator';

define('PAGINATION_MODE', 'backend');
//define('PAGINATION_MODE', 'frontend');

try {
    $pdo = new PDO('sqlite:' . __DIR__ . '/extra_data.sqlite');
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $pdo->exec('PRAGMA foreign_keys = ON');

    
    // redirectionare catre login, daca userul nu este autentificat
    if (!isset($_SESSION['user_id'])) {
        // trebuie verificat daca exista cookie de "remember me"
        if(isset($_COOKIE['remember_me']) ) {
            $token = $_COOKIE['remember_me'];

            // se cauta in baza de date user-ul cu token-ul respectiv
            $stmt = $pdo->prepare("SELECT * FROM Users WHERE remember_token = ?");
            $stmt->execute([$token]);
            $result = $stmt->fetch(PDO::FETCH_ASSOC);

            // daca exista userul, se logheaza automat
            if ($result) {
                $_SESSION["user_id"] = $result['id'];
                $_SESSION["username"] = $result["username"];
                $_SESSION["role"] = $result["role"];
            } 
            else {
                header('Location: login_form.php');
                exit();
            }
        } else {
            // daca nu, se redirectioneaza catre login
            header('Location: login_form.php');
            exit();
        }
    }

    $stmt = $pdo->prepare(
        'SELECT id, flight_number, departure_date, arrival_date, special_needs, document, created_at
         FROM Bookings
         WHERE user_id = ?
         ORDER BY departure_date ASC'
    );
    $stmt->execute([$_SESSION['user_id']]);

    // se salveaza toate rezervarile userului
    $bookings = $stmt->fetchAll(PDO::FETCH_ASSOC);
} catch (PDOException $e) {
    $error = 'Could not load bookings. Please try again later.';
}

// apel AJAX - returneaza o pagina de rezervari ca JSON
if (isset($_GET['ajax'])) {
    $ROWS_PER_PAGE = 5;
    $page = max(1, (int)($_GET['page'] ?? 1));
    $offset = ($page - 1) * $ROWS_PER_PAGE;
    $pageRows = array_slice($bookings, $offset, $ROWS_PER_PAGE);

    header('Content-Type: application/json; charset=utf-8');
    echo json_encode($pageRows);
    exit();
}

// apel AJAX - returneaza o pagina de rezervari ca XML
if (isset($_GET['ajax_xml'])) {
    $ROWS_PER_PAGE = 5;
    $page     = max(1, (int)($_GET['page'] ?? 1));
    $offset   = ($page - 1) * $ROWS_PER_PAGE;
    $pageRows = array_slice($bookings, $offset, $ROWS_PER_PAGE);

    $xml = new SimpleXMLElement('<?xml version="1.0" encoding="UTF-8"?><bookings/>');
    foreach ($pageRows as $b) {
        $booking = $xml->addChild('booking');
        $booking->addChild('flight_number',  htmlspecialchars($b['flight_number']));
        $booking->addChild('departure_date', htmlspecialchars($b['departure_date']));
        $booking->addChild('arrival_date',   htmlspecialchars($b['arrival_date']));
        $booking->addChild('special_needs',  (int)$b['special_needs']);
        $booking->addChild('document',       htmlspecialchars($b['document'] ?? ''));
        $booking->addChild('created_at',     htmlspecialchars($b['created_at']));
    }

    header('Content-Type: application/xml; charset=utf-8');
    echo $xml->asXML();
    exit();
}

// paginare server-side — folosita doar in modul 'backend'
if (PAGINATION_MODE === 'backend' && !empty($bookings)) {
    $ROWS_PER_PAGE = 5;
    $totalBookings = count($bookings);
    $totalPages    = max(1, (int) ceil($totalBookings / $ROWS_PER_PAGE));
    $currentPage   = max(1, min((int) ($_GET['page'] ?? 1), $totalPages));
    $offset        = ($currentPage - 1) * $ROWS_PER_PAGE;
    $pageBookings  = array_slice($bookings, $offset, $ROWS_PER_PAGE);

    // pre-construieste randurile tabelului
    $tableRowsHtml = '';
    foreach ($pageBookings as $i => $b) {
        $docCell = !empty($b['document'])
            ? '<a href="download.php?fisier=' . urlencode($b['document']) . '" target="_blank">Vezi document</a>'
            : '&mdash;';

        $tableRowsHtml .=
            '<tr>' .
            '<td>' . ($offset + $i + 1)                        . '</td>' .
            '<td>' . htmlspecialchars($b['flight_number'])      . '</td>' .
            '<td>' . htmlspecialchars($b['departure_date'])     . '</td>' .
            '<td>' . htmlspecialchars($b['arrival_date'])       . '</td>' .
            '<td>' . ($b['special_needs'] ? 'Da' : 'Nu')       . '</td>' .
            '<td>' . $docCell                                   . '</td>' .
            '<td>' . htmlspecialchars($b['created_at'])         . '</td>' .
            '</tr>';
    }

    // pre-construieste controalele de paginare (butoane identice cu frontend-ul)
    $paginationHtml = '';

    if ($currentPage > 1) {
        $paginationHtml .= '<button class="page-btn" id="prevPage" onclick="location.href=\'my_bookings.php?page=' . ($currentPage - 1) . '\'">Previous</button>';
    } else {
        $paginationHtml .= '<button class="page-btn" id="prevPage" disabled>Previous</button>';
    }

    if ($currentPage < $totalPages) {
        $paginationHtml .= '<button class="page-btn" id="nextPage" onclick="location.href=\'my_bookings.php?page=' . ($currentPage + 1) . '\'">Next</button>';
    } else {
        $paginationHtml .= '<button class="page-btn" id="nextPage" disabled>Next</button>';
    }
}
