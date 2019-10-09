<%@ Page Title="New Words" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Input.aspx.cs" Inherits="WordBank.Input" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="mb-3">
        <asp:Label ID="Redirectlbl" runat="server" Visible="False"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Wordlbl" runat="server" Text="Word"></asp:Label>
        <asp:TextBox ID="WordInput" runat="server" CssClass="form-control" required="true"></asp:TextBox>
    </div>

    <div class="mb-3">
         <br />
        <asp:Label ID="Definitionlbl" runat="server" Text="Definition"></asp:Label>
        <asp:TextBox ID="DefinitionInput" runat="server" CssClass="form-control" required="true"></asp:TextBox>
    </div>


    <div class="mb-3">
        <br />
        <asp:Label ID="Sentence1lbl" runat="server" Text="Sentence  (Your own personal sentence to help you memorize)"></asp:Label>
        <asp:TextBox ID="Sentence1Input" runat="server" CssClass="form-control" required="true"></asp:TextBox>
    </div>

    <div class="mb-3">
        <br />
        <asp:Button ID="SubmitBtn" runat="server" Text="Submit" CssClass="btn btn-primary btn-lg btn-block" OnClick="SubmitBtn_Click" />
    </div>
    <br />
    <asp:Label ID="SubmitResponse" runat="server"></asp:Label>
    <br />
</asp:Content>
