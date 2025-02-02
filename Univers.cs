using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Game_Of_Life.Life;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Game_Of_Life
{
    public partial class Univers : Form
    {
        public static DataGridView DG;
        string nameOfSpace = "space";
        static int cellSize = 18;
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        
        bool lifeHappen = false;
        System.Timers.Timer spanTime = new System.Timers.Timer(333);

        public Univers()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            int ColumnSize = ((screenWidth + ((SizeChoice.univerSize * 10/25 * screenWidth)- ((10 / 25) * screenWidth))) / cellSize);
            int RowSize = ((screenHeight + ((SizeChoice.univerSize * 10 / 25 * screenHeight) - ((10 / 25) * screenHeight))) / cellSize);
            string title = "U N I V E R S";         
            int totalWidth = this.screenWidth / 8;
            int paddingSpaces = (totalWidth - title.Length) / 2;
            this.Text = new string(' ', paddingSpaces) + title + new string(' ', paddingSpaces);
            Space space = new Space(nameOfSpace)
            {
                Visible = false
            };
            space.DoubleBuffered(true);
            space.BuildGrid(cellSize, ColumnSize, RowSize);
            space.ResizeGrid(cellSize, ColumnSize, RowSize);
            space.MouseDown += DataGridView_MouseDown;
            space.KeyPress += space_KeyPress;           
            this.Controls.Add(space);
            DG = space;
            DG.MouseWheel += DG_Ctrl_MouseWheel;
            space.Visible = true;
            space.Refresh();
            Life life = new Life();
            petriDish = life.GetArrayPetriDish(DG);
            spanTime.Elapsed += new System.Timers.ElapsedEventHandler(TimedChange);
            spanTime.Enabled = lifeHappen;
        }
        public static void IsAliveList(int rowIndex, int colIndex, bool alive)
        {
            for (int i = 0; i < petriDish.Count; i++)
            {
                var cell = petriDish[i];
                if (cell.Coordinates[0, 0] == rowIndex && cell.Coordinates[0, 1] == colIndex)
                {
                    cell.IsAlive = alive;
                    break;
                }
            }
        }

        public static void IsDeadList(int rowIndex, int colIndex)
        {
            for (int i = 0; i < petriDish.Count; i++)
            {
                Cell cell = petriDish[i];
                if (cell.Coordinates[0, 0] == rowIndex && cell.Coordinates[0, 1] == colIndex)
                {
                    petriDish[i].IsAlive = false;
                    break;
                }
            }
        }
        private void DataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid != null)
            {
                var hitTest = grid.HitTest(e.X, e.Y);
                if (hitTest.Type == DataGridViewHitTestType.Cell)
                {
                    foreach (DataGridViewCell cell in grid.SelectedCells)
                    {
                        if (cell.Style.BackColor != Color.Red)
                        {
                            cell.Style.BackColor = Color.Red;
                            IsAliveList(cell.RowIndex, cell.ColumnIndex, true);
                        }
                        else
                        {
                            cell.Style.BackColor = Color.Gray;
                            IsDeadList(cell.RowIndex, cell.ColumnIndex);
                        }
                    }
                }
            }
        }

        private void TimedChange(object sender, EventArgs e)
        {
            Life life = new Life();
            petriDish = life.OrderFromChaos(petriDish, DG);
        }

        private void space_KeyPress(object sender, KeyPressEventArgs e)
        {
            Life life = new Life();
            if (e.KeyChar == ' ')
            {
                DG.ClearSelection();
                spanTime.Enabled = !spanTime.Enabled;
            }
    
            if (e.KeyChar == '+' && !spanTime.Enabled)
            {
                DG.ClearSelection();
                petriDish = life.ChaosFromNothing(petriDish, DG);
            }
            if (e.KeyChar == '-' && !spanTime.Enabled)
            {
                DG.ClearSelection();
                petriDish = life.NothingFromChaos(petriDish, DG);
            }
        }

        private void DG_Ctrl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                ((HandledMouseEventArgs)e).Handled = true;
                this.SuspendLayout();
                float zoomFactor = e.Delta > 0 ? 1.1f : 0.9f;
               
                cellSize = (int)(cellSize * zoomFactor);
                if (cellSize > 18)
                    cellSize = 18;
                else if (cellSize < 5)
                    cellSize = 5;
                int ColumnSize = ((screenWidth + ((SizeChoice.univerSize * 10 / 25 * screenWidth) - ((10 / 25) * screenWidth))) / cellSize);
                int RowSize = ((screenHeight + ((SizeChoice.univerSize * 10 / 25 * screenHeight) - ((10 / 25) * screenHeight))) / cellSize);

                DG.Width = cellSize * ColumnSize;
                DG.Height = cellSize * RowSize;
                this.Size = new Size(DG.Width, DG.Height);
             
                foreach (DataGridViewColumn column in DG.Columns)
                {
                    column.Width = cellSize;
                }
                foreach (DataGridViewRow row in DG.Rows)
                {
                    row.Height = cellSize;
                }
                DG.Refresh();
                this.ResumeLayout();
                           
            }
        }
    }

    public static class ControlExtensions
        {
            public static void DoubleBuffered(this Control control, bool enable)
            {
                var type = control.GetType();
                var property = type.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (property != null)
                {
                    property.SetValue(control, enable, null);
                }
            }
        }
   
}
        

    

