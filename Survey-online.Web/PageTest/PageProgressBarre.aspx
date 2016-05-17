<%@ Page Language="C#" 
    Trace="false"
    MasterPageFile="~/PageTest/MasterPageTest.master" 
    AutoEventWireup="true" CodeFile="PageProgressBarre.aspx.cs" 
    Inherits="PageTest_PageProgressBarre" 
    Title="Page sans titre" %>
    
<%@ Register TagPrefix="usr" TagName="ProgressBarre" Src="~/UserControl/ProgressBarre.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<style type="text/css">
.LabelProgressStyle
{
    color:Blue
}
</style> 

    <asp:Button ID="ButtonProgress" runat="server" OnClick="ButtonProgress_Click" Text="Ok" />
    <br />
    <usr:ProgressBarre ID="ProgressBarre" States="3" PanelBarSideHeight="3px" LabelProgressCssClass="LabelProgressStyle" runat="server" />
</asp:Content>

