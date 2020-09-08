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

        private Create create = new Create();
        private Gamestate gamestate = new Gamestate();
        private Win winner = new Win();
        private MenuItem FileClose { get; set; }
        private MenuStrip MainMenu = new MenuStrip();
        private ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
        private ToolStripMenuItem startNewGame = new ToolStripMenuItem();
        private ToolStripMenuItem options = new ToolStripMenuItem();
        private ToolStripMenuItem quitGame = new ToolStripMenuItem();
        private Difficulty difficulty = new Difficulty();
        private Score score = new Score();
        
        private TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
        private PictureBox pictureBox1 = new PictureBox();

        private int boxHeight = 100;
        private int boxWidth = 100;
        private Label newGameButton = new Label();
        private Label closeThis = new Label();
        
        
        
        public Form1()
        {
            this.Size = new Size(640, 480);


            score.StringToIntList();
            IsMdiContainer = true;
            difficulty.easyDifficulty = new ToolStripMenuItem();
            difficulty.mediumDifficulty = new ToolStripMenuItem();
            difficulty.hardDifficulty = new ToolStripMenuItem();
        //Määritetään tekstit valikon objekteille
        fileMenu.Text = "File";
            startNewGame.Text = "New Game";
            score.highScore.Text = "High score";
            options.Text = "Options";
            quitGame.Text = "Quit game";
            difficulty.easyDifficulty.Text = "Easy";
            difficulty.mediumDifficulty.Text = "Medium";
            difficulty.hardDifficulty.Text = "Hard";
            difficulty.onePlayer.Text = "One player";
            difficulty.twoPlayer.Text = "Two players";
            pictureBox1.Image = Image.FromFile("MUISTIPELI.png");
            pictureBox1.Size = new Size(640, 480);

            difficulty.easyDifficulty.Checked = true;
            fileMenu.TextAlign = ContentAlignment.TopLeft;
            
            gamestate.sound = new SoundPlayer(@"c:\Windows\Media\ding.wav");

            //Tiedostopalkin tapahtumat
            difficulty.easyDifficulty.Click += new EventHandler(easyDifficulty_Click);
            difficulty.mediumDifficulty.Click += new EventHandler(mediumDifficulty_Click);
            difficulty.hardDifficulty.Click += new EventHandler(hardDifficulty_Click);
            difficulty.onePlayer.Click += new EventHandler(onePlayer_Click);
            difficulty.twoPlayer.Click += new EventHandler(twoPlayer_Click);
            quitGame.Click += new EventHandler(FileClose_Click);
            score.highScore.Click += new EventHandler(Scoreboard_Click);


            //Liitetään ohjaimet ohjelmaan
            Controls.Add(MainMenu);
            fileMenu.DropDownItems.Add(startNewGame);
            fileMenu.DropDownItems.Add(score.highScore);
            fileMenu.DropDownItems.Add(options);
            fileMenu.DropDownItems.Add(quitGame);
            options.DropDownItems.Add(difficulty.easyDifficulty);
            options.DropDownItems.Add(difficulty.mediumDifficulty);
            options.DropDownItems.Add(difficulty.hardDifficulty);
            startNewGame.DropDownItems.Add(difficulty.onePlayer);
            startNewGame.DropDownItems.Add(difficulty.twoPlayer);
            MainMenu.Items.Add(fileMenu);
            InitializeComponent();
            score.scoreboard.Dock = DockStyle.Bottom;
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
            score.ShowScoreboard();
        }
        /* Yksinpeli- ja kaksinpelioptioita käytetään omilla napeillaan, ettei pelaajamäärää/vaikeusastetta
          voi säätää kesken pelin. Vaikeusasteella on väliä vain uutta peliä aloittaessa.*/
        public void onePlayer_Click(object sender, EventArgs e)
        {
            difficulty.twoPlayerMode = false;
            pictureBox1.Hide();
            createGameBoard();
        }
        public void twoPlayer_Click(object sender, EventArgs e)
        {
            difficulty.twoPlayerMode = true;
            pictureBox1.Hide();
            createGameBoard();
        }
        public void easyDifficulty_Click(object sender, EventArgs e)
        {
            difficulty.SetDifficulty(Difficulty.Level.Easy);
        }

        public void mediumDifficulty_Click(object sender, EventArgs e)
        {
            difficulty.SetDifficulty(Difficulty.Level.Medium);
        }

        public void hardDifficulty_Click(object sender, EventArgs e)
        {
            difficulty.SetDifficulty(Difficulty.Level.Hard);
        }
        private void createGameBoard()
        {
            //Aloitetaan puhdistamalla pelin tila
            ClearGameState();

            //Muutetaan ikkuna halutun kokoiseksi.
            this.Size = new Size(boxWidth * (difficulty.rows + 2), boxHeight * (difficulty.columns + 2));

            //Luodaan pelialueelle napit columni kerrallaan.
            for (int row = 1; row <= difficulty.rows; row++)
            {
                for (int column = 1; column <= difficulty.columns; column++)
                {
                    Label namiska = create.IconBox(difficulty.rows, difficulty.columns, row, column);
                    
                    Controls.Add(namiska);
                }
            }
            score.UpdateScore();
            Controls.Add(score.scoreboard);
        }


        private void Handle_Click(object sender, EventArgs e)
        {
            
            //Käsitellään ruudun klikkaus
            Label clickedLabel = sender as Label;
            gamestate.GameClicker(clickedLabel);
        }
        
        

        private void ClearGameState()
        {
            gamestate.SetInitial();
            create.Clear();
                foreach (Label l in create.allBoxes)
            {
                Controls.Remove(l);
            }
                
            score.playerOne = 0;
            score.playerTwo = 0;
        }
    }
}
