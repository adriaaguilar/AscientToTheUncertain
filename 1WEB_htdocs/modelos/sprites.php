<?php

    include 'conexion.php';
    
    $nombre = $_GET['nombre'];

    if(!$con){
        echo "error";
    }else{
        $sql = "SELECT id_sprite FROM sprites_usuario WHERE nombre = '$nombre'";
        $res = mysqli_query($con, $sql);

        if (mysqli_num_rows($res) > 0){
            
            $i = 0;
            while($fila = mysqli_fetch_row($res)){
                $sprites[$i] = $fila[$i];
                $i++;
            }

            for ($i = 0; $i < count($sprites); $i++){
                echo $sprites[$i];
            }
        }else{
            echo "404";
        }
    }
    
    mysqli_close($con);
?>