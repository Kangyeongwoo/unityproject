<?php
ini_set('display_errors', 'On');


include_once ('./dbconfig.php');
$mysqli = new mysqli($DB['host'], $DB['id'], $DB['pw'], $DB['db']);

mysqli_set_charset($mysqli,"utf8");

if (mysqli_connect_error()) {
    exit('Connect Error (' . mysqli_connect_errno() . ') '. mysqli_connect_error());
}

$monster1id = $_POST['monster1id'];
$monster2id = $_POST['monster2id'];
$monster3id = $_POST['monster3id'];

$mapmonsterData = array();
$monsterData = array();

$q1 = "SELECT * FROM monsterdata WHERE monsterid = '$monster1id'";
$result1 = $mysqli->query( $q1);
$myfictionlist1 = $result1->fetch_array();
$monster1=array("monsterid" => $myfictionlist1["monsterid"] , "monstername"=>$myfictionlist1["monstername"], "monsterpower"=>$myfictionlist1["monsterpower"]
, "monstermaxhp"=>$myfictionlist1["monstermaxhp"], "monstergold"=>$myfictionlist1["monstergold"]);
array_push($monsterData, $monster1);


$q2 = "SELECT * FROM monsterdata WHERE monsterid = '$monster2id'";
$result2 = $mysqli->query( $q2);
$myfictionlist2 = $result2->fetch_array();
$monster2=array("monsterid" => $myfictionlist2["monsterid"] , "monstername"=>$myfictionlist2["monstername"], "monsterpower"=>$myfictionlist2["monsterpower"]
, "monstermaxhp"=>$myfictionlist2["monstermaxhp"], "monstergold"=>$myfictionlist2["monstergold"]);
array_push($monsterData, $monster2);

$q3 = "SELECT * FROM monsterdata WHERE monsterid = '$monster3id'";
$result3 = $mysqli->query( $q3);
$myfictionlist3 = $result3->fetch_array();
$monster3=array("monsterid" => $myfictionlist3["monsterid"] , "monstername"=>$myfictionlist3["monstername"], "monsterpower"=>$myfictionlist3["monsterpower"]
, "monstermaxhp"=>$myfictionlist3["monstermaxhp"], "monstergold"=>$myfictionlist3["monstergold"]);
array_push($monsterData, $monster3);


$mapmonsterData["mapmonsterData"] = $monsterData;

$json = json_encode($mapmonsterData,JSON_UNESCAPED_UNICODE);

echo $json;


?>
