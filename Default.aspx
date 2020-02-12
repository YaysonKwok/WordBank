<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WordBank._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron container-fluid" style="padding: 15px 15px 15px 15px;">
        <h2 class="display-4">Word Bank</h2>
        <hr class="my-5">
        <p class="lead">WordBank is a free website that helps you retain your new vocabulary. It has the following features:</p>
        <p>
            1. Input new words, definitions, and usage sentences.
            <br />
            2. Review through multiple-choice quizzes given words.
            <br />
            3. Review through multiple-choice quizzes given definitions.
            <br />
            4. Review troubled words more frequently through a configurable computer algorithm.
            <br />
            5. Import new words, definitions, and usage sentences from a csv file.
            <br />
            6. Export your new vocabulary to a csv file.
        </p>
        <a class="btn btn-primary btn-lg" href="/Account/Register.aspx" role="button">Sign up today!</a>
    </div>
</asp:Content>
