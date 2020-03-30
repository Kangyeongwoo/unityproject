<?php

include_once ('./dbconfig.php');
$mysqli = new mysqli($DB['host'], $DB['id'], $DB['pw'], $DB['db']);
if (mysqli_connect_error()) {
    exit('Connect Error (' . mysqli_connect_errno() . ') '. mysqli_connect_error());
}
extract($_POST);


use PHPMailer\PHPMailer\PHPMailer;

use PHPMailer\PHPMailer\Exception;

$id =$_POST['id'];

require "./src/PHPMailer.php";

require "./src/SMTP.php";

require "./src/Exception.php";

if(!preg_match("/^[_\.0-9a-zA-Z-]+@([0-9a-zA-Z][0-9a-zA-Z-]+\.)+[a-zA-Z]{2,6}$/i",$id)){
   echo "이메일 형식이 아닙니다.";

}else{

  $q = "SELECT * FROM userdata WHERE id='$id'";
  $result = $mysqli->query( $q);
  if($result->num_rows > 0){

    echo "가입된 이메일 입니다.";

  }else{

    date_default_timezone_set('Asia/Seoul');
    $auth_datetime = date("Y-m-d H:i:s");
    $authkey=$auth_datetime.$id;
    $authkey_sha = hash("sha256", $authkey);


    $mail = new PHPMailer(true);



    try {



    // 서버세팅



    //디버깅 설정을 0 으로 하면 아무런 메시지가 출력되지 않습니다

    $mail -> SMTPDebug = 2; // 디버깅 설정

    $mail -> isSMTP(); // SMTP 사용 설정



    // 지메일일 경우 smtp.gmail.com, 네이버일 경우 smtp.naver.com



    $mail -> Host = "smtp.naver.com";               // 네이버의 smtp 서버

    $mail -> SMTPAuth = true;                         // SMTP 인증을 사용함

    $mail -> Username = "kyw0036@naver.com";    // 메일 계정 (지메일일경우 지메일 계정)

    $mail -> Password = "rkd758dn!";                  // 메일 비밀번호

    $mail -> SMTPSecure = "ssl";                       // SSL을 사용함

    $mail -> Port = 465;                                  // email 보낼때 사용할 포트를 지정



    $mail -> CharSet = "utf-8"; // 문자셋 인코딩



    // 보내는 메일

    $mail -> setFrom("kyw0036@naver.com", "강영우");



    // 받는 메일

    $mail -> addAddress($id, "receive01");

    //$mail -> addAddress("test2@teacher21.com", "receive02");



    // 첨부파일

    //$mail -> addAttachment("./test1.zip");

    //$mail -> addAttachment("./test2.jpg");



    // 메일 내용

    $mail -> isHTML(true); // HTML 태그 사용 여부

    $mail -> Subject = "Robotwar 이메일 인증 메일입니다.";  // 메일 제목

    $content = "다음 링크에 접속하여 이메일 확인을 진행하세요. <a href='http://49.247.131.90/emailauth.php?code={$authkey_sha}'>이메일 인증하기</a>";

    $mail -> Body = $content;     // 메일 내용



    // Gmail로 메일을 발송하기 위해서는 CA인증이 필요하다.

    // CA 인증을 받지 못한 경우에는 아래 설정하여 인증체크를 해지하여야 한다.

    $mail -> SMTPOptions = array(

      "ssl" => array(

      "verify_peer" => false

      , "verify_peer_name" => false

      , "allow_self_signed" => true

      )

    );

    $q3 = "SELECT * FROM temp_userid WHERE tempid='$id'";
    $result3 = $mysqli->query( $q3);
    if($result3->num_rows > 0){

      $q4 = "UPDATE temp_userid SET authtoken='$authkey_sha',emailcheck='0',authtime='$auth_datetime' WHERE tempid='$id'";
      $mysqli->query( $q4);

    }else{

      $q2 = "INSERT INTO temp_userid (tempid, authtoken, emailcheck, authtime) VALUES ('$id','$authkey_sha','0','$auth_datetime')";
      $mysqli->query( $q2);

    }

    $mysqli->close();
    // 메일 전송

    $mail -> send();

    echo "1";




    } catch (Exception $e) {

    echo "인증메일 전송 실패";

    }

  }



}




?>
