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
$gunid = $_POST['gunid'];
$armorid = $_POST['armorid'];
$skill1id = $_POST['skill1id'];
$skill2id = $_POST['skill2id'];
$totalpower = $_POST['totalpower'];
$totalhp = $_POST['totalhp'];


$q = "SELECT * FROM userdata WHERE id='$id'";
$result = $mysqli->query( $q);

if($result->num_rows==1){

  $row = $result->fetch_array(MYSQLI_ASSOC);
  if($row['pw']==$pw){

    $q2 = "UPDATE userdata SET gunid='$gunid',armorid='$armorid',
    skill1id='$skill1id',skill2id='$skill2id',totalpower='$totalpower',totalhp='$totalhp' WHERE id='$id'";
    $mysqli->query( $q2);

    //gunid -> equiped = 1;
    //armorid -> equiped =1;
    //if(skill1id == skill2id){
    // skill1id -> equiped =3;
     //}else{
     // skill1id =1; skill2id=2;
     // }
    $zero = 0;
    $one =1;
    $two =2;
    $three =3;

    $q3 = "UPDATE useritem SET equiped='$zero' WHERE userindex='$userindex' AND equiped != '$zero'";
    $mysqli->query( $q3);

    if($gunid != 0){

      $q4 = "UPDATE useritem SET equiped='$one' WHERE userindex='$userindex' AND itemtype = 'gun'AND itemid = '$gunid'";
      $mysqli->query( $q4);

    }if($armorid != 0){

      $q4 = "UPDATE useritem SET equiped='$one' WHERE userindex='$userindex' AND itemtype = 'armor'AND itemid = '$armorid'";
      $mysqli->query( $q4);

    }if($skill1id !=0){
      /*
      if($skill1id != $skill2id){

        $q4 = "UPDATE useritem SET equiped='$one' WHERE userindex='$userindex' AND itemtype = 'skill'AND itemid = '$skill1id'";
        $mysqli->query( $q4);

      }else{

        $q4 = "UPDATE useritem SET equiped='$three' WHERE userindex='$userindex' AND itemtype = 'skill'AND itemid = '$skill1id'";
        $mysqli->query( $q4);

      }
      */
      $q4 = "UPDATE useritem SET equiped='$one' WHERE userindex='$userindex' AND itemtype = 'skill'AND itemid = '$skill1id'";
      $mysqli->query( $q4);

    }if($skill2id !=0){
      /*
      if($skill1id != $skill2id){

        $q4 = "UPDATE useritem SET equiped='$two' WHERE userindex='$userindex' AND itemtype = 'skill'AND itemid = '$skill2id'";
        $mysqli->query( $q4);

      }else{

        $q4 = "UPDATE useritem SET equiped='$three' WHERE userindex='$userindex' AND itemtype = 'skill'AND itemid = '$skill2id'";
        $mysqli->query( $q4);

      }
      */
      $q4 = "UPDATE useritem SET equiped='$one' WHERE userindex='$userindex' AND itemtype = 'skill'AND itemid = '$skill2id'";
      $mysqli->query( $q4);

    }

  }else{
    //비밀번호가 다릅니다.
    echo "no_pw";
  }


}else{
   echo "no_id";
  //아이디 없음
}




?>
