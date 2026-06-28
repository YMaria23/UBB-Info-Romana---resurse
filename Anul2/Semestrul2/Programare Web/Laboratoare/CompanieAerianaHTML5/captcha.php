<?php
session_start();

// se genereaza un text random pentru CAPTCHA
$text_captcha = substr(str_shuffle("ABCDEFGHJKLMNPQRSTUVWXYZ23456789"), 0, 6);
$_SESSION['captcha_phrase'] = $text_captcha;

// se creaza imaginea de fundal
$img = imagecreatetruecolor(160, 50);
$bg = imagecolorallocate($img, 230, 230, 230);
imagefill($img, 0, 0, $bg);

$font = __DIR__ . '/fonts/Chokokutai-Regular.ttf';

// se deseneaza textul CAPTCHA pe imagine
$black = imagecolorallocate($img, 0, 0, 0);
if (file_exists($font)) {
    // foloseste font-ul ales (din folderul fonts)
    imagettftext($img, 22, rand(-3, 3), 15, 35, $black, $font, $text_captcha);
} else {
    // daca nu-l gaseste, foloseste font-ul implicit
    imagestring($img, 5, 45, 15, $text_captcha, $black);
}


header('Content-type: image/png');

// desenarea imaginii in browser
imagepng($img);

// eliberarea memoriei
imagedestroy($img);
?>