<?php
/*** modify this source code from   ***/
	/*** By Weerachai Nukitram ***/
	/***  http://www.ThaiCreate.Com ***/
              /*** Thank You ***/
	$strProduct = trim($_POST["tProductID"]);

   include ("connect_inc.php") ; 

	$strSQL = "SELECT * FROM member WHERE UserID = '".$strProduct."' ";
	$objQuery = mysql_query($strSQL) or die ("Error Query [".$strSQL."]");
	$objResult = mysql_fetch_array($objQuery);
	if($objResult)
	{
		echo $objResult["NameUser"]."|".$objResult["IDcard"]."|".$objResult["address"]."|".
		$objResult["Phone"]."|".$objResult["career"]."|".$objResult["position"]."|".
		$objResult["salary"]."|".$objResult["Office"]."|".$objResult["RegisterDay"]."|".
		$objResult["NoRegister"]."|".$objResult["NumShare"];
	} 

	mysql_close();
?>
