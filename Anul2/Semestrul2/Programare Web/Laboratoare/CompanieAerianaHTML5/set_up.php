<?php
// inserarea datelor initiale in baza de date SQLite
try {
    $pdo = new PDO('sqlite:' . __DIR__ . '/extra_data.sqlite');
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $pdo->exec('PRAGMA foreign_keys = ON');

    // inserarea datelor initiale pentru tabelul Users
    $utilizatori_test = [
        ['johndoe', '1234', 'Doe', 'John', 'user'],
        ['mirabela', 'mirabela', 'Popescu', 'Mirabela', 'user']
    ];

    $stmt = $pdo->prepare(
        "INSERT INTO Users (username, password, last_name, first_name, role) VALUES (?, ?, ?, ?, ?)"
    );

    foreach ($utilizatori_test as $u) {
        $hash = password_hash($u[1], PASSWORD_DEFAULT);
        try {
            $stmt->execute([$u[0], $hash, $u[2], $u[3], $u[4]]);
            echo "Utilizatorul {$u[0]} a fost creat.<br>";
        } catch (PDOException $e) {
            echo "Eroare la {$u[0]}: " . $e->getMessage() . "<br>";
        }
    }

    // inserarea datelor initiale pentru tabelul Bookings
    $rezervari_test = [
        ['AB123', '2024-07-01', '2024-07-02', 0, 1],
        ['CD456', '2024-08-15', '2024-08-16', 1, 1],
        ['EF789', '2024-09-10', '2024-09-11', 0, 2]
    ];

    $stmt = $pdo->prepare(
        "INSERT INTO Bookings (flight_number, departure_date, arrival_date, special_needs, user_id) VALUES (?, ?, ?, ?, ?)"
    );

    foreach ($rezervari_test as $rez) {
        try {
            $stmt->execute($rez);
            echo "Rezervarea pentru zborul {$rez[0]} a fost creată.<br>";
        } catch (PDOException $e) {
            echo "Eroare la rezervarea pentru zborul {$rez[0]}: " . $e->getMessage() . "<br>";
        }
    }

} catch (PDOException $e) {
    echo "Eroare: " . $e->getMessage();
}
?>