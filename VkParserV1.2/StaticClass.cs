using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;
using System.Drawing;

namespace VkParserV1._2
{
    static class StaticClass
    {
        #region юзеры онлайн и просто мобаил
        public static object[] UsersOnlineMetodGet;

        public static string[] UserOnlineNotMemberGroup;

        public static int CountUserSendInvite;

        public static string[] invitestats;
        #endregion
        #region Работа вкладки контент
        public static Bitmap[] ImagePost;
        public static int i;
        #endregion
        #region флаги для работы парсра групп на контент
        public static bool StopedWhileGroupParserParametr;
        #endregion

        #region поставщик заданий
        //флаг есди заданий нет на выполнении и требуется получения нового задания
        public static bool SettingPostawshik = true;
        #endregion

        #region Переменные граббера груп
        public static bool StoppedWhileGroupParser;

        #region Смещение выборки парсера групп
        public static int GroupParserOffset = 0;
        public static int GroupParserCountPosts = 99999999;

        public static bool GroupParserCountPostsBoll = false;
        #endregion
        #endregion

        #region последние изменения значений регионов и городов
        public static int  LastCity_Id;
        public static int LastCountry_Id;
        #endregion
        #region параметры авторизации

        public static bool AutorisationComplitedFlag = false;

        //0 - токен
        //1 - ид юзера
        public static string[] AutorisationParametr = new string[2];
        #endregion

        #region Флаг занят ли браузер или нет
        public static bool BrowserFLAG = true;
        #endregion

        #region Инормация о группе
        public static XmlDocument DocumentGroupInfo;
        public static bool DocumentGroupInfoLoadedFLAG;
        public static bool GroupInfoWhile;

        #endregion
        #region параметр содржащий информацию о том занят или нет браузер в данный момент
        public static bool ExpectationBrowser
        {
            get { return ExpectationBrowser; }
            set { ExpectationBrowser = value; }

        }
        #endregion
        #region блокировка потоков локер
        static object locker = new object();
        #endregion
        #region Формирование заданий
        //Массив заданий
        public static string[] SettingBrowser;
        //Предельное количество заданий для очистки
        static int SettingCount = 50;
        //Статус присутствуют ли задания
        static bool SettingFLAG;
        #region Поток визор стар и переменная времени сна
        //Выход из вайла потока визора
        public static bool ThreadVisorEnableFLAG = true;
        //Время сна потока
        public static int ThreadVisorTimeSleep;
        //Номер очерезного задания
        public static int SettingsNumber;
        #endregion

        //Добавить задание
        public static void addSettingBrowser(string setting)
        {
            lock (locker)
            {
                int SettingMassCount = 0;
                string[] BufferMassSetting = SettingBrowser;
                SettingBrowser = new string[SettingCount + 1];
                for (int i = 0; i < SettingMassCount; i++)
                {
                    SettingBrowser[i] = BufferMassSetting[i];
                }
                SettingBrowser[SettingMassCount] = setting;
                if (SettingMassCount >= SettingCount)
                {
                    ThreadVisorTimeSleep = 1;
                }
            }
        }

        //Получить задание
        public static string getSettingBrowser()
        {
            lock (locker)
            {
                string returnSetting = SettingBrowser[SettingsNumber];
                SettingBrowser[SettingsNumber] = null;
                SettingsNumber++;
                return returnSetting;

            }

        }

        public static void SettigClear()
        {
            lock (locker)
            {
                if (SettingBrowser.Count() != 0)
                {
                    int SettintNoNullCount = 0;
                    string[] BufferSettingBrowser;
                    foreach (string STRING in SettingBrowser)
                    {
                        if (STRING != null)
                        {
                            SettintNoNullCount++;
                        }

                    }
                    BufferSettingBrowser = new string[SettintNoNullCount];
                    int b = 0;
                    for (int i = 0; i < SettingBrowser.Length - 1; i++)
                    {
                        if (SettingBrowser[i] != null)
                        {
                            BufferSettingBrowser[b] = SettingBrowser[i];
                            b++;
                        }
                    }
                    SettingBrowser = new string[SettintNoNullCount];
                    SettingBrowser = BufferSettingBrowser;
                    SettingsNumber = 0;
                    ThreadVisorTimeSleep = 999999999;
                }

            }
            #endregion

            
    }
}
    #region
    class TreedigVisor
    {
        Thread ThreadVisor;

        public void ThreadVisorStarter()
        {
            ThreadVisor = new Thread(TreadVisorBegin);
            ThreadVisor.Start();
        }

        private void TreadVisorBegin()
        {
            while (StaticClass.ThreadVisorEnableFLAG)
            {
                Thread.Sleep(StaticClass.ThreadVisorTimeSleep);
                StaticClass.SettigClear();
            }
        }
    }
    #endregion
}
