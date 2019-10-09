<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WordBank.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <h1 class="h3 mb-3 font-weight-normal">Login</h1>
        <asp:TextBox ID="UsernameInput" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox>
        <br />
    <asp:Button ID="SignInBtn" runat="server" Text="Sign-In" class="btn btn-primary btn-lg btn-block" OnClick="SignInBtn_Click" />
      <asp:Label ID="Loginlbl" runat="server"></asp:Label>
      <br />
</asp:Content>
