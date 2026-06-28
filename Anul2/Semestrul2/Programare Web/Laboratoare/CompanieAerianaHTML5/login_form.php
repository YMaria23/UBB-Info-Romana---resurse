<?php
session_start();
if (empty($_SESSION['csrf_token'])) {
    $_SESSION['csrf_token'] = bin2hex(random_bytes(32));
}
?>
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Login Form</title>
  <link rel="stylesheet" href="officeStyle.css">
</head>
<body class="booking-page login-page">
  <main class="login-card">
    <h1>Welcome Back</h1>
    <p class="subtitle">Sign in to your account</p>

    <form action="process_login.php" method="post">
      <div class="form-group">
        <label for="username">Username</label>
        <input type="text" id="username" name="username" placeholder="Enter your username" required>
      </div>

      <div class="form-group">
        <label for="password">Password</label>
        <input type="password" id="password" name="password" placeholder="Enter your password" required>
      </div>

      <div class="row">
        <label class="remember-label">
          <input type="checkbox" name="remember"> Remember me
        </label>
      </div>
     
      <div class="captcha-group">
        <label>Verificare de securitate:</label>
        <img src="captcha.php" alt="Cod CAPTCHA" class="captcha-img">
        <input type="text" name="captcha_input" placeholder="Introdu codul" required>
      </div>
  
      <input type="hidden" name="csrf_token" value="<?= htmlspecialchars($_SESSION['csrf_token']) ?>">
      <button type="submit" class="login-btn">Log In</button>
    </form>
  </main>
</body>
</html>
