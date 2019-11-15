﻿<%@ Page Title="New Words" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Input.aspx.cs" Inherits="WordBank.Input" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="mb-3">
        <br />
        <br />
        <asp:Label ID="Redirectlbl" runat="server" Visible="False"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Wordlbl" runat="server" Text="Word"></asp:Label>
        <asp:TextBox ID="WordInput" runat="server" CssClass="form-control" required="true"></asp:TextBox>
    </div>

    <div class="mb-3">
         <br />
        <asp:Label ID="Definitionlbl" runat="server" Text="Definition"></asp:Label>
        <asp:TextBox ID="DefinitionInput" runat="server" CssClass="form-control" required="true" TextMode="multiline"></asp:TextBox>
    </div>


    <div class="mb-3">
        <br />
        <asp:Label ID="Sentence1lbl" runat="server" Text="Sentence  (Your own personal sentence to help you memorize)"></asp:Label>
        <asp:TextBox ID="Sentence1Input" runat="server" CssClass="form-control" required="true" TextMode="multiline"></asp:TextBox>
    </div>

        <div class="mb-3">
        <br />
        <asp:Label ID="Sentence2lbl" runat="server" Text="Sentence  (Contextual sentence where you encountered the word)"></asp:Label>
        <asp:TextBox ID="Sentence2Input" runat="server" CssClass="form-control" required="true" TextMode="multiline"></asp:TextBox>
    </div>

    <div class="mb-3">
        <br />
        <asp:CheckBox ID="InformalCheckBox" runat="server" Text="Informal" />
        <br />
        <br />
        <asp:Button ID="SubmitBtn" runat="server" Text="Submit" CssClass="btn btn-primary btn-sm" OnClick="SubmitBtn_Click" UseSubmitBehavior="False" />
    </div>
    <br />
    <asp:Label ID="SubmitResponse" runat="server"></asp:Label>
    <br />
</asp:Content>
