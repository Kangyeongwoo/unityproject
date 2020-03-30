<?php
ini_set('display_errors', 'On');


include_once ('./dbconfig.php');
$mysqli = new mysqli($DB['host'], $DB['id'], $DB['pw'], $DB['db']);

mysqli_set_charset($mysqli,"utf8");

if (mysqli_connect_error()) {
    exit('Connect Error (' . mysqli_connect_errno() . ') '. mysqli_connect_error());
}


$id = $_POST['id'];
$pw = $_POST['pw'];
$userindex = $_POST['userindex'];
$itemid = $_POST['itemid'];
$itemtype =$_POST['itemtype'];
$itemcount =$_POST['itemcount'];

$q = "SELECT * FROM userdata WHERE id='$id'";
$result = $mysqli->query( $q);

if($result->num_rows==1){

  $row = $result->fetch_array(MYSQLI_ASSOC);
  if($row['pw']==$pw && $row['userindex']==$userindex){

    $q2 = "INSERT INTO useritem (userid,itemtype,itemid,itemcount,userindex) VALUES ('$id','$itemtype','$itemid','$itemcount','$userindex')";
    $mysqli->query( $q2);


   }



}



?>
