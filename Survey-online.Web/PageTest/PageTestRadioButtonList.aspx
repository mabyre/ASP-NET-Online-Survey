<%@ Page Language="C#" MasterPageFile="~/PageTest/MasterPageTest.master" AutoEventWireup="true" CodeFile="PageTestRadioButtonList.aspx.cs" Inherits="PageTest_PageTestCheckBox" Title="Untitled Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="sm1" runat="server"  />
    <asp:CheckBox ID="CheckBox1" runat="server" />
    <br />
    <br />
    <asp:RadioButton ID="RadioButton1" runat="server" Text="Radio 1" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
    <br />
    <br />
    <asp:RadioButton ID="RadioButton2" runat="server" Text="Radio 2" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" /> 
    <br />
    <br />
    <asp:RadioButton ID="RadioButton3" runat="server" Text="Radio 3" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" /> 
    <br />

    <asp:CheckBox ID="CheckBox2" runat="server" />
    <asp:RadioButtonList ID="RadioButtonList1" RepeatColumns = "2" 
        ForeColor="Aquamarine" runat="server" 
        onselectedindexchanged="RadioButtonList1_SelectedIndexChanged">
    </asp:RadioButtonList><br />
    <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
    <br />
    <br />
    <br />
    <br />
    <table style="width:100%;">
        <tr>
            <td>
                <asp:RadioButton ID="RadioButton4" runat="server" Text="Radio 1" />
                <cc1:MutuallyExclusiveCheckBoxExtender 
                    ID="RadioButton4_MutuallyExclusiveCheckBoxExtender" 
                    runat="server" 
                    Enabled="True" 
                    Key="RadioButtonExclude1" 
                    TargetControlID="RadioButton4" />
            </td>
            <td>
                <asp:RadioButton ID="RadioButton5" runat="server" Text="Radio 2" />
                <cc1:MutuallyExclusiveCheckBoxExtender 
                    ID="RadioButton5_MutuallyExclusiveCheckBoxExtender" 
                    runat="server" 
                    Enabled="True" 
                    Key="RadioButtonExclude1" TargetControlID="RadioButton5" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <label>Construits dynamiquement en utilisant RadioButtonExtender.cs ca marche mais ya un postback !</label>
    <asp:Panel ID="PanelRadioButtonList" runat="server">
    </asp:Panel>
    <br />
    <br />
    <label>Construction dynamique en utilisant MutuallyExclusiveCheckBoxExtender c'est possible ?</label>
    <asp:Panel ID="PanelRadioButtonExclusiveCheckBoxExtender" runat="server">
    </asp:Panel>
    <asp:Button ID="ButtonOk" runat="server" Text="Ok" onclick="ButtonOk_Click" />
    <br />
    
</asp:Content>

