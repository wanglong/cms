﻿<%@ Page Language="C#" Inherits="SiteServer.BackgroundPages.Cms.PageContentGroup" %>
  <%@ Register TagPrefix="ctrl" Namespace="SiteServer.BackgroundPages.Controls" Assembly="SiteServer.BackgroundPages" %>
    <!DOCTYPE html>
    <html>

    <head>
      <meta charset="utf-8">
      <!--#include file="../inc/head.html"-->
    </head>

    <body>
      <form class="m-l-15 m-r-15" runat="server">

        <div class="card-box">
          <ul class="nav nav-pills">
            <li class="nav-item">
              <a class="nav-link" href="pageNodeGroup.aspx?publishmentSystemId=<%=PublishmentSystemId%>">栏目组管理</a>
            </li>
            <li class="nav-item active">
              <a class="nav-link" href="pageContentGroup.aspx?publishmentSystemId=<%=PublishmentSystemId%>">内容组管理</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="pageContentTags.aspx?publishmentSystemId=<%=PublishmentSystemId%>">内容标签管理</a>
            </li>
          </ul>
        </div>

        <ctrl:alerts runat="server" />

        <div class="card-box">
          <div class="panel panel-default">
            <div class="panel-body p-0">
              <div class="table-responsive">
                <table id="contents" class="table tablesaw table-hover m-0">
                  <thead>
                    <tr>
                      <th>内容组名称 </th>
                      <th>内容组简介 </th>
                      <th width="50"></th>
                      <th width="50"></th>
                      <th width="120"></th>
                      <th width="60"></th>
                      <th width="60"></th>
                    </tr>
                  </thead>
                  <tbody>
                    <asp:Repeater ID="RptContents" runat="server">
                      <itemtemplate>
                        <tr>
                          <td>
                            <asp:Literal ID="ltlContentGroupName" runat="server"></asp:Literal>
                          </td>
                          <td>
                            <asp:Literal ID="ltlDescription" runat="server"></asp:Literal>
                          </td>
                          <td class="text-center">
                            <asp:HyperLink ID="hlUp" runat="server">
                              <i class="ion-arrow-up-a" style="font-size: 18px"></i>
                            </asp:HyperLink>
                          </td>
                          <td class="text-center">
                            <asp:HyperLink ID="hlDown" runat="server">
                              <i class="ion-arrow-down-a" style="font-size: 18px"></i>
                            </asp:HyperLink>
                          </td>
                          <td class="text-center">
                            <asp:Literal ID="ltlContents" runat="server"></asp:Literal>
                          </td>
                          <td class="text-center">
                            <asp:Literal ID="ltlEdit" runat="server"></asp:Literal>
                          </td>
                          <td class="text-center">
                            <asp:Literal ID="ltlDelete" runat="server"></asp:Literal>
                          </td>
                        </tr>
                      </itemtemplate>
                    </asp:Repeater>
                  </tbody>
                </table>
              </div>
            </div>
          </div>

          <hr />

          <asp:Button id="BtnAddGroup" class="btn btn-primary" text="添加内容组" runat="server" />

        </div>

      </form>
    </body>

    </html>
    <!--#include file="../inc/foot.html"-->