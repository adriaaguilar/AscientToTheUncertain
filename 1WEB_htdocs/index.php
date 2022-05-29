<?php

    session_start();

    if (!isset($_GET['opcion'])){
        $opcion = 'inicio';
    }else{
        $opcion = $_GET['opcion'];
    }

    switch($opcion){
        case "inicio":
            include("controladores/inicioControler.php");
            default;
            break;

        case "login":
            include("controladores/loginControler.php");
            break;

        case "registro":
            include("controladores/registroControler.php");
            break;

        case "salir":
            include("controladores/salirControler.php");
            break;
    }

?>