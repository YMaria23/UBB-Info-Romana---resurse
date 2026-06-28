<?php
session_start();

$user_last_name  = '';
$user_first_name = '';

if (empty($_SESSION['csrf_token'])) {
    // se genereaza un token CSRF nou pentru formularul de rezervare
    $_SESSION['csrf_token'] = bin2hex(random_bytes(32));
}

if (isset($_SESSION['user_id'])) {
    try {
        $pdo = new PDO('sqlite:' . dirname(__DIR__) . '/extra_data.sqlite');
        $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

        $stmt = $pdo->prepare('SELECT first_name, last_name FROM Users WHERE id = :id');
        $stmt->execute([':id' => $_SESSION['user_id']]);
        $user = $stmt->fetch(PDO::FETCH_ASSOC);

        if ($user) {
            $user_last_name  = $user['last_name']  ?? '';
            $user_first_name = $user['first_name'] ?? '';
        }
    } catch (PDOException $e) {
        // nu blocam pagina daca fetch-ul esueaza, campurile raman goale
    }
}
?>
