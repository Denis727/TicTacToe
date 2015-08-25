using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Player
    {
        public string Name { get; set; }   // имя
        public Point? Point { get; set; }  // знак
        public int Score { get; set; }     // очки

        public Player(string name, Point? point)
        {
            Name = name;
            Point = point;
            Score = 0;
        }
    }
}