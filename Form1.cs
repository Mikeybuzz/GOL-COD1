using System;
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
using System.Diagnostics;

namespace GOLstartUp
{
    public partial class Form1 : Form
    {
        public Settings userSettings;
        
        public bool[,] universe;

        bool showNeighborCount = true;
        bool showHud = true;
        bool showGrid = true;

        
        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;
        // seed count
        int seed = 0;

        public Form1()
        {
            InitializeComponent();

            userSettings = new Settings("settings.txt");
            universe = new bool[userSettings.NumRows, userSettings.NumColumns];

            // Setup the timer
            timer.Interval = userSettings.TimerIntervalMS; // milliseconds
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
            // new int
            int cellCount = 0;
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
                        cellCount ++;
                    }

                    
                    // Outline the cell with a pen

                    if (showGrid)
                        e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);

                    if (showNeighborCount)
                    {
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
            }

            if (showHud == true)
            {
                // HUD 

                //Generation counter

                int colLength = universe.GetLength(1);

                Rectangle cellRect = Rectangle.Empty;
                cellRect.X = 3;
                cellRect.Width = 150;
                cellRect.Height = 60;
                Font font = new Font("Arial", 12f);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                cellRect.X = -25;
                cellRect.Y = colLength * cellHeight - 100;
                e.Graphics.DrawString("Generations: " + generations.ToString(), font, Brushes.Red, cellRect, stringFormat);

                //Cell counter

                Rectangle cellRect1 = Rectangle.Empty;
                cellRect1.Width = 150;
                cellRect1.Height = 60;
                cellRect1.X = -27;
                cellRect1.Y = colLength * cellHeight - 80;
                e.Graphics.DrawString("Cell Count: " + cellCount.ToString(), font, Brushes.Red, cellRect1, stringFormat);

                //Boundry type

                Rectangle cellRect2 = Rectangle.Empty;
                cellRect2.Width = 150;
                cellRect2.Height = 60;
                cellRect2.X = -12;
                cellRect2.Y = colLength * cellHeight - 60;
                e.Graphics.DrawString("Boundary Type: " + generations.ToString(), font, Brushes.Red, cellRect2, stringFormat);

                //Size of Universe
                Rectangle cellRect3 = Rectangle.Empty;
                cellRect3.Width = 150;
                cellRect3.Height = 60;
                cellRect3.X = -15;
                cellRect3.Y = colLength * cellHeight - 40;
                e.Graphics.DrawString("Universe Size: " + generations.ToString(), font, Brushes.Red, cellRect3, stringFormat);
            }

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
                        continue;

                    // if xCheck is less than 0 then continue
                    if (xCheck < 0)
                        continue;

                    // if yCheck is less than 0 then continue
                    if (yCheck < 0)
                        continue;

                    // if xCheck is greater than or equal too xLen then continue
                    if (xCheck >= xLen)
                        continue;

                    // if yCheck is greater than or equal too yLen then continue
                    if (yCheck >= yLen)
                        continue;

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

        //clear universe
        private void ClearUniverse()
        {
            timer.Stop();

            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x,y] = false;
                }
            }

            generations = 0;
            graphicsPanel1.Invalidate();
        }

        //Random seed
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

        //New Universe
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            ClearUniverse();
        }

        //Save File
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = saveFileDialog1.FileName;
                saveData(file);
            }
        }

        //Save Data
        private void saveData(string fileName)
        {
            int xlength = universe.GetLength(0);
            int ylength = universe.GetLength(1);
            string[] lines = new string[ylength];
            for (int i = 0; i < xlength; i++)
            {
                string line = "";
                for (int j = 0; j < ylength; j++)
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

        //Open File
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                loadData(file);
            }
        }
        
        //Load File
        private void loadData(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                for (int j = 0; j < line.Length; j++)
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
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModalDialog dlg = new ModalDialog();

            // Set the properties
            dlg.Controls["ComboUniverseWidth"].Text = universe.GetLength(0).ToString();
            dlg.Controls["ComboUniverseHeight"].Text = universe.GetLength(1).ToString();


            if (DialogResult.OK == dlg.ShowDialog())
            {
                // Get the properties
                
                string value = dlg.Controls["ComboTimerInterval"].Text;
                if (value != "")
                {
                    var newInterval = Convert.ToInt32(value);
                    if (newInterval != timer.Interval)
                    {
                        timer.Interval = newInterval;
                    }
                }

                int newWidth = 0;
                int newHeight = 0;

                string value2 = dlg.Controls["ComboUniverseHeight"].Text;
                if (value2 != "")
                {
                    newHeight = Convert.ToInt32(value2);
                }
                string value3 = dlg.Controls["ComboUniverseWidth"].Text;
                if (value3 != "")
                {
                    newWidth = Convert.ToInt32(value3);
                }

                bool areInputsValid = newWidth != 0 && newHeight != 0;
                bool isWidthChanged = newWidth != universe.GetLength(0);
                bool isHeightChanged = newHeight != universe.GetLength(1);
                bool haveInputsChanged = isWidthChanged || isHeightChanged;

                if (areInputsValid && haveInputsChanged)
                {
                    universe = new bool[newWidth, newHeight];
                    graphicsPanel1.Invalidate();
                    Refresh();
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearUniverse();
        }

        //Save file
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = saveFileDialog1.FileName;
                saveData(file);
            }
        }

        //Open File
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                loadData(file);
            }
        }

        //From Seed
        private void fromSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sorry, this feature isn't added yet!");
            int randomNum
        }

        //Generate Seed
        private void generateFromSeed(int seed)
        {
            this.seed = seed;
            Random rnd = new Random(seed);
            for (int i = 0; i < universe.GetLength(0); i++)
            {
                for (int j = 0; j < universe.GetLength(1); j++)
                {
                    if (rnd.NextDouble() < 0.3)
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

        //Reset
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userSettings.ResetToDefaults();

            ApplySettings();
        }

        //Reload to Default
        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplySettings();
        }

        private void ApplySettings()
        {
            bool areRowsDifferent = universe.GetLength(0) != userSettings.NumRows;
            bool areColsDifferent = universe.GetLength(1) != userSettings.NumColumns;
            if (areRowsDifferent || areColsDifferent)
            {
                universe = new bool[userSettings.NumRows, userSettings.NumColumns];
                ClearUniverse();
            }

            timer.Interval = userSettings.TimerIntervalMS;
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            userSettings.TimerIntervalMS = timer.Interval;
            userSettings.NumRows = universe.GetLength(0);
            userSettings.NumColumns = universe.GetLength(1);

            userSettings.Save();
        }
        // Neighbor Toggle
        private void neighborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showNeighborCount = !showNeighborCount;
            graphicsPanel1.Invalidate();
        }
        //HUD toggle
        private void hUDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showHud = !showHud;
            graphicsPanel1.Invalidate();
        }

        // Grid toggle
        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showGrid = !showGrid;
            graphicsPanel1.Invalidate();
        }
        //Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Torodial
        private void toroidalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sorry, this feature isn't added yet!");
        }
        //Finite
        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sorry, this feature isn't added yet!");
        }

        //start
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Start();
        }

        //Pause
        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        //Next
        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NextGeneration();
            this.Invalidate();
        }
    }
}
