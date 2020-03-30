<?php
include_once ('./dbconfig.php');
$mysqli = new mysqli($DB['host'], $DB['id'], $DB['pw'], $DB['db']);
if (mysqli_connect_error()) {
    exit('Connect Error (' . mysqli_connect_errno() . ') '. mysqli_connect_error());
}

$id =$_POST['id'];

  $q="SELECT * FROM temp_userid WHERE tempid='$id'";
  $result = $mysqli->query( $q);
  if($result->num_rows > 0){

      $templist =$result->fetch_array();
      if($templist['emailcheck']=='1'){

        echo "yes";

      }else{
        echo "no";
      }

  }else{
    echo "no";
  }

?>
