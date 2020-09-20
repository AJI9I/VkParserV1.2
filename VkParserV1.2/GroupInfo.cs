using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace VkParserV1._2
{
    public partial class GroupInfo : UserControl
    {
        delegate void GroupBoxDelegate(string str);
        GroupBoxDelegate GROUPBOXDELEGATE;

        PictureBox pickturebox;
        FindFileAndDownload findfileanddownload;
        ImageHeightWightCalibration imageheightwightcalibration;
        GroupGrabberParametr groupgrabberparametr;
        ClassificaionAdd classificationadd;
        Thread QueryBdThread;

        public GroupInfo()
        {
            InitializeComponent();
            //
        }

        #region Забивает данными формы
        public void addParametrForm(string[] arr)
        {
            label6.Text = arr[1];
            label7.Text = arr[0];
            label8.Text = arr[2];
            label9.Text = arr[4];
            label10.Text = arr[8];
            ImageLoad(arr[10]);
            UpdateComboBox();
        }
        #endregion

        #region загрузка изображения
        private void ImageLoad(string fileurl)
        {
            if (pickturebox != null && pickturebox.Created)
                pickturebox.Dispose();
            AddPicktureBox(fileurl);
        }
        
        #region Отрисовка элементов с данными группы
        private void AddPicktureBox(string fileurl)
        {
            findfileanddownload = new FindFileAndDownload();
            imageheightwightcalibration = new ImageHeightWightCalibration();

            pickturebox = new PictureBox();
            pickturebox.Size = new Size(108, 108);
            pickturebox.Location = new Point(1,1);
            pickturebox.Image = imageheightwightcalibration.ScaleImage(findfileanddownload.fileFind(fileurl),108,108);
            this.Controls.Add(pickturebox);

        }
        #endregion

        #endregion

        #region сбор параметров с формы для добавления задания
        private void button1_Click(object sender, EventArgs e)
        {
            groupgrabberparametr = new GroupGrabberParametr();
            groupgrabberparametr.WHParametrParserAddBd(ParametrSettings());
        }

        public object[] GetParametrs()
        {
            return ParametrSettings();
        }

        private object[] ParametrSettings()
        {
            bool[] Parametr = new bool[4];
            string Classifications = "";
            if (comboBox1.SelectedItem != null)
            Classifications = comboBox1.SelectedItem.ToString();
            Parametr[0] = checkBox1.Checked;
            Parametr[1] = checkBox2.Checked;
            Parametr[2] = checkBox3.Checked;
            Parametr[3] = checkBox4.Checked;
            string GroupId = label7.Text;
            object[] returnObject = new object[] { Classifications, Parametr, GroupId };
            return returnObject;

        }
        #endregion

        #region работа комбо бокса
        //private void QueryBd()
        //{
        //    QueryBdThread = new Thread(UpdateComboBox);
        //    QueryBdThread.Start();
        //}
        //private void groupBoxUpdate(string tf)
        //{
        //    comboBox1.Items.Add(tf);
        //}
        private void UpdateComboBox()
        {
            //GROUPBOXDELEGATE = new GroupBoxDelegate(groupBoxUpdate);

            comboBox1.Items.Clear();
            string[,] qury = query();
            if (qury.Length != 0)
            {
                for (int i=0;i<qury.Length/2; i++)
                {
                    //Invoke(GROUPBOXDELEGATE, qury[i, 1]);
                    comboBox1.Items.Add(qury[i,1]);
                }
                //foreach (var qq in qury)
                //{
                //    comboBox1.Items.Add(qq.CLassification);
                //}
            }
            else {
                ClassificationnAddOption();

            }

        }

        private string[,] query()
        {
            string[,] parametr;
            using (ContextClassificationParametr db = new ContextClassificationParametr())
            {
                var rr = db.ClassificationParametrs.Select(p => new { Id = p.Id, CLassification = p.CLassification });
                if (rr.Count() != 0)
                {
                    parametr = new string[rr.Count(), 2];
                    int i = 0;
                    foreach (var p in rr)
                    {
                        parametr[i, 0] = Convert.ToString(p.Id);
                        parametr[i, 1] = p.CLassification;
                        i++;
                    }
                }
                else
                {
                    parametr = new string[0, 0];
                }
            }
                return parametr;

        }
        #endregion

        #region Дабавить классификацию
        private void ClassificationnAddOption()
        {
            classificationadd = new ClassificaionAdd();
            classificationadd.Size = new Size(283, 107);
            classificationadd.Location = new Point((this.ClientSize.Width / 2) - (classificationadd.Width / 2), (this.ClientSize.Height / 2) - (classificationadd.Height / 2));
            classificationadd.Disposed += Classificationadd_Disposed;
            
            Controls.Add(classificationadd);
            classificationadd.BringToFront();
        }

        private void Classificationadd_Disposed(object sender, EventArgs e)
        {
            UpdateComboBox();
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            ClassificationnAddOption();
        }
    }
}
