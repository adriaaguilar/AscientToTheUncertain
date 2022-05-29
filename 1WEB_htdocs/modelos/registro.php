<?php

    include 'conexion.php';
    
    $correo = $_POST['correo'];
    $password = $_POST['password'];
    $nombre = $_POST['nombre'];

    if(!$con){
        $_SESSION['error'] = true;
        header('Location: index.php');
    }else{
        $sql = "SELECT * FROM usuarios WHERE correo = '$correo' OR nombre = '$nombre'";
        $res = mysqli_query($con, $sql);

        if (mysqli_num_rows($res) == 0){
            
            $sql = "INSERT INTO usuarios (correo, password, nombre) VALUES ('$correo', '$password', '$nombre')";
            $res = mysqli_query($con, $sql);

            if (!$res){
                $_SESSION['error'] = true;
                header('Location: index.php');
            }else{
                $_SESSION['error'] = false;
                $_SESSION['usuario'] = $nombre;
                header('Location: index.php');
            }
        }else{
            $_SESSION['error'] = true;
            header('Location: index.php?opcion=registro');
        }
    }
    
    mysqli_close($con);
?>