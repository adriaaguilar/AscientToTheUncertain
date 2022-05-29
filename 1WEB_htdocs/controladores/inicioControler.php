<?php

    if (!isset($_SESSION['usuario'])){
        $descarga_juego = false;
    }else{
        $descarga_juego = true;
    }
    
    include("vistas/inicioVista.php");

?>