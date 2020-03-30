<?php
ini_set('display_errors', 'On');

include_once ('./dbconfig.php');
$mysqli = new mysqli($DB['host'], $DB['id'], $DB['pw'], $DB['db']);
if (mysqli_connect_error()) {
    exit('Connect Error (' . mysqli_connect_errno() . ') '. mysqli_connect_error());
}

$id = $_POST['id'];
$pw = $_POST['pw'];
$userindex = $_POST['userindex'];
$sellitemjson = $_POST['sellitemjson'];
$data_array = json_decode($sellitemjson, true);


$q = "SELECT * FROM userdata WHERE id='$id'";
$result = $mysqli->query( $q);

if($result->num_rows==1){

$row = $result->fetch_array(MYSQLI_ASSOC);

if($row['pw']==$pw && $row['userindex']==$userindex){

if(count($data_array['newskilljs'])!=0){

  $count = count($data_array['newskilljs']);

  for($i=0; $i<$count ; $i++){
    $subdata =  $data_array['newskilljs'][$i];
    $data_array2 = json_decode($subdata, true);
    $skillid = $data_array2['skillid'];

    $q2 = "INSERT INTO useritem (userid,itemtype,itemid,userindex) VALUES ('$id','skill','$skillid','$userindex')";
    $mysqli->query( $q2);

  }


}
if(count($data_array['sellitemjs'])!=0){

  $count = count($data_array['sellitemjs']);
  for($i=0; $i<$count ; $i++){
    $subdata =  $data_array['sellitemjs'][$i];
    $data_array2 = json_decode($subdata, true);
    $useritemindex_it = $data_array2['useritemindex_it'];

    $q3 = "DELETE FROM useritem WHERE useritemindex = '$useritemindex_it'";
    $mysqli->query( $q3);
  }

}
if(count($data_array['sellskilljs'])!=0){
  $count = count($data_array['sellskilljs']);
  for($i=0; $i<$count ; $i++){
    $subdata =  $data_array['sellskilljs'][$i];
    $data_array2 = json_decode($subdata, true);
    $useritemindex_sk = $data_array2['useritemindex_sk'];

    $q4 = "DELETE FROM useritem WHERE useritemindex = '$useritemindex_sk'";
    $mysqli->query( $q4);
  }


}
if(count($data_array['sellskillpartjs'])!=0){

$count = count($data_array['sellskillpartjs']);

  for($i=0; $i<$count ; $i++){
    $subdata =  $data_array['sellskillpartjs'][$i];
    $data_array2 = json_decode($subdata, true);
    $useritemindex_skp = $data_array2['useritemindex_skp'];
    $useritemcount = $data_array2['useritemcount'];

    $q5 = "SELECT * FROM useritem WHERE useritemindex='$useritemindex_skp'";
    $result5 = $mysqli->query( $q5);
    $myfictionlist5 = $result5->fetch_array();
    $originalcount = $myfictionlist5["itemcount"];

    $resultcount = $originalcount-$useritemcount;
    //  $resultcount = $originalcount-1;
    echo $resultcount;
    
    if(($resultcount)==0){

      $q6 = "DELETE FROM useritem WHERE useritemindex = '$useritemindex_skp'";
      $mysqli->query( $q6);


    }else{

      $q7 = "UPDATE useritem SET itemcount = '$resultcount' WHERE useritemindex='$useritemindex_skp'";
      $mysqli->query( $q7);

    }

  }


}






}

}








?>
