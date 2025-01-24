using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Of_Life
{
    internal class Life
    {
        public static List<Cell> petriDish = new List<Cell>();

        public class Cell
        {
            public bool IsAlive { get; set; }
            public int[,] Coordinates { get; private set; }

            public Cell(bool isAlive, int[,] coordinates)
            {
                IsAlive = isAlive;
                Coordinates = new int[coordinates.GetLength(0), coordinates.GetLength(1)];
                Array.Copy(coordinates, Coordinates, coordinates.Length);
            }
        }
        public List<Cell> GetArrayPetriDish(DataGridView x)
        {
            for (int i = 0; i < x.RowCount; i++)
            {
                for (int y = 0; y < x.ColumnCount; y++)
                {
                    int[,] array = { { i, y } };
                    Cell cell = new Cell(false, array);
                    petriDish.Add(cell);
                }
            }
            return petriDish;
        }

        public List<Cell> OrderFromChaos(List<Cell> petriDish,  DataGridView DG)
        {          
            var cellDico = Life.petriDish.ToDictionary(c => (c.Coordinates[0, 0], c.Coordinates[0, 1]), c => c);

            List<Cell> newGeneration = Life.petriDish.Select(cell => new Cell(cell.IsAlive, cell.Coordinates)).ToList();

            for (int i = 0; i < newGeneration.Count; i++)
            {
                var cell = Life.petriDish[i];
                int rowIndex = cell.Coordinates[0, 0];
                int colIndex = cell.Coordinates[0, 1];
                int adjacentAlive = 0;

                for (int x = rowIndex - 1; x <= rowIndex + 1; x++)
                {
                    for (int y = colIndex - 1; y <= colIndex + 1; y++)
                    {
                        if (x == rowIndex && y == colIndex) continue;

                        if (cellDico.TryGetValue((x, y), out var neighbor) && neighbor.IsAlive)
                        {
                            adjacentAlive++;
                        }
                    }
                }
                if (cell.IsAlive)
                {
                    if (adjacentAlive < 2 || adjacentAlive > 3)
                        newGeneration[i].IsAlive = false;
                }
                else
                {
                    if (adjacentAlive == 3)
                        newGeneration[i].IsAlive = true;
                }
            }
            foreach (Cell nextCell in newGeneration)
            {
                int rowIndex = nextCell.Coordinates[0, 0];
                int colIndex = nextCell.Coordinates[0, 1];
                DG.Rows[rowIndex].Cells[colIndex].Style.BackColor = nextCell.IsAlive ? Color.Red : Color.Gray;
            }
            cellDico = null;
            Univers.generation++;          
            return newGeneration;
        }

        public List<Cell> ChaosFromNothing(List<Cell> petriDish, DataGridView DG)
        {
            Random rand = new Random();

            List<Cell> newGeneration = petriDish.Select(cell => new Cell(rand.Next(0, 2) == 0, cell.Coordinates)).ToList();

            DG.SuspendLayout();
            for (int i = 0; i < newGeneration.Count; i++)
            {
                var nextCell = newGeneration[i];
                int rowIndex = nextCell.Coordinates[0, 0];
                int colIndex = nextCell.Coordinates[0, 1];
                DG.Rows[rowIndex].Cells[colIndex].Style.BackColor = nextCell.IsAlive ? Color.Red : Color.Gray;
            }
            DG.ResumeLayout();
            Univers.generation = 0;
            return newGeneration;
        }

        public List<Cell> NothingFromChaos(List<Cell> petriDish, DataGridView DG)
        {
            Random rand = new Random();

            List<Cell> newGeneration = petriDish.Select(cell => new Cell(false, cell.Coordinates)).ToList();

            DG.SuspendLayout();
            for (int i = 0; i < newGeneration.Count; i++)
            {
                var nextCell = newGeneration[i];
                int rowIndex = nextCell.Coordinates[0, 0];
                int colIndex = nextCell.Coordinates[0, 1];
                DG.Rows[rowIndex].Cells[colIndex].Style.BackColor = nextCell.IsAlive ? Color.Red : Color.Gray;
            }
            DG.ResumeLayout();
            Univers.generation = 0;
            return newGeneration;
        }
    }
}
