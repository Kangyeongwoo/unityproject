<?php
ini_set('display_errors', 'On');


include_once ('./dbconfig.php');
$mysqli = new mysqli($DB['host'], $DB['id'], $DB['pw'], $DB['db']);

mysqli_set_charset($mysqli,"utf8");

if (mysqli_connect_error()) {
    exit('Connect Error (' . mysqli_connect_errno() . ') '. mysqli_connect_error());
}


  $shopitemdata = array();
  $gunshopitem = array();
  $armorshopitem = array();

  $q2 = "SELECT * FROM shopdata WHERE itemtype = 'gun'";

  $result2 = $mysqli->query( $q2);
  while( $myfictionlist2 = $result2->fetch_array()){

     $gunitem=array("itemid" => $myfictionlist2["itemid"] , "itemname" =>$myfictionlist2["itemname"], "itemtype" =>$myfictionlist2["itemtype"]
    , "itemdescription"=>$myfictionlist2["itemdescription"], "atk"=>$myfictionlist2["atk"]
     , "def"=>$myfictionlist2["def"] , "atkspeed"=>$myfictionlist2["atkspeed"] , "rare"=>$myfictionlist2["rare"], "price"=>$myfictionlist2["price"] );
     array_push($gunshopitem, $gunitem);

  }

  $q3 = "SELECT * FROM shopdata WHERE itemtype = 'armor'";

  $result3 = $mysqli->query( $q3);
  while( $myfictionlist3 = $result3->fetch_array()){

     $armoritem=array("itemid" => $myfictionlist3["itemid"] , "itemname" =>$myfictionlist3["itemname"], "itemtype" =>$myfictionlist3["itemtype"]
    , "itemdescription"=>$myfictionlist3["itemdescription"], "atk"=>$myfictionlist3["atk"]
     , "def"=>$myfictionlist3["def"] , "atkspeed"=>$myfictionlist3["atkspeed"] , "rare"=>$myfictionlist3["rare"], "price"=>$myfictionlist3["price"] );
     array_push($armorshopitem, $armoritem);

  }

  $shopitemdata["gunshopitem"] = $gunshopitem;
  $shopitemdata["armorshopitem"] = $armorshopitem;


  $json = json_encode($shopitemdata,JSON_UNESCAPED_UNICODE);

  echo $json;


?>
