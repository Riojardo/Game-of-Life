using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Game_Of_Life.Life;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
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
        public static int generation { get; set; }        
        public static string textGeneration = $" Generation : {generation.ToString()}";
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
            int paddingSpaces = (totalWidth);
            this.Text = new string(' ', paddingSpaces) + title ;        
            label1.Text = textGeneration;
           // generation = 0;
            label1.Font = new Font("Arial", 24);
            label1.Dock = DockStyle.Left;
            label1.Dock = DockStyle.Top;
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
            DG.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DG.MouseWheel += DG_Ctrl_MouseWheel;
            DG.SizeChanged += new EventHandler(DG_SizeChanged);
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

        private void UpdateGeneration()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    label1.Text = $"Generation: {generation}";
                }));
            }
            else
            {
                label1.Text = $"Generation: {generation}";
            }
        }

        private void TimedChange(object sender, EventArgs e)
        {
            Life life = new Life();
            petriDish = life.OrderFromChaos(petriDish, DG);
            UpdateGeneration();

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
                UpdateGeneration();
            }
            if (e.KeyChar == '-' && !spanTime.Enabled)
            {
                DG.ClearSelection();
                petriDish = life.NothingFromChaos(petriDish, DG);
                UpdateGeneration();
            }
        }

        private void DG_SizeChanged(object sender, EventArgs e)
        {
            this.Size = new Size(DG.Width + this.Margin.Horizontal, DG.Height + this.Margin.Vertical);
        }
        private void DG_Ctrl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                ((HandledMouseEventArgs)e).Handled = true;
                this.SuspendLayout();
                int[] cellsizes = new int[] { 4, 6, 8, 10, 12, 14, 16, 18 };
                int zoomFactor = e.Delta > 0 ? 1 : -1;
                for(int i = 0; i < cellsizes.Length; i++)
                {
                    if (cellSize == cellsizes[0] && zoomFactor < 0)
                    { cellSize = cellsizes[0]; break; }
                    if (cellSize == cellsizes[7] && zoomFactor > 0)
                    { cellSize = cellsizes[7]; break; }
                    if (cellSize == cellsizes[i])
                    { cellSize = cellsizes[i + zoomFactor]; }   
                }

                if (cellSize == 4 || cellSize == 18)
                {
                    DG.Visible = true;
                }
                else
                {
                    DG.Visible = false;
                }
                int columnCount = DG.Columns.GetColumnCount(DataGridViewElementStates.Visible);
                int rowCount = DG.Rows.GetRowCount(DataGridViewElementStates.Visible);
                int columnWidth = cellSize;
                int rowHeight = cellSize;

                DG.Width = columnCount * columnWidth;
                DG.Height = rowCount * rowHeight;

                foreach (DataGridViewColumn column in DG.Columns)
                {
                    column.Width = columnWidth;
                }
                foreach (DataGridViewRow row in DG.Rows)
                {
                    row.Height = rowHeight;
                }

                int availableWidth = this.ClientSize.Width - DG.Width;
                int availableHeight = this.ClientSize.Height - DG.Height;

                int horizontalOffset = availableWidth / 2;
                int verticalOffset = availableHeight / 2;

                DG.Location = new Point(horizontalOffset, verticalOffset);

                DG.Refresh();
                this.ResumeLayout();
                DG.Visible = true;
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
        

    

