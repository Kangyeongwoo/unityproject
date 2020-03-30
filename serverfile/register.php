<?php
ini_set('display_errors', 'On');

include_once ('./dbconfig.php');
$mysqli = new mysqli($DB['host'], $DB['id'], $DB['pw'], $DB['db']);
if (mysqli_connect_error()) {
    exit('Connect Error (' . mysqli_connect_errno() . ') '. mysqli_connect_error());
}

$id = $_POST['id'];
$pw = $_POST['pw'];
$logincategory = $_POST['logincategory'];
date_default_timezone_set('Asia/Seoul');
$registertime = date("Y-m-d H:i:s");


if($logincategory=="robotwar"){

  $q = "DELETE FROM temp_userid WHERE tempid='$id'";
  $mysqli->query( $q);

  $q2 = "INSERT INTO userdata (id, pw, logincategory, registertime) VALUES ('$id','$pw','$logincategory','$registertime')";
  $mysqli->query( $q2);

  echo "ok";

}elseif($logincategory=="facebook"){

  $q = "SELECT * FROM userdata WHERE id = $id";
  $result = $mysqli->query( $q);

  if($result->num_rows==1){

    echo "ok";

  }else{
    $q2 = "INSERT INTO userdata (id, pw, logincategory, registertime) VALUES ('$id','$pw','$logincategory','$registertime')";
    $mysqli->query( $q2);

    echo "ok";
  }
}else{

  echo "no";

}






?>
