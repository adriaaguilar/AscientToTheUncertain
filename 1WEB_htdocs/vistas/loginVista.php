<!DOCTYPE html>
<html lang = 'es'>
    <head>
        <meta charset = 'UTF-8'/>
        <title>Login</title>
        <link href='styles.css' rel='stylesheet' type='text/css'/>
    </head>
    <body>
        <?php
            include "vistas/includes/header.php";
        ?>
        <div class = 'content'>
            <h1>Login de usuario</h1>
            <form action = 'index.php?opcion=login' method ='POST'>    
                <table>
                    <tr>
                        <th><input type='email' name='correo' placeholder='Correo' required/>
                    </tr>
                    <tr>
                        <th><input type='password' name='password' placeholder='Contraseña' required/>
                    </tr>
                </table>
                <input class='button' type = 'submit' value = 'Entrar'/>
            </form>
            <?php
                if (isset($_SESSION['error']) && $_SESSION['error'] == true){
                    echo "Usuario o contraseña incorrectos.";
                    $_SESSION['error'] = false;
                }
            ?>
        </div>
        <?php
            include "vistas/includes/footer.php";
        ?>
    </body>
</html>