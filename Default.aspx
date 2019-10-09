<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WordBank._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Word Bank</h1>
        <p class="lead">Word Bank is a free online website dedicated to improving your vocabulary!</p>
        <p><a href="~/Input" class="btn btn-primary btn-lg">Try it out here&raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Getting started</h2>
            <p>
                Try it out here!
            </p>
            <p>
                <a class="btn btn-default" href="~/Input">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Future Info here </h2>
            <p>
                Test
            </p>
            <p>
                <a class="btn btn-default" href="~/Input">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Test</h2>
            <p>
                Test3
            </p>
            <p>
                <a class="btn btn-default" href="~/Input">Learn more &raquo;</a>
            </p>
        </div>
    </div>

</asp:Content>
