<!DOCTYPE html>
<html lang = 'es'>
    <head>
        <meta charset = 'UTF-8'/>
        <title>Registro</title>
        <link href='styles.css' rel='stylesheet' type='text/css'/>
    </head>
    <body>
        <?php
            include "vistas/includes/header.php";
        ?>
        <div class = 'content'>
            <h1>Registro de usuario</h1>
            <form action = 'index.php?opcion=registro' method ='POST'>    
                <table>
                    <tr>
                        <th><input type='email' name='correo' placeholder='Correo' required/>
                    </tr>
                    <tr>
                        <th><input type='password' name='password' placeholder='Contraseña' required/>
                    </tr>
                    <tr>
                        <th><input type='text' name='nombre' placeholder='Nombre de jugador' required/>
                    </tr>
                </table>
                <input class='button' type = 'submit' value = 'Registrarme'/>
            </form>
            <?php
                if (isset($_SESSION['error']) && $_SESSION['error'] == true){
                    echo "Usuario ya existente, debe utiizar un correo y nombre de jugador únicos.";
                    $_SESSION['error'] = false;
                }
            ?>
        </div>
        <?php
            include "vistas/includes/footer.php";
        ?>
    </body>
</html>