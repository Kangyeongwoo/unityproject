<?php
ini_set('display_errors', 'On');

include_once ('./dbconfig.php');
$mysqli = new mysqli($DB['host'], $DB['id'], $DB['pw'], $DB['db']);
if (mysqli_connect_error()) {
    exit('Connect Error (' . mysqli_connect_errno() . ') '. mysqli_connect_error());
}

$playergunid= $_POST['playergunid'];

if($playergunid!=0){
$q = "SELECT * FROM itemdata WHERE itemid='$playergunid'";
$result = $mysqli->query($q);
$myfictionlist = $result->fetch_array();
$playergun = $myfictionlist["atkspeed"];
}else{
  $playergun = 0.8;
}

echo "{\"playeratkspeed\":\"".$playergun." }";



?>
