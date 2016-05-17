<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" CodeFile="PageErreur.aspx.cs" Inherits="PageErreur" Title="Page d'erreur" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
</head>
<body>
    <form id="form1" runat="server">
    <div id="body">
        <div class="DivFullWidth">
        <table width="100%" align="center" border="0">
            <tr align="center">
                <td>
                    <h3>Message de l'application</h3>
                    <div class="dashedline"></div>
                </td>
            </tr>
            <tr align="center">
                <td>
			        <asp:Label ID="LabelValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" />
                </td>
            </tr>
        </table>
        </div>
    </div>
    </form>
</body>
</html>


