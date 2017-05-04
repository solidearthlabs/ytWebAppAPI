<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="YoutubeAPIWebApplication.SampleForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        /*#full, #DivKeyword {
            height: 100%;
            display: flex;
            flex-direction: column;
        }*/
        #DropDownListMonthsAgo, #btnSearch, #txtMaxSubscribers {
            /*padding: 0 0 0 100px;*/
            /*top right bottom left*/ 
            margin: 0 10px 0 10px;
            flex-grow: 1;
        }

        #Label3 {
            /*padding: 0 0 0 100px;*/
            /*top right bottom left*/ 
            margin: 0 10px 0 10px;
        }
        #GridViewKeywordSearchResults {
            /*padding: 0 0 0 100px;*/
            /*top right bottom left*/ 
            margin: 10px;
            flex-grow: 1;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="full">
    
        <asp:Panel ID="Panel1" runat="server">
            <div>
                <asp:Label ID="Label1" runat="server" Text="Youtube data API Sample"></asp:Label>
            </div>
            <div>
                <asp:Label ID="Label2" runat="server" Text="Video ID:"></asp:Label>
                <asp:TextBox ID="txtVideoID" runat="server" Width="219px"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="Get Youtube Data" OnClick="Button1_Click" />
            </div>
            <asp:Panel ID="Panel2" runat="server">
                <div>
                    Title: 
                    <asp:Label ID="LabelTitle" runat="server" Text=""></asp:Label>
                </div>
                <div>
                    Publish Date:
                    <asp:Label ID="LabelPublishDate" runat="server" Text=""></asp:Label>
                </div>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server" DefaultButton="btnSearch">
                <div id="DivKeyword">
                    Keyword Search:
                    <asp:TextBox ID="txtKeyword" runat="server" Width="223px"></asp:TextBox>
                    <asp:DropDownList ID="DropDownListMonthsAgo" runat="server">
                        <asp:ListItem Text="1 Month Ago" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="2 Months Ago" Value="2"></asp:ListItem>
                        <asp:ListItem Text="6 Months Ago" Value="6"></asp:ListItem>
                        <asp:ListItem Text="1 Year Ago" Value="12"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <asp:TableCell><asp:Label runat="server" Text="Min Subscribers"></asp:Label></asp:TableCell>
                            <asp:TableCell><asp:TextBox ID="txtMinSubscribers" runat="server" Text="3"></asp:TextBox></asp:TableCell>
                            <asp:TableCell><asp:Label runat="server" Text="Max Subscribers"></asp:Label></asp:TableCell>
                            <asp:TableCell><asp:TextBox ID="txtMaxSubscribers" runat="server" Text="300"></asp:TextBox></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell><asp:Label ID="Label4" runat="server" Text="Min Likes:"></asp:Label></asp:TableCell>
                            <asp:TableCell><asp:TextBox ID="txtMinLikes" runat="server" Text="10"></asp:TextBox></asp:TableCell>
                            
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell><asp:Label ID="Label6" runat="server" Text="Min Comments:"></asp:Label></asp:TableCell>
                            <asp:TableCell><asp:TextBox ID="txtMinComments" runat="server" Text=""></asp:TextBox></asp:TableCell>
                            <%--<asp:TableCell><asp:Label ID="Label7" runat="server" Text="Max Comments:"></asp:Label></asp:TableCell>
                            <asp:TableCell><asp:TextBox ID="txtMaxComments" runat="server" Text=""></asp:TextBox></asp:TableCell>--%>
                        </asp:TableRow>

                    </asp:Table>
                    <asp:Label ID="Label5" runat="server" Text="Max Results:"></asp:Label>
                    <asp:TextBox ID="txtMaxResults" runat="server" Text="300"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                Num Items:
                <asp:Label ID="lblNumItems" runat="server" Text=""></asp:Label>
                    
                </div>
                <asp:GridView ID="GridViewKeywordSearchResults" runat="server" Width="90%" EmptyDataText="Empty">
                    <AlternatingRowStyle BackColor="#EEEEEE" />
                    <HeaderStyle BackColor="#FFFFEC" Font-Bold="True" Font-Size="Large" Wrap="False" />
                    <SelectedRowStyle BackColor="#FFFFCC" />
                </asp:GridView>
            </asp:Panel>
        </asp:Panel>
        
    
    </div>
    </form>
</body>
</html>
