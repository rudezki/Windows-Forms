using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using static Muistipeli_Actual.Gamestate;
using System.Media;

namespace Muistipeli_Actual
{
    class Muistipeli
    {
        public Gamestate gamestate = new Gamestate();
        public Win winner = new Win();
        public Difficulty difficulty = new Difficulty();
        public Score score = new Score();
        public Label secondClicked;
        public Label firstClicked;
        public SoundPlayer sound = new SoundPlayer(@"c:\Windows\Media\ding.wav");

        public void StartGame()
        {
            gamestate.SetInitial();
            score.playerOne = 0;
            score.playerTwo = 0;
        }
        public List<string> GenerateIcons()
        {
            if (difficulty.difficultyModifier == 2)
            {
                return new List<string>()
                {
                "!", "!", "N", "N", ",", ",", "k", "k", "b", "b", "v", "v", "w", "w", "z", "z", "T", "T", "t", "t"
                };
            }
            else if (difficulty.difficultyModifier == 3)
            {
                return new List<string>()
                {
                "!", "!", "N", "N", ",", ",", "k", "k", "b", "b", "v", "v", "w", "w", "z", "z", "T", "T", "t", "t", "x", "x", "X", "X", "s", "s", "S", "S", "g", "g"
                };
            }
            else
            {
                return new List<string>()
                {
                "!", "!", "N", "N", ",", ",", "k", "k", "b", "b", "v", "v", "w", "w", "z", "z"
                };
            }
        }
        public void GameClicker(Label clickedLabel, List<Label> allBoxes)
        {
            Label label = clickedLabel;
            if (clickedLabel.BackColor == Color.Green || clickedLabel.BackColor == Color.Red || clickedLabel.ForeColor == Color.Black)
            {
                return;
            }
            switch (gamestate.state)
            {
                //Määritellään ensimmäinen klikkaus, muutetaan ikoni näkyväksi ja siirrytään seuraavaan tilaan
                case State.Initial:
                    clickedLabel.ForeColor = Color.Black;
                    firstClicked = clickedLabel;
                    gamestate.state++;
                    break;
                //Vertaillaan edellistä ikonia uuteen
                case State.OneOpen:
                    clickedLabel.ForeColor = Color.Black;
                    if (firstClicked.Text == clickedLabel.Text)
                    {
                        firstClicked.BackColor = Color.Green;
                        clickedLabel.BackColor = Color.Green;
                        sound.Play();
                        score.playerOne = score.playerOne + 10;
                        score.UpdateScore(difficulty);
                        if (difficulty.twoPlayerMode)
                        {
                            gamestate.state = State.PlayerTwoInitial;
                        }
                        else
                        {
                            gamestate.state = State.Initial;
                        }
                        firstClicked = null;
                        winner.Check(allBoxes, difficulty, score);
                    }

                    else
                    {
                        firstClicked.ForeColor = Color.Black;
                        clickedLabel.ForeColor = Color.Black;
                        secondClicked = clickedLabel;
                        score.playerOne--;
                        score.UpdateScore(difficulty);
                        gamestate.state++;
                    }
                    break;

                case State.TwoOpen:
                    firstClicked.ForeColor = Color.CornflowerBlue;
                    secondClicked.ForeColor = Color.CornflowerBlue;
                    if (difficulty.twoPlayerMode)
                    {
                        gamestate.state++;
                    }
                    else
                    {
                        gamestate.state = State.Initial;
                    }
                    firstClicked = null;
                    break;
                case State.PlayerTwoInitial:
                    clickedLabel.ForeColor = Color.Black;
                    firstClicked = clickedLabel;
                    gamestate.state++;
                    break;
                case State.PlayerTwoOneOpen:
                    clickedLabel.ForeColor = Color.Black;
                    if (firstClicked.Text == clickedLabel.Text)
                    {
                        firstClicked.BackColor = Color.Red;
                        clickedLabel.BackColor = Color.Red;
                        sound.Play();
                        gamestate.state = State.Initial;
                        firstClicked = null;
                        score.playerTwo = score.playerTwo + 10;
                        score.UpdateScore(difficulty);
                    }

                    else
                    {
                        firstClicked.ForeColor = Color.Black;
                        clickedLabel.ForeColor = Color.Black;
                        secondClicked = clickedLabel;
                        score.playerTwo--;
                        score.UpdateScore(difficulty);
                        gamestate.state++;
                    }
                    winner.Check(allBoxes, difficulty, score);
                    break;
                case State.PlayerTwoTwoOpen:
                    firstClicked.ForeColor = Color.CornflowerBlue;
                    secondClicked.ForeColor = Color.CornflowerBlue;
                    gamestate.state = State.Initial;
                    firstClicked = null;
                    break;
            }
        }
    }
    
}
