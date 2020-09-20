using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace VkParserV1._2
{
    public partial class WebBrowserMonitor : UserControl
    {
        public WebBrowserMonitor()
        {
            InitializeComponent();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (sender != null || e != null)
                {
                    string URl = e.Url.ToString();

                    string url = URl.Remove(41);


                    //"https://oauth.vk.com/authorize?client_id=4667366&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=408063&response_type=token&v=5.74&state=123456"
                    #region Авторизация https://vk.com/
                    if (url == "https://oauth.vk.com/authorize?client_id=")
                    {
                        HtmlDocument htm = webBrowser1.Document;
                        string Title = webBrowser1.Document.Title;

                        if (Title == "ВСоюзе | Вход")
                        {

                            this.webBrowser1.Document.GetElementById("email").SetAttribute("value", "yaaal@mail.ru");
                            this.webBrowser1.Document.GetElementById("pass").SetAttribute("value", "3фзф3ф1989");
                            this.webBrowser1.Document.GetElementById("install_allow").InvokeMember("click");
                        }
                        if (Title == "ВСоюзе | Разрешение доступа")
                        {
                            webBrowser1.Navigate("javascript: document.getElementsByClassName('button_indent')[0].click();");
                        }
                    }
                    #endregion

                    #region Парсим ответ на получение токена https://oauth.vk.com/blank.html#access_token
                    if (URl.Length > 44)
                        url = URl.Substring(0, 44);
                    //получение токена
                    if ((url == "https://oauth.vk.com/blank.html#access_token"))
                    {
                        string accessToken = "";
                        int userId = 0;
                        Regex myReg = new Regex(@"(?<name>[\w\d\x5f]+)=(?<value>[^\x26\s]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        foreach (Match m in myReg.Matches(URl))
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

                        StaticClass.AutorisationParametr[0] = accessToken;
                        StaticClass.AutorisationParametr[1] = Convert.ToString(userId);

                        //Выставляю флаг авторизации на успешный
                        StaticClass.BrowserFLAG = true;
                    }
                    #endregion

                    #region Ответ метода wall.get
                    if (URl.Length > 38)
                        url = url.Substring(0, 38);
                    //https://api.vk.com/method/wall.get.xml
                    if (url == "https://api.vk.com/method/wall.get.xml")
                    {

                        string htm = webBrowser1.Document.GetElementsByTagName("body")[0].InnerText;
                        htm = htm.Replace("\r\n- ", "");
                        htm = htm.Replace("\r\n ", "");
                        htm = htm.Replace("<Xyu>", "").Replace("<algour>", "");
                        htm = htm.Remove(0, 2);
                        //string documentLending = webControl1.ExecuteJavascriptWithResult("document.body.children[0].innerHTML").ToString();
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(htm);

                        if (StaticClass.GroupParserCountPostsBoll)
                        {
                            StaticClass.GroupParserCountPosts = Convert.ToInt32(xml.SelectSingleNode("response/count").InnerText);
                            StaticClass.GroupParserCountPostsBoll = false;
                        }
                        else
                        {
                            GroupWallParserXMLStartThread(xml.SelectSingleNode("response"));
                        }

                    }
                    #endregion

                    #region информация о группе
                    if (URl.Length > 44)
                        url = URl.Substring(0, 50);
                    if (url == "https://api.vk.com/method/groups.getById.xml?group")
                    {
                        string htm = webBrowser1.Document.GetElementsByTagName("body")[0].InnerText;
                        htm = htm.Replace("\r\n- ", "");
                        htm = htm.Replace("\r\n ", "");
                        htm = htm.Remove(0, 2);
                        //string documentLending = webControl1.ExecuteJavascriptWithResult("document.body.children[0].innerHTML").ToString();
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(htm);
                        StaticClass.DocumentGroupInfo = xml;
                        StaticClass.DocumentGroupInfoLoadedFLAG = true;



                    }
                    #endregion

                }
                StaticClass.BrowserFLAG = true;
            }
            catch { StaticClass.BrowserFLAG = true; }
            
        }

        #region авторизация
        #region делегаты для доступа
        delegate void BAUTORISATION(string url);
        BAUTORISATION bautorisation;

        #endregion

        public void WHbrowserAutorisation(string url)
        {
            bautorisation = new BAUTORISATION(browserAutorisation);
            Invoke(bautorisation, url);
        }

        private void browserAutorisation(string url)
        {
            webBrowser1.Navigate(url);
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
            webBrowser1.Navigate(url);
        }

        #endregion

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

    }
}
