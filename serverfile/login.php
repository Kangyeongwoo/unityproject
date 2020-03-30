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
    echo "ok";

  }else{
    //비밀번호가 다릅니다.
    echo "no";
  }


}else{
   echo "no";
  //아이디 없음
}



?>
