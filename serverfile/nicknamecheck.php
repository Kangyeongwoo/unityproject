<?php
ini_set('display_errors', 'On');

include_once ('./dbconfig.php');
$mysqli = new mysqli($DB['host'], $DB['id'], $DB['pw'], $DB['db']);
if (mysqli_connect_error()) {
    exit('Connect Error (' . mysqli_connect_errno() . ') '. mysqli_connect_error());
}


$nickname = $_POST['nickname'];


$q = "SELECT * FROM userdata WHERE nickname='$nickname'";
$result = $mysqli->query( $q);

if($result->num_rows==1){
  echo "no";
}else{
  echo "ok";
}







?>
