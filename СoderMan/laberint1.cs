using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace СoderMan
{
    public partial class laberint1 : Form
    {
        Image playerImage;
        private string[] validDirections = { "top", "right", "left", "down", "help" };
        private int stepSizeX = 30;
        private int stepSizeY = 30;
        private int totalSteps = 0;
        private int StepsByFinall = 15;
        private int squareWidth = 648; // Ширина игровой доски
        private int squareHeight = 650;
        private Point characterLocation; // начальная позиция персонажа
        private int gg = 15;
        private char[,] maze = {
            {'+', '+', '+', '+', '+', '+', '+', '+', '+', '+', '+', '+', '+'},
            {'S', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '+'},
            {'+', ' ', '+', '+', '+', '+', '+', ' ', '+', '+', '+', ' ', '+'},
            {'+', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '+', ' ', ' ', '+'},
            {'+', '+', ' ', '+', '+', '+', '+', '+', ' ', '+', ' ', '+', '+'},
            {'+', ' ', ' ', ' ', ' ', '+', '+', '+', ' ', ' ', ' ', ' ', '+'},
            {'+', ' ', ' ', '+', ' ', '+', '+', '+', '+', '+', ' ', ' ', '+'},
            {'+', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '+', ' ', ' ', ' ', '+'},
            {'+', '+', ' ', '+', '+', '+', '+', '+', '+', '+', ' ', '+', '+'},
            {'+', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '+'},
            {'+', '+', ' ', '+', '+', '+', ' ', '+', '+', '+', '+', ' ', '+'},
            {'+', ' ', ' ', ' ', ' ', ' ', ' ', '+', '+', '+', '+', ' ', 'E'},
            {'+', '+', '+', ' ', '+', '+', ' ', '+', '+', '+', '+', ' ', '+'},
            {'+', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '+'},
            {'+', '+', '+', '+', '+', '+', '+', '+', '+', '+', '+', '+', '+'}
        };
        private const int TimerInterval = 50;
        private Point redBallLocation;
        private const int StepSize = 5;
        private int actualSteps = 0;
        private bool isGameFinished = false;
        private bool isMoving;
        private bool isPlayerDrawn = false;
        public laberint1()
        {
            InitializeComponent();

           
            characterLocation = FindStartPoint();

            this.Paint += laberint1_Paint;
            maskedTextBox1.KeyPress += maskedTextBox1_KeyPress;




        }
        private void laberint1_Paint(object sender, PaintEventArgs e)
        {
            // Draw the maze on the background of the main form
            DrawMaze(e.Graphics);

                e.Graphics.FillEllipse(Brushes.Red, characterLocation.X, characterLocation.Y, 20, 20);
              
            
        }




        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (isGameFinished)
            {
                // Ignore input while the player is moving
                e.Handled = true;
                return;
            }
            if (IsCyrillic(e.KeyChar))
            {

                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;

                string enteredText = maskedTextBox1.Text.Trim();


                textBox1.AppendText(enteredText + "\r\n");

                if (enteredText.ToLower() == "help")
                {
                    try
                    {

                        string pdfFilePath = @"\\Mac\Home\Desktop\курсовой\код\СoderMan\СoderMan\Assets\help\help.pdf";




                        System.Diagnostics.Process.Start(pdfFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при открытии файла PDF: {ex.Message}");
                    }
                }
                if (IsValidCommand(enteredText.ToLower()))
                {
                    Console.WriteLine("Команда '" + enteredText + "' введена.");

                    ProcessCommand(enteredText);
                }
                else
                {
                    Console.WriteLine("Неизвестная команда: " + enteredText);
                }

                maskedTextBox1.Clear();
            }
        }

        private void DrawMaze(Graphics g)
        {
            int cellSize = 30; // Adjust the size of each maze cell as needed

            for (int y = 0; y < maze.GetLength(0); y++)
            {
                for (int x = 0; x < maze.GetLength(1); x++)
                {
                    if (maze[y, x] == '+')
                    {
                        // Draw a wall
                        g.FillRectangle(Brushes.Black, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                    else if (maze[y, x] == 'S')
                    {
                        // Draw the start point
                        g.FillRectangle(Brushes.Green, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                    else if (maze[y, x] == 'E')
                    {
                        // Draw the end point
                        g.FillRectangle(Brushes.Red, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                    // Add more conditions based on your maze content (if needed)
                }
            }
        }
        private Point FindStartPoint()
        {
            for (int y = 0; y < maze.GetLength(0); y++)
            {
                for (int x = 0; x < maze.GetLength(1); x++)
                {
                    if (maze[y, x] == 'S')
                    {
                        return new Point(x * stepSizeX, y * stepSizeY);
                    }
                }
            }

            // Return a default position if 'S' is not found (adjust as needed)
            return new Point(0, 0);
        }
        private bool IsValidCommand(string command)
        {
            foreach (var direction in validDirections)
            {
                if (command.StartsWith(direction) && command.EndsWith(")"))
                {
                    return true;
                }
            }
            return false;
        }


        private void ProcessCommand(string command)
        {

            // Извлечение направления и числа шагов в скобках


        int openBracketIndex = command.IndexOf('(');
        int closeBracketIndex = command.IndexOf(')');

        if (openBracketIndex != -1 && closeBracketIndex != -1 && closeBracketIndex > openBracketIndex)
        {
            string direction = command.Substring(0, openBracketIndex);
            string stepsString = command.Substring(openBracketIndex + 1, closeBracketIndex - openBracketIndex - 1);

            if (int.TryParse(stepsString, out int steps))
            {
                totalSteps += steps; // Обновление общего числа шагов
                StepsByFinall -= steps;

                int deltaX = 0;
                int deltaY = 0;

                switch (direction.ToLower())
                {
                    case "top":
                        deltaY = -steps * stepSizeY; // Двигаться вверх
                        break;
                    case "right":
                        deltaX = steps * stepSizeX; // Двигаться вправо
                        break;
                    case "left":
                        deltaX = -steps * stepSizeX; // Двигаться влево
                        break;
                    case "down":
                        deltaY = steps * stepSizeY; // Двигаться вниз
                        break;
                    default:
                        Console.WriteLine($"Неизвестное направление: {direction}");
                        return; // Выход из метода, чтобы не вызывать MovePictureBox
                }

                // Проверка перед вызовом MovePictureBox
                if (IsValidMove(deltaX, deltaY, characterLocation.X + deltaX, characterLocation.Y + deltaY))
                {
                    MovePictureBox(deltaX, deltaY);

                    // Update the text of label1 and label2

                    // Disable input during movement
                    isMoving = true;

                    // Start a timer to enable input after a short delay (adjust as needed)
                    Timer enableInputTimer = new Timer();
                    enableInputTimer.Interval = TimerInterval;
                    enableInputTimer.Tick += (s, e) =>
                    {
                        // Enable input after the timer interval
                        isMoving = false;
                        enableInputTimer.Stop();
                        enableInputTimer.Dispose();
                    };
                    enableInputTimer.Start();
                }
                else
                {
                    Console.WriteLine($"Недопустимые координаты после перемещения.");
                }
            }
            else
            {
                Console.WriteLine($"Недопустимое количество шагов: {stepsString}");
            }
        }
        else
        {
            Console.WriteLine($"Недопустимый формат команды: {command}");
        }
    

        }
        private bool IsAtFinalPoint()
        {
            int playerX = characterLocation.X / stepSizeX;
            int playerY = characterLocation.Y / stepSizeY;

            return maze[playerY, playerX] == 'E';
        }

        private bool IsValidMove(int deltaX, int deltaY, int newX, int newY)
        {
            // Ensure the new position is within the boundaries of the maze
            if (newX >= 0 && newX <= squareWidth - pictureBox2.Width &&
                newY >= 0 && newY <= squareHeight - pictureBox2.Height)
            {
                // Check for collision with walls in the maze
                int playerX = newX / stepSizeX;
                int playerY = newY / stepSizeY;

                // Adjust the collision logic considering the size of the player
                if (maze[playerY, playerX] == '+')
                {
                    // Collision detected with a wall
                    return false;
                }

                return true; // The move is valid
            }

            return false;
        }
        private void MovePictureBox(int deltaX, int deltaY)
        {
            int stepsX = Math.Abs(deltaX) / stepSizeX;
            int stepsY = Math.Abs(deltaY) / stepSizeY;

            int directionX = Math.Sign(deltaX);
            int directionY = Math.Sign(deltaY);

            for (int i = 0; i < Math.Max(stepsX, stepsY); i++)
            {
                int newX = characterLocation.X + directionX * stepSizeX;
                int newY = characterLocation.Y + directionY * stepSizeY;

                // Check if the new position is valid
                if (IsValidMove(directionX * stepSizeX, directionY * stepSizeY, newX, newY))
                {
                    // Move to the new position
                    characterLocation = new Point(newX, newY);
                    pictureBox2.Location = characterLocation;
                    textBox1.AppendText($"Moved to X:{characterLocation.X}, Y:{characterLocation.Y}{Environment.NewLine}");

                    // Check if the player is on the target line
                    if (IsAtFinalPoint())
                    {
                        // Open your menu here
                        OpenMenu();
                        return;
                    }

                    // Pause for a short interval to simulate smooth movement
                    System.Threading.Thread.Sleep(TimerInterval - 30);
                    Application.DoEvents();
                }
                else
                {
                    textBox1.AppendText($"Collision detected at X:{newX}, Y:{newY}{Environment.NewLine}");
                    return;
                }
            }
        }
        private void OpenMenu()
        {
            // Add your menu-opening logic here
            MessageBox.Show("Congratulations! You reached the final point.");
        }
        private bool IsCyrillic(char c)
        {

            return (c >= 0x0400 && c <= 0x04FF);
        }


        private void label4_Click(object sender, EventArgs e)
        {
            RulesPages f1 = new RulesPages();
            f1.Show();
            this.Hide();
        }

    }
}