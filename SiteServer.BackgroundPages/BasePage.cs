﻿using System;
using System.Web.UI;
using BaiRong.Core;
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Plugin;

namespace SiteServer.BackgroundPages
{
    public class BasePage : Page
    {
        private MessageUtils.Message.EMessageType _messageType;
        private string _message = string.Empty;
        private string _scripts = string.Empty;

        protected virtual bool IsAccessable => false; // 页面默认情况下是不能直接访问

        protected virtual bool IsSinglePage => false; // 是否为单页（即是否需要放在框架页内运行,false表示需要）

        protected bool IsForbidden { get; private set; }

        public RequestContext Body { get; private set; }

        private void SetMessage(MessageUtils.Message.EMessageType messageType, Exception ex, string message)
        {
            _messageType = messageType; 
            _message = ex != null ? $"{message}<!-- {ex} -->" : message;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Body = new RequestContext();

            if (!IsAccessable && !Body.IsAdminLoggin) // 如果页面不能直接访问且又没有登录则直接跳登录页
            {
                IsForbidden = true;
                PageUtils.RedirectToLoginPage();
            }

            //防止csrf攻击
            Response.AddHeader("X-Frame-Options", "SAMEORIGIN");
            //tell Chrome to disable its XSS protection
            Response.AddHeader("X-XSS-Protection", "0");
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(_message))
            {
                MessageUtils.SaveMessage(_messageType, _message);
            }

            base.Render(writer);

            if (!IsAccessable && !IsSinglePage) // 页面不能直接访问且不是单页，需要加一段框架检测代码，检测页面是否运行在框架内
            {
                writer.Write($@"<script type=""text/javascript"">
if (window.top.location.href.toLowerCase().indexOf(""main.aspx"") == -1){{
	window.top.location.href = ""{PageInitialization.GetRedirectUrl()}"";
}}
</script>");
            }

            if (!string.IsNullOrEmpty(_scripts))
            {
                writer.Write(@"<script type=""text/javascript"">{0}</script>", _scripts);
            }
        }

        public void AddScript(string script)
        {
            _scripts += script;
        }

        public void AddWaitAndRedirectScript(string redirectUrl)
        {
            _scripts += $@"
setTimeout(function() {{
    location.href = '{redirectUrl}';
}}, 1500);
$('.operation-area').hide();
";
        }

        public void AddWaitAndScript(string scripts)
        {
            _scripts += $@"
setTimeout(function() {{
    {scripts}
}}, 1500);
$('.operation-area').hide();
";
        }

        public void FailMessage(Exception ex, string message)
        {
            SetMessage(MessageUtils.Message.EMessageType.Error, ex, message);
        }

        public void FailMessage(string message)
        {
            SetMessage(MessageUtils.Message.EMessageType.Error, null, message);
        }

        public void SuccessMessage(string message)
        {
            SetMessage(MessageUtils.Message.EMessageType.Success, null, message);
        }

        public void SuccessMessage()
        {
            SuccessMessage("操作成功！");
        }

        public void InfoMessage(string message)
        {
            SetMessage(MessageUtils.Message.EMessageType.Info, null, message);
        }

        public void SuccessDeleteMessage()
        {
            SuccessMessage(MessageUtils.DeleteSuccess);
        }

        public void SuccessUpdateMessage()
        {
            SuccessMessage(MessageUtils.UpdateSuccess);
        }

        public void SuccessCheckMessage()
        {
            SuccessMessage(MessageUtils.CheckSuccess);
        }

        public void SuccessInsertMessage()
        {
            SuccessMessage(MessageUtils.InsertSuccess);
        }

        public void FailInsertMessage(Exception ex)
        {
            FailMessage(ex, MessageUtils.InsertFail);
        }

        public void FailUpdateMessage(Exception ex)
        {
            FailMessage(ex, MessageUtils.UpdateFail);
        }

        public void FailDeleteMessage(Exception ex)
        {
            FailMessage(ex, MessageUtils.DeleteFail);
        }

        public void FailCheckMessage(Exception ex)
        {
            FailMessage(ex, MessageUtils.CheckFail);
        }

        public string MaxLengthText(string str, int length)
        {
            return StringUtils.MaxLengthText(str, length);
        }

        public Control FindControlBySelfAndChildren(string controlId)
        {
            return ControlUtils.FindControlBySelfAndChildren(controlId, this);
        }

        public void VerifyAdministratorPermissions(params string[] permissionArray)
        {
            PermissionsManager.VerifyAdministratorPermissions(Body.AdminName, permissionArray);
        }

        public virtual void Submit_OnClick(object sender, EventArgs e)
        {
            LayerUtils.Close(Page);
        }

        public static string GetShowHintScript()
        {
            return GetShowHintScript("操作进行中");
        }

        public static string GetShowHintScript(string message)
        {
            return GetShowHintScript(message, 120);
        }

        public static string GetShowHintScript(string message, int top)
        {
            return $@"hideBoxAndShowHint(this, '{message}, 请稍候...', {top});";
        }

        public void ClientScriptRegisterClientScriptBlock(string key, string script)
        {
            if (!ClientScript.IsStartupScriptRegistered(key))
            {
                ClientScript.RegisterClientScriptBlock(GetType(), key, script);
            }
        }

        public void ClientScriptRegisterStartupScript(string key, string script)
        {
            if (!ClientScript.IsStartupScriptRegistered(key))
            {
                ClientScript.RegisterStartupScript(GetType(), key, script);
            }
        }

        public bool ClientScriptIsStartupScriptRegistered(string key)
        {
            return ClientScript.IsStartupScriptRegistered(key);
        }

        public static string GetShowImageScript(string imageClientId, string publishmentSystemUrl)
        {
            return GetShowImageScript("this", imageClientId, publishmentSystemUrl);
        }

        public static string GetShowImageScript(string objString, string imageClientId, string publishmentSystemUrl)
        {
            return
                $"showImage({objString}, '{imageClientId}', '{PageUtils.ApplicationPath}', '{publishmentSystemUrl}')";
        }

        public string SwalError(string title, string message)
        {
            var script = $@"swal({{
  title: '{title}',
  text: '{StringUtils.ReplaceNewline(message, string.Empty)}',
  icon: 'error',
  button: '关 闭',
}});";
            ClientScript.RegisterClientScriptBlock(GetType(), nameof(SwalError), script, true);

            return script;
        }

        public string SwalDom(string title, string elementId)
        {
            var script = $@"swal({{
  title: '{title}',
  content: $('#{elementId}')[0],
  button: '关 闭',
}});";
            ClientScript.RegisterClientScriptBlock(GetType(), nameof(SwalDom), script, true);

            return script;
        }
    }
}
