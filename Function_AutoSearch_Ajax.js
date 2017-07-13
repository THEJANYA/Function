
	   var HttPRequest = false;

function doCallAjax(ftxtid,ftxtname,ftxtIDcard,ftxtaddress,ftxtphone,ftxtcareer,
	   ftxtposition,ftxtsalary,ftxtoffice,fday1,ftxtnoRe,ftxtshare) 
{
		  HttPRequest = false;
		  if (window.XMLHttpRequest) { // Mozilla, Safari,...
			 HttPRequest = new XMLHttpRequest();
			 if (HttPRequest.overrideMimeType) {
				HttPRequest.overrideMimeType('text/html');
			 }
		  } else if (window.ActiveXObject) { // IE
			 try {
				HttPRequest = new ActiveXObject("Msxml2.XMLHTTP");
			 } catch (e) {
				try {
				   HttPRequest = new ActiveXObject("Microsoft.XMLHTTP");
				} catch (e) {}
			 }
		  } 
		  
		  if (!HttPRequest) {
			 alert('Cannot create XMLHTTP instance');
			 return false;
		  }

		  var url = 'member_edit_return.php';
		  var pmeters = "tProductID=" + encodeURI( document.getElementById(ftxtid).value);

			HttPRequest.open('POST',url,true);

			HttPRequest.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
			HttPRequest.setRequestHeader("Content-length", pmeters.length);
			HttPRequest.setRequestHeader("Connection", "close");
			HttPRequest.send(pmeters);
			
			
			HttPRequest.onreadystatechange = function()
			{

				if(HttPRequest.readyState == 10) // Return Request
				{
					var myProduct = HttPRequest.responseText;
					if(myProduct != "")
					{
						var myArr = myProduct.split("|");
						document.getElementById(ftxtname).value = myArr[0];
						document.getElementById(ftxtIDcard).value = myArr[1];
						document.getElementById(ftxtaddress).value = myArr[2];
						document.getElementById(ftxtphone).value = myArr[3];  
						document.getElementById(ftxtcareer).value = myArr[4];
						document.getElementById(ftxtposition).value = myArr[5];
						document.getElementById(ftxtsalary).value = myArr[6];
						document.getElementById(ftxtoffice).value = myArr[7];
						document.getElementById(fday1).value = myArr[8];
						document.getElementById(ftxtnoRe).value = myArr[9];
						document.getElementById(ftxtshare).value = myArr[10];
					}
				}
				
			}

 }
