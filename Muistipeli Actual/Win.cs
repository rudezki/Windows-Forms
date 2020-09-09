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
        public void Check(List<Label> allBoxes, Difficulty difficulty, Score score)
        {
            foreach (Label l in allBoxes)
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
