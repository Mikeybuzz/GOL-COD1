﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.VisualStyles;
using System.Globalization;
using System.Drawing.Printing;
using GOLStartUp;
using System.Drawing.Drawing2D;

namespace GOLstartUp
{

    public partial class Form1 : Form
    {
        // The universe array
        bool[,] universe = new bool[20, 20];

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        public Form1()
        {
            InitializeComponent();

            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {


            // Increment generation count
            generations++;
            applyRules();
            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
            graphicsPanel1.Invalidate();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            int livingCells = 0;
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    livingCells += CountNeighborsFinite(x, y);
                    // A rectangle to represent each cell in pixels
                    Rectangle cellRect = Rectangle.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    Font font1 = new Font("Arial", 14f);
                    int count = CountNeighborsFinite(x, y);
                    if (count > 0)
                    {
                        StringFormat stringFormat1 = new StringFormat();
                        stringFormat1.Alignment = StringAlignment.Center;
                        stringFormat1.LineAlignment = StringAlignment.Center;
                        e.Graphics.DrawString(count.ToString(), font1, Brushes.Red, cellRect, stringFormat1);
                    }
                }
            }
            Font font = new Font("Arial", 16f);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            int i = graphicsPanel1.ClientSize.Width;
            int j = graphicsPanel1.ClientSize.Height;
            
            Rectangle rect = new Rectangle( 0, j - 100, 300, 100);
            string msg = "Generations: " + generations;
            // int neighbors = 8;

             e.Graphics.DrawString(msg, font, Brushes.Red, rect, stringFormat);
            msg = "Number Of Living Cells: " + livingCells;
            Rectangle rect2 = new Rectangle(0, j - 80, 300, 100);
            e.Graphics.DrawString(msg, font, Brushes.Red, rect2, stringFormat);

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = e.Y / cellHeight;

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }

            //Font font = new Font("Arial", 20f);

            //StringFormat stringFormat = new StringFormat();
            // stringFormat.Alignment = StringAlignment.Center;
            // stringFormat.LineAlignment = StringAlignment.Center;

            // Rectangle rect = new Rectangle(0, 0, 100, 100);
            // int neighbors = 8;

            // e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Black, rect, stringFormat);

            //bool[,] universe = new bool[5, 5];
            //bool[,] scratchPad = new bool[5, 5];

            // Swap them...
           // bool[,] temp = universe;
           // universe = scratchPad;
           // scratchPad = temp;
        }


       private void applyRules()
       {
            //Living cells with less than 2 living neighbors die in the next generation.
            //Living cells with more than 3 living neighbors die in the next generation.
            //Living cells with 2 or 3 living neighbors live in the next generation.
            //Dead cells with exactly 3 living neighbors live in the next generation.
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    int count = CountNeighborsFinite(x, y);
                    if (count < 2)
                    {
                        universe[x, y] = false;
                    }
                    if (count > 3)
                    {
                        universe[x, y] = false;
                    }
                    if (count == 2 || count == 3)
                    {
                        universe[x, y] = true;
                    }
                    if (count == 3 && universe[x, y] == false)
                    {
                        universe[x, y] = true;
                    }
                }
            }
       }


        private int CountNeighborsFinite(int x, int y)
            {
                int count = 0;
                int xLen = universe.GetLength(0);
                int yLen = universe.GetLength(1);
                for (int yOffset = -1; yOffset <= 1; yOffset++)
                {
                    for (int xOffset = -1; xOffset <= 1; xOffset++)
                    {
                        int xCheck = x + xOffset;
                        int yCheck = y + yOffset;
                        // if xOffset and yOffset are both equal to 0 then continue
                        if (xCheck == 0 && yOffset == 0)
                        {
                            continue;
                        }
                        // if xCheck is less than 0 then continue
                        if (xCheck < 0)
                        {
                            continue;
                        }
                        // if yCheck is less than 0 then continue
                        if (yCheck < 0)
                        {
                            continue;
                        }
                        // if xCheck is greater than or equal too xLen then continue
                        if (xCheck >= xLen)
                        {
                            continue;
                        }
                        // if yCheck is greater than or equal too yLen then continue
                        if (yCheck >= yLen)
                        {
                            continue;
                        }

                        if (universe[xCheck, yCheck] == true)
                            count++;
                    }
                }
            
            return count;
        }


        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if(xOffset == 0 && yCheck == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then set to xLen - 1
                    if(xCheck < 0)
                    {
                        xLen = -1;
                    }
                    // if yCheck is less than 0 then set to yLen - 1
                    if(yCheck < 0)
                    {
                        yLen = -1;
                    }
                    // if xCheck is greater than or equal too xLen then set to 0
                    if(xCheck >= xLen)
                    {
                        xLen = 0;
                    }
                    // if yCheck is greater than or equal too yLen then set to 0
                    if(yCheck >= yLen)
                    {
                        yLen = 0;
                    }
                    if (universe[xCheck, yCheck] == true) 
                        count++;
                }
            }
            return count;
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            //no interact
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //next
            NextGeneration();
            this.Invalidate();

        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //pause
            timer.Stop();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //start
            timer.Start();        
        }


        private void Next_Click(object sender, EventArgs e)
        {
            //next
            NextGeneration();
            this.Invalidate();
        }

        private void clearUniverse()
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x,y] = false;
                }

             }
        }

        private void randomize(int seedValue)
        {
            Random rnd = new Random(seedValue);
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (rnd.Next()%2 == 0)
                    {
                        universe[x, y] = false;
                    }
                    else if (rnd.Next()%2 == 1)
                    {
                        universe[x, y] = true;
                    }
                   
                }

            }
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
           
            clearUniverse();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            string fileName = "SaveFile.txt";
            saveData(fileName);
        }


        private void saveData(string fileName)
        {
            int xlength = universe.GetLength(0);
            int ylength = universe.GetLength(1);
            string[] lines = new string[ylength];
            for (int i = 0; i < ylength; i++)
            {
                string line = "";
                for (int j = 0; j < xlength; j++)
                {
                    if (universe[i, j] == true)
                    {
                        line += "O";
                    }
                    else
                    {
                        line += "X";
                    }
                }
                lines[i] = line;
            }
            File.WriteAllLines(fileName, lines);
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            string fileName = "SaveFile.txt";
            loadData(fileName);
        }
        
        private void loadData(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                for (int j = 0; j < lines.Length; j++)
                {
                    if (line[j] == 'O')
                    {
                        universe[i, j] = true;
                    }
                    else
                    {
                        universe[i, j] = false;
                    }
                }
            }
            graphicsPanel1.Invalidate();
        }

        private void writeInfo()
        {
            Font font = new Font("Arial", 20f);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

             Rectangle rect = new Rectangle(0, 0, 100, 100);

            string msg = "Living Cells: ";
           
           // e.Graphics.DrawString(msg, font, Brushes.Black, rect, stringFormat);

            //bool[,] universe = new bool[5, 5];
            //bool[,] scratchPad = new bool[5, 5];

            // Swap them...
            // bool[,] temp = universe;
            // universe = scratchPad;
            // scratchPad = temp;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModalDialog dlg = new ModalDialog();
          
            // Set the properties
            
            if (DialogResult.OK == dlg.ShowDialog())
            {
                // Get the properties
                
                string value = dlg.Controls["ComboTimerInterval"].Text;
                if (value != "")
                {
                    timer.Interval = Convert.ToInt32(value);
                }
                int newwidth = 0;
                int newheight = 0;

                string value2 = dlg.Controls["ComboUniverseHeight"].Text;
                if (value2 != "")
                {
                    newheight = Convert.ToInt32(value2);
                   // graphicsPanel1.Height = Convert.ToInt32(value2);
                }
                string value3 = dlg.Controls["ComboUniverseWidth"].Text;
                if (value3 != "")
                {
                    newwidth = Convert.ToInt32(value3);
                    //graphicsPanel1.Width = Convert.ToInt32(value3);
                }
                if (newwidth != 0 && newheight != 0)
                {
                    bool[,] universe = new bool[newwidth, newheight];
                    graphicsPanel1.Invalidate();
                    Refresh();
                }

            }
        }

        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}