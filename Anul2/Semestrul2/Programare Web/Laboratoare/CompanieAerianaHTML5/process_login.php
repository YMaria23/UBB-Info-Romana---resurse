<?php

// pentru managementul sesiunii
session_start();

// conexiunea la baza de date
try{
    // se incearca conexiunea
    $pdo = new PDO('sqlite:' . __DIR__ . '/extra_data.sqlite');
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

    if($_SERVER["REQUEST_METHOD"] == "POST"){

        // verificarea token-ului CSRF
        if (empty($_POST['csrf_token']) || empty($_SESSION['csrf_token'])
            || !hash_equals($_SESSION['csrf_token'], $_POST['csrf_token'])) {
            http_response_code(403);
            die("Eroare de securitate: Token-ul CSRF este invalid!");
        }
        unset($_SESSION['csrf_token']);
        $input_username = $_POST["username"];
        $input_password = $_POST["password"];
        $input_remember = $_POST["remember"] ?? false;

        $user_captcha = $_POST['captcha_input'] ?? '';
        $session_captcha = $_SESSION['captcha_phrase'] ?? '';

        // se verifica CAPTCHA
        if ($user_captcha !== $session_captcha || empty($session_captcha)) {
            echo "CAPTCHA invalid.";
            exit();
        }

        // se sterge CATCHA-ul curent
        unset($_SESSION['captcha_phrase']);

        // se cauta user-ul in baza de date
        $statement = $pdo->prepare("SELECT id,username,password,role FROM Users WHERE username = ?");
        $statement->execute([$input_username]);
        $user = $statement->fetch(PDO::FETCH_ASSOC);

        // se verifica daca user-ul exista si daca parola este corecta
        if($user && password_verify($input_password, $user["password"])){
            // se salveaza informatiile in sesiune
            $_SESSION["user_id"] = $user["id"];
            $_SESSION["username"] = $user["username"];
            $_SESSION["role"] = $user["role"];

            // functionalitatea de "remember me"
             if ($input_remember) {
                $token = bin2hex(random_bytes(16));
                setcookie("remember_me", $token, time() + (7*24*60*60), "/");

                // se salveaza token-ul si in baza de date
                $update_stmt = $pdo->prepare("UPDATE Users SET remember_token = ? WHERE username = ?");
                $update_stmt->execute([$token, $user["username"]]);
            }

            // redirectionare catre dashboard sau pagina principala
            header("Location: my_bookings.php");
            exit();
        } else {
            // autentificare esuata
            echo "Invalid username or password.";
        }
    }


}catch(PDOException $e){
    echo "A aparut o eroare. Va rugam incercati din nou mai tarziu.";
}