<?php
// conectarea la sqlite - baza de date principala a aplicatiei
try {
    $pdo = new PDO('sqlite:' . __DIR__ . '/extra_data.sqlite');
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $pdo->exec('PRAGMA foreign_keys = ON');

    // tabelul de utilizatori
    $pdo->exec("CREATE TABLE IF NOT EXISTS Users (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        username TEXT NOT NULL UNIQUE,
        password TEXT NOT NULL,
        role TEXT NOT NULL DEFAULT 'user' CHECK(role IN ('admin', 'user')),
        remember_token TEXT DEFAULT NULL,
        first_name TEXT,
        last_name TEXT,
        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    )");

    // tabelul de rezervari
    $pdo->exec("CREATE TABLE IF NOT EXISTS Bookings (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        flight_number TEXT NOT NULL,
        departure_date DATE NOT NULL,
        arrival_date DATE NOT NULL,
        special_needs INTEGER DEFAULT 0,
        user_id INTEGER,
        document TEXT DEFAULT NULL,
        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
        FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE SET NULL
    )");

    // tabelul de statistici ale site-ului
    $pdo->exec("CREATE TABLE IF NOT EXISTS site_stats (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        page_name TEXT,
        access_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    )");

    $stmt = $pdo->prepare("INSERT INTO site_stats (page_name) VALUES (?)");
    $stmt->execute(['Pagina de Rezervari']);

    $stats = $pdo->query("SELECT COUNT(*) as total FROM site_stats")->fetch(PDO::FETCH_ASSOC);
    $total_vizite = $stats['total'];

} catch (PDOException $e) {
    echo "Eroare SQLite: " . $e->getMessage();
}
?>