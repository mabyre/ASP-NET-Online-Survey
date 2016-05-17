<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TextBoxDate.ascx.cs" Inherits="TextBoxDate" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:Label ID="LabelTextBoxDate" runat="server" 
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
