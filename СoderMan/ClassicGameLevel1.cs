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
    public partial class ClassicGameLevel1 : Form
    {
        Image playerImage;
        private string[] validDirections = { "top", "right", "left", "down", "help" };
        private int stepSizeX = 49;
        private int stepSizeY = 51;
        private int totalSteps = 0;
        private int StepsByFinall = 10;
        private int squareWidth = 648; // Ширина игровой доски
        private int squareHeight = 650;
        private Point characterLocation; // начальная позиция персонажа
        private int gg = 10;
        private const int TimerInterval = 50;
        private const int StepSize = 5;



        public ClassicGameLevel1()
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

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
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
                    if (IsValidMove(deltaX, deltaY))
                    {
                        MovePictureBox(deltaX, deltaY);

                        // Обновление текста Label1 с общим количеством шагов
                        label1.Text = $"Total Steps: {totalSteps}";
                        label2.Text = $"Remaining Steps: {StepsByFinall}";
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

         
           
            if (totalSteps <= gg && IsOnTargetLine())
            {

                label3.Text = "Уровень пройден!";
                label3.BackColor = Color.Green;
                label3.Visible = true;
                label4.Visible = true;
                label4.BackColor = Color.DarkGreen;
                label5.Text = "Следующий";
                label5.BackColor = Color.DarkGreen;
                label5.Visible = true;

            }
            else if (totalSteps >= gg)
            {
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
            } 
        }
        private bool IsOnTargetLine()
        {
            // Check if the player is on the target line (adjust the coordinates as needed)
            return (pictureBox2.Location.X >= 100 && pictureBox2.Location.X <= 394) && pictureBox2.Location.Y == 11;
      
        }

        private bool IsValidMove(int deltaX, int deltaY)
        {
            int newX = characterLocation.X + deltaX;
            int newY = characterLocation.Y + deltaY;

            // Adjust the boundaries based on the specified values for x1 and x2
            int x1 = 100; // Adjust this value as needed
            int x2 = 394; // Adjust this value as needed

            // Ensure the new position is within the boundaries of the game field (550x550)
            return newX >= 0 && newX <= squareWidth - pictureBox2.Width &&
                   newY >= 0 && newY <= squareHeight - pictureBox2.Height &&
                   newX >= x1 && newX <= x2 && newY <= 550;
        }

        private void MovePictureBox(int deltaX, int deltaY)
        {
            int stepsX = Math.Abs(deltaX) / StepSize;
            int stepsY = Math.Abs(deltaY) / StepSize;

            int directionX = Math.Sign(deltaX);
            int directionY = Math.Sign(deltaY);

            for (int i = 0; i < Math.Max(stepsX, stepsY); i++)
            {
                if (i < stepsX)
                {
                    characterLocation = new Point(characterLocation.X + directionX * StepSize, characterLocation.Y);
                }

                if (i < stepsY)
                {
                    characterLocation = new Point(characterLocation.X, characterLocation.Y + directionY * StepSize);
                }

                pictureBox2.Location = characterLocation;
                textBox1.AppendText($"Moved to X:{characterLocation.X}, Y:{characterLocation.Y}{Environment.NewLine}");

                // Pause briefly to make the movement visible
                System.Threading.Thread.Sleep(TimerInterval-30);
                Application.DoEvents(); // Allow the form to refresh
            }
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

            // Очистить textBox1
            textBox1.Clear();

            // Скрыть label3, label4, label5
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;

            // Сбросить количество шагов
            StepsByFinall = 10;
            totalSteps = 0;

            // Обновить тексты label1 и label2
            label1.Text = $"Total Steps: {totalSteps}";
            label2.Text = $"Remaining Steps: {StepsByFinall}";

            // Сбросить текущую позицию
            characterLocation = pictureBox2.Location;
        }

        private void NextLevel()
        {
            lvl2 f1 = new lvl2();
            f1.Show();
            this.Hide();
        }

        private void ClassicGameLevel1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}