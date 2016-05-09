<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QuestionnaireStatControl.ascx.cs" Inherits="UserControls_QuestionnaireStatControl" %>
<table border="0px" cellpadding="0" cellspacing="0">
    <tr class="BoiteGaucheHeaderStyle" >
        <td valign="top">
            <table border="0" cellpadding="0" cellspacing="0" align="left">
                <tr>
                    <td width="20px" height="40px"></td>
                    <td>
                    <h3>Questionnaire Stat</h3>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr class="BoiteGaucheCentreStyle">
        <td>
            <table border="0" cellpadding="0" cellspacing="0" align="left">
                <tr>
                    <td width="15px"></td>
                    <td align="left" width="150px">
                    <div id="PollControlStyle">
                       <UserControl:QuestionnaireStatControl ID="PollControl1" runat="server" />
                    </div>
                    </td>
                    <td width="18px"></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr class="BoiteGaucheCorpsStyle" height="10px">
        <td>
        </td>
    </tr>
    <tr class="BoiteGaucheFooterStyle">
        <td>
        </td>
    </tr>
</table>