using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Muistipeli_Actual
{

    class Create

    {
        private Random random = new Random();
        private int boxHeight = 100;
        private int boxWidth = 100;
        private Win winner = new Win();
        public List<string> icons = new List<string>();
        private Difficulty difficulty = new Difficulty();
        private Gamestate gamestate = new Gamestate();
        public List<Label> allBoxes = new List<Label>();
        private void Handle_Click(object sender, EventArgs e)
        {

            //Käsitellään ruudun klikkaus
            Label clickedLabel = sender as Label;
            gamestate.GameClicker(clickedLabel);
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
        public void Clear()
        {
            allBoxes.Clear();
            gamestate.SetInitial();
            if (difficulty.difficultyModifier == 2)
            {
                icons = new List<string>()
                {
                "!", "!", "N", "N", ",", ",", "k", "k", "b", "b", "v", "v", "w", "w", "z", "z", "T", "T", "t", "t"
                };
            }
            else if (difficulty.difficultyModifier == 3)
            {
                icons = new List<string>()
                {
                "!", "!", "N", "N", ",", ",", "k", "k", "b", "b", "v", "v", "w", "w", "z", "z", "T", "T", "t", "t", "x", "x", "X", "X", "s", "s", "S", "S", "g", "g"
                };
            }
            else
            {
                icons = new List<string>()
                {
                "!", "!", "N", "N", ",", ",", "k", "k", "b", "b", "v", "v", "w", "w", "z", "z"
                };
            }


            
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
