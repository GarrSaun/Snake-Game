using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class HighScore
    {
        public string name;
        public int score;

        public HighScore(string _name, int _score)
        {
            name = _name;
            score = _score;
        }
    }
}
