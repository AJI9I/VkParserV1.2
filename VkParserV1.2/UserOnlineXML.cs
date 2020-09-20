using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.Entity;

namespace VkParserV1._2
{
    class UserOnlineXML
    {
        #region обработка пользователей онлайн
        public void WHUserOnlineXML(XmlNode xml)
        {
            UserOnlineXml(xml);
        }

        private void UserOnlineXml(XmlNode xml)
        {
            XmlNodeList xmllistUserOnline = xml.SelectNodes("online");
            string[] UserOnlineUserArr = new string[xmllistUserOnline.Count];
            int i= 0;
            foreach (XmlNode node in xmllistUserOnline)
            {
                UserOnlineUserArr[i] = node.InnerText;
                i++;
            }

            XmlNodeList xmllistUserOnlineMobile = xml.SelectNodes("online_mobile");
            string[] UserOnlineUserMobileArr = new string[xmllistUserOnlineMobile.Count];
            int b = 0;

            foreach (XmlNode node in xmllistUserOnlineMobile)
            {
                UserOnlineUserMobileArr[b] = node.InnerText;
                b++;
            }
            users(UserOnlineUserMobileArr, UserOnlineUserArr);

        }

        private void users(string[] UserOnlineUserMobileArr, string[] UserOnlineUserArr)
        {
            string usersMobile="";
            int a = 0;
            int countusermobile = UserOnlineUserMobileArr.Count() - 1;
            foreach (string user in UserOnlineUserMobileArr)
            {
                if (countusermobile == a)
                {
                    usersMobile += user;
                }
                else
                {
                    usersMobile += user + ",";
                }
                a++;
            }

            string users = "";
            int b = 0;
            int countuse = UserOnlineUserArr.Count() - 1;
            foreach (string user in UserOnlineUserArr)
            {
                if (countuse == b)
                {
                    users += user;
                }
                else
                {
                    users += user + ",";
                }
                b++;
            }

            StaticClass.UsersOnlineMetodGet = new object[2];
            StaticClass.UsersOnlineMetodGet[0] = users;
            StaticClass.UsersOnlineMetodGet[1] = usersMobile;
        }
        #endregion

        #region обработка ответа состоят ли запрошенные пользователи в группе
        public void WHUserOnlineMemberGroup(XmlNode xml)
        {
            UserOnlineMemberGroup(xml);
        }

        private void UserOnlineMemberGroup(XmlNode xml)
        {
            XmlNodeList nodelist = xml.SelectNodes("response");
            string[] usersNotGroup = new string[CountNotMemberGroup(nodelist)];
            int i = 0;
            foreach (XmlNode node in nodelist)
            {
                if (node.SelectSingleNode("invitation") == null)
                {
                    if (node.SelectSingleNode("member").InnerText == "0")
                    {
                        usersNotGroup[i] = node.SelectSingleNode("user_id").InnerText;
                        i++;
                    }
                }
            }

            StaticClass.UserOnlineNotMemberGroup = usersNotGroup;
        }

        private int CountNotMemberGroup(XmlNodeList nodelist)
        {
            int count = 0;
            foreach (XmlNode node in nodelist)
            {
                if (node.SelectSingleNode("invitation") == null)
                {
                    if (node.SelectSingleNode("member").InnerText == "0")
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        #endregion
    } 
     
    #region ответ браузера после приглашения
    class UserInviteInGroup
    {
        public void WHUserInviteInGroup(XmlNode xml)
        {
            UserInvite(xml);
        }

        private void UserInvite(XmlNode xml)
        {
            if (xml.SelectSingleNode("response") != null)
            {
                StaticClass.invitestats = new string[1];
                StaticClass.invitestats[0] = xml.SelectSingleNode("response").InnerText;
            }
            if (xml.SelectSingleNode("error") != null)
            {
                StaticClass.invitestats = new string[2];
                StaticClass.invitestats[0] = xml.SelectSingleNode("error").SelectSingleNode("error_code").InnerText;
                StaticClass.invitestats[1] = xml.SelectSingleNode("error").SelectSingleNode("error_text").InnerText;
            }
        }
        
        private void UserAddDb()
        {
           
        }
        
    }
    #endregion

    #region контекст для проверки пользователей которым отправленно приглашение
    class UserInviteGroup
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public int Invite { get; set; }
        public bool UserMember { get; set; }
        public string pole { get; set; }
    }

    class ContextUserInviteGroup : DbContext
    {
        public ContextUserInviteGroup() : base("DbConnection") { }
        public DbSet<UserInviteGroup> UserInviteGroups { get; set; }
    }
    #endregion
}
