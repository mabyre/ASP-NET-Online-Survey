<%@ Page Language="C#" 
    MasterPageFile="~/MasterPage.master" 
    MaintainScrollPositionOnPostback="true" 
    AutoEventWireup="true" 
    CodeFile="PageValidation.aspx.cs" 
    Inherits="PageValidation" 
    Title="Page de validation" 
    ValidateRequest="false"
%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<a name="HautDePage"></a>
<div class="DivPageAccueilStyle">

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
</asp:Content>

