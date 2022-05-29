<div id = 'header'>
    <a id = 'logo' href='index.php?opcion=inicio'><img src="images/espada.png" alt="Ascent to the Uncertain"/>
    <div id = 'menu'>
    <?php

        if ($descarga_juego == true){
            echo "<a class='links' href='index.php?opcion=salir'>Cerrar session</a>";
        }else{
            echo "<a class='links' href='index.php?opcion=login'>Login</a><br/>";
            echo "<a class='links' href='index.php?opcion=registro'>Registro</a>";
        }
        
    ?>
    </div>
</div>