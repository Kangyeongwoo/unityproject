<?php

$pw1 =$_POST['pw1'];
$pw2 =$_POST['pw2'];

if(!preg_match("/^[a-z0-9_]{4,20}$/",$pw1)){
   echo "no";

}else{

   echo "ok";
}
?>
