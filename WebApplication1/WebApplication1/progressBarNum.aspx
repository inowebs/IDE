<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="progressBarNum.aspx.vb" Inherits="WebApplication1.progressBarNum" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server"><base target="_self" />
    <title>Facturaselectronicascfdi.com Procesando</title>
    <style type="text/css">
        .auto-style1 {
            font-family: Arial, Helvetica, sans-serif;
        }
        .auto-style2 {
            font-size: large;
            color: #003399;
        }
    </style>
    </head>
<body>        
        <form id="Form1" method="post" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" updateMode="Conditional"> 
            <ContentTemplate>
                <asp:Label ID="Label2" runat="server" 
                    Text="Procesando, espere hasta que termine este proceso, es crucial no interrumpirlo " 
                    style="font-size: small; color: #003366" CssClass="auto-style1"></asp:Label>
                <br />
                <br />

                <div style="background-color:White; height:15px; width:300px">
                    <div id="bar" runat="server" style="height:15px; width:0px; background-color:LightSteelBlue">
                    </div>
                </div>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="0 %" CssClass="auto-style2"></asp:Label>
                <asp:Timer ID="TimerControl1" runat="server" OnTick="TimerControl1_Tick" Interval="1000">
                </asp:Timer>            

                <asp:Label id="lblErr" runat="server" 
                    style="text-align: center; color: #CC0000; font-family: Arial, Helvetica, sans-serif; font-size: small;"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        </form>
</body>
</html>
