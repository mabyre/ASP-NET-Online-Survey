<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Strict="false" ValidateRequest="false" CodeFile="Details.aspx.cs" Inherits="Member_Details" Title="Détails d'un membre" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<div id="body">
    <div id="columnright">
        <div class="rightblock">
            <h3>Modifier les détails de mon compte</h3>
            <div class="dashedline"></div>
            <p>
            Utilisez ce formulaire pour changer les détails de votre compte. C'est valeurs sont
            utilisées dans la liste des membres et pour les administrateurs du site.
            </p>
            
          <asp:UpdatePanel ID="up1" runat="server"><ContentTemplate>
                <table border="0">
                    <tbody>
                        <tr>
                            <td style="WIDTH: 138px" class="formlabel">
                                <label for="Email"><strong>E-mail :</strong></label>
                            </td>
                            <td>
                                <asp:TextBox ID="Email" runat="server" CssClass="txtfield"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email"
                                    ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator><strong>
                                    </strong>
                            </td>
                        </tr>
                        <tr style="font-weight: bold">
                            <td style="WIDTH: 138px; HEIGHT: 40px" class="formlabel">
                                <label for="fname">
                                    First Name :</label>
                            </td>
                            <td style="HEIGHT: 40px">
                                <asp:TextBox ID="fname" runat="server" CssClass="txtfield"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="fname"
                                    ErrorMessage="First Name is required." ToolTip="First Name is required." ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 138px" class="formlabel">
                                <strong>
                                <label for="lname">
                                    Last Name :</label>
                                </strong>
                            </td>
                            <td>
                                <asp:TextBox ID="lname" runat="server" CssClass="txtfield"></asp:TextBox><strong> </strong>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="lname"
                                    ErrorMessage="Last Name is required." ToolTip="Last Name is required." ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 138px" class="formlabel">
                                <strong>
                                <label for="Addr">
                                    Addresse :</label>
                                </strong>
                            </td>
                            <td>
                                <asp:TextBox ID="Addr" runat="server" CssClass="txtblock" Rows="3" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 138px" class="formlabel">
                                <strong>
                                <label for="Phone">
                                    Téléphone :</label>
                                </strong>
                            </td>
                            <td>
                                <asp:TextBox ID="Phone" runat="server" CssClass="txtfield"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 138px" class="formlabel">
                                <strong>
                                Receive Newsletter:</strong></td>
                            <td>
                                <asp:CheckBox ID="NewsletterCheck" runat="server" /></td>
                        </tr>
                    </tbody>
                </table>
            <p>
                <asp:Label ID="ContactStatus" runat="server" EnableViewState="False" Font-Bold="True"
                    Font-Underline="True" ForeColor="#FF0033"></asp:Label>
                <br />
                <br />
                <UserControl:RolloverButton ID="update" runat="server" OnClick="update_Click" Text="Update Info" />
            </p>
            </ContentTemplate></asp:UpdatePanel>
            <asp:UpdateProgress id="UpdateProgress3" runat="server" AssociatedUpdatePanelID="up1">
            <progresstemplate>
                <asp:Image ID="loading1" runat="server" SkinID="ImageLoading" />
            </progresstemplate>
            </asp:UpdateProgress>
            </div>
            
            <div class="rightblock">
            <asp:UpdatePanel ID="updateprofile" runat="server"><ContentTemplate>
            <table width="100%">
                <tr>
                    <td colspan="2">
                        <h2>Mise à jour Profile</h2>
                        <div class="dashedline"></div>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">
                        Signature :
                    </td>
                    <td valign="top">
                        <fckeditorv2:fckeditor id="SignatureTextBox" runat="server" basepath="~/FCKeditor/"
                            skinpath="skins/office2003/" toolbarset="Default" usebroncarriagereturn="true"></fckeditorv2:fckeditor>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">
                        Bio :
                    </td>
                    <td valign="top" width="80%">
                        <fckeditorv2:fckeditor id="BioTextBox" runat="server" basepath="~/FCKeditor/" skinpath="skins/office2003/"
                            toolbarset="Default" usebroncarriagereturn="true"></fckeditorv2:fckeditor>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <b>Préférences</b>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">
                        Montrer mon Addresse Email :
                    </td>
                    <td valign="top">
                        <asp:CheckBox ID="ShowEmailCheckBox" runat="server" Text="Allow others to see my Email Address" />
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">
                        Messages par Page :
                    </td>
                    <td valign="top">
                        <asp:TextBox ID="MsgsPerPageTextBox" runat="server" Columns="3">
                            </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="MsgsPerPageTextBox"
                            ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="MsgsPerPageTextBox"
                            ErrorMessage="RangeValidator" MaximumValue="100" MinimumValue="1" Type="Integer">Enter an Integer between 1 and 100!</asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">
                        Sort Descending :
                    </td>
                    <td valign="top">
                        <asp:CheckBox ID="SortDescendingCheckBox" runat="server" Text="Display Newest Messages First" />
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">
                        Show Signatures :
                    </td>
                    <td valign="top">
                        <asp:CheckBox ID="ShowSignaturesCheckBox" runat="server" Text="Display User Signatures in Posts" />
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">
                        Show Avatars :
                    </td>
                    <td valign="top">
                        <asp:CheckBox ID="ShowAvatarsCheckBox" runat="server" Text="Display User Avatars in Posts" />
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">
                        Send Watch Emails :
                    </td>
                    <td valign="top">
                        <asp:CheckBox ID="SendWatchEmailsCheckBox" runat="server" Text="Email new Messages in Threads I am Watching (If enabled on this site)" />
                    </td>
                </tr>
            </table>
            <UserControl:RolloverButton ID="UpdateProfileButton" runat="server" CausesValidation="True" Text="Update Profile" OnClick="UpdateProfileButton_Click" />
        <asp:Label ID="ProfStatus" runat="server" EnableViewState="False" Font-Bold="True"
                    Font-Underline="True" ForeColor="#FF0033"></asp:Label>
                    
    </ContentTemplate>
    </asp:UpdatePanel>
                    
                    <asp:UpdateProgress ID="updateprofileloading" runat="server" AssociatedUpdatePanelID="updateprofile"><ProgressTemplate>
                    <asp:Image ID="loadingprof" runat="server" SkinID="ImageLoading" />
                    </ProgressTemplate></asp:UpdateProgress>
        </div>
        

        <div class="rightblock">
            <h2>Changez votre Mot de Passe</h2>
            <div class="dashedline"></div>
            <asp:UpdatePanel id="UpdatePanel2" runat="server" RenderMode="Inline">
                <contenttemplate>
                <asp:ChangePassword id="ChangePassword1" runat="server">
                <ChangePasswordTemplate>
                <table cellPadding="1" border="0">
                    <tr><td>
                    <table cellPadding="0" border="0"><tr><td align="right">
                    <asp:Label id="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword" Font-Bold="True">Mot de Passe :&nbsp;</asp:Label>
                    </td><td>
                    <asp:TextBox id="CurrentPassword" runat="server" TextMode="Password"></asp:TextBox> 
                    <asp:RequiredFieldValidator id="CurrentPasswordRequired" runat="server" ValidationGroup="ChangePassword1" ToolTip="Password is required." ErrorMessage="Password is required." ControlToValidate="CurrentPassword">*</asp:RequiredFieldValidator> 
                    </td>
                    </tr>
                    <tr><td align="right">
                    <asp:Label id="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword" Font-Bold="True">Nouveau Mot de Passe :&nbsp;</asp:Label>
                    </td><td>
                    <asp:TextBox id="NewPassword" runat="server" TextMode="Password"></asp:TextBox> 
                    <asp:RequiredFieldValidator id="NewPasswordRequired" runat="server" ValidationGroup="ChangePassword1" ToolTip="New Password is required." ErrorMessage="New Password is required." ControlToValidate="NewPassword">*</asp:RequiredFieldValidator> </td></tr><tr>
                    <td align=right>
                    <asp:Label id="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword" Font-Bold="True">Confirmez Nouveau Mot de Passe :&nbsp;</asp:Label>
                    </td><td>
                    <asp:TextBox id="ConfirmNewPassword" runat="server" TextMode="Password"></asp:TextBox> 
                    <asp:RequiredFieldValidator id="ConfirmNewPasswordRequired" runat="server" ValidationGroup="ChangePassword1" ToolTip="Confirm New Password is required." ErrorMessage="Confirm New Password is required." ControlToValidate="ConfirmNewPassword">*</asp:RequiredFieldValidator> 
                    </td></tr><tr>
                    <td style="HEIGHT: 16px" align="center" colSpan="2"><asp:CompareValidator id="NewPasswordCompare" runat="server" ValidationGroup="ChangePassword1" ErrorMessage="The Confirm New Password must match the New Password entry." ControlToValidate="ConfirmNewPassword" Display="Dynamic" ControlToCompare="NewPassword" Font-Bold="True" Font-Underline="True"></asp:CompareValidator> 
                    </td></tr><tr><td style="COLOR: red; HEIGHT: 13px" align=center colSpan=2>
                    <asp:Literal id="FailureText" runat="server" EnableViewState="False"></asp:Literal> 
                    </td></tr><tr>
                    <td style="TEXT-ALIGN: left" colSpan=2>
                    <UserControl:RolloverButton id="ChangePasswordPushButton" runat="server" Text="Modifier" Style="font-size:12px;" ValidationGroup="ChangePassword1" CommandName="ChangePassword" />
                    </td></tr>
                    </table>
                    </td></tr>
                </table>
                </ChangePasswordTemplate>
                <SuccessTemplate>
                <table style="BORDER-COLLAPSE: collapse" cellSpacing="0" cellPadding="1" border="0"><tr><td>
                <table cellPadding="0" border="0">
                <tr>
                    <td align="center" colSpan="2">Changement de Mot de Passe Terminé</td>
                </tr>
                <tr>
                    <td>Votre Mot de Passe a été changé avec succès</td>
                </tr>
                <tr>
                <td align=right colSpan=2>&nbsp;</td></tr>
                </table>
                </td>
                </tr>
                </table>
                </SuccessTemplate>
                </asp:ChangePassword>
                </contenttemplate>
            
            </asp:UpdatePanel>
            <asp:UpdateProgress id="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                <progresstemplate>
                <asp:Image ID="loading2" runat="server" SkinID="ImageLoading" />
                </progresstemplate>
            </asp:UpdateProgress>
        </div>
    </div>
    
</div>
</asp:Content>
