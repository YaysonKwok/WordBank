<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="WordBank.Account.Settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row">
            <!-- Profile Settings-->
            <div class="col-lg-8 pb-5">
                <div class="row">
                    <div class="col-md-8">
                        <div class="form-group">
                            <label for="account-ln">Post-Login Redirect Location</label>
                            <asp:DropDownList runat="server" CssClass="form-control" ID="RedirectList">
                                <asp:ListItem Text="Select One" Value="None"></asp:ListItem>
                                <asp:ListItem Text="Single Word Entry" Value="Input"></asp:ListItem>
                                <asp:ListItem Text="Import List" Value="Import"></asp:ListItem>
                                <asp:ListItem Text="Review Word" Value="WordPractice"></asp:ListItem>
                                <asp:ListItem Text="Review Definition" Value="DefinitionPractice"></asp:ListItem>
                                <asp:ListItem Text="Word List" Value="Words"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label for="amount-resort">Attempts before resorts</label>
                            <asp:TextBox runat="server" ID="ResortAmount" TextMode="Number" min="1" CssClass="form-control" value="<%= resortValue %>" />
                        </div>
                        <div class="form-group">
                            <label for="account-pass">New Password</label>
                            <asp:TextBox runat="server" ID="NewPassword" TextMode="Password" CssClass="form-control" /> <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                CssClass="text-danger"
                                ErrorMessage="Password must contain at least 8 characters, 1 digit, 1 special character"
                                ValidationExpression="^.*(?=.{8,})(?=.*[\d])(?=.*[\W]).*$"
                                ControlToValidate="NewPassword" />
                        </div>

                        <div class="form-group">
                            <label for="account-confirm-pass">Confirm Password</label>
                            <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" />
                        </div>
                        <hr class="mt-2 mb-3">
                        <div class="d-flex flex-wrap justify-content-between align-items-center">
                            <asp:Button ID="SubmitBtn" runat="server" Text="Update Profile" CssClass="btn btn-primary" OnClick="SubmitBtn_Click" UseSubmitBehavior="False" />
                            <asp:Label ID="RedirectResponse" runat="server"></asp:Label>
                            <asp:Label ID="PasswordResponse" runat="server"></asp:Label>
                            <asp:Label ID="ResortResponse" runat="server"></asp:Label>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
