
<%@ Page
    Language="C#"
    MasterPageFile="~/PageTest/MasterPageTest.master"
    AutoEventWireup="true"
    CodeFile="QuestionOuverte.aspx.cs"
    Inherits="QuestionOuverte_Page"
    Title="PopupControl Sample" %>

<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="SMOnSenFou" runat="server"  />
<style>
.popupControl
{
    
}
</style>
   
<div>
        <h3>Question ouverte Demonstration</h3>
    
       
<table width="100%">
    <tr>
        <td>
        Essayer la question ouverte 1 : 
        </td>
    </tr>
    <tr>
        <td height="150px" align="center">
        <asp:CheckBox ID="CheckBox1" runat="server" Text="Choix 1 :" autocomplete="off"/>
        <br />        
        <asp:CheckBox ID="CheckBox2" runat="server" Text="Choix 2 :" autocomplete="off"/>
        <br />
        <asp:CheckBox ID="CheckBox3" runat="server" Text="Choix 3 :" autocomplete="off"/>
        <br />
        <asp:CheckBox ID="CheckBoxQuestionOuverte" runat="server" Text="Autre précisez :"/>
        <asp:Panel ID="PanelQuestionOuverte" runat="server" CssClass="popupControl">
            <div style="border: 1px outset white;">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <asp:TextBox ID="TextBoxQuestionOuverte" runat="server" Width="180" TextMode="MultiLine" Rows="3" autocomplete="off" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>
        <ajaxToolkit:PopupControlExtender ID="PopupControlExtender3" runat="server"
            TargetControlID="CheckBoxQuestionOuverte"
            PopupControlID="PanelQuestionOuverte" 
            Position="Left"
            OffsetX="150" 
            DynamicControlID="CheckBoxQuestionOuverte" 
            DynamicServiceMethod="coucou"
             />
        </td>
    </tr>
</table>

</div>
</asp:Content>