using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Media;

namespace Muistipeli_Actual
{

    class Gamestate
    {

        public Label secondClicked;
        public Label firstClicked;
        public SoundPlayer sound = new SoundPlayer(@"c:\Windows\Media\ding.wav");
        public Score score = new Score();
        private Difficulty difficulty = new Difficulty();
        private Win winner = new Win();
        public enum State
        {
            Initial,
            OneOpen,
            TwoOpen,
            PlayerTwoInitial,
            PlayerTwoOneOpen,
            PlayerTwoTwoOpen
        }
        public State state = State.Initial;
        public void SetInitial()
        {
            state = State.Initial;
        }

        public void GameClicker(Label clickedLabel)
        {
            Label label = clickedLabel;
            if (clickedLabel.BackColor == Color.Green || clickedLabel.BackColor == Color.Red || clickedLabel.ForeColor == Color.Black)
            {
                return;
            }
            switch (state)
            {
                //Määritellään ensimmäinen klikkaus, muutetaan ikoni näkyväksi ja siirrytään seuraavaan tilaan
                case State.Initial:
                    clickedLabel.ForeColor = Color.Black;
                    firstClicked = clickedLabel;
                    state++;
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
                        score.UpdateScore();
                        if (difficulty.twoPlayerMode)
                        {
                            state = State.PlayerTwoInitial;
                        }
                        else
                        {
                            state = State.Initial;
                        }
                        firstClicked = null;
                        winner.Check();
                    }

                    else
                    {
                        firstClicked.ForeColor = Color.Black;
                        clickedLabel.ForeColor = Color.Black;
                        secondClicked = clickedLabel;
                        score.playerOne--;
                        score.UpdateScore();
                        state++;
                    }
                    break;

                case State.TwoOpen:
                    firstClicked.ForeColor = Color.CornflowerBlue;
                    secondClicked.ForeColor = Color.CornflowerBlue;
                    if (difficulty.twoPlayerMode)
                    {
                        state++;
                    }
                    else
                    {
                        state = State.Initial;
                    }
                    firstClicked = null;
                    break;
                case State.PlayerTwoInitial:
                    clickedLabel.ForeColor = Color.Black;
                    firstClicked = clickedLabel;
                    state++;
                    break;
                case State.PlayerTwoOneOpen:
                    clickedLabel.ForeColor = Color.Black;
                    if (firstClicked.Text == clickedLabel.Text)
                    {
                        firstClicked.BackColor = Color.Red;
                        clickedLabel.BackColor = Color.Red;
                        sound.Play();
                        state = State.Initial;
                        firstClicked = null;
                        score.playerTwo = score.playerTwo + 10;
                        score.UpdateScore();
                    }

                    else
                    {
                        firstClicked.ForeColor = Color.Black;
                        clickedLabel.ForeColor = Color.Black;
                        secondClicked = clickedLabel;
                        score.playerTwo--;
                        score.UpdateScore();
                        state++;
                    }
                    winner.Check();
                    break;
                case State.PlayerTwoTwoOpen:
                    firstClicked.ForeColor = Color.CornflowerBlue;
                    secondClicked.ForeColor = Color.CornflowerBlue;
                    state = State.Initial;
                    firstClicked = null;
                    break;
            }
        }

    }
}
