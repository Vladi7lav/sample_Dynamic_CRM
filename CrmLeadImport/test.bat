cd /D "%~dp0"  
CrmSvcUtil.exe /url:http://10.60.200.48:5555/CRM2016/XRMServices/2011/Organization.svc?wsdl /out:Context.cs /serviceContextName:Context /n:CRM /u:Administrator /p:Pass@word99
pause