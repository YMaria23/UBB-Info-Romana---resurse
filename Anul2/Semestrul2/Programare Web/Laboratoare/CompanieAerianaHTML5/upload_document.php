<?php
session_start();

try {
    $pdo = new PDO('sqlite:' . __DIR__ . '/extra_data.sqlite');
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $pdo->exec('PRAGMA foreign_keys = ON');

    if ($_SERVER['REQUEST_METHOD'] === 'POST') {

        $flight_number  = $_POST['flightNr']      ?? '';
        $departure_date = $_POST['date']           ?? '';
        $arrival_date   = $_POST['date']           ?? '';
        $special_needs  = !empty(trim($_POST['specialNeeds'] ?? '')) ? 1 : 0;
        $user_id        = $_SESSION['user_id']     ?? null;
        $csrf_token     = $_POST['csrf_token']     ?? '';

        // verificarea autentificarii
        if (!isset($_SESSION['user_id'])) {
            http_response_code(401);
            die("Trebuie sa fii logat pentru a face o rezervare.");
        }

        // verificam token-ul CSRF
        if (!isset($_POST['csrf_token']) || !isset($_SESSION['csrf_token'])) {
            http_response_code(403);
            die("Eroare de securitate: Token-ul CSRF lipseste!");
         }

        if (!hash_equals($_SESSION['csrf_token'], $_POST['csrf_token'])) {
            http_response_code(403);
            die("Eroare de securitate: Token-ul CSRF este invalid!");
        }

        // se salveaza rezervarea in baza de date
        $stmt = $pdo->prepare(
            'INSERT INTO Bookings (flight_number, departure_date, arrival_date, special_needs, user_id)
             VALUES (:flight_number, :departure_date, :arrival_date, :special_needs, :user_id)'
        );
        $stmt->execute([
            ':flight_number'  => $flight_number,
            ':departure_date' => $departure_date,
            ':arrival_date'   => $arrival_date,
            ':special_needs'  => $special_needs,
            ':user_id'        => $user_id,
        ]);

        $booking_id = $pdo->lastInsertId();

        // daca s-a incarcat un fisier, se proceseaza
        if (isset($_FILES['specialNeedFiles']) && $_FILES['specialNeedFiles']['error'] === UPLOAD_ERR_OK) {
            // se verifica daca se afla printre extensiile acceptate
            $extensii = [".pdf",".docx",".jpg",".jpeg",".png",".ppt",".pptx",".doc"];

            // se extrage extensia fisierului
            $file_extension = strtolower(pathinfo($_FILES['specialNeedFiles']['name'], PATHINFO_EXTENSION));

            if (!in_array("." . $file_extension, $extensii)) {
                die("Fisierul incarcat nu este de un tip acceptat! Tipuri acceptate: " . implode(", ", $extensii));
            }

            // verificarea tipului MIME real al fisierului (nu doar extensia)
            $allowed_mimes = [
                'pdf'  => ['application/pdf'],
                'jpg'  => ['image/jpeg'],
                'jpeg' => ['image/jpeg'],
                'png'  => ['image/png'],
                'doc'  => ['application/msword'],
                'docx' => ['application/vnd.openxmlformats-officedocument.wordprocessingml.document', 'application/zip'],
                'ppt'  => ['application/vnd.ms-powerpoint'],
                'pptx' => ['application/vnd.openxmlformats-officedocument.presentationml.presentation', 'application/zip'],
            ];
            $finfo       = finfo_open(FILEINFO_MIME_TYPE);
            $actual_mime = finfo_file($finfo, $_FILES['specialNeedFiles']['tmp_name']);
            finfo_close($finfo);
            if (!isset($allowed_mimes[$file_extension]) || !in_array($actual_mime, $allowed_mimes[$file_extension])) {
                die("Continutul fisierului nu corespunde tipului declarat.");
            }

            $target_dir = __DIR__ . '/uploads/';
            if (!is_dir($target_dir)) {
                mkdir($target_dir, 0755, true);
            }

            // se verifica dimensiunea fisierului (max 5MB)
            if ($_FILES['specialNeedFiles']['size'] > 5000000) {
                die("Fisierul este prea mare! Dimensiunea maxima permisa este 5MB.");
            }

            // se genereaza un nume unic pentru fisier
            $file_extension = pathinfo($_FILES['specialNeedFiles']['name'], PATHINFO_EXTENSION);
            $new_file_name  = "doc_" . $booking_id . "_" . time() . "." . $file_extension;
            $target_file    = $target_dir . $new_file_name;

            if (move_uploaded_file($_FILES['specialNeedFiles']['tmp_name'], $target_file)) {
                $upd = $pdo->prepare('UPDATE Bookings SET document = ? WHERE id = ?');
                $upd->execute([$new_file_name, $booking_id]);
            } else {
                echo "A aparut o eroare la mutarea fisierului pe server.";
            }
        }

        header("Location: my_bookings.php?success=1");

        //se sterge token-ul CSRF dupa utilizare pentru a preveni atacurile de tip replay
        unset($_SESSION['csrf_token']);

        exit();
    }

} catch (PDOException $e) {
    echo "A aparut o eroare. Va rugam incercati din nou mai tarziu.";
}
?>