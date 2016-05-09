<%@ Page Language="C#" Debug="true"%>
<%@ Register TagPrefix="obout" Namespace="OboutInc.ColorPicker" Assembly="obout_ColorPicker" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html>
<head runat="server">
<title>obout ASP.NET ColorPicker example</title>
<link rel="stylesheet" href="example_styles/style.css" />

<script runat=server language="c#">
  void Page_load(object sender, EventArgs e){
    if(!IsPostBack){
       picker.InitialColor = color.Style["background-color"] = previous.Style["background-color"] = "#FFFFFF";  
    }
  }
  private void Color_CallBack(object sender, ColorPostBackEventArgs e){
    color.Style["background-color"] = e.Color;
    previous.Style["background-color"] = e.PreviousColor;
  }
</script>
</head>
<body class="bodyStyle">
<a href="Default.aspx">< Back to examples</a>
<br /><br /><br /><br /><br /><br /><br /><br />
<form runat="server">
   <center>
      Click to the box:
      <obout:ColorPicker runat="server" ID="picker"
             TargetId="color" TargetProperty="style.backgroundColor" 
             OnColorPostBack="Color_CallBack" PickButton="false" AutoPostBack="true" >
             <asp:TextBox readOnly="true" id="color" style="cursor: pointer;" runat="server"/>
      </obout:ColorPicker>
      Previous color:
      <asp:TextBox readOnly="true" id="previous" style="width: 40px;" runat="server"/>
   </center>
</form>
<br /><br /><br /><br /><br /><br /><br /><br />
</body>
</html>
