using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Muistipeli_Actual
{

    public class Difficulty
    {
        public int rows = 4;
        public int columns = 4;
        public int difficultyModifier = 1;
        public bool twoPlayerMode = false;
        public ToolStripMenuItem onePlayer = new ToolStripMenuItem();
        public ToolStripMenuItem twoPlayer = new ToolStripMenuItem();
        public ToolStripMenuItem easyDifficulty;
        public ToolStripMenuItem mediumDifficulty;
        public ToolStripMenuItem hardDifficulty;
        public enum Level
        {
            Easy,
            Medium,
            Hard
        }
        public void SetDifficulty(Level level)
        {
            if (level == Difficulty.Level.Easy)
            {
                this.rows = 4;
                this.columns = 4;
                this.difficultyModifier = 1;
                this.easyDifficulty.Checked = true;
                this.mediumDifficulty.Checked = false;
                this.hardDifficulty.Checked = false;
            } else if (level == Difficulty.Level.Medium)
            {
                this.rows = 5;
                this.columns = 4;
                this.difficultyModifier = 2;
                this.easyDifficulty.Checked = false;
                this.mediumDifficulty.Checked = true;
                this.hardDifficulty.Checked = false;
            } else
            {
                this.rows = 6;
                this.columns = 5;
                this.difficultyModifier = 3;
                this.easyDifficulty.Checked = false;
                this.mediumDifficulty.Checked = false;
                this.hardDifficulty.Checked = true;
            }

        }
    }
}
