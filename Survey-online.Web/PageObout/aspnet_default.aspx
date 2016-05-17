<%@ Page Language="C#" Debug="true"%>
<%@ Register TagPrefix="obout" Namespace="OboutInc.ColorPicker" Assembly="obout_ColorPicker" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html>
<head runat="server">
<title>obout ASP.NET ColorPicker example</title>
<link rel="stylesheet" href="example_styles/style.css" />
</head>
<body class="bodyStyle">
<a href="Default.aspx">< Back to examples</a>
<br /><br /><br /><br /><br /><br /><br /><br />
<form runat="server">
    <center>
    <asp:Label ID="Label1" runat="server" Text="Texte avant colorpicker"></asp:Label><br />
       <asp:TextBox readOnly="true" id="color" style="vertical-align: middle;" runat="server"/>
       &nbsp;&nbsp;Click here:
       <obout:ColorPicker runat="server" TargetId="color" /><br />
    <asp:Label ID="Label2" runat="server" Text="Texte après colorpicker"></asp:Label>
    </center>
</form>
<br /><br /><br /><br /><br /><br /><br /><br />
</body>
</html>
