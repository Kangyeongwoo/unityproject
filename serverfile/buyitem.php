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
$useritemjson = $_POST['buyitemjson'];
$gold = $_POST['gold'];
$data_array = json_decode($useritemjson, true);


$q = "SELECT * FROM userdata WHERE id='$id'";
$result = $mysqli->query( $q);

if($result->num_rows==1){

  $row = $result->fetch_array(MYSQLI_ASSOC);
  if($row['pw']==$pw && $row['userindex']==$userindex){

    $totalgold = $gold;
    $q8 = "UPDATE userdata SET gold = '$totalgold' WHERE id='$id'";
    $mysqli->query( $q8);

      $count = count($data_array['buylist_js']);
      for($i=0; $i<$count ; $i++){

        $buyitem =  $data_array['buylist_js'][$i];
        $data_array2 = json_decode($buyitem, true);
        $itemtype = $data_array2['itemtype'];
        $itemid = $data_array2['itemid'];

        $q2 = "INSERT INTO useritem (userid,itemtype,itemid,userindex) VALUES ('$id','$itemtype','$itemid','$userindex')";
        $mysqli->query( $q2);



      }




  }


echo "ok";

}





?>
