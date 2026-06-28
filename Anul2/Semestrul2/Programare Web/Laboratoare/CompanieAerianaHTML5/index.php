<?php session_start(); ?>
<!DOCTYPE html>
<html lang="ro">
    <head>
        <meta charset="UTF-8">
        <title>AERO</title>

        <link rel="stylesheet" href="officeStyle.css">
    </head>
    <body>
        <nav class="top-menu" aria-label="Navigare principală">
            <ul>
                <li><a href="index.php" class="menu-home active">Acasă</a></li>
                <li><a href="information/travelerInfo.html" class="menu-services">Servicii</a></li>
                <li><a href="index.php#destinatii" class="menu-destinations">Destinații</a></li>
                <!--<li><a href="booking/book.php" class="menu-booking">Rezervări</a></li>-->
                <li><a href="my_bookings.php" class="menu-booking">Rezervări</a></li>
                <li><a href="maze/maze.html" class="menu-games">✈️ AERO Kids</a></li>
                <li class="nav-auth">
                    <?php if (isset($_SESSION['user_id'])): ?>
                        <span class="nav-username"><?= htmlspecialchars($_SESSION['username']) ?></span>
                        <a href="logout.php" class="nav-logout-btn">Logout</a>
                    <?php else: ?>
                        <a href="login_form.php" class="nav-logout-btn">Login</a>
                    <?php endif; ?>
                </li>
            </ul>
        </nav>

        <!--<img src="https://www.publicdomainpictures.net/pictures/310000/velka/airplane-flying-1567518082n0Z.jpg" alt="airplane image" width="1515" height="450">-->

        <section id="carousel-container" class="carousel">
            <button id="prevBtn" class="nav-btn">&lt;</button>
            <button id="nextBtn" class="nav-btn">&gt;</button>

            <div class="carousel-content">
                <a href="#" id="carousel-link" class="carousel-button">Text asociat</a>
            </div>
        </section>
        <h1>Bine ați venit la AERO</h1>
        <div>
            <p title = "About AERO">La <span><strong>AERO</strong></span>, credem că fiecare călătorie trebuie să fie mai mult decât un simplu
                zbor — trebuie să fie o experiență memorabilă. Cu o flotă modernă și o echipă dedicată,
                oferim pasagerilor noștri siguranță, confort și punctualitate la cele mai înalte standarde
                internaționale.</p>
            <p>Ne conectăm destinații importante din întreaga lume, facilitând atât călătoriile de afaceri, cât și vacanțele de neuitat.
                Fiecare detaliu, de la serviciile la bord până la asistența oferită la sol, este gândit pentru a transforma timpul petrecut
                în aer într-o experiență plăcută și relaxantă. <br> <br>
                Misiunea noastră este să aducem oamenii mai aproape, să deschidem noi orizonturi și să redefinim modul în care lumea călătorește.
                Cu <b>AERO</b>, destinația începe din momentul în care urcați la bord.</p>
        </div>
        <h2>We are bringing the world closer to you!</h2>
        <br>
        <h3>De ce să zburați cu noi:</h3> 
        <ul>
            <li>Siguranță și confort la cele mai înalte standarde</li>
            <li>Flotă modernă și echipă dedicată</li>
            <li>Punctualitate și servicii de calitate</li>
        </ul>

        <br>
        <h4 id="servicii">Servicii oferite:</h4>
        <ul>
            <li class="expandabil">Opțiuni de zbor pentru orice buget
                <ul class="sublista hidden">
                    <li>Economy Class</li>
                    <li>Business Class</li>
                    <li>First Class</li>
                </ul>
            </li>
            <li class="expandabil">Servicii la bord
                <ul class="sublista hidden">
                    <li>Mese și băuturi</li>
                    <li>Divertisment în zbor (exclusiv zborurilor charter)</li>
                    <li>Wi-Fi</li>
                </ul>
            </li>
            <li class="expandabil">Servicii aeroport
                <ul class="sublista hidden">
                    <li>Check-in online</li>
                    <li>Prioritate la îmbarcare</li>
                    <li>Lounge-uri exclusive (destinate pasagerilor Business și First Class)</li>
                </ul>
            </li>
            <li class="expandabil">Servicii suplimentare
                <ul class="sublista hidden">
                    <li>Alegerea locului</li>
                    <li>Asigurare de călătorie</li>
                    <li>Posibilitatea anulării rezervării și recuperarea parțială a sumei achitate</li>
                </ul>
            </li>
        </ul>

        <br>
        <h5 id="destinatii">Destinații populare</h5>
        <h6>Top 5 destinații preferate de pasagerii noștri:</h6>
        <ol start="1">
            <li>New York</li>
            <li>Paris</li>
            <li>Tokyo</li>
            <li>Dubai</li>
            <li>Sydney</li>
        </ol>

        <br>
        <div>
            <h1>Zboruri zilnice:</h1>
            <table id="tabelOrizontal">
            </table>

            <br>

            <table id="tabelVertical">
            </table>
        </div>

        <br>

        <button class="complaint-btn"><a href="complaint/complaint.html">Depune o plângere</a></button>
        <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
        <script src="index.js"></script>
    </body>
</html>