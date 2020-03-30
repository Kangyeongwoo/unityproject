<?php
ini_set('display_errors', 'On');

include_once ('./dbconfig.php');
$mysqli = new mysqli($DB['host'], $DB['id'], $DB['pw'], $DB['db']);
if (mysqli_connect_error()) {
    exit('Connect Error (' . mysqli_connect_errno() . ') '. mysqli_connect_error());
}


$id = $_POST['id'];
$pw = $_POST['pw'];

$q = "SELECT * FROM userdata WHERE id='$id'";
$result = $mysqli->query( $q);

if($result->num_rows==1){

  $row = $result->fetch_array(MYSQLI_ASSOC);
  if($row['pw']==$pw){

    if(empty($row['nickname'])){
      echo "no_nick";
    }else{
      $q2 = "SELECT * FROM userdata WHERE id='$id'";
      $result = $mysqli->query( $q2);
                $userdata =$result->fetch_array();
                $userindex = $userdata["userindex"];
                $nickname = $userdata["nickname"];
                $level = $userdata["level"];
                $gold = $userdata["gold"];
                $wincount = $userdata["wincount"];
                $losecount = $userdata["losecount"];
                $power = $userdata["power"];
                $hp = $userdata["hp"];
                $gunid = $userdata["gunid"];
                $armorid = $userdata["armorid"];
                $skill1id = $userdata["skill1id"];
                $skill2id = $userdata["skill2id"];
                $drawcount = $userdata["drawcount"];
                $totalpower =$userdata["totalpower"];
                $totalhp =$userdata["totalhp"];
                $maxchapter = $userdata["maxchapter"];
                $maxstage = $userdata["maxstage"];

      echo "{\"userindex\":\"".$userindex."\",\"nickname\":\"".$nickname."\",\"level\":\"".$level."\",\"gold\":\"".$gold."\",
        \"wincount\":\"".$wincount."\",\"losecount\":\"".$losecount."\",\"power\":\"".$power."\",\"hp\":\"".$hp."\",
        \"gunid\":\"".$gunid."\",\"armorid\":\"".$armorid."\",\"skill1id\":\"".$skill1id."\",\"skill2id\":\"".$skill2id."\",
        \"drawcount\":\"".$drawcount."\",\"totalpower\":\"".$totalpower."\",\"totalhp\":\"".$totalhp."\",\"maxchapter\":\"".$maxchapter."\",
       \"maxstage\":\"".$maxstage."\"}";
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
