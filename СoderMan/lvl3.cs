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
    public partial class lvl3 : Form
    {
        Image playerImage;
        private string[] validDirections = { "top", "right", "left", "down", "help" };
        private int stepSizeX = 49;
        private int stepSizeY = 51;
        private int totalSteps = 0;
        private int StepsByFinall = 16;
        private int squareWidth = 648; // Ширина игровой доски
        private int squareHeight = 650;
        private Point characterLocation; // начальная позиция персонажа
        private int gg = 15;
        private const int TimerInterval = 50;
        private const int StepSize = 5;
        private int actualSteps = 0;
        private bool isGameFinished = false;
        private bool isMoving;
        public lvl3()
        {
            InitializeComponent();

            playerImage = new Bitmap(@"\\Mac\Home\Desktop\курсовой\код\СoderMan\СoderMan\Assets\classicGAme\hero\13.png");
            maskedTextBox1.Size = new Size(348, 100);
            Image part = new Bitmap(56, 75);
            Graphics g = Graphics.FromImage(part);

            g.DrawImage(playerImage, 0, 0, new Rectangle(new Point(615, 65), new Size(56, 75)), GraphicsUnit.Pixel);
            //615
            pictureBox2.Size = new Size(56, 75);
            pictureBox2.Image = part;
            Console.WriteLine(pictureBox2.Location);
            maskedTextBox1.KeyPress += maskedTextBox1_KeyPress;
            characterLocation = pictureBox2.Location;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;

            // Инициализация таймера для плавного движения
        }
        //   private void update(object sender, EventArgs e)
        //   {        
        //    Console.WriteLine("firimv");
        //playAnimation();
        //     currFrame +=50;
        //   }
        //     private void playAnimation()
        //     {
        //       Image part = new Bitmap(56, 75);
        //         Graphics g = Graphics.FromImage(part);
        //
        //        g.DrawImage(playerImage, 0, 0, new Rectangle(new Point(615+currFrame, 65), new Size(56, 75)), GraphicsUnit.Pixel);
        //         //615
        //         pictureBox2.Size = new Size(56, 75);
        //         pictureBox2.Image = part;
        //     }
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
                    if (IsValidMove(deltaX, deltaY))
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
        private bool IsOnTargetLine()
        {
            // Check if the player is on the target line (adjust the coordinates as needed)
            return (pictureBox2.Location.X >= 100 && pictureBox2.Location.X <= 394) && (pictureBox2.Location.Y == 11 && pictureBox2.Location.Y == 80);

        }

        private bool IsValidMove(int deltaX, int deltaY)
        {
            int newX = characterLocation.X + deltaX;
            int newY = characterLocation.Y + deltaY;

            // Adjust the boundaries based on the specified values for x1 and x2
            int x1 = 100; // Adjust this value as needed
            int x2 = 450; // Adjust this value as needed
         

            // Check for collision with pictureBox1
            if (newX < pictureBox1.Right &&
                newX + pictureBox2.Width > pictureBox1.Left &&
                newY < pictureBox1.Bottom &&
                newY + pictureBox2.Height > pictureBox1.Top)
            {
                // Collision detected, handle it here
                textBox1.AppendText($"Collision with object at X:{pictureBox1.Location.X}, Y:{pictureBox1.Location.Y}{Environment.NewLine}");
                return false; // Invalid move due to collision
            }

            // Ensure the new position is within the boundaries of the game field (550x550)
            if (newX >= 0 && newX <= squareWidth - pictureBox2.Width &&
                newY >= 0 && newY <= squareHeight - pictureBox2.Height &&
                newX >= x1 && newX <= x2 && newY >= 11 && newY <= 550)
            {
                // Check if the character crosses the boundary of a 50x50 square
                int squareSize = 50;
                if ((characterLocation.X / squareSize) != (newX / squareSize) ||
                    (characterLocation.Y / squareSize) != (newY / squareSize))
                {
                    // Character crossed into a new square, increment actualSteps
                    actualSteps++;
                    Console.WriteLine(actualSteps);
                }

                return true; // Valid move
            }

            return false;
        }

        private void MovePictureBox(int deltaX, int deltaY)
        {
            int stepsX = Math.Abs(deltaX) / StepSize;
            int stepsY = Math.Abs(deltaY) / StepSize;

            int directionX = Math.Sign(deltaX);
            int directionY = Math.Sign(deltaY);

            for (int i = 0; i < Math.Max(stepsX, stepsY); i++)
            {
                // Calculate the new position
                int newX = characterLocation.X + directionX * StepSize;
                int newY = characterLocation.Y + directionY * StepSize;

                // Check for collision at the new position
                if (!IsValidMove(directionX * StepSize, directionY * StepSize))
                {
                    // If collision detected and steps are not completed, stop movement
                    if (i < Math.Max(stepsX, stepsY) - 1)
                    {
                        textBox1.AppendText($"Collision detected at X:{newX}, Y:{newY}{Environment.NewLine}");
                        return;
                    }
                }

                // Update the characterLocation
                characterLocation = new Point(newX, newY);

                // Update the PictureBox location
                pictureBox2.Location = characterLocation;
                textBox1.AppendText($"Moved to X:{characterLocation.X}, Y:{characterLocation.Y}{Environment.NewLine}");

                // Pause briefly to make the movement visible
                System.Threading.Thread.Sleep(TimerInterval - 30);
                Application.DoEvents(); // Allow the form to refresh
            }

            // Check if the character has reached the destination

            int newYY = characterLocation.Y + deltaY;
            if (newYY < 60 && actualSteps <= gg)
            {
                isGameFinished = true;
                label3.Text = "Уровень пройден!";
                label3.BackColor = Color.Green;
                label3.Visible = true;
                label4.Visible = true;
                label4.BackColor = Color.DarkGreen;
                label5.Text = "Следующий";
                label5.BackColor = Color.DarkGreen;
                label5.Visible = true;

            }
            else if (newYY > 60 && actualSteps >= gg)
            {
                isGameFinished = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
            }

            // Update total steps with actual steps taken
            // Update the text of label1 and label2
            label7.Text = $"Total Steps: {actualSteps}";
            label6.Text = $"Remaining Steps: {StepsByFinall - actualSteps}";
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

        private void label5_Click_1(object sender, EventArgs e)
        {
            if (label5.Text == "Следующий")
            {
                NextLevel();
            }
            else
            {
                RestartLevel();
            }

        }

        private void RestartLevel()
        {
            // Сбросить координаты pictureBox2 в начальные значения
            pictureBox2.Location = new Point(345, 521);
            isGameFinished = false;
            // Очистить textBox1
            textBox1.Clear();

            // Скрыть label3, label4, label5
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
             
            // Сбросить количество шагов
            StepsByFinall = 16;
            actualSteps = 0;

            // Обновить тексты label1 и label2
            label7.Text = $"Total Steps: {actualSteps}";
            label6.Text = $"Remaining Steps: {StepsByFinall - actualSteps}";

            // Сбросить текущую позицию
            characterLocation = pictureBox2.Location;
        }

        private void NextLevel()
        {
            lvl4 f1 = new lvl4();
            f1.Show();
            this.Hide();
            isGameFinished = false;
        }

        private void lvl3_Load(object sender, EventArgs e)
        {

        }
    }
}