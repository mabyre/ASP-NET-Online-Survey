<%@ Page Language="C#" MasterPageFile="~/PageTest/MasterPageTest.master" AutoEventWireup="true" CodeFile="PageObout.aspx.cs" Inherits="PageTest_PageObout" Title="Page sans titre" %>
<%@ Register TagPrefix="obout" Namespace="OboutInc.ColorPicker" Assembly="obout_ColorPicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <center>
      Click to the box:
      <obout:ColorPicker runat="server" ID="picker"
             TargetId="color" TargetProperty="style.backgroundColor" 
             OnColorPostBack="Color_CallBack" PickButton="false" AutoPostBack="true" >
             <asp:TextBox readOnly="true" id="color" style="cursor: pointer;" runat="server"/>
      </obout:ColorPicker>
      Previous color:
      <asp:TextBox readOnly="true" id="previous" style="width: 40px;" runat="server"/>
   </center>
</asp:Content>

