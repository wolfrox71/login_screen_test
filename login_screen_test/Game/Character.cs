using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_screen_test.Game
{
    internal class Character
    {
        public int health;
        public string name;


        public Character(int health, string name)
        {
            this.health = health;
            this.name = name;
        }
    }
}
