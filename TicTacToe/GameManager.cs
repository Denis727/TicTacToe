namespace TicTacToe
{
    enum Point { X, O } // перечисление возможных знаков для поля
    class GameManager
    {
        public PlayerManager playerManager { get; set; }
        public delegate void WinnerUpdate(Player player);
        public event WinnerUpdate WinnerUpdated; // победитель определен

        public int WinSize;             // последовательность для выигрыша
        private Point?[][] field;       // поле
        private int fieldFill;          // заполненность поля
        private int fieldLastIndex;     // индекс крайнего элемента поля
        private bool isFirstPlayerMove; // ходит первый игрок или второй

        public GameManager()
        {
            playerManager = new PlayerManager();
        }

        // Начало игры
        public void Start(string firstName, string secondName, int fieldSize, int winSize)
        {
            WinSize = winSize;
            isFirstPlayerMove = true;
            playerManager.Start(firstName, secondName);

            fieldFill = fieldSize * fieldSize;
            fieldLastIndex = fieldSize - 1;

            // Очистка поля
            field = new Point?[fieldSize][];
            for (int i = 0; i < fieldSize; i++)
            {
                field[i] = new Point?[fieldSize];
                for (int j = 0; j < fieldSize; j++)
                    field[i][j] = null;
            }
        }

        // Установка значений на игровом поле
        public Point? Move(int i, int j)
        {
            if (i < field.Length && j < field.Length)
            {
                if (field[i][j] == null)
                {
                    if (isFirstPlayerMove) field[i][j] = Point.X;
                    else field[i][j] = Point.O;
                    isFirstPlayerMove = !isFirstPlayerMove;
                    fieldFill--;
                }
                return field[i][j];
            }
            return null;
        }

        // Проверка на выигрыш
        public void CheckWin()
        {
            int i = 0, j = 0, k = 0, curSize = 0;
            int lMinusW = field.Length - WinSize;
            Point? point = null;

            // Строки
            for (i = 0; i < field.Length; i++)
            {
                curSize = 0;
                for (j = 0; j < field.Length; j++)
                    if (CheckField(i, j, ref point, ref curSize)) return;
            }

            // Столбцы
            for (j = 0; j < field.Length; j++)
            {
                curSize = 0;
                for (i = 0; i < field.Length; i++)
                   if( CheckField(i, j, ref point, ref curSize)) return;
            }

            // Параллели выше побочной диагонали
            for (j = WinSize - 1; j < field.Length; j++)
            {
                curSize = 0;
                for (i = 0, k = j; i < field.Length && k >= 0; i++, k--)
                    if(CheckField(i, k, ref point, ref curSize)) return;
            }

            // Параллели ниже побочной диагонали
            for (i = 1; i <= lMinusW; i++)
            {
                curSize = 0;
                for (j = fieldLastIndex, k = i; j >= 0 && k <= fieldLastIndex; j--, k++)
                    if(CheckField(k, j, ref point, ref curSize)) return;
            }

            //Параллели выше главной диагонали
            for (j = fieldLastIndex; j >= 0; j--)
            {
                curSize = 0;
                for (i = 0, k = j; i < field.Length && k < field.Length; i++, k++)
                    if(CheckField(i, k, ref point, ref curSize)) return;
            }

            // Параллели ниже главной диагонали
            for (i = 1; i <= lMinusW; i++)
            {
                curSize = 0;
                for (j = 0, k = i; j <= fieldLastIndex && k <= fieldLastIndex; j++, k++)
                    if(CheckField(k, j, ref point, ref curSize)) return;
            }

            if (fieldFill == 0) WinnerUpdated(null);
        }

        // Проверка последовательности знаков
        private bool CheckField(int i, int j, ref Point? point, ref int curSize)
        {
            if (field[i][j] == null)
            {
                if (point != null) { point = null; curSize = 0; }
                return false;
            }
            else
            {
                if (point == null) { point = field[i][j]; curSize++; return false; }
                else
                {
                    if (field[i][j] == Point.X)
                    {
                        if (point == Point.O) { point = null; curSize = 0; }
                        else curSize++;
                    }
                    else if (point == Point.X) { point = null; curSize = 0; }
                    else curSize++;
                    if (curSize == WinSize) { WinnerUpdated(playerManager.Win(point)); return true; }
                    else return false;
                }
            }
        }
    }
}