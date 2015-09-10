using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LightsOut
{
    public partial class MainForm : Form
    {
        private const int GRID_OFFSET = 25;     // Distance from upper-left side of window
        private const int NUM_CELLS = 3;        // Number of cells in grid
        private const int GRID_LENGTH = 200;    // Size in pixels of grid
        private const int CELL_LENGTH = GRID_LENGTH / NUM_CELLS;

        private bool[,] grid;           // Stores on/off state of cells in grid
        private Random rand;            // Creates a random number of cells


        public MainForm()
        {
            InitializeComponent();

            rand = new Random();

            grid = new bool[NUM_CELLS, NUM_CELLS];

            for (int r = 0; r < NUM_CELLS; r++)
                for (int c = 0; c < NUM_CELLS; c++)
                    grid[r, c] = true;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int r =0; r < NUM_CELLS; r++)
                for (int c = 0; c < NUM_CELLS; c++)
                {

                    Brush brush;
                    Pen pen;

                    if (grid[r, c])
                    {
                        pen = Pens.Black;
                        brush = Brushes.White;
                    }

                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black;

                    }


                    int x = c * CELL_LENGTH + GRID_OFFSET;
                    int y = r * CELL_LENGTH + GRID_OFFSET;


                   g.DrawRectangle(pen, x, y, CELL_LENGTH, CELL_LENGTH);

                    g.FillRectangle(brush, x + 1, y + 1, CELL_LENGTH - 1, CELL_LENGTH - 1);
                }
        }


        private void MainForm_MouseDown(object sender, System.Windows.Forms.MouseEventArgs  e)
        {
            // Make sure click was inside the grid
            if (e.X < GRID_OFFSET || e.X > CELL_LENGTH * NUM_CELLS + GRID_OFFSET ||
            e.Y < GRID_OFFSET || e.Y > CELL_LENGTH * NUM_CELLS + GRID_OFFSET)
                return;


            // Find row, col of mouse press
            int r = (e.Y - GRID_OFFSET) / CELL_LENGTH;
            int c = (e.X - GRID_OFFSET) / CELL_LENGTH;


            // Invert selected box and all surrounding boxes
            for (int i = r - 1; i <= r + 1; i++)
                for (int j = c - 1; j <= c + 1; j++)
                    if (i >= 0 && i < NUM_CELLS && j >= 0 && j < NUM_CELLS)
                        grid[i, j] = !grid[i, j];


            // Redraw grid
            this.Invalidate();

            // Check to see if puzzle has been solved
            if (PlayerWon())
            {
                // Display winner dialog box
                MessageBox.Show(this, "Congratulations! You've won!", "Lights Out!",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 

        }

        private bool PlayerWon()
        {

            bool winner = false;


            for (int r =0; r < NUM_CELLS; r++)
                for (int c = 0; c < NUM_CELLS; c++)
                {


                    int x = c * CELL_LENGTH + GRID_OFFSET;
                    int y = r * CELL_LENGTH + GRID_OFFSET;

                    if (!grid[r,c])
                    {
                        winner = true ;
                    }

                    else
                    {
                        return false;
                    }

                }
            return winner;            
        }


        private void newGameButton_Click(object sender, EventArgs e)
        {
            // Fill grid with either white or black
            for (int r = 0; r < NUM_CELLS; r++)
                for (int c = 0; c < NUM_CELLS; c++)
                    grid[r, c] = rand.Next(2) == 1;
            // Redraw grid
            this.Invalidate(); 
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newGameButton_Click(sender, e);
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void About_Click(object sender, EventArgs e)
        {
            AboutFormm aboutBox = new AboutFormm();
            aboutBox.ShowDialog(this);
        }


        

    }
}
