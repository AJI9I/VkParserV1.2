using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Controls;

namespace VkParserV1._2
{
    public partial class Form1 : Form
    {
        #region Подключаеммые классы
        //BrowserMonitor browsermonitor;

        WebBrowserMonitor webbrowsermonitor;

        UserAccount useraccount;
        Grabber grabber;
        Content content;
        CronSetings cronsetings;
        //XML Parser
        GroupInfoParserXML groupinfoparserxml;
        //авторизация
        ProfileInfo profileinfo;
        #endregion

        #region Отрисовка формы
        public Form1()
        {
            InitializeComponent();
            #region подключение и установка браузера для обмена с внешним миром
            //browsermonitor = new BrowserMonitor();
            webbrowsermonitor = new WebBrowserMonitor();
            MonitorAdd();
            #endregion
            //Запуск авториации
            AutorisationThreding();

            //Запуск чека статуса браузера
            BrowserThreadStatusStart();
            this.Size = new Size(800, 485);
        }
        #endregion

        #region Состояние браузера
        Thread BrowserThreadStatus;

        delegate void BROWSERSTAUSDELEGATE(string status);
        BROWSERSTAUSDELEGATE browserstatusdelegate;

        private void BrowserThreadStatusStart()
        {
            BrowserThreadStatus = new Thread(browserstatus);
            BrowserThreadStatus.Start();

        }

        private void browserstatus()
        {
            browserstatusdelegate = new BROWSERSTAUSDELEGATE(AddBrowserStatuLabel);
            while (true)
            {
                Thread.Sleep(200);
                if (StaticClass.BrowserFLAG == true)
                    Invoke(browserstatusdelegate, "Свободен");
                if (StaticClass.BrowserFLAG == false)
                    Invoke(browserstatusdelegate, "Занят");
            }
        }

        private void AddBrowserStatuLabel(string status)
        {
            this.label2.Text = status;
        }

        #endregion

        #region Шапка с параметрами авторизации
        //12; 5 локация

        delegate void ProfileInfoAddDelegate();
        ProfileInfoAddDelegate profileinfodelegate;
        private void CallProfileInfoDelegate()
        {
            profileinfodelegate = new ProfileInfoAddDelegate(ProfileInfoVisible);
            Invoke(profileinfodelegate);
        }

        bool userProfileInfoAdded = false;
        private void ProfileInfoAdd()
        {
            Controls.Add(profileinfo);
            userProfileInfoAdded = true;
        }

        private void ProfileInfoClose()
        {
            //767; 44
            profileinfo.Size = new Size(1, 1);
            profileinfo.Location = new Point(1, 1);
        }

        private void ProfileInfoDispose()
        {
            profileinfo.Dispose();
            userProfileInfoAdded = false;
        }

        private void ProfileInfoVisible()
        {
            if (profileinfo == null || !profileinfo.Created)
                profileinfo = new ProfileInfo();
            profileinfo.Size = new Size(767, 44);
            profileinfo.Location = new Point(12, 5);
            if (!userProfileInfoAdded)
                ProfileInfoAdd();

        }
        #endregion

        #region тестовый блок
        public string testParametr { get { return testParametr; }
            set {
                testParametr = value;
                obr();
            }
        }

        private void obr()
        {
            string gffdf = "тарам пама пам";
        }
        #endregion

        #region переменные размеров контролов управления
        int UserControlHeight = 359;
        int UserControlWidth = 658;

        int UserControlLocationX = 114;
        int UserControlLocationY = 55;

        int UserControlClosedHeight = 1;
        int UserControlClosedWidth = 1;

        int UserControlClosedLocationX = 1;
        int UserControlClosedLocationY = 1;
        #endregion

        #region Удаление элементов управления из области видимости с формы НЕ ОЧИСТКА РЕСУРСОВ!!!
        #region Флаги активности контролов
        bool UserAccountFLAG = false;
        bool UserGrabberFLAG = false;
        bool UserContentFLAG = false;
        bool UserCronSetingsFLAG = false;

        #endregion
        private void ClosedUserControlForm()
        {
            if (UserAccountFLAG)
                AccountControllClose();
            if (UserGrabberFLAG)
                GrabberClose();
            if (UserContentFLAG)
                ContentClose();
            if (UserCronSetingsFLAG)
                CronSetingsClose();
        }
        #endregion

        #region Код Задания CronSetings cronsetings;

        bool userCronSetingsAdded = false;

        private void button4_Click(object sender, EventArgs e)
        {
            ClosedUserControlForm();
            CronSetingsVisible();
        }

        private void CronSetingsAdd()
        {
            Controls.Add(cronsetings);
            userCronSetingsAdded = true;
        }

        private void CronSetingsClose()
        {
            cronsetings.Size = new Size(UserControlClosedWidth, UserControlClosedHeight);
            cronsetings.Location = new Point(UserControlClosedLocationX, UserControlClosedLocationY);
            UserCronSetingsFLAG = false;
        }

        private void CronSetingsDispose()
        {
            cronsetings.Dispose();
            userCronSetingsAdded = false;
            UserCronSetingsFLAG = false;
        }

        private void CronSetingsVisible()
        {
            if (cronsetings == null || !cronsetings.Created)
                cronsetings = new CronSetings();
            cronsetings.Size = new Size(UserControlWidth, UserControlHeight);
            cronsetings.Location = new Point(UserControlLocationX, UserControlLocationY);
            if (!userCronSetingsAdded)
                CronSetingsAdd();
            UserCronSetingsFLAG = true;

        }
        #endregion

        #region Код контент
        bool userContentAdded = false;

        private void button3_Click(object sender, EventArgs e)
        {
            ClosedUserControlForm();
            ContentVisible();
        }

        private void ContentAdd()
        {
            Controls.Add(content);
            userContentAdded = true;
        }

        private void ContentClose()
        {
            content.Size = new Size(UserControlClosedWidth, UserControlClosedHeight);
            content.Location = new Point(UserControlClosedLocationX, UserControlClosedLocationY);
            UserContentFLAG = false;
        }

        private void ContentDispose()
        {
            content.Dispose();
            userContentAdded = false;
            UserContentFLAG = false;
        }

        private void ContentVisible()
        {
            if (content == null || !content.Created)
                content = new Content();
            content.Size = new Size(UserControlWidth, UserControlHeight);
            content.Location = new Point(UserControlLocationX, UserControlLocationY);
            if (!userContentAdded)
                ContentAdd();
            UserContentFLAG = true;

        }
        #endregion

        #region Граббер
        #region Код граббер
        bool userGraberAdded = false;

        private void button2_Click(object sender, EventArgs e)
        {
            ClosedUserControlForm();
            GrabberVisible();
        }

        private void GrabberAdd()
        {
            Controls.Add(grabber);
            userGraberAdded = true;
        }

        private void GrabberClose()
        {
            grabber.Size = new Size(UserControlClosedWidth, UserControlClosedHeight);
            grabber.Location = new Point(UserControlClosedLocationX, UserControlClosedLocationY);
            UserGrabberFLAG = false;
            GrabberHaxuiButtonForm();
        }

        private void GrabberDispose()
        {
            grabber.Dispose();
            userGraberAdded = false;
            UserGrabberFLAG = false;
        }

        private void GrabberVisible()
        {
            if (grabber == null || !grabber.Created)
                grabber = new Grabber();
            grabber.Size = new Size(UserControlWidth, UserControlHeight);
            grabber.Location = new Point(UserControlLocationX, UserControlLocationY);
            if (!userGraberAdded)
                GrabberAdd();
            UserGrabberFLAG = true;

            GrabberAddButtonForm();

        }

        private void GrabberAddButtonForm()
        {
            button7.Location = new Point(300, 64);
        }
        private void GrabberHaxuiButtonForm()
        {
            button7.Location = new Point(919, 23);
        }
        #endregion

        #region Кнопка ограбить
        private void button7_Click(object sender, EventArgs e)
        {
            GetGroupId();
        }

        private void GetGroupId()
        {
            //grabber.GetFormParametr();
            ThreadStartGetGroupInfo(grabber.GetFormParametr());

        }
        #endregion
        #endregion

        #region Код управления аккаунтами
        bool userAccountControlAdded = false;
        //Добавляет контрол для управления
        private void button1_Click(object sender, EventArgs e)
        {
            ClosedUserControlForm();
            AccountControllVisible();

        }

        private void AccountControllAdd()
        {
            Controls.Add(useraccount);
            userAccountControlAdded = true;
        }

        //Скрывает контрол из области видимости но код остается рабочий и потоки продолжают выполняться
        private void AccountControllClose()
        {
            useraccount.Size = new Size(UserControlClosedWidth, UserControlClosedHeight);
            useraccount.Location = new Point(UserControlClosedLocationX, UserControlClosedLocationY);
            UserAccountFLAG = false;
        }

        //Очищает ресурсы контроллла
        private void AcoounControlDispose()
        {
            useraccount.Dispose();
            userAccountControlAdded = false;
            UserAccountFLAG = false;
        }

        private void AccountControllVisible()
        {
            if (useraccount == null || !useraccount.Created)
                useraccount = new UserAccount();
            useraccount.Size = new Size(UserControlWidth, UserControlHeight);
            useraccount.Location = new Point(UserControlLocationX, UserControlLocationY);
            if (!userAccountControlAdded)
                AccountControllAdd();
            UserAccountFLAG = true;
        }
        #endregion

        #region Обслуживание функций открыть и закрыть монитор со всеми прилагающимися переменными
        //Кнопка открыть и закрыть монитор
        #region переменные расположение\\размеров
        int monitoWidht = 760;
        int monitorHight = 140;
        int monitorLocationX = 12;
        int monitorLocationY = 449;
        int monitorMinSizeHight = 1;
        int monitorMaxSizeHightFormControl = 600;
        int monitorMinSizeHightFormControl = 450;
        bool monitorVisible;
        #endregion
        private void button5_Click(object sender, EventArgs e)
        {
            if (monitorVisible)
            {
                MonitorOFF();
            }
            else { MonitorON(); };
        }

        private void MonitorON()
        {
            ClientSize = new Size(ClientSize.Width, monitorMaxSizeHightFormControl);
            webbrowsermonitor.Size = new Size(monitoWidht, monitorHight);
            monitorVisible = true;
            button5.Text = "Закрыть монитор";

        }
        private void MonitorOFF()
        {
            ClientSize = new Size(ClientSize.Width, monitorMinSizeHightFormControl);
            webbrowsermonitor.Size = new Size(monitoWidht, monitorMinSizeHight);
            monitorVisible = false;
            button5.Text = "Открыть монитор";
        }

        private void MonitorAdd()
        {
            webbrowsermonitor.Location = new Point(monitorLocationX, monitorLocationY);
            webbrowsermonitor.Size = new Size(monitoWidht, monitorMinSizeHight);
            this.Controls.Add(webbrowsermonitor);
            monitorVisible = false;
            button5.Text = "Открыть монитор";
        }




        #endregion

        #region получение информации о пользователяю сделавших репосты в группах
        Thread UserInfoRepostedGroupPostThread;

        private void UserInfoRepostedGroupPostThreadStart()
        {

        }

        private void UserInfoRepostedGroupPostWhile()
        {
            while (true)
            {
                //string[] ParametrGroupParser = 
                if (StaticClass.BrowserFLAG)
                {
                    StaticClass.BrowserFLAG = false;
                }
            }
        }

        //тут надо добавить информацию о постах
        private void GetParametrUsersAndGroupPosts()
        {
            using (ContextUserAndGroup db = new ContextUserAndGroup())
            {
                //var tt = db.UserAndGroups
            }
        }

        private void UserInfoRepostedGroupPost(string Group_id, string Post_id, string Offset, string count)
        {
            webbrowsermonitor.WHBrowseUrl(String.Format("https://api.vk.com/method/wall.getReposts.xml?owner_id=-32718884&post_id=17513&offset=&count=1000&v=5.40&access_token={0}", StaticClass.AutorisationParametr[0]));
        }
        #endregion

        #region поток на ожидание авторизации

        Thread ThreadingAutorisationComplited;

        private void AutorisationThreding()
        {
            ThreadingAutorisationComplited = new Thread(autorisationbrobsercomplited);
            ThreadingAutorisationComplited.Start();
        }

        public void Autorisation()
        {

            bool autorisationfalg = true;
            //, "3фзф3ф1989", "yaaal@mail.ru"
            webbrowsermonitor.WHbrowserAutorisation(String.Format("https://oauth.vk.com/authorize?client_id={0}&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope={1}&response_type=token&v=5.74&state=123456 ", Program.appId, Program.scope));
            while (autorisationfalg)
            {
                Thread.Sleep(200);
                if (StaticClass.AutorisationComplitedFlag)
                {
                    //ProfileInfoVisible();
                    CallProfileInfoDelegate();
                    profileinfo.AddTextLabelDelegateStart();
                    autorisationfalg = false;
                }

            }


        }

        private void autorisationbrobsercomplited()
        {
            bool whileopen = true;
            while (whileopen)
            {
                Thread.Sleep(200);
                if (StaticClass.BrowserFLAG)
                {
                    StaticClass.BrowserFLAG = false;
                    Autorisation();
                    whileopen = false;
                }
            }
        }

        #endregion

        #region получить информацию о сообществе
        Thread ThreadGroupInfo;
        Thread ThreadGroupInfoAdded;

        private void ThreadStartGetGroupInfo(string groupId)
        {
            ThreadGroupInfo = new Thread(GetGroupInfo);
            ThreadGroupInfo.Start(groupId);
        }

        private void GetGroupInfo(object groupid)
        {
            StaticClass.GroupInfoWhile = true;
            StaticClass.DocumentGroupInfoLoadedFLAG = false;
            while (StaticClass.GroupInfoWhile)
            {
                if (StaticClass.BrowserFLAG)
                {
                    StaticClass.BrowserFLAG = false;
                    webbrowsermonitor.WHBrowseUrl(String.Format("https://api.vk.com/method/groups.getById.xml?group_id={0}&fields=members_count&v=5.74&access_token={1}", (string)groupid, StaticClass.AutorisationParametr[0]));

                    while (StaticClass.GroupInfoWhile)
                    {
                        if (StaticClass.DocumentGroupInfoLoadedFLAG)
                        {
                            groupinfoparserxml = new GroupInfoParserXML();
                            //string[] ParametrGroup = ;
                            ThreadStartAddGroupInfoInGrabber(groupinfoparserxml.XmlGroupAddParametr(StaticClass.DocumentGroupInfo));
                            StaticClass.GroupInfoWhile = false;
                        }
                        Thread.Sleep(200);
                    }
                }
                Thread.Sleep(200);
            }
        }

        private void ThreadStartAddGroupInfoInGrabber(string[] arr)
        {
            ThreadGroupInfoAdded = new Thread(AddGroupInfoInGrabber);
            ThreadGroupInfoAdded.Start(arr);

        }
        private void AddGroupInfoInGrabber(object arr)
        {
            grabber.InvokeMetod((string[])arr);
            AddGrupGrabberThreadButtot();

        }

         System.Windows.Forms.Button GroupGrabberStart;

        delegate void addControllsButton(System.Windows.Forms.Button bt);
        addControllsButton addcontrollsbutton;
        private void AddGrupGrabberThreadButtot()
        {
            if (GroupGrabberStart == null)
            {
                GroupGrabberStart = new System.Windows.Forms.Button();
                GroupGrabberStart.Click += GroupGrabberStart_Click;
                GroupGrabberStart.Name = "GroupGrabber";
                GroupGrabberStart.Size = new Size(100,20);
                GroupGrabberStart.Text = "Запустить";
                GroupGrabberStart.Location = new Point(600, 200);
                //Controls.Add(GroupGrabberStart);
                //GroupGrabberStart.BringToFront();
                addcontrollsbutton = new addControllsButton(InvokeDelegate);
                Invoke(addcontrollsbutton, GroupGrabberStart);
            }
            
        }

        private void InvokeDelegate(System.Windows.Forms.Button bt)
        {
            Controls.Add(bt);
            bt.BringToFront();
        }


        private void GroupGrabberStart_Click(object sender, EventArgs e)
        {
            object[] parametr = grabber.GetParametrGrabber();
            GetGroupWallOnePostThread(parametr);
        }

        #endregion

        #region Граббер групп
        #region Получениеп первой записи с группы для оценки фронта работы
        Thread GetOnePostWallThread;
        private void GetGroupWallOnePostThread(object[] parametr)
        {
            object[] param =  parametr;
            GetOnePostWallThread = new Thread(GetGroupWallOnePost);
            GetOnePostWallThread.Start(param);
        }

        private void GetGroupWallOnePost(object parametr)
        {
            object[] parametrr = (object[])parametr;
            string groupID = "-"+(string)parametrr[2];
            bool stop = true;

            while (stop)
            {
                if (StaticClass.BrowserFLAG)
                {
                    StaticClass.BrowserFLAG = false;
                    
                    StaticClass.GroupParserCountPostsBoll = true;
                    WallContent(groupID, 0, 1);
                    stop = false;
                }
                
            }
            GetGroupWallOnePostStopedWhile();
            if (StaticClass.GroupParserCountPosts != 0)
            {
                ThreedingStarterParser(groupID);
            }

        }

        private void GetGroupWallOnePostStopedWhile()
        {
            bool stop = true;
            while (stop)
            {
                if (!StaticClass.GroupParserCountPostsBoll)
                {
                    stop = false;
                }
            }
        }
        #endregion
        #region получение записи с группы
        Thread GroupParserThreading;
        Thread GroupParserUrlBrowserThreading;

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    ThreedingStarterParser();
        //}
        //Запуск процесса граббера групп
        private void ThreedingStarterParser(object id)
        {
            GroupParserThreading = new Thread(OffsetPostsGroup);
            GroupParserThreading.Start(id);
        }

        #region
        ////Получает ответ от браузера и передает на обработку
        //private void ThreedigStarted(XmlNode xmlResponse)
        //{
        //    thread = new Thread(CountGroupPosts);
        //    thread.Start(xmlResponse);

        //}
        #endregion


        private void StopedWhile(string group, int offset, int count)
        {
            while (StaticClass.StoppedWhileGroupParser)
            {
                if(StaticClass.BrowserFLAG)
                {
                StaticClass.BrowserFLAG = false;
                WallContent(group, offset, count);
                StaticClass.StoppedWhileGroupParser = false;
                }
                Thread.Sleep(200);
            }
        }

        //Смещение значний для получения записей со стены
        private void OffsetPostsGroup(object id)
        {
            string groupID = (string)id;
            int countPost = StaticClass.GroupParserCountPosts;
            int offset;
            int count = 100;

            for (offset = StaticClass.GroupParserCountPosts; offset > 0; offset -= 100)
            {
                if (countPost < 100 || offset < 0 )
                {
                    offset = 0;
                }
                if (offset == StaticClass.GroupParserCountPosts && StaticClass.GroupParserCountPosts > 100)
                {
                    offset=offset - 100;
                }
                
                StaticClass.StoppedWhileGroupParser = true;
                StopedWhile(groupID, offset, count);
            }


            //for (StaticClass.GroupParserOffset = 0; StaticClass.GroupParserOffset < StaticClass.GroupParserCountPosts; StaticClass.GroupParserOffset += 100)
            //{
                
            //}
            //надо добавить условие остановки из цикла

        }

        private void WallContent(string groupID, int offset, int count)
        {
            webbrowsermonitor.WHBrowseUrl((String.Format("https://api.vk.com/method/wall.get.xml?owner_id={0}&offset={1}&count={2}&filter=all&v=5.37&access_token={3}", groupID, offset, count, StaticClass.AutorisationParametr[0])));
        }

        #region получение общего количества постов
        //Обновление количества записей которое необходимо принять
        //private void CountGroupPosts(object xmlResponse)
        //{
        //    if (Program.CountGroupPosted == 999999999)
        //    {
        //        XmlNode xmlresponse = (XmlNode)xmlResponse;
        //        if (xmlresponse.FirstChild.FirstChild.Name == "count")
        //        {
        //            Program.CountGroupPosted = Convert.ToInt32(xmlresponse.FirstChild.FirstChild.InnerText);
        //        }
        //    }
        //}

        #endregion

        #region какой то вспомогательный вовод
        //private void textFormControl(int a)
        //{
        //    this.textBox1.AppendText(a + Environment.NewLine);
        //}
        #endregion

        #endregion
        #endregion

        #region потоки выполнения заданий
        Thread ChekedSetingThread;
        // Запуск сновного поставщика заданий
        // Пока без выхода из цикла
        private void ChekedSetting()
        {
            ChekedSetingThread = new Thread(ChekedSettingWhile);
            ChekedSetingThread.Start();
        }
        //поток в постоянной работе цикла ваил
        private void ChekedSettingWhile()
        {
            while (true)
            {
                Thread.Sleep(300);
                // определяется если выполнние каких то заданий или нет в процессе работы программы
                // если очередь свободна то идет просмотр база дынных на наличие заданий
                if(StaticClass.SettingPostawshik)
                {
                    // Очередь закрыватеся и идет поиск
                    StaticClass.SettingPostawshik = false;
                    ChekedSettingBd();
                }
            }
        }
        //запрос в таблицу на получение одного элемента с флагом не выполненного задания
        private void ChekedSettingBd()
        {
            using (ContextSetting db = new ContextSetting())
            {
                var  tt = db.Settings.Where(p => p.ComplitedResult == false).Take(1);
                //если количество не равно нулю
                if (tt.Count() != 0)
                {
                    //то определяется тип поставляемого задания для послудующей работы и сбора сопутствующих данных
                    switch (tt.First().TypeSetting)
                    {
                        case "Group":
                            //тут надо определить функцию которая будет обрабатывать задания связанные с группами либо как то классифицировать это все
                            break;
                    }
                }
                else
                {
                    // если запрос не дал результатов то поставщик переходит в вайл и ждет время выдержки на следующий опрос
                    // флага на поставку заданий
                    StaticClass.SettingPostawshik = true;
                }
            }
            
        }

        private void GroupSettingFind()
        {

        }
        #endregion

        #region сбор информации о друзьях
        private void userGroupMember()
        {
            if (StaticClass.UsersOnlineMetodGet == null)
            {
                UserOnlineGet();
            }
            string user = (string)StaticClass.UsersOnlineMetodGet[0];
            string userMobile = (string)StaticClass.UsersOnlineMetodGet[1];
            string u = user + userMobile;
            UserGroupIsMember(u);
        }

        private void UserOnlineGet()
        {
            bool flagWhile = true;
            while (flagWhile)
            { 
                if(StaticClass.BrowserFLAG)
                {
                    StaticClass.BrowserFLAG = false;
                    flagWhile = false;
                    StaticClass.UsersOnlineMetodGet = null;
                    webbrowsermonitor.WHBrowseUrl((String.Format("https://api.vk.com/method/friends.getOnline?user_id={0}&online_mobile=1&order=hints&v=5.37&access_token={1}", StaticClass.AutorisationParametr[1], StaticClass.AutorisationParametr[0])));
                }
            }
            while (StaticClass.UsersOnlineMetodGet == null)
            {
                Thread.Sleep(200);
            }
        }

        private void UserGroupIsMember(string user)
        {
            bool flagWhile = true;
            while (flagWhile)
            {
                if (StaticClass.BrowserFLAG)
                {
                    StaticClass.BrowserFLAG = false;
                    StaticClass.UserOnlineNotMemberGroup = null;
                    flagWhile = false;
                    webbrowsermonitor.WHBrowseUrl((String.Format("https://api.vk.com/method/groups.isMember?group_id={0}&user_ids={1}&extended=1&v=5.37&access_token={2}", "32718884", user, StaticClass.AutorisationParametr[0])));
                }
            }
            while (StaticClass.UserOnlineNotMemberGroup == null)
            {
                Thread.Sleep(200);
            }
            userAddGroup(StaticClass.UserOnlineNotMemberGroup, "32718884");
        }
        private void userAddGroup(string[] users, string groupid)
        {
            bool flagWhile;
            foreach (string user in users)
            {
                flagWhile = true;
                while (flagWhile)
                {
                    
                    if (StaticClass.BrowserFLAG)
                    {
                        StaticClass.BrowserFLAG = false;
                        flagWhile = true;
                        StaticClass.invitestats = null;
                        webbrowsermonitor.WHBrowseUrl((String.Format("https://api.vk.com/method/groups.invite?group_id={0}&user_id={1}&v=5.41&access_token={2}", groupid, user, StaticClass.AutorisationParametr[0])));
                    }
                }
                while (StaticClass.invitestats == null)
                {
                    Thread.Sleep(threadSleepRand());
                }

                statusAddForm(StaticClass.invitestats);
            }
        }
        private void statusAddForm(string[] cell)
        {

        }

        Random rnd;
        private int threadSleepRand()
        {
            rnd = new Random();
            return rnd.Next(500, 4000);
        }

        Thread UserMemberget;
        private void ThreadUserMemberStart()
        {
            UserMemberget = new Thread(userGroupMember);
            UserMemberget.Start();
        }
        #endregion

        #region Тестовая кнопка
        private void button6_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
