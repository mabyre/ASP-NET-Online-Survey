<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ClientCallback.aspx.cs" Inherits="ClientCallback" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
function LookUpStock()
{
    var lb = document.forms[0].ListBox1
    var product = lb.options(lb.selectedIndex).value 
    CallServer(product, "");
}

function ReceiveServerData(rValue)
{
    Results.innerText = rValue;
}
</script>
<div>
  <asp:ListBox ID="ListBox1" Runat="server"></asp:ListBox>
  <br />
  <br />
  <button onclick="LookUpStock()">Look Up Stock</button>
  <br />
  <br />
  Items in stock: <span ID="Results"></span>
  <br />
</div>

</asp:Content>

