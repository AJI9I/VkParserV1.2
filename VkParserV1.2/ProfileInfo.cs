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
    public partial class ProfileInfo : UserControl
    {
        #region
        BrowserMonitor browsermonitor;

        #endregion
        public ProfileInfo()
        {
            InitializeComponent();
            browsermonitor = new BrowserMonitor();
        }

        #region заполнение полей авторизации
        delegate void AddtextLabelDelegate();
        AddtextLabelDelegate addtextlabeldelegate;

        public void AddTextLabelDelegateStart()
        {
            addtextlabeldelegate = new AddtextLabelDelegate(AddParametrForm);
            Invoke(addtextlabeldelegate);
        }

        public void AddParametrForm()
        {
            label2.Text = StaticClass.AutorisationParametr[0];
            label4.Text = StaticClass.AutorisationParametr[1];
        }
        #endregion

        
    }
}
