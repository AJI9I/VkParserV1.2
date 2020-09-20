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
    public partial class ClassificaionAdd : UserControl
    {
        public ClassificaionAdd()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            addclassification();
        }
        private void addclassification()
        {
            if (textBox1.Text != null || textBox1.Text.Length < 4)
            {
                string NewClassification = textBox1.Text;
                if (ChekedClassification(NewClassification) == 0)
                {
                    AddClassificationBd(NewClassification);
                    Dispose();
                }
                else
                {
                    //Такая классификация уже существует
                }
            }
            else
            {
                //ошибка либо не ввели либо слишком короткое имя
            }

        }
        private void AddClassificationBd(string classifications)
        {
            using (ContextClassificationParametr db = new ContextClassificationParametr())
            {
                ClassificationParametr CP = new ClassificationParametr {
                    CLassification = classifications
                };
                db.ClassificationParametrs.Add(CP);
                db.SaveChanges();
            }
        }

        private int ChekedClassification(string classifications)
        {
            int count;
            using (ContextClassificationParametr db = new ContextClassificationParametr())
            {
                var rr = db.ClassificationParametrs.Select(p =>new  { Id = p.Id, CLassification = p.CLassification })
                    .Where(p => p.CLassification == classifications);
                count = rr.Count();
            }
            return count;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
