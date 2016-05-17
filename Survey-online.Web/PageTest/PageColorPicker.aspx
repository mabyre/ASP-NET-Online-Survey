<%@ Page Language="C#" Trace="false" MasterPageFile="~/PageTest/MasterPageTest.master" AutoEventWireup="true" CodeFile="PageColorPicker.aspx.cs" Inherits="PageTest_PageStyle" Title="Page sans titre" %>
<%@ Register TagPrefix="usr" TagName="ColorPicker" Src="~/UserControl/ColorPicker.ascx" %>

<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>
<center>
            Couleur choisie grace au picker :<br />
            <asp:TextBox ID="TextBoxMessage" runat="server" Width="80" autocomplete="off" />
            <br />
            <br />
            <br />
            <br />
            <br />

<ajaxToolkit:ToolkitScriptManager runat="Server" EnablePartialRendering="true" ID="ScriptManager1" />
<asp:TextBox ID="TextBoxColor" runat="server" Width="80" autocomplete="off" />
<asp:Panel ID="PanelColorPicker" runat="server">
    <asp:UpdatePanel runat="server" ID="UpdatePanelColorPicker">
        <ContentTemplate>    
            
            <usr:ColorPicker ID="ColorPicker" runat="server" />

    </ContentTemplate>

</asp:UpdatePanel>
</asp:Panel>

<!-- On ne peut pas mettre ImageButton comme TargetControlID plantage javascript -->
<ajaxToolkit:PopupControlExtender ID="PopupControlExtenderColorPicker" runat="server"
    TargetControlID="TextBoxColor"
    PopupControlID="PanelColorPicker"
    Position="Bottom"
    CommitScript="e.value" />       

</center>
</div>
</asp:Content>

