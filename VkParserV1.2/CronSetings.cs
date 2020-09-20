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
    public partial class CronSetings : UserControl
    {
        public CronSetings()
        {
            InitializeComponent();
            AddFormCronElements();
        }

        private void AddFormCronElements()
        {
            AddDatagreed();
        }

        private void AddDatagreed()
        {
            DataGridView songsDataGridView = new DataGridView();
            this.Controls.Add(songsDataGridView);

            songsDataGridView.ColumnCount = 5;

            songsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            songsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            songsDataGridView.BackgroundColor = Color.White;

            songsDataGridView.Name = "songsDataGridView";
            songsDataGridView.Location = new Point(5, 150);
            songsDataGridView.Size = new Size(642, 200);
            songsDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            songsDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            songsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            songsDataGridView.GridColor = Color.Black;
            songsDataGridView.RowHeadersVisible = false;

            songsDataGridView.Columns[0].Name = "№";
            songsDataGridView.Columns[0].Width = 30;
            songsDataGridView.Columns[1].Name = "Имя";
            songsDataGridView.Columns[1].Width = 200;
            songsDataGridView.Columns[2].Name = "Страница";
            songsDataGridView.Columns[2].Width = 150;
            songsDataGridView.Columns[3].Name = "Задание";
            songsDataGridView.Columns[3].Width = 150;
            songsDataGridView.Columns[4].Name = "Album";
            songsDataGridView.Columns[4].Width = 109;

            songsDataGridView.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            songsDataGridView.MultiSelect = false;
            //songsDataGridView.Dock = DockStyle.Fill;
            songsDataGridView.BringToFront();

        }

    }
}
