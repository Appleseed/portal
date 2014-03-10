<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div>
Please check configuration!<br />
<br />
Install / Update Packages needs httpRuntime WaitChangeNotification &gt; 5 to function properly.<br />
<br />
&nbsp;&lt;system.web&gt;<br />
&nbsp;&nbsp;&nbsp;&nbsp;&lt;httpRuntime <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;waitChangeNotification=&quot;10&quot;<br />
&nbsp;&nbsp;&nbsp;&nbsp;/&gt;<br />
&nbsp;&lt;/system.web&gt;<br />

</div>