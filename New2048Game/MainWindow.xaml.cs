using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace New2048Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Button[,] gamezone = new Button[4, 4];
        int Score = 0;
        int HighScore = 0;
        bool Flag2048 = false;

        public MainWindow()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()
        {
            Score = 0;
            ScoreTB.Text = "0";
            Flag2048 = false;
            DrawGameZone();
            SpreadOutNumber();
            SpreadOutNumber();
        }

        private void DrawGameZone()
        {
            gamezone = new Button[4, 4];
            grid1.Children.Clear();
            int x = -233;
            int y = -233;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Button tb = new Button();
                    gamezone[i, j] = tb;
                    gamezone[i, j].Template = this.FindResource("Template0") as ControlTemplate;
                    if (i == 0 && j == 0)
                    {
                        gamezone[i, j].Margin = new System.Windows.Thickness(x, y, 4, 3);
                    }
                    else if (i == 0)
                    {
                        y = -233;
                        x += 158;
                    }
                    else if (j == 0)
                    {
                        x = -233;
                        y += 158;
                    }
                    else
                    {
                        x += 158;
                    }
                    gamezone[i, j].Content = "";
                    gamezone[i, j].Margin = new System.Windows.Thickness(x, y, 4, 3);
                    grid1.Children.Add(gamezone[i, j]);
                }
            }
        }

        private void SpreadOutNumber()
        {
            Random random = new Random();
            int result = 2;
            int chance = random.Next(1, 11);
            if (chance <= 1)
                result = 4;
            else
                result = 2;
            while (true)
            {
                int i = random.Next(0, 4);
                int j = random.Next(0, 4);
                if (i < 4 && j < 4 && gamezone[i, j].Content.ToString() == "")
                {
                    gamezone[i, j].Template = this.FindResource("Template" + result) as ControlTemplate;
                    gamezone[i, j].Content = "" + result;
                    break;
                }
            }
        }

        private bool Check()
        {
            bool temp = false;
            int count1 = 0;
            int count2 = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (gamezone[i, j].Content.ToString() != "")
                    {
                        if (gamezone[i, j].Content.ToString() == "2048")
                        {
                            Flag2048 = true;
                            return true;
                        }
                        else
                            count1++;
                    }
                }
            }
            if (count1 == 16)
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (i != 0)
                            if (gamezone[i, j].Content.ToString() == gamezone[i - 1, j].Content.ToString())
                                count2++;
                        if (i != 3)
                            if (gamezone[i, j].Content.ToString() == gamezone[i + 1, j].Content.ToString())
                                count2++;
                        if (j != 0)
                            if (gamezone[i, j].Content.ToString() == gamezone[i, j - 1].Content.ToString())
                                count2++;
                        if (j != 3)
                            if (gamezone[i, j].Content.ToString() == gamezone[i, j + 1].Content.ToString())
                                count2++;
                    }
                }
            if (count2 == 0 && count1 == 16)
                temp = true;
            return temp;
        }

        private void GameOver()
        {
            MessageBoxResult result = new MessageBoxResult();
            if(!Flag2048)
                result= MessageBox.Show("Restart Game?", "2048 - Game Over", MessageBoxButton.YesNo);
            else if(Flag2048)
                result= MessageBox.Show("YOU WIN!!!! Restart Game?", "2048 - YOU WIN!!!", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                StartGame();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Up")
            {
                if (PressedUpArrow())
                    SpreadOutNumber();
            }
            if (e.Key.ToString() == "Down")
            {
                if (PressedDownArrow())
                    SpreadOutNumber();
            }
            if (e.Key.ToString() == "Left")
            {
                if (PressedLeftArrow())
                    SpreadOutNumber();
            }
            if (e.Key.ToString() == "Right")
            {
                if (PressedRightArrow())
                    SpreadOutNumber();
            }

            if (Score >= HighScore)
                HighScore = Score;
            ScoreTB.Text = Score.ToString();
            HScoreTB.Text = HighScore.ToString();
            if (Check())
                GameOver();
        }

        private bool PressedUpArrow()
        {
            bool temp = false;

            temp = MoveUp();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (gamezone[i, j].Content.ToString() == gamezone[i + 1, j].Content.ToString() && gamezone[i, j].Content.ToString() != ""&& gamezone[i, j].Content.ToString() != "2048")
                    {
                        Score += Convert.ToInt32(gamezone[i, j].Content.ToString()) + Convert.ToInt32(gamezone[i + 1, j].Content.ToString());

                        gamezone[i, j].Content = Convert.ToInt32(gamezone[i, j].Content.ToString()) + Convert.ToInt32(gamezone[i + 1, j].Content.ToString());
                        if (Convert.ToInt32(gamezone[i, j].Content.ToString()) < 2048)
                            gamezone[i, j].Template = this.FindResource("Template" + gamezone[i, j].Content.ToString()) as ControlTemplate;
                        if (Convert.ToInt32(gamezone[i, j].Content.ToString()) == 2048)
                        {
                            gamezone[i, j].Template = this.FindResource("Template" + gamezone[i, j].Content.ToString()) as ControlTemplate;
                            Flag2048 = true;
                        }

                        gamezone[i + 1, j].Content = "";
                        gamezone[i + 1, j].Template = this.FindResource("Template0") as ControlTemplate;
                        temp = true;
                    }
                }
            }
            if (!temp)
                temp = MoveUp();
            else
                MoveUp();

            return temp;
        }

        private bool PressedDownArrow()
        {
            bool temp = false;

            temp = MoveDown();

            for (int i = 3; i >= 1; i--)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (gamezone[i, j].Content.ToString() == gamezone[i - 1, j].Content.ToString() && gamezone[i, j].Content.ToString() != "")
                    {
                        Score += Convert.ToInt32(gamezone[i, j].Content.ToString()) + Convert.ToInt32(gamezone[i - 1, j].Content.ToString());

                        temp = true;
                        gamezone[i, j].Content = Convert.ToInt32(gamezone[i, j].Content.ToString()) + Convert.ToInt32(gamezone[i - 1, j].Content.ToString());
                        if (Convert.ToInt32(gamezone[i, j].Content.ToString()) < 2048)
                            gamezone[i, j].Template = this.FindResource("Template" + gamezone[i, j].Content.ToString()) as ControlTemplate;
                        if (Convert.ToInt32(gamezone[i, j].Content.ToString()) == 2048)
                        {
                            gamezone[i, j].Template = this.FindResource("Template" + gamezone[i, j].Content.ToString()) as ControlTemplate;
                            Flag2048 = true;
                        }

                        gamezone[i - 1, j].Content = "";
                        gamezone[i - 1, j].Template = this.FindResource("Template0") as ControlTemplate;
                    }
                }
            }

            if (!temp)
                temp = MoveDown();
            else
                MoveDown();

            return temp;
        }

        private bool PressedLeftArrow()
        {
            bool temp = false;

            temp = MoveLeft();

            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (gamezone[i, j].Content.ToString() == gamezone[i, j + 1].Content.ToString() && gamezone[i, j].Content.ToString() != "")
                    {
                        Score += Convert.ToInt32(gamezone[i, j].Content.ToString()) + Convert.ToInt32(gamezone[i, j + 1].Content.ToString());

                        gamezone[i, j].Content = Convert.ToInt32(gamezone[i, j].Content.ToString()) + Convert.ToInt32(gamezone[i, j + 1].Content.ToString());
                        if (Convert.ToInt32(gamezone[i, j].Content.ToString()) < 2048)
                            gamezone[i, j].Template = this.FindResource("Template" + gamezone[i, j].Content.ToString()) as ControlTemplate;
                        if (Convert.ToInt32(gamezone[i, j].Content.ToString()) == 2048)
                        {
                            gamezone[i, j].Template = this.FindResource("Template" + gamezone[i, j].Content.ToString()) as ControlTemplate;
                            Flag2048 = true;
                        }

                        gamezone[i, j + 1].Content = "";
                        gamezone[i, j + 1].Template = this.FindResource("Template0") as ControlTemplate;
                        temp = true;
                    }
                }
            }
            if (!temp)
                temp = MoveLeft();
            else
                MoveLeft();

            return temp;
        }

        private bool PressedRightArrow()
        {
            bool temp = false;

            temp = MoveRight();

            for (int j = 3; j >= 1; j--)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (gamezone[i, j].Content.ToString() == gamezone[i, j - 1].Content.ToString() && gamezone[i, j].Content.ToString() != "")
                    {
                        Score += Convert.ToInt32(gamezone[i, j].Content.ToString()) + Convert.ToInt32(gamezone[i, j - 1].Content.ToString());

                        temp = true;
                        gamezone[i, j].Content = Convert.ToInt32(gamezone[i, j].Content.ToString()) + Convert.ToInt32(gamezone[i, j - 1].Content.ToString());
                        if (Convert.ToInt32(gamezone[i, j].Content.ToString()) < 2048)
                            gamezone[i, j].Template = this.FindResource("Template" + gamezone[i, j].Content.ToString()) as ControlTemplate;
                        if (Convert.ToInt32(gamezone[i, j].Content.ToString()) == 2048)
                        {
                            gamezone[i, j].Template = this.FindResource("Template" + gamezone[i, j].Content.ToString()) as ControlTemplate;
                            Flag2048 = true;
                        }

                        gamezone[i, j - 1].Content = "";
                        gamezone[i, j - 1].Template = this.FindResource("Template0") as ControlTemplate;
                    }
                }
            }

            if (!temp)
                temp = MoveRight();
            else
                MoveRight();

            return temp;
        }

        private bool MoveUp()
        {
            bool temp = false;
            for (int j = 0; j < 4; j++)
            {
                bool t = true;
                while (t)
                {
                    int count = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (gamezone[i, j].Content.ToString() != "")
                            count++;
                    }
                    switch (count)
                    {
                        case 0:
                            t = false;
                            break;
                        case 1:
                            if (gamezone[0, j].Content.ToString() != "" && gamezone[1, j].Content.ToString() == "" && gamezone[2, j].Content.ToString() == "" && gamezone[3, j].Content.ToString() == "")
                                t = false;
                            break;
                        case 2:
                            if (gamezone[0, j].Content.ToString() != "" && gamezone[1, j].Content.ToString() != "" && gamezone[2, j].Content.ToString() == "" && gamezone[3, j].Content.ToString() == "")
                                t = false;
                            break;
                        case 3:
                            if (gamezone[0, j].Content.ToString() != "" && gamezone[1, j].Content.ToString() != "" && gamezone[2, j].Content.ToString() != "" && gamezone[3, j].Content.ToString() == "")
                                t = false;
                            break;
                        case 4:
                            t = false;
                            break;
                    }
                    if (t)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (gamezone[i + 1, j].Content.ToString() != "" && gamezone[i, j].Content.ToString() == "")
                            {
                                gamezone[i, j].Content = gamezone[i + 1, j].Content.ToString();
                                gamezone[i, j].Template = this.FindResource("Template" + gamezone[i + 1, j].Content.ToString()) as ControlTemplate;
                                temp = true;

                                gamezone[i + 1, j].Content = "";
                                gamezone[i + 1, j].Template = this.FindResource("Template0") as ControlTemplate;
                            }
                        }
                    }
                }
            }
            return temp;
        }

        private bool MoveDown()
        {
            bool temp = false;
            for (int j = 0; j < 4; j++)
            {
                bool t = true;
                while (t)
                {
                    int count = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (gamezone[i, j].Content.ToString() != "")
                            count++;
                    }
                    switch (count)
                    {
                        case 0:
                            t = false;
                            break;
                        case 1:
                            if (gamezone[0, j].Content.ToString() == "" && gamezone[1, j].Content.ToString() == "" && gamezone[2, j].Content.ToString() == "" && gamezone[3, j].Content.ToString() != "")
                                t = false;
                            break;
                        case 2:
                            if (gamezone[0, j].Content.ToString() == "" && gamezone[1, j].Content.ToString() == "" && gamezone[2, j].Content.ToString() != "" && gamezone[3, j].Content.ToString() != "")
                                t = false;
                            break;
                        case 3:
                            if (gamezone[0, j].Content.ToString() == "" && gamezone[1, j].Content.ToString() != "" && gamezone[2, j].Content.ToString() != "" && gamezone[3, j].Content.ToString() != "")
                                t = false;
                            break;
                        case 4:
                            t = false;
                            break;
                    }
                    if (t)
                    {
                        for (int i = 3; i >= 1; i--)
                        {
                            if (gamezone[i - 1, j].Content.ToString() != "" && gamezone[i, j].Content.ToString() == "")
                            {
                                gamezone[i, j].Content = gamezone[i - 1, j].Content.ToString();
                                gamezone[i, j].Template = this.FindResource("Template" + gamezone[i - 1, j].Content.ToString()) as ControlTemplate;

                                gamezone[i - 1, j].Content = "";
                                gamezone[i - 1, j].Template = this.FindResource("Template0") as ControlTemplate;
                                temp = true;
                            }
                        }
                    }
                }
            }
            return temp;
        }

        private bool MoveLeft()
        {
            bool temp = false;
            for (int i = 0; i < 4; i++)
            {
                bool t = true;
                while (t)
                {
                    int count = 0;
                    for (int j = 0; j < 4; j++)
                    {
                        if (gamezone[i, j].Content.ToString() != "")
                            count++;
                    }
                    switch (count)
                    {
                        case 0:
                            t = false;
                            break;
                        case 1:
                            if (gamezone[i, 0].Content.ToString() != "" && gamezone[i, 1].Content.ToString() == "" && gamezone[i, 2].Content.ToString() == "" && gamezone[i, 3].Content.ToString() == "")
                                t = false;
                            break;
                        case 2:
                            if (gamezone[i, 0].Content.ToString() != "" && gamezone[i, 1].Content.ToString() != "" && gamezone[i, 2].Content.ToString() == "" && gamezone[i, 3].Content.ToString() == "")
                                t = false;
                            break;
                        case 3:
                            if (gamezone[i, 0].Content.ToString() != "" && gamezone[i, 1].Content.ToString() != "" && gamezone[i, 2].Content.ToString() != "" && gamezone[i, 3].Content.ToString() == "")
                                t = false;
                            break;
                        case 4:
                            t = false;
                            break;
                    }
                    if (t)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (gamezone[i, j + 1].Content.ToString() != "" && gamezone[i, j].Content.ToString() == "")
                            {
                                gamezone[i, j].Content = gamezone[i, j + 1].Content.ToString();
                                gamezone[i, j].Template = this.FindResource("Template" + gamezone[i, j + 1].Content.ToString()) as ControlTemplate;
                                temp = true;

                                gamezone[i, j + 1].Content = "";
                                gamezone[i, j + 1].Template = this.FindResource("Template0") as ControlTemplate;
                            }
                        }
                    }
                }
            }
            return temp;
        }

        private bool MoveRight()
        {
            bool temp = false;
            for (int i = 0; i < 4; i++)
            {
                bool t = true;
                while (t)
                {
                    int count = 0;
                    for (int j = 0; j < 4; j++)
                    {
                        if (gamezone[i, j].Content.ToString() != "")
                            count++;
                    }
                    switch (count)
                    {
                        case 0:
                            t = false;
                            break;
                        case 1:
                            if (gamezone[i, 0].Content.ToString() == "" && gamezone[i, 1].Content.ToString() == "" && gamezone[i, 2].Content.ToString() == "" && gamezone[i, 3].Content.ToString() != "")
                                t = false;
                            break;
                        case 2:
                            if (gamezone[i, 0].Content.ToString() == "" && gamezone[i, 1].Content.ToString() == "" && gamezone[i, 2].Content.ToString() != "" && gamezone[i, 3].Content.ToString() != "")
                                t = false;
                            break;
                        case 3:
                            if (gamezone[i, 0].Content.ToString() == "" && gamezone[i, 1].Content.ToString() != "" && gamezone[i, 2].Content.ToString() != "" && gamezone[i, 3].Content.ToString() != "")
                                t = false;
                            break;
                        case 4:
                            t = false;
                            break;
                    }
                    if (t)
                    {
                        for (int j = 3; j >= 1; j--)
                        {
                            if (gamezone[i, j - 1].Content.ToString() != "" && gamezone[i, j].Content.ToString() == "")
                            {
                                gamezone[i, j].Content = gamezone[i, j - 1].Content.ToString();
                                gamezone[i, j].Template = this.FindResource("Template" + gamezone[i, j - 1].Content.ToString()) as ControlTemplate;
                                temp = true;

                                gamezone[i, j - 1].Content = "";
                                gamezone[i, j - 1].Template = this.FindResource("Template0") as ControlTemplate;
                            }
                        }
                    }
                }
            }
            return temp;
        }
    }
}
