<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AspNetWebForms.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <p>This is the default.aspx page executed by the ASP .NET infrastructure</p>
            <ul>
                <li><a href="OtherPage.aspx">Other ASPX page</a></li>
                <li><a href="/mvc/">/mvc/ page (ASP .NET Core project) => Home/Index</a></li>
                <li><a href="/mvc/home/redirection302about/">/mvc/home/redirection302about/ page (ASP .NET Core project) => Home/Redirection302</a></li>
            </ul>
        </div>
    </form>
</body>
</html>
