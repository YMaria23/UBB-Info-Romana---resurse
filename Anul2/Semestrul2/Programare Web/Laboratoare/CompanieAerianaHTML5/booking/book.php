<?php require_once 'book_logic.php'; 
require_once '../extra_db.php'; 
?>
<!DOCTYPE html>
<html lang="ro">
    <head>
        <meta charset="UTF-8">
        <title>Book a Flight</title>
        <link rel="stylesheet" href="../officeStyle.css">
        <link rel="stylesheet" href="bookingStyle.css">
    </head>
    <body class="booking-page">
        <nav class="top-menu" aria-label="Navigare principală">
            <ul>
                <li><a href="../index.php" class="menu-home">Acasă</a></li>
                <li><a href="../information/travelerInfo.html" class="menu-services">Servicii</a></li>
                <li><a href="../index.php#destinatii" class="menu-destinations">Destinații</a></li>
                <li><a href="../my_bookings.php" class="menu-booking active">Rezervări</a></li>
                <li><a href="../maze/maze.html" class="menu-games">✈️ AERO Kids</a></li>
            </ul>
        </nav>

        <main class="booking-shell">
            <h1>Rezervă-ți zborul</h1>
            <p class="booking-subtitle">Completează formularul de mai jos și noi ne ocupăm de restul.</p>

            <form action="../upload_document.php" method="POST" enctype="multipart/form-data" id="reservationForm" target="_self" class="booking-form">
                <fieldset>
                    <legend>Informații personale</legend>

                    <div class="form-row">
                        <label for="name">Nume</label>
                        <input type="text" id="name" name="name" maxlength="30"
                               value="<?= htmlspecialchars($user_last_name) ?>">
                    </div>

                    <div class="form-row">
                        <label for="firstName">Prenume</label>
                        <input type="text" id="firstName" name="firstName"
                               value="<?= htmlspecialchars($user_first_name) ?>">
                    </div>

                    <div class="form-row">
                        <label for="dateOfBirth">Data nașterii</label>
                        <input type="date" id="dateOfBirth" name="dateOfBirth" min="1920-01-01" max="2008-12-31">
                    </div>

                    <div class="form-row">
                        <label for="age">Vârstă</label>
                        <input type="text" id="age" name="age">
                    </div>

                    <div class="form-row">
                        <label for="email">Email</label>
                        <input type="email" id="email" name="email">
                    </div>

                    <div class="form-row">
                        <label for="phone">Număr de telefon</label>
                        <input type="tel" id="phone" name="phone" pattern="[0-9]{10}" placeholder="Ex: 0712345678">
                    </div>
                </fieldset>

                <fieldset>
                    <legend>Informații zbor și nevoi speciale</legend>

                    <div class="form-row">
                        <label for="flightNr">Număr zbor</label>
                        <input type="text" id="flightNr" name="flightNr" value="AERO101">
                    </div>

                    <div class="form-row">
                        <label for="departure">Plecare</label>
                        <input type="text" id="departure" name="departure" value="Bucuresti" disabled>
                    </div>

                    <div class="form-row">
                        <label for="arrival">Destinație</label>
                        <input type="text" id="arrival" name="arrival" value="Cluj-Napoca" disabled>
                    </div>

                    <div class="form-row">
                        <label for="date">Data</label>
                        <input type="date" id="date" name="date" min="2026-03-09" max="2027-05-21" required>
                    </div>

                    <div class="form-row class-row">
                        <span>Clasa</span>
                        <div class="class-options">
                            <label><input type="radio" id="economy" name="class" value="economy" checked> Economy</label>
                            <label><input type="radio" id="business" name="class" value="business"> Business</label>
                            <label><input type="radio" id="first" name="class" value="first"> First</label>
                        </div>
                    </div>

                    <div class="form-row">
                        <label for="baggage">Tip bagaj</label>
                        <select id="baggage" name="baggage">
                            <option value="smallCarryOn" selected>Bagaj de mână mic</option>
                            <option value="largeCarryOn">Bagaj de mână mare</option>
                            <option value="checked">Bagaj de cală</option>
                        </select>
                    </div>

                    <div class="form-row full-row">
                        <label for="specialNeeds">Nevoi speciale</label>
                        <textarea id="specialNeeds" name="specialNeeds" rows="6" placeholder="Dacă aveți nevoi speciale, vă rugăm să le menționați aici."></textarea>
                    </div>

                    <div class="form-row full-row">
                        <label for="specialNeedFiles">Documente justificative</label>
                        <input type="file" id="specialNeedFiles" name="specialNeedFiles">
                    </div>

                    <div class="form-row full-row">
                        <label for="rating">Cât de ușor de completat a fost formularul? (numere impare 1-9)</label>
                        <input type="number" id="rating" name="rating" step="2" min="1" max="9">
                    </div>

                    <div class="full-row">
                        <input type="hidden" name="csrf_token" value="<?= htmlspecialchars($_SESSION['csrf_token']) ?>">
                        <input type="submit" id="bookingSubmit" value="Rezervă">
                    </div>
                </fieldset>
            </form>
        </main>

       <!-- <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
        <script src="book.js"></script> -->
    </body>
</html>
