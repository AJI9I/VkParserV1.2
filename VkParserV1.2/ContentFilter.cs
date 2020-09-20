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
    public partial class ContentFilter : UserControl
    {
        public ContentFilter()
        {
            InitializeComponent();

            //Запрещаем редактирование comboBox, только выбор из готовых вариантов
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        }
        #region параметры фильтра string[] ParametrContentFilter = new string[3];
        //Название
        //Группа
        //Период постов
        string[] ParametrContentFilter = new string[3];
        #endregion

        #region закрытие контрола Сохранить Готово
        //Кнопка сохранить
        private void button2_Click(object sender, EventArgs e)
        {
            if(SaveParametrContentFilter())
            DisposeControlContentFilter();
        }
        //Кнопка отмена
        private void button3_Click(object sender, EventArgs e)
        {
            DisposeControlContentFilter();
        }

        private bool SaveParametrContentFilter()
        {
            bool AllParametrComplited = true;

            if (textBox1.Text != "")
                ParametrContentFilter[0] = textBox1.Text;
            if (comboBox1.SelectedItem != null)
                ParametrContentFilter[1] = comboBox1.SelectedText;
            if (numericUpDown1.Value != 0)
                ParametrContentFilter[2] = Convert.ToString(numericUpDown1.Value);
            return AllParametrComplited;

        }

        private void DisposeControlContentFilter()
        {
            Dispose();
        }
        #endregion

        
    }
}
