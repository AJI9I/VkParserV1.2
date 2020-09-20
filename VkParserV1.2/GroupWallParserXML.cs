using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Threading;
using System.Data.Entity;

namespace VkParserV1._2
{
    class GroupWallParserXML
    {
        #region WH точка начиная с респонза получает полный xml фаил
        public void WHParseXml(XmlNode n)
        {
            ParseXml(n.SelectSingleNode("items"));
        } 
        #endregion

        #region входная часть получает узел XML 
        private void ParseXml(XmlNode n)
        {

            foreach (XmlNode nodeItem in n)
            {
                //id
                //откуда взят
                //дата публикации
                //текст
                //количество комментариев
                //количество лайков
                //количество дизлайков
                //количество репостов
                //количество репостов пользователями
                string[] postText = new string[9];
                if (nodeItem.Name == "count")
                { }
                else
                if (nodeItem.Name == "post")
                {
                    postText[0] = nodeItem.SelectSingleNode("id").InnerText;
                    //postid = postText[0];
                    postText[1] = nodeItem.SelectSingleNode("from_id").InnerText;
                    postText[2] = nodeItem.SelectSingleNode("date").InnerText;
                    postText[3] = nodeItem.SelectSingleNode("text").InnerText;
                    postText[4] = nodeItem.SelectSingleNode("comments").FirstChild.InnerText;

                    //  threadStartPhotoVideoParser(nodeItem.SelectNodes("attachments"));

                    foreach (XmlNode nodeLikes in nodeItem.SelectSingleNode("likes").ChildNodes)
                    {
                        if (nodeLikes.Name == "count")
                        {
                            postText[5] = nodeLikes.InnerText;
                        }
                        if (nodeLikes.Name == "user_likes")
                        {
                            postText[6] = nodeLikes.InnerText;
                        }
                    }

                    foreach (XmlNode nodeReposts in nodeItem.SelectSingleNode("reposts").ChildNodes)
                    {

                        if (nodeReposts.Name == "count")
                        {
                            postText[7] = nodeReposts.InnerText;
                        }
                        if (nodeReposts.Name == "user_reposted")
                        {
                            postText[8] = nodeReposts.InnerText;
                        }

                    }
                    // вызов потока для распарсевания в бд
                    if (bdChek(postText) == true)
                    {
                        treadAddBd(postText);
                        if(nodeItem.SelectSingleNode("attachments") != null)
                        XmlGroupParserVideo(nodeItem.SelectSingleNode("attachments"), postText[1], postText[0]);
                    }
                    //  ADDBD.AddBdVallContent(postText);
                }
            }
        }
        #endregion

        #region Чекаем базу данных для проверки есть ли у нас подобрая информация
        private bool bdChek(string[] postText)
        {
            int postID = Convert.ToInt32(postText[0]);
            int groupID = Convert.ToInt32(postText[1]);
            using (ContextGroupWallTextPost db = new ContextGroupWallTextPost())
            {
                var tt = db.GroupWallTextPosts.Where(p => p.post_id == postID).Intersect(db.GroupWallTextPosts.Where(p => p.from_id == groupID));
                if (tt.Count() == 0)
                    return true;
                return false;
            }
        }
        #endregion

        #region Отдельный поток для записи полученного поста в базу данных
        Thread threadAddBd;
        private void treadAddBd(string[] postText)
        {
            threadAddBd = new Thread(functionAdd);
            threadAddBd.Start(postText);
        }

        void functionAdd(object postText)
        {
            string[] vallString = (string[])postText;
            #region описание модели
            //id
            //откуда взят
            //дата публикации
            //текст
            //количество комментариев
            //количество лайков
            //количество дизлайков
            //количество репостов
            //количество репостов пользователями
            //public int Id { get; set; }
            //public string post_id { get; set; }
            //public string from_id { get; set; }
            //public string date { get; set; }
            //public string text { get; set; }
            //public string count_comments { get; set; }
            //public string count_likes { get; set; }
            //public string count_user_likes { get; set; }
            //public string count_repost { get; set; }
            //public string count_user_repost { get; set; }
            #endregion
            using (ContextGroupWallTextPost db = new ContextGroupWallTextPost())
            {
                GroupWallTextPost cgp = new GroupWallTextPost { post_id = Convert.ToInt32(vallString[0]), from_id = Convert.ToInt32(vallString[1]), date = Convert.ToInt32(vallString[2]), text = vallString[3], count_comments = Convert.ToInt32(vallString[4]), count_likes = Convert.ToInt32(vallString[5]), count_user_likes = Convert.ToInt32(vallString[6]), count_repost = Convert.ToInt32(vallString[7]), count_user_repost = Convert.ToInt32(vallString[8]), postedTable = false };
                db.GroupWallTextPosts.Add(cgp);
                db.SaveChanges();
            }
        }
        #endregion

        #region разбор фото и видо
        public void XmlGroupParserVideo(XmlNode attachmentsXmlNodeList, string GroupId, string PostId)
        {
            foreach (XmlNode attachmentsNode in attachmentsXmlNodeList.SelectNodes("attachment"))
            {
                if (attachmentsNode.FirstChild.InnerText == "photo")
                {
                    //передаем управление в фото
                    //XML узел
                    // ID группы
                    // ID Поста
                    object[] GroupIdANDNodeAttachments = new object[] { attachmentsNode, GroupId, PostId };
                    ThreadAttachmentsNodePhotoStarted(GroupIdANDNodeAttachments);
                }
                if (attachmentsNode.FirstChild.InnerText == "video")
                {
                    //XML узел
                    // ID группы
                    // ID Поста
                    object[] GroupIdANDNodeAttachments = new object[] { attachmentsNode, GroupId, PostId };
                    //передаем управление в видео
                    ThreadAttachmentsNodeVideoStarted(GroupIdANDNodeAttachments);
                }
            }
        }
        #endregion

        #region запуск потоков видео фото
        Thread VideoParserthread;
        private void ThreadAttachmentsNodeVideoStarted(object[] XmlNodeVideo)
        {
            VideoParserthread = new Thread(XmlNodeAttachmentsVideoParser);
            VideoParserthread.Start(XmlNodeVideo);

        }

        Thread PhotoParserthread;
        private void ThreadAttachmentsNodePhotoStarted(object[] XmlNodePhoto)
        {
            PhotoParserthread = new Thread(XmlNodeAttachmentsPhotoParser);
            PhotoParserthread.Start(XmlNodePhoto);

        }
        #endregion

        #region парсинг видео узла из XML файла
        private void XmlNodeAttachmentsVideoParser(object videoNode)
        {
            // XML узел
            // ID группы
            // ID Поста
            string[] VideoPostParaMetr = new string[5];
            object[] dropObject = (object[])videoNode;
            XmlNode video = (XmlNode)dropObject[0];
            foreach (XmlNode vdeoNode in video.SelectNodes("video"))
            {
                VideoPostParaMetr[0] = (string)dropObject[1];
                VideoPostParaMetr[1] = vdeoNode.SelectSingleNode("access_key").InnerText;
                VideoPostParaMetr[2] = vdeoNode.SelectSingleNode("owner_id").InnerText;
                VideoPostParaMetr[3] = vdeoNode.SelectSingleNode("id").InnerText;
                VideoPostParaMetr[4] = (string)dropObject[2];
                VideoDbAdd(VideoPostParaMetr);
                //Videothread = new Thread(VideoDbAdd);
                //Videothread.Start(VideoPostParaMetr);
                /// <summary>
                /// Дописать расположени в бд
                /// </summary>
            }
        }
        #endregion

        #region парсинг фото узла XML файла
        private void XmlNodeAttachmentsPhotoParser(object photoNode)
        {
            object[] droppObject = (object[])photoNode;
            string[] PhotoPostParaMetr = new string[4];
            XmlNode photo = (XmlNode)droppObject[0];
            foreach (XmlNode phtoNode in photo.SelectNodes("photo"))
            {
                foreach (XmlNode phtosize in phtoNode)
                {
                    if (phtosize.Name.Length > 5)
                    {
                        string photosizeW = phtosize.Name.Substring(0, 5);
                        if (photosizeW == "photo")
                            PhotoPostParaMetr[0] = phtosize.InnerText;
                    }

                }
                //нужно узнать ид поста к которому относится фото

                //PhotoPostParaMetr[1] = phtoNode.SelectSingleNode("access_key").InnerText;
                PhotoPostParaMetr[2] = (string)droppObject[2];
                PhotoPostParaMetr[3] = (string)droppObject[1];
                //Thread.Sleep(100);
                PhotoDbAdd(PhotoPostParaMetr);
                //Photothread = new Thread(PhotoDbAdd);
                //Photothread.Start(PhotoPostParaMetr);
                /// <summary>
                /// Дописать расположени в бд
                /// </summary>
            }
        }
        #endregion

        #region Запись собранных данных в БД фото параметры
        private void PhotoDbAdd(string[] parametr)
        {
            string[] param = parametr;
            using (ContextGroupWallPhoto db = new ContextGroupWallPhoto())
            {
                GroupWallPhoto cgp = new GroupWallPhoto { photo = param[0], access_key = param[1], post_id = Convert.ToInt32(param[2]), GroupId = Convert.ToInt32(param[3]) };
                db.GroupWallPhotos.Add(cgp);
                db.SaveChanges();
            }
        }
        #endregion

        #region Запись в бд собранных данных о видео
        private void VideoDbAdd(string[] parametr)
        {
            string[] param = parametr;
            using (ContextGroupWallVideo db = new ContextGroupWallVideo())
            {
                GroupWallVideo cgp = new GroupWallVideo { id_video = Convert.ToInt32(param[3]), post_id = Convert.ToInt32(param[0]), access_key = param[1], owner_id = Convert.ToInt32(param[2]), GroupId = Convert.ToInt32(param[4]) };
                db.GroupWallVideos.Add(cgp);
                db.SaveChanges();
            }
        }
        #endregion
    }

    #region Контекст для хранения данных со стены модель базы данных
    public class GroupWallTextPost
    {
        #region
        //id
        //откуда взят
        //дата публикации
        //текст
        //количество комментариев
        //количество лайков
        //количество дизлайков
        //количество репостов
        //количество репостов пользователями
        #endregion
        public int Id { get; set; }
        public int post_id { get; set; }
        public int from_id { get; set; }
        public int date { get; set; }
        public string text { get; set; }
        public int count_comments { get; set; }
        public int count_likes { get; set; }
        public int count_user_likes { get; set; }
        public int count_repost { get; set; }
        public int count_user_repost { get; set; }
        public bool postedTable { get; set; }
    }

    class ContextGroupWallTextPost : DbContext
    {
        public ContextGroupWallTextPost() : base("DbConnection") { }
        public DbSet<GroupWallTextPost> GroupWallTextPosts { get; set; }
    }
    #endregion

    #region Контекст для хранения данных видео
    public class GroupWallVideo
    {
        public int Id { get; set; }
        public int id_video { get; set; }
        public string access_key { get; set; }
        public int post_id { get; set; }
        public int owner_id { get; set; }
        public int GroupId { get; set; }
    }

    class ContextGroupWallVideo : DbContext
    {
        public ContextGroupWallVideo() : base("DbConnection") { }
        public DbSet<GroupWallVideo> GroupWallVideos { get; set; }
    }
    #endregion

    #region Контекст для хранения данных фото
    public class GroupWallPhoto
    {
        public int Id { get; set; }
        public string photo { get; set; }
        public string access_key { get; set; }
        public int post_id { get; set; }
        public int GroupId { get; set; }
        public string PhotoName { get; set; }
    }

    class ContextGroupWallPhoto : DbContext
    {
        public ContextGroupWallPhoto() : base("DbConnection") { }
        public DbSet<GroupWallPhoto> GroupWallPhotos { get; set; }
    }
    #endregion

    #region Контекст для записи постов к публикации
    public class PostedGroupContent
    {
        public int Id { get; set; }
        public int groupId { get; set; }
        public int postId { get; set; }
        public int GroupPublication { get; set; }

        public int Data1 { get; set; }
        public string Data2 { get; set; }
        public bool Data3 { get; set; }
    }

    class ContextPostedGroupContent : DbContext
    {
        public ContextPostedGroupContent() : base("DbConnection") { }
        public DbSet<PostedGroupContent> PostedGroupContents { get; set; }
    }
    #endregion
}
