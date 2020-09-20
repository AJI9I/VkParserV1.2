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
    class UserInfoParseXML
    {
        #region парсинг xml файла с информацией о пользователе users.get 
        Thread ThreadUserInfoParametr;

        public void WHXMLUserInfoParser(XmlDocument xml)
        {
            XMLUserInfoParser(xml);
        }

        private void XMLUserInfoParser(XmlDocument xml)
        {
            XmlNode xmlResponse = xml.SelectSingleNode("item");
            foreach (XmlNode node in xmlResponse)
            {
                StartThreadingParametrUserInfo(node);
            }
        }

        private void StartThreadingParametrUserInfo(XmlNode xml)
        {
            ThreadUserInfoParametr = new Thread(ParametrUserInfo);
            ThreadUserInfoParametr.Start(xml);
        }

        private void ParametrUserInfo(object xml)
        {
            XmlNode xmlnodResponse = (XmlNode)xml;
            string[] ParametrUserInfo = new string[8];

            ParametrUserInfo[0] = xmlnodResponse.SelectSingleNode("id").InnerText;
            ParametrUserInfo[1] = xmlnodResponse.SelectSingleNode("first_name").InnerText;
            ParametrUserInfo[2] = xmlnodResponse.SelectSingleNode("last_name").InnerText;
            ParametrUserInfo[3] = xmlnodResponse.SelectSingleNode("sex").InnerText;
            if(xmlnodResponse.SelectSingleNode("bdate") != null)
            ParametrUserInfo[4] = xmlnodResponse.SelectSingleNode("bdate").InnerText;
            ParametrUserInfo[5] = xmlnodResponse.SelectSingleNode("city").SelectSingleNode("id").InnerText;
            CityXmlParserThreadStart(xmlnodResponse.SelectSingleNode("city"));
            ParametrUserInfo[6] = xmlnodResponse.SelectSingleNode("country").SelectSingleNode("id").InnerText;
            CountryXmlParserThreadStart(xmlnodResponse.SelectSingleNode("country"));
            ParametrUserInfo[7] = xmlnodResponse.SelectSingleNode("photo_200").InnerText;
            AddBDParametrUserInfo(ParametrUserInfo);
        }

        private string[] UserBDCheked(int User_id)
        {
            using (ContextUserInfo db = new ContextUserInfo())
            {
                var pp = db.UserInfos
                    .Select(p => new { user_id = p.User_Id, first_name = p.first_name, last_name = p.last_name, sex = p.sex, bdate = p.bdate, city = p.city_Id, country = p.country_Id, photo_200 = p.photo_200 })
                    .Where(p => p.user_id == User_id);

                //IQueryable tt = db.UserInfos.Where(p => p.User_Id == User_id);
                if(pp.Count()!= 0)
                { 
                string[] query = new string[8];
                query[0] = Convert.ToString(pp.First().user_id);
                query[1] = pp.First().first_name;
                query[2] = pp.First().last_name;
                query[3] = Convert.ToString(pp.First().sex);
                query[4] = Convert.ToString(pp.First().bdate);
                query[5] = Convert.ToString(pp.First().city);
                query[6] = Convert.ToString(pp.First().country);
                query[7] = pp.First().photo_200;
                    return query;
                }
                else
                {
                    string[] query = new string[] { "null" };
                    return query;
                }
            }
        }
        private void AddBDParametrUserInfo(string[] ParametrUserInfo)
        {
            using (ContextUserInfo db = new ContextUserInfo())
            {
                int User_id = Convert.ToInt32(ParametrUserInfo[0]);
                string[] tt = UserBDCheked(User_id);
                if (tt[0] == "null"|| tt[1] == null|| tt[2] == null|| tt[3] == null)
                {
                    UserInfo UI = new UserInfo
                    {
                        User_Id = Convert.ToInt32(ParametrUserInfo[0]),
                        first_name = ParametrUserInfo[1],
                        last_name = ParametrUserInfo[2],
                        sex = Convert.ToInt32(ParametrUserInfo[3]),
                        bdate = ParametrUserInfo[4],
                        city_Id = Convert.ToInt32(ParametrUserInfo[5]),
                        country_Id = Convert.ToInt32(ParametrUserInfo[6]),
                        photo_200 = ParametrUserInfo[7]
                    };
                    db.UserInfos.Add(UI);
                    db.SaveChanges();
                }
                else { }
            }
        }

        #region обработка городов
        Thread CityXmlThread;
        

        private void CityXmlParserThreadStart(XmlNode CityXml)
        {
            CityXmlThread = new Thread(CityXmlParser);
            CityXmlThread.Start(CityXml);

        }
        private void CityXmlParser(object CityXml)
        {
            XmlNode xml = (XmlNode)CityXml;
            string[] ParametrCity = new string[2];
            ParametrCity[0] = xml.SelectSingleNode("id").InnerText;
            ParametrCity[1] = xml.SelectSingleNode("title").InnerText;
            AddBdCityParametr(ParametrCity);
        }

        private void AddBdCityParametr(string[] CityParametr)
        {
            using (ContextCity db = new ContextCity())
            {
                int City_id = Convert.ToInt32(CityParametr[0]);
                var tt = db.CittyInfos.Where(p => p.City_Id == City_id);
                if (tt == null || tt.Count() == 0)
                {
                    CityInfo CI = new CityInfo
                    {
                        City_Id = Convert.ToInt32(CityParametr[0]),
                        City_Name = CityParametr[1]

                    };
                    db.CittyInfos.Add(CI);
                    db.SaveChanges();
                }
            }
        }
        #endregion

        #region обработка стран
        Thread CountryXmlParserThread;

        private void CountryXmlParserThreadStart(XmlNode xml)
        {
            CountryXmlParserThread = new Thread(CountryXmlParser);
            CountryXmlParserThread.Start(xml);
        }

        private void CountryXmlParser(object xml)
        {
            XmlNode CountryXmlNode = (XmlNode)xml;
            string[] CountryParametr = new string[2];

            CountryParametr[0] = CountryXmlNode.SelectSingleNode("id").InnerText;
            CountryParametr[1] = CountryXmlNode.SelectSingleNode("title").InnerText;
            AddBDCountryParametr(CountryParametr);
        }

        private void AddBDCountryParametr(string[] CountryParametr)
        {
            using (ContextCountry db = new ContextCountry())
            {
                int Country_id = Convert.ToInt32(CountryParametr[0]);
                if(Country_id != StaticClass.LastCountry_Id)
                { 
                var tt = db.CountryInfos.Where(p => p.Country_Id == Country_id);
                if (tt == null|| tt.Count() == 0)
                {
                    CountryInfo CI = new CountryInfo
                    {
                        Country_Id = Convert.ToInt32(CountryParametr[0]),
                        Country_Name = CountryParametr[1]
                    };
                    db.CountryInfos.Add(CI);
                    db.SaveChanges();
                    StaticClass.LastCountry_Id = Country_id;
                }
                }
            }
        }
        #endregion
        #endregion

        #region парсинг информации о репостах пользователей

        Thread UserRepostedInformationThread;

        public void WHUserRepostedParserXML(XmlNode xml)
        {
            UserREpostedParserXML(xml);
        }

        private void UserREpostedParserXML(XmlNode xml)
        {
            xml = xml.SelectSingleNode("HeaderXml").SelectSingleNode("response");
            XmlNodeList XmlResponse = xml.SelectNodes("profiles");
            string XmlNodeGroup = xml.SelectSingleNode("groups").SelectSingleNode("id").InnerText;
            foreach (XmlNode ProfilesNode in XmlResponse)
            {
                object[] ProfilesNodeAndGroupIdObjectArr = new object[] { ProfilesNode, XmlNodeGroup};
                UserRepostedProfileNodeThreadStart(ProfilesNodeAndGroupIdObjectArr);
            }
        }

        private void UserRepostedProfileNodeThreadStart(object[] ProfilesNodeAndGroupIdObjectArr)
        {
            UserRepostedInformationThread = new Thread(UserRepostedProfileNode);
            UserRepostedInformationThread.Start(ProfilesNodeAndGroupIdObjectArr);
        }

        private void UserRepostedProfileNode(object ProfilesNodeAndGroupIdObjectArr)
        {
            object[] ob = (object[])ProfilesNodeAndGroupIdObjectArr;
            XmlNode ProfileNodeXml = (XmlNode)ob[0];
            string GroupId = (string)ob[1];

            string[] ProfileInformation = new string[6];

            ProfileInformation[0] = ProfileNodeXml.SelectSingleNode("id").InnerText;
            if(ProfileNodeXml.SelectSingleNode("first_name")!= null)
                ProfileInformation[1] = ProfileNodeXml.SelectSingleNode("first_name").InnerText;
            if (ProfileNodeXml.SelectSingleNode("last_name") != null)
                ProfileInformation[2] = ProfileNodeXml.SelectSingleNode("last_name").InnerText;
            if (ProfileNodeXml.SelectSingleNode("sex") != null)
                ProfileInformation[3] = ProfileNodeXml.SelectSingleNode("sex").InnerText;
            if (ProfileNodeXml.SelectSingleNode("screen_name") != null)
                ProfileInformation[4] = ProfileNodeXml.SelectSingleNode("screen_name").InnerText;
            ProfileInformation[5] = GroupId;
            AddBdUserId(ProfileInformation);
        }

        private void AddBdUserId(string[] ProfileInformation)
        {
            if(UserAndGroupAddBd(Convert.ToInt32(ProfileInformation[0]), Convert.ToInt32(ProfileInformation[5])))
            { 
            using (ContextUserInfo db = new ContextUserInfo())
            {
                string[] tt = UserBDCheked(Convert.ToInt32(ProfileInformation[0]));
                if (tt[0] == "null")
                {
                    UserInfo UI = new UserInfo
                    {

                        User_Id = Convert.ToInt32(ProfileInformation[0]),
                        first_name = ProfileInformation[1],
                        last_name = ProfileInformation[2],
                        sex = Convert.ToInt32(ProfileInformation[3]),
                    };
                    db.UserInfos.Add(UI);
                    db.SaveChanges();
                }
            }
            }
        }

        private bool UserAndGroupAddBd(int User_Id, int Group_Id)
        {
            using (ContextUserAndGroup db = new ContextUserAndGroup())
            {
                IEnumerable<UserAndGroup> query = UserAndGroupCheked(User_Id, Group_Id);
                if (query.Count() == 0)
                {
                    UserAndGroup UAG = new UserAndGroup
                    {
                        User_id = User_Id,
                        Group_Id = Group_Id
                    };
                    db.UserAndGroups.Add(UAG);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region проверка прявязан ли пользователь к группе
        private IEnumerable<UserAndGroup> UserAndGroupCheked(int User_Id, int Group_Id)
        {
            using (ContextUserAndGroup bd = new ContextUserAndGroup())
            {
                var tt = bd.UserAndGroups.Where(p => p.User_id == User_Id).Intersect(bd.UserAndGroups.Where(p => p.Group_Id == Group_Id)).ToList();
                return tt;
            }
        }
        #endregion
    }

    #region Классификация групп и пользователей выдернутых из групп

    public class GroupClassification
    {
        public int Id { get; set; }
        public int Group_Id { get; set; }
        public int Classification { get; set; }
    }

    class ContextGroupClassification : DbContext
    {
        public ContextGroupClassification(): base("DbConnection") { }
        public DbSet<GroupClassification> GroupClassifications { get; set; }
    }
    #endregion

    #region пользователи конкретно относящиеся к группам из которых они выбранны
    public class UserAndGroup
    {
        public int Id { get; set; }
        public int Group_Id { get; set; }
        public int User_id { get; set; }
    }

    class ContextUserAndGroup : DbContext
    {
        public ContextUserAndGroup(): base("DbConnection") { }
        public DbSet<UserAndGroup> UserAndGroups { get; set; }
    }
    #endregion

    #region Контекст для информации о пользователях для доступа к бд
    public class UserInfo
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int sex { get; set; }
        public string bdate { get; set; }
        public int city_Id { get; set; }
        public int country_Id { get; set; }
        public string photo_200 { get; set; }
        //Значчение countris  по запросу к отдельному человеку пока прото внесем что бы было 
        public int groups_count { get; set; }
        public int friends_count { get; set; }
        public int Users_Attribute { get; set; }
    }

    class ContextUserInfo : DbContext
    {
        public ContextUserInfo(): base("DbConnection") { }
        public DbSet<UserInfo> UserInfos { get; set; }
    }
    #endregion

    #region Контекст городов и стран
    public class CityInfo
    {
        public int Id { get; set; }
        public int City_Id { get; set; }
        public string City_Name { get; set; }
    }

    class ContextCity : DbContext
    {
        public ContextCity() : base("DbConnection") { }
        public DbSet<CityInfo> CittyInfos { get; set; }
    }

    public class CountryInfo
    {
        public int Id { get; set; }
        public int Country_Id { get; set; }
        public string Country_Name { get; set; }
    }

    class ContextCountry : DbContext
    {
        public ContextCountry() : base("DbConnection") { }
        public DbSet<CountryInfo> CountryInfos { get; set; }
    }
    #endregion


}
