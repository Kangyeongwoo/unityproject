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

$q = "SELECT * FROM userdata WHERE id='$id'";
$result = $mysqli->query( $q);

if($result->num_rows==1){

  $row = $result->fetch_array(MYSQLI_ASSOC);
  if($row['pw']==$pw && $row['userindex']==$userindex){


    $useritemData = array();
    $itemData = array();
    $skillData = array();
    $activepartData = array();
    $pessivepartData = array();

  //  $q2 = "SELECT * FROM useritem INNER JOIN itemdata ON (useritem.itemid = itemdata.itemid AND useritem.itemtype = ("gun" OR "armor"))";
    //user 가 가진 총과 갑옷의 정보를 가져온다
    $q2 = "SELECT * FROM useritem INNER JOIN itemdata ON (useritem.itemid = itemdata.itemid) WHERE userindex = '$userindex' AND (useritem.itemtype ='gun' OR useritem.itemtype='armor')";

    $result2 = $mysqli->query( $q2);
    while( $myfictionlist2 = $result2->fetch_array()){

       $item=array("useritemindex" => $myfictionlist2["useritemindex"] ,"itemid" => $myfictionlist2["itemid"] , "itemname" =>$myfictionlist2["itemname"], "itemtype" =>$myfictionlist2["itemtype"] , "itemcount"=>$myfictionlist2["itemcount"]
     , "equiped"=>$myfictionlist2["equiped"] , "itemdescription"=>$myfictionlist2["itemdescription"], "atk"=>$myfictionlist2["atk"]
       , "def"=>$myfictionlist2["def"] , "atkspeed"=>$myfictionlist2["atkspeed"] , "rare"=>$myfictionlist2["rare"], "price"=>$myfictionlist2["price"] );
       array_push($itemData, $item);



    }
    //user가 가진 skill의 정보를 가져온다
    $q3 = "SELECT * FROM useritem INNER JOIN skilldata ON (useritem.itemid = skilldata.skillid) WHERE userindex = '$userindex' AND (useritem.itemtype ='skill')";

    $result3 = $mysqli->query( $q3);
    while( $myfictionlist3 = $result3->fetch_array()){

       $skill=array("useritemindex" => $myfictionlist3["useritemindex"] ,"skillid" => $myfictionlist3["skillid"] ,"skillname" =>$myfictionlist3["skillname"], "skilltype" =>$myfictionlist3["itemtype"] ,"skillcount" =>$myfictionlist3["itemcount"]
     , "equiped"=>$myfictionlist3["equiped"] , "skilldescription"=>$myfictionlist3["skilldescription"], "atk"=>$myfictionlist3["atk"], "cooltime"=>$myfictionlist3["cooltime"]
      ,"price"=>$myfictionlist3["price"] , "skilltype2"=>$myfictionlist3["skilltype"]);
       array_push($skillData, $skill);

    }
     //유저가 가진 activeskillpart 정보를 가져온다
  //  $q4 = "SELECT * FROM useritem WHERE userindex = '$userindex' AND itemtype ='activeskillpart' ";
     $q4 = "SELECT * FROM useritem INNER JOIN skilldata ON (useritem.itemid = skilldata.skillid) WHERE userindex = '$userindex' AND (useritem.itemtype ='activeskillpart') ";
    $result4 = $mysqli->query( $q4);
    while( $myfictionlist4 = $result4->fetch_array()){

       $activepart=array("useritemindex" => $myfictionlist4["useritemindex"] ,"itemtype" => $myfictionlist4["itemtype"] ,"itemid" =>$myfictionlist4["itemid"],"skillname" =>$myfictionlist4["skillname"],"skilldescription"=>$myfictionlist4["skilldescription"]
      , "atk"=>$myfictionlist4["atk"], "cooltime"=>$myfictionlist4["cooltime"]  ,"price"=>$myfictionlist4["price"] , "skilltype2"=>$myfictionlist4["skilltype"],  "itemcount" =>$myfictionlist4["itemcount"]);
       array_push($activepartData, $activepart);

    }
 //유저가 가진 passiveskillpart 정보를 가져온다
    $q5 = "SELECT * FROM useritem INNER JOIN skilldata ON (useritem.itemid = skilldata.skillid) WHERE userindex = '$userindex' AND (useritem.itemtype ='passiveskillpart') ";

    $result5 = $mysqli->query( $q5);
    while( $myfictionlist5 = $result5->fetch_array()){

      $pessivepart=array("useritemindex" => $myfictionlist5["useritemindex"] ,"itemtype" => $myfictionlist5["itemtype"] ,"itemid" =>$myfictionlist5["itemid"], "skillname" =>$myfictionlist5["skillname"],"skilldescription"=>$myfictionlist5["skilldescription"]
      , "atk"=>$myfictionlist5["atk"], "cooltime"=>$myfictionlist5["cooltime"]  ,"price"=>$myfictionlist5["price"] , "skilltype2"=>$myfictionlist5["skilltype"],"itemcount" =>$myfictionlist5["itemcount"]);
      array_push($pessivepartData, $pessivepart);

    }


    $useritemData["useritemData"] = $itemData;
    $useritemData["userskillData"] = $skillData;
    $useritemData["activepartData"]=$activepartData;
    $useritemData["pessivepartData"]=$pessivepartData;

    $json = json_encode($useritemData,JSON_UNESCAPED_UNICODE);

    echo $json;

  }



}





?>
