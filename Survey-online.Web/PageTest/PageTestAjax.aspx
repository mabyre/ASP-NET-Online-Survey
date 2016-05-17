<%@ Page Language="C#" Culture="fr-FR" MasterPageFile="~/PageTest/MasterPageTest.master" AutoEventWireup="true" CodeFile="PageTestAjax.aspx.cs" Inherits="PageTest_PageTestAjax" Title="Page sans titre" %>
<%@ Register TagPrefix="usr" TagName="TextBoxDate" Src="~/UserControl/TextBoxDate.ascx" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:CheckBox ID="CheckBox2" runat="server" Text="Choix 1 :" />
    <br />
    <asp:CheckBox ID="CheckBox1" runat="server" Text="Autre précisez : " />
    <asp:TextBox ID="TextBox1" runat="server" />
    <cc1:PopupControlExtender ID="TextBox1_PopupControlExtender" runat="server" 
        TargetControlID="CheckBox1"
        PopupControlID="TextBox1" 
        DynamicControlID="CheckBox1" 
        DynamicServiceMethod="GetDynamicContent" 
        DynamicServicePath="" 
        Enabled="True" 
        OffsetX="150" 
        ExtenderControlID="">
    </cc1:PopupControlExtender>
    
    <br />
    
    <br />
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
    <asp:ImageButton ID="ImageButtonCalendar" runat="server" ImageUrl="~/Images/Calendar.png" CausesValidation="False" />
    <cc1:CalendarExtender ID="TextBox2_CalendarExtender" runat="server" 
        Enabled="True" TargetControlID="TextBox2" Format="dd/MM/yyyy" 
        PopupButtonID="ImageButtonCalendar" >
    </cc1:CalendarExtender>
    <cc1:MaskedEditExtender ID="TextBox2_MaskedEditExtender" runat="server" 
        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
        TargetControlID="TextBox2" CultureName="fr-FR" Mask="99/99/9999" 
        UserDateFormat="DayMonthYear" MaskType="Date">
    </cc1:MaskedEditExtender>
    
    <br />
    <br />
        <asp:Button ID="ButtonOk" runat="server" onclick="ButtonOk_Click" 
        Text="Ok" />
        <br />
        <usr:TextBoxDate ID="TextBoxDate" runat="server"/> 

    
</asp:Content>

