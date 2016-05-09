<%@ Page Language="C#" Culture="fr-FR" MasterPageFile="~/PageTest/MasterPageTest.master" AutoEventWireup="true" CodeFile="PageTestTextBoxDate.aspx.cs" Inherits="PageTest_PageTestAjax" Title="Page sans titre" %>
<%@ Register TagPrefix="usr" TagName="TextBoxDate" Src="~/UserControl/TextBoxDate.ascx" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<%@ Reference Control="~/UserControl/TextBoxDate.ascx"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />

<asp:Button ID="ButtonOk" runat="server" OnClick="ButtonOk_Click" Text="Ok" />
<br />
<table>
    <tr>
        <td>
            <asp:Label ID="LabelTextBoxDate" CssClass="LabelStyle" runat="server" 
                Text="Entrez une Date (format: <em>99/99/9999</em>):" />
        </td>
        <td>
            <asp:TextBox ID="TextBoxDateText" runat="server" Width="80px" MaxLength="1" 
                style="text-align:justify" ValidationGroup="MKE" 
                ontextchanged="TextBoxDateText_TextChanged" />
            <asp:ImageButton ID="ImageButtonCalendar" runat="server" 
                ImageUrl="~/Images/Calendar.png" CausesValidation="False" />
            <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" runat="server" 
                CssClass="TextBoxDateMaskedEditValidatorStyle"
                ControlExtender="MaskedEditExtender1"
                ControlToValidate="TextBoxDateText"
                EmptyValueMessage="Date demandée"
                InvalidValueMessage="Date non valide"
                Display="Dynamic"
                TooltipMessage="Entrez une date"
                EmptyValueBlurredText="*"
                InvalidValueBlurredMessage="*"
                ValidationGroup="MKE" />
            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server"
                TargetControlID="TextBoxDateText"
                Mask="99/99/9999"
                MessageValidatorTip="true"
                OnFocusCssClass="MaskedEditFocus"
                OnInvalidCssClass="MaskedEditError"
                MaskType="Date"
                DisplayMoney="Left"
                AcceptNegative="Left"
                ErrorTooltipEnabled="True" />
            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
                TargetControlID="TextBoxDateText" Format="dd/MM/yyyy" 
                PopupButtonID="ImageButtonCalendar" />
        </td>
    </tr>
</table>
        
<br />
<br />
<br />
<br />
<usr:TextBoxDate ID="TextBoxDate" Text="Entrez un date :" runat="server" />
<br /><br /><br />

<asp:Button ID="ButtonLoadTextBoxDate" runat="server" OnClick="ButtonLoadTextBoxDate_Click" Text="Load" />
<asp:Panel ID="PanelTextBoxDate" runat="server" />

</asp:Content>

