<?php

    if(!$_POST){
        include("vistas/loginVista.php");
    }else{
        include("modelos/login.php");
    }
?>