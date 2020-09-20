using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VkParserV1._2
{
    public partial class Content : UserControl
    {
        #region подключаемые классы
        ContentFilter contentfilter;
        ImageHeightWightCalibration imageheighwightcalibration; 
        #endregion
        int groupId = -20629724;

        int[] ID;
        string[] ComboBoxString;
        #region для изображений
        PictureBox[] picturebox;
        int picktureBoxH;
        
       
        #endregion


        public Content()
        {
            InitializeComponent();
            //Запрещаем редактирование comboBox, только выбор из готовых вариантов
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            textBox1.ScrollBars = ScrollBars.Vertical;
            ComboBoxLoadedTextGroup();
        }

        #region Зарузка комбо бокса имеющимися группами для выбора контента
        private void ComboBoxLoadedTextGroup()
        {
            ID = new int[0];
            ComboBoxString = new string[0];

            using (ContextGroupInfo db = new ContextGroupInfo())
            {
                //GroupInfoo gf = db.GroupInfos.Select(gf.Id, gf.name, gf.group_id)
                var GroupName = from c in db.GroupInfos select new { c.group_id, c.name, c.Id };
                foreach (var c in GroupName)
                {
                    Array.Resize(ref ID, ID.Length + 1);
                    Array.Resize(ref ComboBoxString, ComboBoxString.Length + 1);

                    ID[ID.Length - 1] = c.Id;
                    ComboBoxString[ComboBoxString.Length - 1] = c.name + " | " + c.group_id;
                }
            }

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(ComboBoxString);
        }

        #endregion

        #region добавить фильтр для контента
        bool ContentFilterAdded = false;
        bool ContentFilterVisibleFLAG = false;
        private void button5_Click(object sender, EventArgs e)
        {
            ContentFilterVisible();
        }
        //ContentFilter contentfilter;
        private void ContentFilterAdd()
        {
            Controls.Add(contentfilter);
            ContentFilterAdded = true;
        }

        private void ContentFilterClose()
        {
            contentfilter.Size = new Size(1,1);
            contentfilter.Location = new Point(1,1);
            ContentFilterVisibleFLAG = false;
        }

        private void ContentFilterDispose()
        {
            contentfilter.Dispose();
            ContentFilterAdded = false;
        }

        private void ContentFilterVisible()
        {
            if (contentfilter == null||!contentfilter.Created)
                contentfilter = new ContentFilter();
            contentfilter.Location = new Point((this.ClientSize.Width / 2) - (contentfilter.Size.Width / 2), (this.ClientSize.Height / 2) - (contentfilter.Size.Height / 2));
            //if (!ContentFilterAdded)
                ContentFilterAdd();
            contentfilter.BringToFront();
            ContentFilterVisibleFLAG = true;
        }
        #endregion

        #region функция обновления записи
        private void PostUpdate()
        {
            string[] post = SataticClassContent.PostsFuncion(groupId);
            label3.Text = post[0];
            label4.Text = post[1];

            textBox1.Clear();
            foreach (string str in post[2].Split('\n'))
            {
                textBox1.AppendText(str + Environment.NewLine);
            }

            //очищаем созданные пикчер боксы
            if (picturebox != null)
                PictureBoxDispose();

            //передаем управление создаюнию новых пикчер боксов и загрузку картинок принадлежащих посту
            picktureBoxH = 0;
            PostImg(post[3], post[4]);
            SataticClassContent.i++;
        }

        #region удаляем уже созданные пикчер боксы для добавления новых с новыми картинками
        private void PictureBoxDispose()
        {
            if (picturebox.Count() > 0)
            {
                foreach (PictureBox picBox in picturebox)
                {
                    picBox.Dispose();
                }
            }
        }
        #endregion

        #region получаем изображения прикрепленные к посту
        /// <summary>
        /// вывод изображений на форму 
        /// после ваполнения функции 
        /// если не произошло ошибок приходит
        /// true
        /// значить можно обратиться в массиву
        /// Programm.ImagePost[]
        /// в формате Bitmap
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="groupId"></param>
        private void PostImg(string postId, string groupId)
        {
            //Проверяем есить ли картинки принадлежащие к посту
            if (ContentImagePost.ImageFunction(Convert.ToInt32(postId), Convert.ToInt32(groupId)))
            {
                if (StaticClass.ImagePost != null)
                {
                    int z = 0;
                    picturebox = new PictureBox[StaticClass.ImagePost.Count()];

                    foreach (Bitmap img in StaticClass.ImagePost)
                    {
                        AddImgForm(z, img);
                        z++;
                    }
                }
            }
        }
        #endregion

        #region добавляем пикчер боксы на форму 
        private void AddImgForm(int numberBoxArr, Bitmap image)
        {
            picturebox[numberBoxArr] = new PictureBox();
            picturebox[numberBoxArr].Name = Convert.ToString(numberBoxArr);
            picturebox[numberBoxArr].Size = new Size(150, 100);
            picturebox[numberBoxArr].Location = new Point(422, 40 + picktureBoxH);
            picturebox[numberBoxArr].Click += pictureBox_click;
            //
            picktureBoxH = picturebox[numberBoxArr].Height + picturebox[numberBoxArr].Location.Y;
            //
            picturebox[numberBoxArr].Image = HWImageCalibration(image, 150, 100);
            this.Controls.Add(picturebox[numberBoxArr]);
            picturebox[numberBoxArr].BringToFront();

        }
        #endregion

        #region передача картинки для калибровки размеров
        private Image HWImageCalibration(Bitmap image, int w, int h)
        {
            imageheighwightcalibration = new ImageHeightWightCalibration();
            return imageheighwightcalibration.ScaleImage(image, w,h);
        }
        #endregion

        #region увеличение картинки по необходимости
        /// <summary>
        /// пикчер бокс тут будут храниться предыдушие настройки для пермешения по экрану
        /// </summary>
        #region
        public bool OpenPBFull = false;
        public Image ImageMinSize;
        public string PBCName { get; set; }
        public int LocationXpos { get; set; }
        public int LicationYpos { get; set; }
        #endregion
        private void pictureBox_click(object sender, EventArgs e)
        {
            string pbc = (sender as PictureBox).Name;
            if (!OpenPBFull)
            {
                OpenPBFull = true;
                PBCName = pbc;
                LocationXpos = picturebox[Convert.ToInt32(pbc)].Location.X;
                LicationYpos = picturebox[Convert.ToInt32(pbc)].Location.Y;
                //picturebox[Convert.ToInt32(pbc)].Location;
                ImageMinSize = picturebox[Convert.ToInt32(pbc)].Image;
                if (StaticClass.ImagePost[Convert.ToInt32(pbc)].Width > 600 || StaticClass.ImagePost[Convert.ToInt32(pbc)].Height > 500)
                {
                    Image img = HWImageCalibration(StaticClass.ImagePost[Convert.ToInt32(pbc)], 600, 500);
                    picturebox[Convert.ToInt32(pbc)].Image = img;
                    picturebox[Convert.ToInt32(pbc)].Size = new Size(picturebox[Convert.ToInt32(pbc)].Image.Width, picturebox[Convert.ToInt32(pbc)].Image.Height);
                    picturebox[Convert.ToInt32(pbc)].Location = new Point((this.ClientSize.Width / 2) - (picturebox[Convert.ToInt32(pbc)].Width / 2), (this.ClientSize.Height / 2) - (picturebox[Convert.ToInt32(pbc)].Height / 2));
                    picturebox[Convert.ToInt32(pbc)].BringToFront();
                }
                else
                {
                    picturebox[Convert.ToInt32(pbc)].Size = new Size(StaticClass.ImagePost[Convert.ToInt32(pbc)].Width, StaticClass.ImagePost[Convert.ToInt32(pbc)].Height);
                    picturebox[Convert.ToInt32(pbc)].Image = StaticClass.ImagePost[Convert.ToInt32(pbc)];
                    picturebox[Convert.ToInt32(pbc)].Location = new Point((this.ClientSize.Width / 2) - (picturebox[Convert.ToInt32(pbc)].Width / 2), (this.ClientSize.Height / 2) - (picturebox[Convert.ToInt32(pbc)].Height / 2));
                    picturebox[Convert.ToInt32(pbc)].BringToFront();
                }
                //Program.ImagePost[Convert.ToInt32(pbc)].Width;
                //picturebox[Convert.ToInt32(pbc)];
                OpenPBFull = true;
                PBCName = pbc;
            }
            else
            {
                if (PBCName == pbc)
                {
                    picturebox[Convert.ToInt32(pbc)].Size = new Size(150, 100);
                    picturebox[Convert.ToInt32(pbc)].Image = ImageMinSize;
                    picturebox[Convert.ToInt32(pbc)].Location = new Point(LocationXpos, LicationYpos);
                    OpenPBFull = false;
                    //LocationXpos = 0;
                    //LicationYpos = 0;

                }
            }

        }
        #endregion

        ///<summary>
        /// обновление Бд запись подошла или нет вообще выставляем просмотренные записи и нет
        /// и выносим записи для публикации в другую таблицу
        /// </summary>
        #region  
        private void BdPostUpdate(int groupId, int postId)
        {
            using (ContextPostedGroupContent db = new ContextPostedGroupContent())
            {
                PostedGroupContent cgp = new PostedGroupContent { groupId = groupId, postId = postId };
                db.PostedGroupContents.Add(cgp);
                db.SaveChanges();
            }
        }
        private void bdVallUpdate(int Id)
        {
            using (ContextGroupWallTextPost db = new ContextGroupWallTextPost())
            {
                GroupWallTextPost PostedParametr = db.GroupWallTextPosts.Find(Id);
                PostedParametr.postedTable = true;
                db.SaveChanges();
            }
        }
        #endregion

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            PostUpdate();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
