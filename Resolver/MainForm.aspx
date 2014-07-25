<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="Resolver.MainForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Resolver</title>
    <style>
        body {
             color: black;
             font-family: Georgia;
             text-align: center;
        }
        input[type="submit"] {
             background: blue;
             color: white;
             border: 0px;
        }
        input{
             font-size: 18px;
             padding: 5px;
             font-family: Georgia;
        }
        input[type="text"] {
             width: 300px;
        }
    </style>
</head>
<body>
    <form id="mainform" runat="server">
    <div>
        <img src="Res/resolver.png" height="175" width="400"/><br />
        <asp:TextBox ID="inputbox" runat="server" />
        <asp:Button id="submit" text="Work out" onClick="submit_Click" runat="server" />
        
        <br /><br />
        <asp:Label ID="result" runat="server" Text="" />
        <br /><a href="mailto:lucamozzo995@hotmail.it">Report errors</a>
    </div>
        
    </form>
</body>
</html>
