<!DOCTYPE html>
<html lang = 'es'>
    <head>
        <meta charset = 'UTF-8'/>
        <title>Inicio</title>
        <link href='styles.css' rel='stylesheet' type='text/css'/>
    </head>
    <body>
        <?php
            include "vistas/includes/header.php";
        ?>
        <div class = 'content'>
            <img id="title" src="images/titulo.png" alt="Ascent to the Uncertain"/>
            <?php
                if ($descarga_juego == true){
                    echo "Bienvenido\a ".$_SESSION['usuario'];
                }
            ?>
            </br><a class='links' href='prueba.zip'>Descarga el juego</a>
        </div>
        <?php
            include "vistas/includes/footer.php";
        ?>
    </body>
</html>