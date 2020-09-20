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
    public partial class Grabber : UserControl
    {
        GroupInfo groupinfo;

        public Grabber()
        {
            InitializeComponent();
        }

        #region получить данные кнопкой ограбить
        public string GetFormParametr()
        {
            return textBox1.Text;
        }
        #endregion

        #region вывод информации о группе
        #region Длегат на вывод информации о группе
        delegate void AddGroupInfoDelegate(string[] grinfo);
        AddGroupInfoDelegate addgroupinfodelegate;

        public void InvokeMetod(string[] arrInfo)
        {
            object ob = arrInfo;
            addgroupinfodelegate = new AddGroupInfoDelegate(AddGroupInfo);
            Invoke(addgroupinfodelegate, ob);
        }
        #endregion


        public object[] GetParametrGrabber()
        {
            return groupinfo.GetParametrs();
        }

        public void AddGroupInfo(object arrInfo)
        {
            GroupInfoVisible();
            groupinfo.addParametrForm((string[])arrInfo);
        }

        //GroupInfo groupinfo;
        bool userGroupInfoAdded = false;

        private void ProfileInfoAdd()
        {
            Controls.Add(groupinfo);
            userGroupInfoAdded = true;
        }

        private void GroupInfoClose()
        {
            groupinfo.Size = new Size(1, 1);
            groupinfo.Location = new Point(1, 1);
        }

        private void GroupInfoDispose()
        {
            groupinfo.Dispose();
            userGroupInfoAdded = false;
        }

        private void GroupInfoVisible()
        {
            if (groupinfo == null || !groupinfo.Created)
                groupinfo = new GroupInfo();
            groupinfo.Size = new Size(640, 296);
            groupinfo.Location = new Point(5, 50);
            if (!userGroupInfoAdded)
                ProfileInfoAdd();
            groupinfo.BringToFront();

        }
        #endregion
    }
}
