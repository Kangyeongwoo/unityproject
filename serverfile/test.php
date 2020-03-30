<?php

$q = '2016-11-30 23:00:00' ;
$w = '2016-11-30 22:00:00' ;

$r = strtotime($q) - strtotime($w) ;


echo  ceil($r ) ;      // 2초
echo  ceil($r / 60 ) ; // 1분
echo  ceil($r / (60*60   )) ; // 1시간
echo  ceil($r / (60*60 *24   )) ; // 1일


?>
