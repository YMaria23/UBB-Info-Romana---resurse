<?php
session_start();

/*
$servername = "localhost";
$username = "root";
$dbname = "myDatabase";
$password = "";
*/

// conexiunea la baza de date
try{
    // se incearca conexiunea
    //$pdo = new PDO("mysql:host=$servername;dbname=$dbname", $username, $password);
    //$pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $pdo = new PDO('sqlite:' . __DIR__ . '/extra_data.sqlite');
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $pdo->exec('PRAGMA foreign_keys = ON');


    // se verifica numele utilizatorului pentru a se putea descarca fisiere
    if (!isset($_SESSION['user_id'])) {
        http_response_code(401);
        die("Trebuie sa fii logat pentru a descarca fisiere.");
    }

    $fisier = $_GET['fisier'] ?? '';

    if ($fisier === '') {
        http_response_code(400);
        die("Niciun fisier specificat.");
    }

    // basename() - elimina orice cale din numele fisierului, prevenind astfel atacurile de tip path traversal
    $safe_name = basename($fisier);

    // se interogheaza baza de date pentru a verifica daca fisierul apartine utilizatorului logat
    $stmt = $pdo->prepare('SELECT user_id FROM Bookings WHERE document = ?');
    $stmt->execute([$safe_name]);
    $booking = $stmt->fetch(PDO::FETCH_ASSOC);

    if (!$booking || $booking['user_id'] != $_SESSION['user_id']) {
        http_response_code(403);
        die("Nu ai permisiunea de a accesa acest fisier.");
    }


    $uploads_dir = realpath(__DIR__ . '/uploads');
    $cale        = $uploads_dir . DIRECTORY_SEPARATOR . $safe_name;

    // realpath() - rezolva calea absoluta reala (inclusiv symlink-uri, encoding-uri)
    // si verificam ca fisierul se afla efectiv in interiorul /uploads/
    $real_cale = realpath($cale);

    if ($real_cale === false || strpos($real_cale, $uploads_dir . DIRECTORY_SEPARATOR) !== 0) {
        http_response_code(403);
        die("Acces interzis.");
    }

    if (!file_exists($real_cale)) {
        http_response_code(404);
        die("Fisierul nu a fost gasit.");
    }

    $mime = mime_content_type($real_cale);

    // sanitizarea numelui de fisier pentru a preveni header injection
    $safe_filename = str_replace(['"', "\r", "\n"], '', basename($real_cale));

    header('Content-Type: ' . $mime);
    header('Content-Disposition: inline; filename="' . $safe_filename . '"');
    header('Content-Length: ' . filesize($real_cale));

    readfile($real_cale);
    exit();
    }catch(PDOException $e){
    http_response_code(500);
    die("A aparut o eroare. Va rugam incercati din nou mai tarziu.");
    }
?>