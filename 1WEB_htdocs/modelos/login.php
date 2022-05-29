<?php

    include 'conexion.php';
    
    if($_POST){
        $correo = $_POST['correo'];
        $password = $_POST['password'];
    }else{
        $correo = $_GET['correo'];
        $password = $_GET['password'];
    }

    if(!$con){
        $_SESSION['error'] = true;
        header('Location: index.php');
    }else{
        $sql = "SELECT nombre FROM usuarios WHERE correo = '$correo' AND password = '$password'";
        $res = mysqli_query($con, $sql);

        if (mysqli_num_rows($res) > 0){
            while($fila = mysqli_fetch_row($res)){
                $nombre = $fila[0];
            }

            if($_POST){
                $_SESSION['error'] = false;
                $_SESSION['usuario'] = $nombre;
                header('Location: index.php');
            }else{
                echo $nombre;
            }
        }else{
            if ($_POST){
                $_SESSION['error'] = true;
                header('Location: index.php?opcion=login');
            }else{
                echo "404";
            }
        }
    }
    
    mysqli_close($con);
?>