using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;
using System.IO;

namespace Muistipeli_Actual
{
    class Score
    {
        private Difficulty difficulty = new Difficulty();
        public string scoreBoard;
        public Label scoreboard = new Label();
        public int playerOne;
        public int playerTwo;
        public int newHighScore = 0;
        public List<string> highscoreList = new List<string>();
        public List<int> highscoreIntList = new List<int>();
        public ToolStripMenuItem highScore = new ToolStripMenuItem();
        public void StringToIntList()
        {
        string highScoreFile = File.ReadAllText("highscore.txt");
        //huipputuloksista luodaan ensiksi string-tyyppinen lista ja poistetaan viimeinen 
        highscoreList = highScoreFile.Split(' ').ToList();
            //Poistetaan listan viimeinen ' '-osa.
            highscoreList.RemoveAt(10);
            //Muutetaan lista int-listaksi että siihen voidaan lisätä numeroita.
            foreach (string i in highscoreList)
            {
                highscoreIntList.Add(int.Parse(i));
            }
        }
        public void ShowScoreboard()
        {
            //Tyhjennetään scoreBoard-string ennen käyttöä, ettei siihen tule useita kertoja samaa.
            scoreBoard = null;
            //Tehdään int-listasta numerotaulukko, jotta saadaan se näkymään tekstiboksissa
            foreach (int i in highscoreIntList)
            {
                scoreBoard = String.Concat(scoreBoard, i.ToString() + "\n");
            }

        }
        public void UpdateScore()
        {
            if (!difficulty.twoPlayerMode)
            {
                scoreboard.Text = "Player 1 score: " + playerOne;
            }
            else
            {
                scoreboard.Text = "Player 1 score: " + playerOne + ", Player 2 score: " + playerTwo;
            }
        }
        public void UpdateHighScore(int checkScore)
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
    }
}
