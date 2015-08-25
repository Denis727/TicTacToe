using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class PlayerManager
    {
        public Player FirstPlayer { get; set; }
        public Player SecondPlayer { get; set; }

        public delegate void WinnerUpdate(Player player);
        public event WinnerUpdate WinnerUpdated;

        private bool isFirstLaunch;

        public PlayerManager()
        {
            isFirstLaunch = true;
            FirstPlayer = new Player("", Point.X);
            SecondPlayer = new Player("", Point.O);
        }

        // Начало игры
        public void Start(string firstName, string secondName)
        {
            if (FirstPlayer.Name == firstName && SecondPlayer.Name == secondName && !isFirstLaunch)
            {
                // Продолжение игры
                SwitchPoint();
            }
            else
            {
                if (isFirstLaunch) isFirstLaunch = !isFirstLaunch;
                // Обнуление статистики
                FirstPlayer = new Player(firstName, Point.X);
                SecondPlayer = new Player(secondName, Point.O);
            }
        }

        // Определение победителя
        public Player Win(Point? point)
        {
            if (point != null)
            {
                if (FirstPlayer.Point == point)
                {
                    FirstPlayer.Score++;
                    return FirstPlayer;
                }
                else
                {
                    SecondPlayer.Score++;
                    return SecondPlayer;
                }
            }
            else return null;
        }

        // Смена знаков
        private void SwitchPoint()
        {
            Point? p = FirstPlayer.Point;
            FirstPlayer.Point = SecondPlayer.Point;
            SecondPlayer.Point = p;
        }
    }
}