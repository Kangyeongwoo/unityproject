<?php
ini_set('display_errors', 'On');

include_once ('./dbconfig.php');
$mysqli = new mysqli($DB['host'], $DB['id'], $DB['pw'], $DB['db']);
if (mysqli_connect_error()) {
    exit('Connect Error (' . mysqli_connect_errno() . ') '. mysqli_connect_error());
}

$playergunid= $_POST['playergunid'];
$playerskill1id = $_POST['playerskill1id'];
$enemygunid = $_POST['enemygunid'];
$enemyskill1id = $_POST['enemyskill1id'];

if($playergunid!=0){
$q = "SELECT * FROM itemdata WHERE itemid='$playergunid'";
$result = $mysqli->query($q);
$myfictionlist = $result->fetch_array();
$playergun = $myfictionlist["atkspeed"];
}else{
  $playergun = 0.8;
}

if($enemygunid!=0){
$q2 = "SELECT * FROM itemdata WHERE itemid='$enemygunid'";
$result2 = $mysqli->query($q2);
$myfictionlist2 = $result2->fetch_array();
$enemygun = $myfictionlist2["atkspeed"];
}else{
   $enemygun = 0.8;
}

if($playerskill1id!=0){
$q3 = "SELECT * FROM skilldata WHERE skillid='$playerskill1id'";
$result3 = $mysqli->query($q3);
$myfictionlist3 = $result3->fetch_array();
$playerskill = $myfictionlist3["atk"];
}else{
  $playerskill=0;
}

if($enemyskill1id!=0){
$q4 = "SELECT * FROM skilldata WHERE skillid='$enemyskill1id'";
$result4 = $mysqli->query($q4);
$myfictionlist4 = $result4->fetch_array();
$enemyskill = $myfictionlist4["atk"];
}else{
  $enemyskill=0;
}

echo "{\"playeratkspeed\":\"".$playergun."\",\"enemyatkspeed\":\"".$enemygun."\",\"playerskillatk\":\"".$playerskill."\",\"enemyskillatk\":\"".$enemyskill."\" }";



?>
