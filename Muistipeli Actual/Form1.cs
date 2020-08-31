using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace Muistipeli_Actual
{
    public partial class Form1 : Form
    {
        //Pelin tilan switch case enumeraattori luodaan.
        private enum GameState
        {
            Initial,
            OneOpen,
            TwoOpen,
            PlayerTwoInitial,
            PlayerTwoOneOpen,
            PlayerTwoTwoOpen
        }
        //UI:n esineet luodaan
        private SoundPlayer sound;
        private GameState state = GameState.Initial;
        private MenuItem FileClose { get; set; }
        private MenuStrip MainMenu = new MenuStrip();
        private ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
        private ToolStripMenuItem startNewGame = new ToolStripMenuItem();
        private ToolStripMenuItem highScore = new ToolStripMenuItem();
        private ToolStripMenuItem options = new ToolStripMenuItem();
        private ToolStripMenuItem quitGame = new ToolStripMenuItem();
        private ToolStripMenuItem easyDifficulty = new ToolStripMenuItem();
        private ToolStripMenuItem mediumDifficulty = new ToolStripMenuItem();
        private ToolStripMenuItem hardDifficulty = new ToolStripMenuItem();
        private ToolStripMenuItem onePlayer = new ToolStripMenuItem();
        private ToolStripMenuItem twoPlayer = new ToolStripMenuItem();
        private TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
        private PictureBox pictureBox1 = new PictureBox();
        
        private Label scoreboard = new Label();
        private int boxHeight = 100;
        private int boxWidth = 100;
        private Label firstClicked = null;
        private Label secondClicked = null;
        private Label newGameButton = new Label();
        private Label closeThis = new Label();
        private int difficultyModifier = 1;
        private int rows = 4;
        private int columns = 4;
        private bool twoPlayerMode;
        private int playerOneScore;
        private int playerTwoScore;
        private int newHighScore;
        private string scoreBoard;

        //Haetaan highscore.txt:stä edelliset huipputulokset
        private string highScoreFile = File.ReadAllText("highscore.txt");
        private List<string> highscoreList = new List<string>();
        private List<int> highscoreIntList = new List<int>();
        
        List<string> icons = null;

        List<Label> allBoxes = new List<Label>();
        
        public Form1()
        {
            this.Size = new Size(640, 480);

        
            //huipputuloksista luodaan ensiksi string-tyyppinen lista ja poistetaan viimeinen 
            highscoreList = highScoreFile.Split(' ').ToList();
            //Poistetaan listan viimeinen ' '-osa.
            highscoreList.RemoveAt(10);
            //Muutetaan lista int-listaksi että siihen voidaan lisätä numeroita.
            foreach (string i in highscoreList)
            {
                highscoreIntList.Add(int.Parse(i));
            }
            IsMdiContainer = true;
            
            //Määritetään tekstit valikon objekteille
            fileMenu.Text = "File";
            startNewGame.Text = "New Game";
            highScore.Text = "High score";
            options.Text = "Options";
            quitGame.Text = "Quit game";
            easyDifficulty.Text = "Easy";
            mediumDifficulty.Text = "Medium";
            hardDifficulty.Text = "Hard";
            onePlayer.Text = "One player";
            twoPlayer.Text = "Two players";
            pictureBox1.Image = Image.FromFile("MUISTIPELI.png");
            pictureBox1.Size = new Size(640, 480);

            easyDifficulty.Checked = true;
            fileMenu.TextAlign = ContentAlignment.TopLeft;
            
            sound = new SoundPlayer(@"c:\Windows\Media\ding.wav");

            //Tiedostopalkin tapahtumat
            easyDifficulty.Click += new EventHandler(easyDifficulty_Click);
            mediumDifficulty.Click += new EventHandler(mediumDifficulty_Click);
            hardDifficulty.Click += new EventHandler(hardDifficulty_Click);
            onePlayer.Click += new EventHandler(onePlayer_Click);
            twoPlayer.Click += new EventHandler(twoPlayer_Click);
            quitGame.Click += new EventHandler(FileClose_Click);
            highScore.Click += new EventHandler(Scoreboard_Click);


            //Liitetään ohjaimet ohjelmaan
            Controls.Add(MainMenu);
            fileMenu.DropDownItems.Add(startNewGame);
            fileMenu.DropDownItems.Add(highScore);
            fileMenu.DropDownItems.Add(options);
            fileMenu.DropDownItems.Add(quitGame);
            options.DropDownItems.Add(easyDifficulty);
            options.DropDownItems.Add(mediumDifficulty);
            options.DropDownItems.Add(hardDifficulty);
            startNewGame.DropDownItems.Add(onePlayer);
            startNewGame.DropDownItems.Add(twoPlayer);
            MainMenu.Items.Add(fileMenu);
            InitializeComponent();
            scoreboard.Dock = DockStyle.Bottom;
            Controls.Add(pictureBox1);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        //Poistumisnappula
        private void FileClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Scoreboard_Click(object sender, EventArgs e)
        {
            //Tyhjennetään scoreBoard-string ennen käyttöä, ettei siihen tule useita kertoja samaa.
            scoreBoard = null;
            //Tehdään int-listasta numerotaulukko, jotta saadaan se näkymään tekstiboksissa
            foreach (int i in highscoreIntList)
            {
                scoreBoard = String.Concat(scoreBoard, i.ToString() + "\n");
            }

            MessageBox.Show(scoreBoard, "High Scores");
        }
        /* Yksinpeli- ja kaksinpelioptioita käytetään omilla napeillaan, ettei pelaajamäärää/vaikeusastetta
          voi säätää kesken pelin. Vaikeusasteella on väliä vain uutta peliä aloittaessa.*/
        public void onePlayer_Click(object sender, EventArgs e)
        {
            twoPlayerMode = false;
            pictureBox1.Hide();
            createGameBoard();
        }
        public void twoPlayer_Click(object sender, EventArgs e)
        {
            twoPlayerMode = true;
            pictureBox1.Hide();
            createGameBoard();
        }
        public void easyDifficulty_Click(object sender, EventArgs e)
        {
            rows = 4;
            columns = 4;
            difficultyModifier = 1;
            //Lisätään indikaattorit vaikeusasteesta yläpalkkiin.
            easyDifficulty.Checked = true;
            mediumDifficulty.Checked = false;
            hardDifficulty.Checked = false;
        }

        public void mediumDifficulty_Click(object sender, EventArgs e)
        {
            rows = 5;
            columns = 4;
            difficultyModifier = 2;
            easyDifficulty.Checked = false;
            mediumDifficulty.Checked = true;
            hardDifficulty.Checked = false;
        }

        public void hardDifficulty_Click(object sender, EventArgs e)
        {
            rows = 6;
            columns = 5;
            difficultyModifier = 3;
            easyDifficulty.Checked = false;
            mediumDifficulty.Checked = false;
            hardDifficulty.Checked = true;
        }
        private void createGameBoard()
        {
            //Aloitetaan puhdistamalla pelin tila
            ClearGameState();

            //Muutetaan ikkuna halutun kokoiseksi.
            this.Size = new Size(boxWidth * (rows + 2), boxHeight * (columns + 2));

            //Luodaan pelialueelle napit columni kerrallaan.
            for (int row = 1; row <= rows; row++)
            {
                for (int column = 1; column <= columns; column++)
                {
                    Label namiska = new Label();

                    //Poimitaan satunnainen ikoni listasta metodilla GetRandomIcon().
                    namiska.Text = GetRandomIcon();
                    namiska.BackColor = Color.CornflowerBlue;
                    namiska.ForeColor = Color.CornflowerBlue;
                    namiska.BorderStyle = BorderStyle.FixedSingle;
                    namiska.Size = new Size(boxWidth, boxHeight);
                    namiska.Location = new Point(boxWidth * row, boxHeight * column);
                    namiska.Click += new EventHandler(Handle_Click);
                    namiska.Font = new Font("Webdings", 52F, FontStyle.Regular, GraphicsUnit.Point, (byte)(2));
                    namiska.TextAlign = ContentAlignment.MiddleCenter;
                    allBoxes.Add(namiska);
                    Controls.Add(namiska);
                }
            }
            UpdateScore();
            Controls.Add(scoreboard);
        }

        private string GetRandomIcon()
        {

            Random random = new Random();
            int randomNumber = random.Next(icons.Count);
            string selectedIcon = icons[randomNumber];
            icons.RemoveAt(randomNumber);
            return selectedIcon;

        }

        private void Handle_Click(object sender, EventArgs e)
        {
            //Käsitellään ruudun klikkaus
            Label clickedLabel = sender as Label;
            //Jos klikataan väärää kohtaa, palataan alkuun.
            if (clickedLabel.BackColor == Color.Green || clickedLabel.BackColor == Color.Red || clickedLabel.ForeColor == Color.Black)
            {
                return;
            }

            switch (state)
            {
                //Määritellään ensimmäinen klikkaus, muutetaan ikoni näkyväksi ja siirrytään seuraavaan tilaan
                case GameState.Initial:
                    clickedLabel.ForeColor = Color.Black;
                    firstClicked = clickedLabel;
                    state++;
                    break;
                //Vertaillaan edellistä ikonia uuteen
                case GameState.OneOpen:
                    clickedLabel.ForeColor = Color.Black;
                    if (firstClicked.Text == clickedLabel.Text)
                    {
                        firstClicked.BackColor = Color.Green;
                        clickedLabel.BackColor = Color.Green;
                        sound.Play();
                        playerOneScore = playerOneScore + 10;
                        UpdateScore();
                        if (twoPlayerMode)
                        {
                            state = GameState.PlayerTwoInitial;
                        } else
                        {
                            state = GameState.Initial;
                        }
                        firstClicked = null;
                        CheckForWinner();
                    }

                    else
                    {
                        firstClicked.ForeColor = Color.Black;
                        clickedLabel.ForeColor = Color.Black;
                        secondClicked = clickedLabel;
                        playerOneScore--;
                        UpdateScore();
                        state++;
                    }
                    break;

                case GameState.TwoOpen:
                    firstClicked.ForeColor = Color.CornflowerBlue;
                    secondClicked.ForeColor = Color.CornflowerBlue;
                    if (twoPlayerMode)
                    {
                        state++;
                    }
                    else
                    {
                        state = GameState.Initial;
                    }
                    firstClicked = null;
                    break;
                case GameState.PlayerTwoInitial:
                    clickedLabel.ForeColor = Color.Black;
                    firstClicked = clickedLabel;
                    state++;
                    break;
                case GameState.PlayerTwoOneOpen:
                    clickedLabel.ForeColor = Color.Black;
                    if (firstClicked.Text == clickedLabel.Text)
                    {
                        firstClicked.BackColor = Color.Red;
                        clickedLabel.BackColor = Color.Red;
                        sound.Play();
                        state = GameState.Initial;
                        firstClicked = null;
                        playerTwoScore = playerTwoScore + 10;
                        UpdateScore();
                    }

                    else
                    {
                        firstClicked.ForeColor = Color.Black;
                        clickedLabel.ForeColor = Color.Black;
                        secondClicked = clickedLabel;
                        playerTwoScore--;
                        UpdateScore();
                        state++;
                    }
                    CheckForWinner();
                    break;
                case GameState.PlayerTwoTwoOpen:
                    firstClicked.ForeColor = Color.CornflowerBlue;
                    secondClicked.ForeColor = Color.CornflowerBlue;
                    state = GameState.Initial;
                    firstClicked = null;
                    break;
            }
            
        }
        private void CheckForWinner()
        {
            foreach (Label l in allBoxes)
            {
                if (l.BackColor == Color.CornflowerBlue)
                {
                    return;
                }
            }
            if (!twoPlayerMode)
            {
                MessageBox.Show("Onneksi olkoon! Sait " + playerOneScore + " pistettä");
                UpdateHighScore(playerOneScore);
            } else if (twoPlayerMode && playerOneScore < playerTwoScore)
            {
                MessageBox.Show("Kakkospelaaja voitti!");
                UpdateHighScore(playerOneScore);
                UpdateHighScore(playerTwoScore);
            } else if (twoPlayerMode && playerTwoScore < playerOneScore)
            {
                MessageBox.Show("Ykköspelaaja voitti!");
                UpdateHighScore(playerOneScore);
                UpdateHighScore(playerTwoScore);
            } else if (twoPlayerMode && playerTwoScore == playerOneScore)
            {
                MessageBox.Show("Peli päättyi tasapeliin!");
                UpdateHighScore(playerOneScore);
                UpdateHighScore(playerTwoScore);
            }
        }
        private void UpdateScore()
        {
            if (!twoPlayerMode)
            {
                scoreboard.Text = "Player 1 score: " + playerOneScore;
            }
            else
            {
                scoreboard.Text = "Player 1 score: " + playerOneScore + ", Player 2 score: " + playerTwoScore;
            }
        }
        private void UpdateHighScore(int checkScore)
        {
            //Verrataan uutta tulosta jokaiseen listan huipputulokseen.
            foreach (int score in highscoreIntList)
            {
                if (checkScore > score)
                {
                    //Jos listalta löytyy yksikin pienempi tulos, päivitetään uutta huipputulosta
                    newHighScore = checkScore;
                }
            }
            /*Lisätään uusi tulos listaan, järjestetään lista uudestaan, käännetään lista laskevaan muotoon
             ja poistetaan viimeinen tulos. Lopulta muutetaan newHighscore takaisin nollaksi, ettei uudella
             kerralla sama tulos livahda listalle*/
            highscoreIntList.Add(newHighScore);
            highscoreIntList.Sort();
            highscoreIntList.Reverse();
            highscoreIntList.RemoveAt(10);
            newHighScore = 0;
            File.WriteAllText("highscore.txt", String.Empty);
            using (StreamWriter highscorefile = File.AppendText("highscore.txt"))
            {

                foreach (int i in highscoreIntList)
                {
                    highscorefile.Write(i + " \n");
                }
            }

        }

        private void ClearGameState()
        {
            state = GameState.Initial;
            if (difficultyModifier == 2)
                {
                icons = new List<string>()
                {
                "!", "!", "N", "N", ",", ",", "k", "k", "b", "b", "v", "v", "w", "w", "z", "z", "T", "T", "t", "t"
                };
                } else if (difficultyModifier == 3)
                {
                icons = new List<string>()
                {
                "!", "!", "N", "N", ",", ",", "k", "k", "b", "b", "v", "v", "w", "w", "z", "z", "T", "T", "t", "t", "x", "x", "X", "X", "s", "s", "S", "S", "g", "g"
                };
                } else
               {
                icons = new List<string>()
                {
                "!", "!", "N", "N", ",", ",", "k", "k", "b", "b", "v", "v", "w", "w", "z", "z"
                };
                } 
                foreach (Label l in allBoxes)
            {
                Controls.Remove(l);
            }

            allBoxes.Clear();
            playerOneScore = 0;
            playerTwoScore = 0;
        }
    }
}
