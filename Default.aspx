<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WordBank._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Word Bank</h1>
        <p class="lead">Word Bank is a free online website dedicated to improving your vocabulary!</p>
    </div>

    <div class="row">
        <div class="col-md-8">
            <h2>Want to test out your knowledge of English vocabulary?</h2>
            <p>
                Try out a quiz here!
            </p>
            <p>
                <a class="btn btn-default" href="~/Input">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Interested but not sold?</h2>
            <p>
                How about a test run of the site?
            </p>
            <p>
                <a class="btn btn-default" href="~/Input">Learn more &raquo;</a>
            </p>
        </div>
    </div>

</asp:Content>
