using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Of_Life
{
    
    internal class Space : DataGridView
    {

        public Space(string Name)
        {
            string space = Name;
            this.Name = space;
            this.Visible = false;
            this.ReadOnly = true;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            this.AllowUserToResizeColumns = false;
            this.AllowUserToResizeRows = false;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RowHeadersVisible = false;
            this.ColumnHeadersVisible = false;
            this.ScrollBars = ScrollBars.None; 
            this.Location = new Point(0, 0);
            this.Name = "DG";
            this.TabIndex = 0;
            this.GridColor = Color.Black;

            this.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = this.DefaultCellStyle.BackColor != Color.Gray ? Color.Gray : Color.Red,
                SelectionForeColor = Color.Red,
            };

            this.Dock = DockStyle.None;
        }
        public void BuildGrid(int cellSize, int ColumnSize, int RowSize)
        {
            this.SuspendLayout();

            for (int i = 0; i < ColumnSize; i++)
            {
                var column = new DataGridViewTextBoxColumn
                {
                    Width = cellSize
                };
                this.Columns.Add(column);
            }

            for (int i = 0; i < RowSize; i++)
            {
                this.Rows.Add();
                this.Rows[i].Height = cellSize;
            }        
        }
        public void ResizeGrid(int cellSize,int ColumnSize,int RowSize)
        {
            this.Width = cellSize * ColumnSize;
            this.Height = cellSize * RowSize;
            
            this.ResumeLayout();
        }

        public List<(bool, int[,])> GetArray(DataGridView x)
        {
            List<(bool, int[,])> Position = new List<(bool, int[,])>();
            int[,] array = new int[x.RowCount, x.ColumnCount];
            Position.Add((false, array));

            return Position;
        }
    }
}
