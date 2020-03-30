<?php
ini_set('display_errors', 'On');

include_once ('./dbconfig.php');
$mysqli = new mysqli($DB['host'], $DB['id'], $DB['pw'], $DB['db']);
if (mysqli_connect_error()) {
    exit('Connect Error (' . mysqli_connect_errno() . ') '. mysqli_connect_error());
}
$id = $_POST['id'];
$q = "DELETE FROM temp_userid WHERE tempid='$id'";
$mysqli->query( $q);

echo "ok";
 ?>
