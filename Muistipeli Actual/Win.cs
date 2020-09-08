using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Muistipeli_Actual
{
    
    class Win
    {
        private Difficulty difficulty = new Difficulty();
        private Score score = new Score();
        private Create create = new Create();
        public void Check()
        {
            foreach (Label l in create.allBoxes)
            {
                if (l.BackColor == Color.CornflowerBlue)
                {
                    return;
                }
            }
            if (!difficulty.twoPlayerMode)
            {
                MessageBox.Show("Onneksi olkoon! Sait " + score.playerOne + " pistettä");
                score.UpdateHighScore(score.playerOne);
            }
            else if (difficulty.twoPlayerMode && score.playerOne < score.playerTwo)
            {
                MessageBox.Show("Kakkospelaaja voitti!");
                score.UpdateHighScore(score.playerOne);
                score.UpdateHighScore(score.playerTwo);
            }
            else if (difficulty.twoPlayerMode && score.playerTwo < score.playerOne)
            {
                MessageBox.Show("Ykköspelaaja voitti!");
                score.UpdateHighScore(score.playerOne);
                score.UpdateHighScore(score.playerTwo);
            }
            else if (difficulty.twoPlayerMode && score.playerTwo == score.playerOne)
            {
                MessageBox.Show("Peli päättyi tasapeliin!");
                score.UpdateHighScore(score.playerOne);
                score.UpdateHighScore(score.playerTwo);
            }
        }
    }
}
