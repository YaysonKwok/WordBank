<%@ Page Title="Edit" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditWord.aspx.cs" Inherits="WordBank.EditWord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %> <asp:Label ID="WordTitle" runat="server" Text="Label"></asp:Label></h2>

    <div class="form-horizontal">
    <div class="mb-3">
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
        <asp:Label ID="Sentence1lbl" runat="server" Text="Sentence"></asp:Label>
        <asp:TextBox ID="Sentence1Input" runat="server" CssClass="form-control" required="true"></asp:TextBox>
    </div>

    <div class="mb-3">
        <br />
        <asp:CheckBox ID="InformalCheckBox" runat="server" Text=" Informal" Checked='<%# Eval("Informal") %>'/>
        <br />
        <br />
        <asp:Button ID="SubmitBtn" runat="server" Text="Submit" CssClass="btn btn-primary btn-sm" OnClick="EditWord_Click" UseSubmitBehavior="False"/>

        <asp:Button ID="CancelBtn" runat="server" Text="Cancel" CssClass="btn btn-primary btn-sm" OnClick="Cancel_Click" UseSubmitBehavior="False"/>
    </div>
    <br />
    <asp:Label ID="SubmitResponse" runat="server"></asp:Label>
    <br />

    </div>
</asp:Content>
