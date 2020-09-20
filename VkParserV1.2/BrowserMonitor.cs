using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Awesomium.Core;
using Awesomium.Web;
using Awesomium.ComponentModel;
using Awesomium.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;
using System.Threading;
using System.Net;

namespace VkParserV1._2
{
    public partial class BrowserMonitor : UserControl
    {
        WebSession session;

        #region
        public string parametrBrowser;
        #endregion

        

        public BrowserMonitor()
        {
            //session = WebCore.CreateWebSession(new WebPreferences()
            //{
            //    SmoothScrolling = true,
            //    CustomCSS = "body { background-color : rgb(153, 255, 204); }"
            //});

            InitializeComponent();
            #region событие прихода ответа в браузер
            webControl1.LoadingFrameComplete += Awesomium_Windows_Forms_WebControl_LoadingFrameComplete;
            #endregion
        }

        #region обработка события прихода отвта браузеру
        private void Awesomium_Windows_Forms_WebControl_LoadingFrameComplete(object sender, Awesomium.Core.FrameEventArgs e)
        {
            XmlDocument docc = new XmlDocument();
            if (e.IsMainFrame)
            {
                string WebcontrolHTML = webControl1.HTML;
                string WebcontrolAbsoluteUrl = webControl1.Source.AbsoluteUri;
                string WebcontrolTitle = webControl1.Title;

                #region
                ////длинна принятого html документа
                ////int a = webControl1.HTML.Length;
                //string b = webControl1.HTML;
                ////webSessionProvider1.DataPath = @"C:\sdfg";
                ////Аддрес html документа
                //var site = webControl1.Source.AbsoluteUri;
                ////var img = webControl1
                //var sst = webControl1.Title;
                ////textBox3.Text = site;
                #endregion

                if (WebcontrolAbsoluteUrl.Length > 44)
                    {
                    //st = WebcontrolAbsoluteUrl.Substring(0, 41);


                    #region получение друзей онлайн
                    UserOnlineXML useronlinexml = new UserOnlineXML();
                    string LenghtWebcontrolAbsoluteUrl = WebcontrolAbsoluteUrl.Substring(0, 52);
                    if (LenghtWebcontrolAbsoluteUrl == "https://api.vk.com/method/friends.getOnline?user_id=")
                    {
                        string mmysite = webControl1.ExecuteJavascriptWithResult("document.body.children[0].innerHTML").ToString();
                        docc = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(mmysite);
                        useronlinexml.WHUserOnlineXML(docc.SelectSingleNode("response"));
                        StaticClass.BrowserFLAG = true;
                    }
                    #endregion

                    #region Проверка состоят ли дорузья в группе
                    LenghtWebcontrolAbsoluteUrl = WebcontrolAbsoluteUrl.Substring(0, 51);
                    if (LenghtWebcontrolAbsoluteUrl == "https://api.vk.com/method/groups.isMember?group_id=")
                    {
                        string mmysite = webControl1.ExecuteJavascriptWithResult("document.body.children[0].innerHTML").ToString();
                        docc = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(mmysite, "user");
                        useronlinexml.WHUserOnlineMemberGroup(docc.SelectSingleNode("user"));
                        StaticClass.BrowserFLAG = true;
                    }
                    #endregion

                    #region приглагние пользователю отправленно
                    UserInviteInGroup userinviteingroup;
                    LenghtWebcontrolAbsoluteUrl = WebcontrolAbsoluteUrl.Substring(0, 49);
                    if (LenghtWebcontrolAbsoluteUrl == "https://api.vk.com/method/groups.invite?group_id=")
                    {
                        userinviteingroup = new UserInviteInGroup();
                        string mmysite = webControl1.ExecuteJavascriptWithResult("document.body.children[0].innerHTML").ToString();
                        docc = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(mmysite, "user");
                        
                        userinviteingroup.WHUserInviteInGroup(docc.SelectSingleNode("user"));

                        StaticClass.BrowserFLAG = true;
                        return;
                    }
                    #endregion

                    #region операции по получению сообщений с группы ОДИНОЧНАЯ 
                    LenghtWebcontrolAbsoluteUrl = WebcontrolAbsoluteUrl.Substring(0, 44);
                    //обработка запроса на получение сообщений со стены группы
                    if (LenghtWebcontrolAbsoluteUrl == "https://api.vk.com/method/wall.get?owner_id=")
                        {
                            groupwallparserxml = new GroupWallParserXML();

                            //// Сам html документ
                            string mmysite = webControl1.ExecuteJavascriptWithResult("document.body.children[0].innerHTML").ToString();
                            docc = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(mmysite);
                            GroupWallParserXMLStartThread(docc.SelectSingleNode("response"));
                            StaticClass.BrowserFLAG = true;

                        //ThreedigStarted(docc);
                        //xmlvallgroupparser.ParseXml(docc.SelectSingleNode("response"));
                        //Program.StopedWhileParametr = false;
                        ////xmlvallgroupparser.GroupVallParserXml(docc.SelectSingleNode("response"));
                        }
                    #endregion

                    #region
                    LenghtWebcontrolAbsoluteUrl = WebcontrolAbsoluteUrl.Substring(0, 58);
                    if (LenghtWebcontrolAbsoluteUrl == "https://api.vk.com/method/photos.getUploadServer?group_id=")
                    {
                        UploadImage upi = new UploadImage();
                        string mmysite = webControl1.ExecuteJavascriptWithResult("document.body.children[0].innerHTML").ToString();
                        docc = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(mmysite);
                        XmlNode ddocc = docc.SelectSingleNode("response");
                        //upi.param(ddocc);
                        upi.UploadFuncion(docc, @"C:\\Users\\albert\\Desktop\\VkParserV1.2\\VkParserV1.2\\VkParserV1.2\\bin\\Debug\\img\\v621916973\\90b4\\BWrJOKDG-sU.jpg");
                    }
                    #endregion

                    #region информация о группе
                    LenghtWebcontrolAbsoluteUrl = WebcontrolAbsoluteUrl.Substring(0, 50);
                    if (LenghtWebcontrolAbsoluteUrl == "https://api.vk.com/method/groups.getById?group_id=")
                    {
                        string documentLending = webControl1.ExecuteJavascriptWithResult("document.body.children[0].innerHTML").ToString();
                        StaticClass.DocumentGroupInfo = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(documentLending);
                        StaticClass.DocumentGroupInfoLoadedFLAG = true;
                        StaticClass.BrowserFLAG = true;
                        

                    }
                    #endregion

                    #region инфорация о пользователе
                    LenghtWebcontrolAbsoluteUrl = WebcontrolAbsoluteUrl.Substring(0, 45);
                    if (LenghtWebcontrolAbsoluteUrl == "https://api.vk.com/method/users.get?user_ids=")
                    {
                        UserInfoParseXML userinfoparsexml = new UserInfoParseXML();
                        string documentLendingg = webControl1.ExecuteJavascriptWithResult("document.body.children[0].innerHTML").ToString();
                        docc = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(documentLendingg, "item");
                        userinfoparsexml.WHXMLUserInfoParser(docc);
                    }
                    #endregion

                    #region инфорация о репостах
                    LenghtWebcontrolAbsoluteUrl = WebcontrolAbsoluteUrl.Substring(0, 51);
                    if (LenghtWebcontrolAbsoluteUrl == "https://api.vk.com/method/wall.getReposts?owner_id=")
                    {
                        UserInfoParseXML userinfoparsexml = new UserInfoParseXML();
                        string documentLendingg = webControl1.ExecuteJavascriptWithResult("document.body.children[0].innerHTML").ToString();
                        docc = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(documentLendingg, "HeaderXml");
                        userinfoparsexml.WHUserRepostedParserXML(docc);
                    }
                    #endregion

                    #region Комплекс операциий по освоению аккаунта
                    #region операции по получению токена
                    //нажимаем кнопку разрешить права
                    LenghtWebcontrolAbsoluteUrl = WebcontrolAbsoluteUrl.Substring(0, 40);
                //http://oauth.vk.com/authorize?client_id=
                    if (LenghtWebcontrolAbsoluteUrl == "http://oauth.vk.com/authorize?client_id=")
                    {
                        webControl1.ExecuteJavascript("document.getElementByName('email').value='yaaal@mail.ru';");
                        webControl1.ExecuteJavascript("document.getElementByName('pass').value='3фзф3ф1989';");
                        webControl1.ExecuteJavascript("document.getElementById('install_allow').click();");


                        //webControl1.ExecuteJavascript("document.getElementById('install_allow').click();");
                    }
                    LenghtWebcontrolAbsoluteUrl = WebcontrolAbsoluteUrl.Substring(0, 41);
                    if (LenghtWebcontrolAbsoluteUrl == "https://oauth.vk.com/authorize?client_id=")
                    {
                        //webControl1.ExecuteJavascript("document.getElementById('install_allow').click();");
                        //webControl1.ExecuteJavascript("function onclick(event) {  return allow(button);}");
                        webControl1.ExecuteJavascript("element.getElementsByClassName('flat_button fl_r button_indent').click();");

                    }
                    LenghtWebcontrolAbsoluteUrl = WebcontrolAbsoluteUrl.Substring(0, 44);
                    //получение токена
                    if (LenghtWebcontrolAbsoluteUrl == "https://oauth.vk.com/blank.html#access_token")
                        {
                            string accessToken = "";
                            int userId = 0;
                            Regex myReg = new Regex(@"(?<name>[\w\d\x5f]+)=(?<value>[^\x26\s]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            foreach (Match m in myReg.Matches(WebcontrolAbsoluteUrl))
                            {
                                if (m.Groups["name"].Value == "access_token")
                                {
                                    accessToken = m.Groups["value"].Value;
                            }
                            else if (m.Groups["name"].Value == "user_id")
                                {
                                    userId = Convert.ToInt32(m.Groups["value"].Value);
                                }
                            }
                        //formUpdate(accessToken, userId);
                        StaticClass.AutorisationParametr[0] = accessToken;
                        StaticClass.AutorisationParametr[1] = Convert.ToString(userId);
                        StaticClass.BrowserFLAG = true;
                        ThreadFlagStart();

                    }
                    }
                #endregion
                    #region операция авторизации
                if (WebcontrolAbsoluteUrl == "https://vk.com/")
                    {
                        webControl1.ExecuteJavascript("document.getElementById('index_email').value='yaaal@mail.ru';");
                        webControl1.ExecuteJavascript("document.getElementById('index_pass').value='3фзф3ф1989';");
                        webControl1.ExecuteJavascript("document.getElementById('index_login_button').click();");
                    }
                #endregion
                    #region запуск операций по получению токена после успешной авторизации
                if (WebcontrolAbsoluteUrl == "https://vk.com/feed")
                    {
                    
                        addToken();
                    }
                #endregion
                #endregion

            }

        }

        #region поток для отправки полученного xml файла передача на обработку в отдельном потоке
        #region Подключаемые классы
        GroupWallParserXML groupwallparserxml;
        Thread GroupWallParserXMLThread;
        #endregion
        private void GroupWallParserXMLStartThread(XmlNode n)
        {
            GroupWallParserXMLThread = new Thread(GroupWallParserXMLParametr);
            GroupWallParserXMLThread.Start(n);
        }

        private void GroupWallParserXMLParametr(object n)
        {
            XmlNode node = (XmlNode)n;
            groupwallparserxml = new GroupWallParserXML();
            groupwallparserxml.WHParseXml(node);
        }
        #endregion

        #region поток для выставления переменной флага

        Thread ThreadFlag;
        private void ThreadFlagStart()
        {
            ThreadFlag = new Thread(ThreadFlagOk);
            ThreadFlag.Start();

        }

        private void ThreadFlagOk()
        {
            StaticClass.AutorisationComplitedFlag = true;
            
        }
        #endregion

        #region запрос токена
        private void addToken()
        {
            //webControl1.Source = (String.Format("http://api.vkontakte.ru/oauth/authorize?client_id={0}&scope={1}&display=popup&response_type=token", Program.appId, Program.scope)).ToUri();
            webControl1.Source = (String.Format("http://oauth.vk.com/authorize?client_id={0}&display=touch&redirect_uri=https://oauth.vk.com/blank.html&scope={1}&response_type=token&v=5.68&state=123456", Program.appId, Program.scope)).ToUri();
        }
        #endregion

        #region авторизация
        #region делегаты для доступа
        delegate void BAUTORISATION(string url);
        BAUTORISATION bautorisation;

        #endregion
        //, string password, string login
        public void WHbrowserAutorisation(string url)
        {
            bautorisation = new BAUTORISATION(browserAutorisation);
            Invoke(bautorisation, url);
            //browserAutorisation(url,password,login);

        }
        //, string password, string login
        private void browserAutorisation(string url)
        {
            webControl1.Source = url.ToUri();
        }
        #endregion

        #region Входные параметры в браузер

        delegate void BROWSERURL(string url);
        BROWSERURL browseurl;

        public void WHBrowseUrl(string url)
        {
            browseurl = new BROWSERURL(BrowseUrl);
            Invoke(browseurl, url);
        }
        private void BrowseUrl(string url)
        {
            webControl1.Source = url.ToUri();
        }

        #endregion
        #endregion
    }
}
