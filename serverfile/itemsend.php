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
$useritemjson = $_POST['itemjson'];
$gold = $_POST['gold'];
$maxchapter = $_POST['maxchapter'];
$data_array = json_decode($useritemjson, true);

$q = "SELECT * FROM userdata WHERE id='$id'";
$result = $mysqli->query( $q);

if($result->num_rows==1){

  $row = $result->fetch_array(MYSQLI_ASSOC);
  if($row['pw']==$pw && $row['userindex']==$userindex){

    //얻은 돈이 있으면 업데이트
    if($gold!=0){

     $totalgold = $row['gold'] + $gold;
     $q8 = "UPDATE userdata SET gold = '$totalgold' WHERE id='$id'";
     $mysqli->query( $q8);

    }

    //기존 최대 챕터 보다 크면 업데이트
    if($row['maxchapter']<$maxchapter){

      $q9 = "UPDATE userdata SET maxchapter = '$maxchapter' WHERE id='$id'";
      $mysqli->query( $q9);
    }


    if(count($data_array['passivearray'])!=0){
      $count = count($data_array['passivearray']);
      for($i=0; $i<$count ; $i++){
        $test =  $data_array['passivearray'][$i];
        $data_array2 = json_decode($test, true);
       $itemtype = $data_array2['itemtype'];
       $itemid = $data_array2['itemid'];
       $itemcount =$data_array2['itemcount'];

       $q4 = "SELECT * FROM useritem WHERE userindex ='$userindex' AND itemid='$itemid' AND itemtype='$itemtype'";
       $result2 = $mysqli->query( $q4);
       if($result2->num_rows==1){
         //파편을 가지고 있을 때
         $row2= $result2->fetch_array(MYSQLI_ASSOC);
         $totalcount = $row2['itemcount']+$itemcount;
         if($totalcount >= 10){
           $totalcoutn = 10;
         }
         $q6 = "UPDATE useritem SET itemcount = '$totalcount' WHERE userindex ='$userindex' AND itemid='$itemid' AND itemtype='$itemtype'";
         $mysqli->query( $q6);

       }else{
         //파편이 없으면 스킬이 있는지 없는지 확인
         $q5 = "SELECT * FROM useritem WHERE userindex ='$userindex' AND itemid='$itemid' AND itemtype='skill'";
         $result3 = $mysqli->query( $q5);
         if($result3->num_rows==1){
           //스킬을 가지고 있을 경우
           //수정예정
           $q2 = "INSERT INTO useritem (userid,itemtype,itemid,itemcount,userindex) VALUES ('$id','$itemtype','$itemid','$itemcount','$userindex')";
           $mysqli->query( $q2);
         }else{
           //스킬이 없을 경우
           $q2 = "INSERT INTO useritem (userid,itemtype,itemid,itemcount,userindex) VALUES ('$id','$itemtype','$itemid','$itemcount','$userindex')";
           $mysqli->query( $q2);
         }

       }

     }
   }
   if(count($data_array['activearray'])!=0){
     $count = count($data_array['activearray']);
     for($i=0; $i<$count ; $i++){
       $test = $data_array['activearray'][$i];
       $data_array2 = json_decode($test, true);
       $itemtype = $data_array2['itemtype'];
       $itemid = $data_array2['itemid'];
       $itemcount = $data_array2['itemcount'];


       $q4 = "SELECT * FROM useritem WHERE userindex ='$userindex' AND itemid='$itemid' AND itemtype='$itemtype'";
       $result2 = $mysqli->query( $q4);
       if($result2->num_rows==1){
         //파편을 가지고 있을 때
         $row2= $result2->fetch_array(MYSQLI_ASSOC);
         $totalcount = $row2['itemcount']+$itemcount;
         if($totalcount >= 10){
           $totalcoutn = 10;
         }
         $q6 = "UPDATE useritem SET itemcount = '$totalcount' WHERE userindex ='$userindex' AND itemid='$itemid' AND itemtype='$itemtype'";
         $mysqli->query( $q6);
       }else{
         //파편이 없으면 스킬이 있는지 없는지 확인
         $q5 = "SELECT * FROM useritem WHERE userindex ='$userindex' AND itemid='$itemid' AND itemtype='skill'";
         $result3 = $mysqli->query( $q5);
         if($result3->num_rows==1){
           //스킬을 가지고 있을 경우
         }else{
           //스킬이 없을 경우
           $q2 = "INSERT INTO useritem (userid,itemtype,itemid,itemcount,userindex) VALUES ('$id','$itemtype','$itemid','$itemcount','$userindex')";
           $mysqli->query( $q2);
         }

       }
    }
  }

  }


echo "ok";

}







?>
