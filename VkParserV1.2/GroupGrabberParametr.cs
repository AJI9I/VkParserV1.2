using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Threading;

namespace VkParserV1._2
{
   

    class GroupGrabberParametr
    {
        Thread GroupGrabberParametrAddBdThread;
        public void WHParametrParserAddBd(object[] parametr)
        {
            ParametrParserAddBd(parametr);
        }

        private void ParametrParserAddBdThreadStart(object[] parametr)
        {
            GroupGrabberParametrAddBdThread = new Thread(ParametrParserAddBd);
            GroupGrabberParametrAddBdThread.Start(parametr);
        }

        private void ParametrParserAddBd(object parametrr)
        {
            object[] parametr = (object[])parametrr;
            int group_id = Convert.ToInt32(parametr[2]);
            string Classifications = (string)parametr[0];
            bool[] Parametr = (bool[])parametr[1];
            for (int i=0; i<Parametr.Length; i++)
            {
                switch (i)
                {
                    case 0 :
                        if(Parametr[i])
                        { 
                        WallGroupContent(group_id);
                        }
                        break;
                    case 1:
                        if (Parametr[i])
                        {
                            PhotoAlbomGroupContent(group_id);
                        }
                        break;
                    case 2:
                        if (Parametr[i])
                        {
                            VideoAlbomGroupContent(group_id);
                        }
                        break;
                    case 3:
                        if (Parametr[i])
                        {
                            CellAuditoryGroupContent(group_id);
                        }
                        break;
                }
            }
        }

        #region заполнение бд данными с параметрами групп

        #region проверка существует ли такое задание в базе
        private int ChekedGroupContent(int groupId, string ConttentParametr)
        {
            using (ContextGrabberGroupParametr db = new ContextGrabberGroupParametr())
            {
                var tt = db.GrabberGroupParametrs.Where(p => p.Group_Id == groupId).Intersect(db.GrabberGroupParametrs.Where(p=> p.ContentParametr == ConttentParametr)).ToList();
                return tt.Count();
            }
        }
        #endregion

        private void WallGroupContent(int groupId)
        {
            if(ChekedGroupContent(groupId, "Wall") == 0)
            { 
            using (ContextGrabberGroupParametr db = new ContextGrabberGroupParametr())
            {
                GrabberGroupParametr GGP = new GrabberGroupParametr
                {
                    Group_Id = groupId,
                    ContentParametr = "Wall"
                };
                    db.GrabberGroupParametrs.Add(GGP);
                    db.SaveChanges();
            }
            }
        }
        private void PhotoAlbomGroupContent(int groupId)
        {
            if (ChekedGroupContent(groupId, "PhotoAlbom") == 0)
            {
                using (ContextGrabberGroupParametr db = new ContextGrabberGroupParametr())
                {
                    GrabberGroupParametr GGP = new GrabberGroupParametr
                    {
                        Group_Id = groupId,
                        ContentParametr = "PhotoAlbom"
                    };
                    db.GrabberGroupParametrs.Add(GGP);
                    db.SaveChanges();
                }
            }
        }
        private void VideoAlbomGroupContent(int groupId)
        {
            if (ChekedGroupContent(groupId, "VideoAlbom") == 0)
            {
                using (ContextGrabberGroupParametr db = new ContextGrabberGroupParametr())
                {
                    GrabberGroupParametr GGP = new GrabberGroupParametr
                    {
                        Group_Id = groupId,
                        ContentParametr = "VideoAlbom"
                    };
                    db.GrabberGroupParametrs.Add(GGP);
                    db.SaveChanges();
                }
            }
        }
        private void CellAuditoryGroupContent(int groupId)
        {
            if (ChekedGroupContent(groupId, "CellAuditory") == 0)
            {
                using (ContextGrabberGroupParametr db = new ContextGrabberGroupParametr())
                {
                    GrabberGroupParametr GGP = new GrabberGroupParametr
                    {
                        Group_Id = groupId,
                        ContentParametr = "CellAuditory"
                    };
                    db.GrabberGroupParametrs.Add(GGP);
                    db.SaveChanges();
                }
            }
        }
        #endregion

        #region добавление информации о группе в общую копилку
        private void ParametrParserSetting(int group_id,string group_classification_content)
        {
            
            using (ContextGroupSetting db = new ContextGroupSetting())
            {
                var tt = db.GroupSettings.Where(p => p.Group_id == group_id);
                if (tt.Count() == 0||tt.First().Group_classification_Content != group_classification_content)
                {
                    GroupSetting GS = new GroupSetting
                    {
                        Group_id = group_id,
                        Group_classification_Content = group_classification_content
                    };
                }
            }
        }
        #endregion

    }

    #region Контекст данных GrabberGroupParametr
    public class GrabberGroupParametr
    {
        public int Id { get; set; }
        public int Group_Id { get; set; }
        public string ContentParametr { get; set; }
        public int CountContent { get; set; }
        public int LastContent { get; set; }
        public int FirstContent { get; set; }
        public string LastData { get; set; }
        public bool StartSetings { get; set; }


        //public string Tematic {get; set;}
        //public bool Wall { get; set; }
        //public bool PhotoAlbom { get; set; }
        //public bool VideoAlbom { get; set; }
        //public bool CellAuditory { get; set; }
        //public int PhotoAlbomOffset { get; set; }
        //public int VideoAlbomOffset { get; set; }
        //public int CellAuditoryOffset { get; set; }

    }

    class ContextGrabberGroupParametr : DbContext
    {
        public ContextGrabberGroupParametr() : base("DbConnection") { }
        public DbSet<GrabberGroupParametr> GrabberGroupParametrs { get; set; }
    }
    #endregion
}
