<?php
session_start();

$servername = "localhost";
$username = "root";
$dbname = "myDatabase";
$password = "";

$user = $_SESSION['username'] ?? 'Utilizator';

// conexiunea la baza de date
try{
    // se incearca conexiunea
    //$pdo = new PDO("mysql:host=$servername;dbname=$dbname", $username, $password);
    //$pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

    $pdo = new PDO('sqlite:' . __DIR__ . '/extra_data.sqlite');
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $pdo->exec('PRAGMA foreign_keys = ON');

    // se sterge token-ul din baza de date
    if (isset($_SESSION['user_id'])) {
        $update_stmt = $pdo->prepare("UPDATE Users SET remember_token = NULL WHERE id = ?");
        $update_stmt->execute([$_SESSION['user_id']]);
    }

    $_SESSION = [];
    session_destroy();

    // stergerea cookie-ului de "remember me"
    if (isset($_COOKIE['remember_me'])) {
        setcookie("remember_me", "", time() - 3600, "/");
    }

    header('Location: index.php');
    exit();
    
    }catch(PDOException $e){
    echo "Connection failed: " . $e->getMessage();
}
?>
