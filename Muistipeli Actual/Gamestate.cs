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
    }
}
