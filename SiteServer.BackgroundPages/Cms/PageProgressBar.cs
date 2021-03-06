using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.BackgroundPages.Ajax;
using SiteServer.BackgroundPages.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Create;
using SiteServer.CMS.Model.Enumerations;

namespace SiteServer.BackgroundPages.Cms
{
    public class PageProgressBar : BasePageCms
    {
        public Literal LtlTitle;
        public Literal LtlRegisterScripts;

        protected override bool IsSinglePage => true;

        public static readonly string CookieNodeIdCollection = "SiteServer.BackgroundPages.Cms.BackgroundProgressBar.NodeIDCollection";

        public static readonly string CookieContentIdentityCollection = "SiteServer.BackgroundPages.Cms.BackgroundProgressBar.ContentIdentityCollection";

        public static readonly string CookieTemplateIdCollection = "SiteServer.BackgroundPages.Cms.BackgroundProgressBar.TemplateIDCollection";

        public static string GetCreatePublishmentSystemUrl(int publishmentSystemId, bool isUseSiteTemplate, bool isImportContents, bool isImportTableStyles, string siteTemplateDir, bool isUseTables, string userKeyPrefix, string returnUrl)
        {
            return PageUtils.GetCmsUrl(nameof(PageProgressBar), new NameValueCollection
            {
                {"CreatePublishmentSystem", true.ToString()},
                {"PublishmentSystemID", publishmentSystemId.ToString()},
                {"IsUseSiteTemplate", isUseSiteTemplate.ToString()},
                {"IsImportContents", isImportContents.ToString()},
                {"IsImportTableStyles", isImportTableStyles.ToString()},
                {"SiteTemplateDir", siteTemplateDir},
                {"isUseTables", isUseTables.ToString()},
                {"UserKeyPrefix", userKeyPrefix},
                {"returnUrl", returnUrl}
            });
        }

        public static string GetCreatePublishmentSystemUrl(int publishmentSystemId, bool isUseSiteTemplate, bool isImportContents, bool isImportTableStyles, string siteTemplateDir, bool isUseTables, string userKeyPrefix, string returnUrl, bool isTop)
        {
            return PageUtils.GetCmsUrl(nameof(PageProgressBar), new NameValueCollection
            {
                {"CreatePublishmentSystem", true.ToString()},
                {"PublishmentSystemID", publishmentSystemId.ToString()},
                {"IsUseSiteTemplate", isUseSiteTemplate.ToString()},
                {"IsImportContents", isImportContents.ToString()},
                {"IsImportTableStyles", isImportTableStyles.ToString()},
                {"SiteTemplateDir", siteTemplateDir},
                {"isUseTables", isUseTables.ToString()},
                {"UserKeyPrefix", userKeyPrefix},
                {"returnUrl", returnUrl},
                {"isTop", isTop.ToString()}
            });
        }

        public static string GetCreatePublishmentSystemUrl(int publishmentSystemId, bool isUseSiteTemplate, bool isImportContents, bool isImportTableStyles, string siteTemplateDir, bool isUseTables, string userKeyPrefix, string returnUrl, bool isTop, bool isCreateAll)
        {
            return PageUtils.GetCmsUrl(nameof(PageProgressBar), new NameValueCollection
            {
                {"CreatePublishmentSystem", true.ToString()},
                {"PublishmentSystemID", publishmentSystemId.ToString()},
                {"IsUseSiteTemplate", isUseSiteTemplate.ToString()},
                {"IsImportContents", isImportContents.ToString()},
                {"IsImportTableStyles", isImportTableStyles.ToString()},
                {"SiteTemplateDir", siteTemplateDir},
                {"isUseTables", isUseTables.ToString()},
                {"UserKeyPrefix", userKeyPrefix},
                {"returnUrl", returnUrl},
                {"isTop", isTop.ToString()},
                {"isCreateAll", isCreateAll.ToString()}
            });
        }

        public static string GetBackupUrl(int publishmentSystemId, string backupType, string userKeyPrefix)
        {
            return PageUtils.GetCmsUrl(nameof(PageProgressBar), new NameValueCollection
            {
                {"PublishmentSystemID", publishmentSystemId.ToString()},
                {"Backup", true.ToString()},
                {"BackupType", backupType},
                {"UserKeyPrefix", userKeyPrefix}
            });
        }

        public static string GetRecoveryUrl(int publishmentSystemId, string isDeleteChannels, string isDeleteTemplates, string isDeleteFiles, bool isZip, string path, string isOverride, string isUseTable, string userKeyPrefix)
        {
            return PageUtils.GetCmsUrl(nameof(PageProgressBar), new NameValueCollection
            {
                {"PublishmentSystemID", publishmentSystemId.ToString()},
                {"Recovery", true.ToString()},
                {"IsDeleteChannels", isDeleteChannels},
                {"IsDeleteTemplates", isDeleteTemplates},
                {"IsDeleteFiles", isDeleteFiles},
                {"IsZip", isZip.ToString()},
                {"Path", path},
                {"IsOverride", isOverride},
                {"IsUseTable", isUseTable},
                {"UserKeyPrefix", userKeyPrefix}
            });
        }

        public static string GetDeleteAllPageUrl(int publishmentSystemId, ETemplateType templateType)
        {
            return PageUtils.GetCmsUrl(nameof(PageProgressBar), new NameValueCollection
            {
                {"PublishmentSystemID", publishmentSystemId.ToString()},
                {"TemplateType", ETemplateTypeUtils.GetValue(templateType)},
                {"DeleteAllPage", true.ToString()}
            });
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            var userKeyPrefix = Body.GetQueryString("UserKeyPrefix");

            if (Body.IsQueryExists("CreatePublishmentSystem"))
            {
                LtlTitle.Text = "新建站点";
                var pars = AjaxCreateService.GetCreatePublishmentSystemParameters(PublishmentSystemId, Body.GetQueryBool("IsUseSiteTemplate"), Body.GetQueryBool("IsImportContents"), Body.GetQueryBool("IsImportTableStyles"), Body.GetQueryString("SiteTemplateDir"), Body.GetQueryBool("IsUseTables"), userKeyPrefix, Body.GetQueryBool("isTop"), Body.GetQueryString("returnUrl"));
                LtlRegisterScripts.Text = AjaxManager.RegisterProgressTaskScript(AjaxCreateService.GetCreatePublishmentSystemUrl(), pars, userKeyPrefix, AjaxCreateService.GetCountArrayUrl(), true);
            }
            else if (Body.IsQueryExists("Backup") && Body.IsQueryExists("BackupType"))
            {
                LtlTitle.Text = "数据备份";

                var parameters =
                    AjaxBackupService.GetBackupParameters(PublishmentSystemId, Body.GetQueryString("BackupType"), userKeyPrefix);
                LtlRegisterScripts.Text = AjaxManager.RegisterWaitingTaskScript(AjaxBackupService.GetBackupUrl(), parameters);
            }
            else if (Body.IsQueryExists("Recovery") && Body.IsQueryExists("IsZip"))
            {
                LtlTitle.Text = "数据恢复";
                var parameters = AjaxBackupService.GetRecoveryParameters(PublishmentSystemId,
                    Body.GetQueryBool("IsDeleteChannels"), Body.GetQueryBool("IsDeleteTemplates"),
                    Body.GetQueryBool("IsDeleteFiles"), Body.GetQueryBool("IsZip"),
                    PageUtils.UrlEncode(Body.GetQueryString("Path")), Body.GetQueryBool("IsOverride"),
                    Body.GetQueryBool("IsUseTable"), userKeyPrefix);
                LtlRegisterScripts.Text = AjaxManager.RegisterWaitingTaskScript(AjaxBackupService.GetRecoveryUrl(), parameters);
            }
            else if (Body.IsQueryExists("DeleteAllPage") && Body.IsQueryExists("TemplateType"))
            {
                DeleteAllPage();
            }
            else if (Body.IsQueryExists("CreateIndex"))
            {
                CreateIndex();
            }
        }

        //生成首页
        private void CreateIndex()
        {
            LtlTitle.Text = "生成首页";
            var link = new HyperLink
            {
                NavigateUrl = PageUtility.GetIndexPageUrl(PublishmentSystemInfo, false),
                Text = "浏览"
            };
            if (link.NavigateUrl != PageUtils.UnclickedUrl)
            {
                link.Target = "_blank";
            }
            link.Style.Add("text-decoration", "underline");
            try
            {
                CreateManager.CreateChannel(PublishmentSystemId, PublishmentSystemId);
                //FileSystemObject FSO = new FileSystemObject(base.PublishmentSystemID);

                //FSO.AddIndexToWaitingCreate();

                LtlRegisterScripts.Text = @"
<script>
$(document).ready(function(){
    writeResult('首页生成成功。', '');
})
</script>
";
            }
            catch (Exception ex)
            {
                LtlRegisterScripts.Text = $@"
<script>
$(document).ready(function(){{
    writeResult('', '{ex.Message}');
}})
</script>
";
            }
        }

        private void DeleteAllPage()
        {
            var templateType = ETemplateTypeUtils.GetEnumType(Body.GetQueryString("TemplateType"));

            if (templateType == ETemplateType.ChannelTemplate)
            {
                LtlTitle.Text = "删除已生成的栏目页文件";
            }
            else if (templateType == ETemplateType.ContentTemplate)
            {
                LtlTitle.Text = "删除所有已生成的内容页文件";
            }
            else if (templateType == ETemplateType.FileTemplate)
            {
                LtlTitle.Text = "删除所有已生成的文件页";
            }

            try
            {
                if (templateType == ETemplateType.ChannelTemplate)
                {
                    var nodeIdList = DataProvider.NodeDao.GetNodeIdListByPublishmentSystemId(PublishmentSystemId);
                    DirectoryUtility.DeleteChannelsByPage(PublishmentSystemInfo, nodeIdList);
                }
                else if (templateType == ETemplateType.ContentTemplate)
                {
                    var nodeIdList = DataProvider.NodeDao.GetNodeIdListByPublishmentSystemId(PublishmentSystemId);
                    DirectoryUtility.DeleteContentsByPage(PublishmentSystemInfo, nodeIdList);
                }
                else if (templateType == ETemplateType.FileTemplate)
                {
                    DirectoryUtility.DeleteFiles(PublishmentSystemInfo, DataProvider.TemplateDao.GetTemplateIdListByType(PublishmentSystemId, ETemplateType.FileTemplate));
                }

                Body.AddSiteLog(PublishmentSystemId, LtlTitle.Text);

                LtlRegisterScripts.Text = @"
<script>
$(document).ready(function(){
    writeResult('任务执行成功。', '');
})
</script>
";
            }
            catch (Exception ex)
            {
                LtlRegisterScripts.Text = $@"
<script>
$(document).ready(function(){{
    writeResult('', '{ex.Message}');
}})
</script>
";
            }
        }
    }
}
