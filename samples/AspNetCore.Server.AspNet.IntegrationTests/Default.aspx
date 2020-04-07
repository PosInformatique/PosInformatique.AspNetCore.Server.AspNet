<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PosInformatique.AspNetCore.Server.AspNet.IntegrationTests.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="jquery-3.4.1.js"></script>
</head>
<body>
<%--    <form id="form1" runat="server">
        
    </form>--%>
    <div>
        <h1>WEB API</h1>
        <ul>
            <li>
                <a href="api/values">GET api/values</a>
            </li>
            <li>
                <a href="api/values/1234">GET api/values/1234</a>
            </li>
            <li>
                <a href="api/values/json">GET api/values/json</a>
            </li>
            <li>
                <a href="api/values/json?q1=9999&q2=zzzz">GET api/values/json?q1=9999&q2=zzzz</a>
            </li>
            <li>
                <a href="api/values/actionWith/a-param/eter">GET api/values/actionWith/a-param/eter</a>
            </li>
            <li>
                <a href="api/values/error404">GET api/values/error404</a>
            </li>
            <li>
                <a href="api/values/exception">GET api/values/exception</a>
            </li>
            <li>
                <a href="api/values/request_comparison?a=1&b=2">GET api/values/request_comparison</a>
            </li>
            <li>
                <a href="swagger/">Swagger</a>
            </li>
            <li>
                <input type="submit" value="api/values/postData" id="apiValuesPostData" />

                <script type="text/javascript">
                    $("#apiValuesPostData").click(function () {
                        $.ajax("api/values/postData", {
                            data: JSON.stringify({ "StringValue": "The string value" }),
                            contentType: 'application/json',
                            type: 'POST',
                            success: function (result, textStatus, request) {
                                $("#apiValuesPostDataContent").text(JSON.stringify(result));
                                $("#apiValuesPostDataStatus").text(textStatus);
                                $("#apiValuesPostDataContentType").text(request.getResponseHeader("Content-Type"));
                                $("#apiValuesPostDataMyHeader").text(request.getResponseHeader("MyHeader"));
                            },
                            error: function () {
                                alert("FAILED !");
                            }
                        });

                    });
                </script>

                <p>Results:</p>
                <ul>
                    <li>Contents: <span id="apiValuesPostDataContent" /></li>
                    <li>Status: <span id="apiValuesPostDataStatus" /></li>
                    <li>[HEADER] Content-Type: <span id="apiValuesPostDataContentType" /></li>
                    <li>[HEADER] MyHeader: <span id="apiValuesPostDataMyHeader" /></li>
                </ul>
            </li>
        </ul>
    </div>
</body>
</html>
