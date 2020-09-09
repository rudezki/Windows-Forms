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
        //UI:n esineet luodaan

        private Muistipeli muistipeli = new Muistipeli();
        private MenuItem FileClose { get; set; }
        private MenuStrip MainMenu = new MenuStrip();
        private ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
        private ToolStripMenuItem startNewGame = new ToolStripMenuItem();
        private ToolStripMenuItem options = new ToolStripMenuItem();
        private ToolStripMenuItem quitGame = new ToolStripMenuItem();

        private TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
        private PictureBox pictureBox1 = new PictureBox();

        private int boxHeight = 100;
        private int boxWidth = 100;
        private Label newGameButton = new Label();
        private Label closeThis = new Label();
        private List<Label> allBoxes = new List<Label>();
        private Random random = new Random();
        private List<string> icons = new List<string>();

        public Form1()
        {
            this.Size = new Size(640, 480);


            muistipeli.score.StringToIntList();
            IsMdiContainer = true;
            muistipeli.difficulty.easyDifficulty = new ToolStripMenuItem();
            muistipeli.difficulty.mediumDifficulty = new ToolStripMenuItem();
            muistipeli.difficulty.hardDifficulty = new ToolStripMenuItem();
            //Määritetään tekstit valikon objekteille
            fileMenu.Text = "File";
            startNewGame.Text = "New Game";
            muistipeli.score.highScore.Text = "High score";
            options.Text = "Options";
            quitGame.Text = "Quit game";
            muistipeli.difficulty.easyDifficulty.Text = "Easy";
            muistipeli.difficulty.mediumDifficulty.Text = "Medium";
            muistipeli.difficulty.hardDifficulty.Text = "Hard";
            muistipeli.difficulty.onePlayer.Text = "One player";
            muistipeli.difficulty.twoPlayer.Text = "Two players";
            pictureBox1.Image = Image.FromFile("MUISTIPELI.png");
            pictureBox1.Size = new Size(640, 480);

            muistipeli.difficulty.easyDifficulty.Checked = true;
            fileMenu.TextAlign = ContentAlignment.TopLeft;

            muistipeli.sound = new SoundPlayer(@"c:\Windows\Media\ding.wav");

            //Tiedostopalkin tapahtumat
            muistipeli.difficulty.easyDifficulty.Click += new EventHandler(easyDifficulty_Click);
            muistipeli.difficulty.mediumDifficulty.Click += new EventHandler(mediumDifficulty_Click);
            muistipeli.difficulty.hardDifficulty.Click += new EventHandler(hardDifficulty_Click);
            muistipeli.difficulty.onePlayer.Click += new EventHandler(onePlayer_Click);
            muistipeli.difficulty.twoPlayer.Click += new EventHandler(twoPlayer_Click);
            quitGame.Click += new EventHandler(FileClose_Click);
            muistipeli.score.highScore.Click += new EventHandler(Scoreboard_Click);


            //Liitetään ohjaimet ohjelmaan
            Controls.Add(MainMenu);
            fileMenu.DropDownItems.Add(startNewGame);
            fileMenu.DropDownItems.Add(muistipeli.score.highScore);
            fileMenu.DropDownItems.Add(options);
            fileMenu.DropDownItems.Add(quitGame);
            options.DropDownItems.Add(muistipeli.difficulty.easyDifficulty);
            options.DropDownItems.Add(muistipeli.difficulty.mediumDifficulty);
            options.DropDownItems.Add(muistipeli.difficulty.hardDifficulty);
            startNewGame.DropDownItems.Add(muistipeli.difficulty.onePlayer);
            startNewGame.DropDownItems.Add(muistipeli.difficulty.twoPlayer);
            MainMenu.Items.Add(fileMenu);
            InitializeComponent();
            muistipeli.score.scoreboard.Dock = DockStyle.Bottom;
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
            muistipeli.score.ShowScoreboard();
        }
        /* Yksinpeli- ja kaksinpelioptioita käytetään omilla napeillaan, ettei pelaajamäärää/vaikeusastetta
          voi säätää kesken pelin. Vaikeusasteella on väliä vain uutta peliä aloittaessa.*/
        public void onePlayer_Click(object sender, EventArgs e)
        {
            muistipeli.difficulty.twoPlayerMode = false;
            pictureBox1.Hide();
            createGameBoard();
        }
        public void twoPlayer_Click(object sender, EventArgs e)
        {
            muistipeli.difficulty.twoPlayerMode = true;
            pictureBox1.Hide();
            createGameBoard();
        }
        public void easyDifficulty_Click(object sender, EventArgs e)
        {
            muistipeli.difficulty.SetDifficulty(Difficulty.Level.Easy);
        }

        public void mediumDifficulty_Click(object sender, EventArgs e)
        {
            muistipeli.difficulty.SetDifficulty(Difficulty.Level.Medium);
        }

        public void hardDifficulty_Click(object sender, EventArgs e)
        {
            muistipeli.difficulty.SetDifficulty(Difficulty.Level.Hard);
        }
        private void createGameBoard()
        {
            //Aloitetaan puhdistamalla pelin tila
            allBoxes.Clear();
            icons = muistipeli.GenerateIcons();
            muistipeli.StartGame();
            foreach (Label l in allBoxes)
            {
                Controls.Remove(l);
            }

            //Muutetaan ikkuna halutun kokoiseksi.
            this.Size = new Size(boxWidth * (muistipeli.difficulty.rows + 2), boxHeight * (muistipeli.difficulty.columns + 2));

            //Luodaan pelialueelle napit columni kerrallaan.
            for (int row = 1; row <= muistipeli.difficulty.rows; row++)
            {
                for (int column = 1; column <= muistipeli.difficulty.columns; column++)
                {

                    Label namiska = IconBox(muistipeli.difficulty.rows, muistipeli.difficulty.columns, row, column);

                    Controls.Add(namiska);
                }
            }
            muistipeli.score.UpdateScore(muistipeli.difficulty);
            Controls.Add(muistipeli.score.scoreboard);
        }

        private void Handle_Click(object sender, EventArgs e)
        {

            //Käsitellään ruudun klikkaus
            Label clickedLabel = sender as Label;
            muistipeli.GameClicker(clickedLabel, allBoxes);
        }
        public Label IconBox(int rows, int columns, int row, int column)
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

            return namiska;
        }
        private string GetRandomIcon()
        {

            int randomNumber = random.Next(icons.Count);
            string selectedIcon = icons[randomNumber];
            icons.RemoveAt(randomNumber);
            return selectedIcon;

        }
    }
}
