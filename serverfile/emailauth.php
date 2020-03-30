<?php
ini_set('display_errors', 'On');

include_once ('./dbconfig.php');
$mysqli = new mysqli($DB['host'], $DB['id'], $DB['pw'], $DB['db']);
if (mysqli_connect_error()) {
    exit('Connect Error (' . mysqli_connect_errno() . ') '. mysqli_connect_error());
}

$authkey = $_GET["code"];


if(empty($authkey)){

  echo "잘못된 접근입니다.";

}else{

  $q="SELECT * FROM temp_userid WHERE authtoken='$authkey'";

  $result = $mysqli->query( $q);
  if($result->num_rows > 0){

    $templist =$result->fetch_array();

    date_default_timezone_set('Asia/Seoul');
    $curdatetime = date("Y-m-d H:i:s");
    $authdatetim = $templist['authtime'];
    $r = strtotime($curdatetime) - strtotime($authdatetim) ;

    if( ceil($r / 60 ) <= "60"){
    // 1시간 안됬
        if($templist['emailcheck']=='0'){
          //1로 바꿔준다
          $q4 = "UPDATE temp_userid SET emailcheck='1' WHERE authtoken='$authkey'";
          $mysqli->query( $q4); ?>
          <h1 style="font-size:100px;">RobotWar 이메일 인증</h1>
          <h4 style="font-size:70px;">인증이 완료되었습니다. 가입을 완료해주세요</h4>
          <input type="button" style="width:200px;height:150px;font-size:30px;" name="" value="닫기" onclick="javascript();">
          <script language="javascript">
          //버튼클릭시 javascript 호출합니다.
          function javascript(){
              self.close();   //자기자신창을 닫습니다.
          }
          </script>


        <?php
        }else if($templist['emailcheck']=='1'){

          ?>

          <h1 style="font-size:100px;">RobotWar 이메일 인증</h1>
          <h4 style="font-size:70px;">이미 인증하셨습니다.</h4>
          <input type="button" style="width:200px;height:150px;font-size:30px;" name="" value="닫기" onclick="javascript();">
          <script language="javascript">
          //버튼클릭시 javascript 호출합니다.
          function javascript(){
              self.close();   //자기자신창을 닫습니다.
          }
          </script>
      <?php  }

     }else{
     //1시간 이상 지
       $q4 = "DELETE FROM temp_userid WHERE authtoken='$authkey'";
       $mysqli->query( $q4);
       ?>
       <h1 style="font-size:100px;">RobotWar 이메일 인증</h1>
       <h4 style="font-size:70px;">만료된 인증번호 입니다.</h4>
       <input type="button" style="width:200px;height:150px;font-size:30px;" name="" value="닫기" onclick="javascript();">
       <script language="javascript">
       //버튼클릭시 javascript 호출합니다.
       function javascript(){
           self.close();   //자기자신창을 닫습니다.
       }
       </script>
    <?php }

  }else{
    ?>
    <h1 style="font-size:100px;">RobotWar 이메일 인증</h1>
    <h4 style="font-size:70px;">만료된 인증번호 입니다.</h4>
    <input type="button" style="width:200px;height:150px;font-size:30px;" name="" value="닫기" onclick="javascript();">
    <script language="javascript">
    //버튼클릭시 javascript 호출합니다.
    function javascript(){
        self.close();   //자기자신창을 닫습니다.
    }
    </script>
<?php
  }


}



?>
