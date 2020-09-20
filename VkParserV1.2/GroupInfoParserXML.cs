using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.Entity;
using System.Threading;

namespace VkParserV1._2
{
    class GroupInfoParserXML
    {
        Thread ThreadGroupInfo;

        public string[] XmlGroupAddParametr(XmlDocument xml)
        { 
            XmlNode xxml = xml.SelectSingleNode("response/group");
            string[] GroupInfoArr = new string[12];
            GroupInfoArr[0] = xxml.SelectSingleNode("id").InnerText;
            GroupInfoArr[1] = xxml.SelectSingleNode("name").InnerText;
            GroupInfoArr[2] = xxml.SelectSingleNode("screen_name").InnerText;
            GroupInfoArr[3] = xxml.SelectSingleNode("is_closed").InnerText;
            GroupInfoArr[4] = xxml.SelectSingleNode("type").InnerText;
            if (xxml.SelectSingleNode("is_admin") != null)
            GroupInfoArr[5] = xxml.SelectSingleNode("is_admin").InnerText;
            if (xxml.SelectSingleNode("admin_level") != null)
                GroupInfoArr[6] = xxml.SelectSingleNode("admin_level").InnerText;
            if (xxml.SelectSingleNode("is_member") != null)
                GroupInfoArr[7] = xxml.SelectSingleNode("is_member").InnerText;
            GroupInfoArr[8] = xxml.SelectSingleNode("members_count").InnerText;
            GroupInfoArr[9] = xxml.SelectSingleNode("photo_50").InnerText;
            GroupInfoArr[10] = xxml.SelectSingleNode("photo_100").InnerText;
            GroupInfoArr[11] = xxml.SelectSingleNode("photo_200").InnerText;

            StartThreadAddGroupInfoBd(GroupInfoArr);
            return GroupInfoArr;
        }

        private void StartThreadAddGroupInfoBd(string[] GroupInfoParametr)
        {
            ThreadGroupInfo = new Thread(AddBdGroupInfoFind);
            ThreadGroupInfo.Start(GroupInfoParametr);

        }

        private void AddBdGroupInfoFind(object GroupInfoParametr)
        {
            string[] GroupInfoParametrString = (string[])GroupInfoParametr;

            using (ContextGroupInfo db = new ContextGroupInfo())
            {
                int paramGroupId = Convert.ToInt32(GroupInfoParametrString[0]);
                var tt = db.GroupInfos.Where(p => p.group_id == paramGroupId);
                if (tt == null ||tt.Count() == 0)
                {
                    addInfoBd(GroupInfoParametrString);
                }
                else
                {
                    GroupInfoo gu = db.GroupInfos.Where(p => p.group_id == paramGroupId).FirstOrDefault();

                    gu.name = GroupInfoParametrString[1];
                    gu.screen_name = GroupInfoParametrString[2];
                    gu.is_closed = Convert.ToInt32(GroupInfoParametrString[3]);
                    gu.type = GroupInfoParametrString[4];
                    gu.is_admin = Convert.ToInt32(GroupInfoParametrString[5]);
                    gu.admin_level = Convert.ToInt32(GroupInfoParametrString[6]);
                    gu.is_member = Convert.ToInt32(GroupInfoParametrString[7]);
                    gu.members_count = Convert.ToInt32(GroupInfoParametrString[8]);
                    gu.photo_50 = GroupInfoParametrString[9];
                    gu.photo_100 = GroupInfoParametrString[10];
                    gu.photo_200 = GroupInfoParametrString[11];

                    db.SaveChanges();
                }
            }
        }

        private void addInfoBd(string[] GroupInfoParametr)
        {
            using (ContextGroupInfo db = new ContextGroupInfo())
            {
                GroupInfoo GI = new GroupInfoo {group_id = Convert.ToInt32(GroupInfoParametr[0]),
                    name = GroupInfoParametr[1],
                    screen_name = GroupInfoParametr[2],
                    is_closed = Convert.ToInt32(GroupInfoParametr[3]),
                    type = GroupInfoParametr[4],
                    is_admin = Convert.ToInt32(GroupInfoParametr[5]),
                    admin_level = Convert.ToInt32(GroupInfoParametr[6]),
                    is_member = Convert.ToInt32(GroupInfoParametr[7]),
                    members_count = Convert.ToInt32(GroupInfoParametr[8]),
                    photo_50 = GroupInfoParametr[9],
                    photo_100 = GroupInfoParametr[10],
                    photo_200 = GroupInfoParametr[11]
                    };
                db.GroupInfos.Add(GI);
                db.SaveChanges();
            }
        }
    }

    public class GroupInfoo
    {
        public int Id { get; set; }
        public int group_id { get; set; }
        public string name { get; set; }
        public string screen_name { get; set; }
        public int is_closed { get; set; }
        public string type { get; set; }
        public int is_admin { get; set; }
        public int admin_level { get; set; }
        public int is_member { get; set; }
        public int members_count { get; set; }
        public string photo_50 { get; set; }
        public string photo_100 { get; set; }
        public string photo_200 { get; set; }
    }

    class ContextGroupInfo : DbContext
    {
        public ContextGroupInfo() : base("DbConnection") { }
        public DbSet<GroupInfoo> GroupInfos { get; set; }
    }
     
}
