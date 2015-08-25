using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe
{
    public partial class MainWindow : Window
    {
        private GameManager gameManager;
        private Grid StartMenu;
        private Grid GameField;
        private int SizeField;
        private Button btnLastClicked;
        private Button btnStart;
        private TextBox tbFirstName;
        private TextBox tbSecondName;
        private ComboBox cbFieldSize;
        private ComboBox cbWinSize;
        private Button[][] buttons;

        public delegate void FieldUpdate(Grid field);
        public event FieldUpdate FieldUpdated;

        public MainWindow()
        {
            InitializeComponent();

            gameManager = new GameManager();

            // Обновление игрового поля
            FieldUpdated = (field) => { this.Content = field; };

            // Вывод победителя
            gameManager.WinnerUpdated += (player) =>
            {
                if (player == null) MessageBox.Show("Ничья!");
                else MessageBox.Show("Игрок " + player.Name + " выиграл!");
                FieldUpdated(StartMenu);
                ShowScore();
            };

            CreateStartMenu();
        }

        // Вывод статистики
        private void ShowScore()
        {
            this.Title = gameManager.playerManager.FirstPlayer.Name + ": "
                + gameManager.playerManager.FirstPlayer.Point + ", "
                + gameManager.playerManager.FirstPlayer.Score + "   "
                + gameManager.playerManager.SecondPlayer.Name + ": "
                + gameManager.playerManager.SecondPlayer.Point + ", "
                + gameManager.playerManager.SecondPlayer.Score;
        }

        // Нажатие кнопки Старт
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (tbFirstName.Text != "" && tbSecondName.Text != "")
            {
                SizeField = (int)cbFieldSize.SelectedItem;
                gameManager.Start(tbFirstName.Text, tbSecondName.Text, SizeField, (int)cbWinSize.SelectedItem);
                CreateGameField(SizeField);
                ShowScore();
            }
            else MessageBox.Show("Введите имена игроков");
        }

        // Нажатие любой кнопки на игровом поле
        private void Field_Click(object sender, RoutedEventArgs e)
        {
            btnLastClicked = (Button)sender;
            if (btnLastClicked.Content == null)
            {
                for (int i = 0; i < SizeField; i++)
                    for (int j = 0; j < SizeField; j++)
                    {
                        if (btnLastClicked == buttons[i][j])
                        {
                            btnLastClicked.Content = gameManager.Move(i, j).ToString();
                            gameManager.CheckWin();
                            return;
                        }
                    }
            }
        }

        // Масштабирование значений последовательности
        private void CbFieldSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int size = (int)cbFieldSize.SelectedItem;
            if (size > 6) size = 6;
            cbWinSize.Items.Clear();
            for (int i = 3; i < size; i++) cbWinSize.Items.Add(i);
            cbWinSize.SelectedIndex = 0;
        }

        // Нажатие на элемент контекстного меню для выхода в главное меню
        private void ContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FieldUpdated(StartMenu);
        }

        // Создание главного меню
        private void CreateStartMenu()
        {
            StartMenu = new Grid
            {
                Background = Brushes.AntiqueWhite
            };
            StartMenu.RowDefinitions.Add(new RowDefinition());
            StartMenu.RowDefinitions.Add(new RowDefinition());
            StartMenu.RowDefinitions.Add(new RowDefinition());
            StartMenu.RowDefinitions.Add(new RowDefinition());
            StartMenu.RowDefinitions.Add(new RowDefinition());
            StartMenu.ColumnDefinitions.Add(new ColumnDefinition());
            StartMenu.ColumnDefinitions.Add(new ColumnDefinition());

            TextBlock tblockOne = new TextBlock
            {
                Text = "Имя первого игрока:",
                FontSize = 15,
                Margin = new Thickness(10, 10, 10, 10),
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(tblockOne, 0);
            Grid.SetColumn(tblockOne, 0);
            StartMenu.Children.Add(tblockOne);

            TextBlock tblockTwo = new TextBlock
            {
                Text = "Имя второго игрока:",
                FontSize = 15,
                Margin = new Thickness(10, 10, 10, 10),
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(tblockTwo, 1);
            Grid.SetColumn(tblockTwo, 0);
            StartMenu.Children.Add(tblockTwo);

            TextBlock tblockThree = new TextBlock
            {
                Text = "Размер поля:",
                FontSize = 15,
                Margin = new Thickness(10, 10, 10, 10),
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(tblockThree, 2);
            Grid.SetColumn(tblockThree, 0);
            StartMenu.Children.Add(tblockThree);

            TextBlock tblockFour = new TextBlock
            {
                Text = "Последовательность:",
                FontSize = 15,
                Margin = new Thickness(10, 10, 10, 10),
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(tblockFour, 3);
            Grid.SetColumn(tblockFour, 0);
            StartMenu.Children.Add(tblockFour);

            tbFirstName = new TextBox
            {
                Text = "Первый",
                FontSize = 30,
                Background = Brushes.WhiteSmoke,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(tbFirstName, 0);
            Grid.SetColumn(tbFirstName, 1);
            StartMenu.Children.Add(tbFirstName);

            tbSecondName = new TextBox
            {
                Text = "Второй",
                FontSize = 30,
                Background = Brushes.WhiteSmoke,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(tbSecondName, 1);
            Grid.SetColumn(tbSecondName, 1);
            StartMenu.Children.Add(tbSecondName);

            cbFieldSize = new ComboBox
            {
                FontSize = 30,
                SelectedIndex = 0,
                Background = Brushes.WhiteSmoke,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(cbFieldSize, 2);
            Grid.SetColumn(cbFieldSize, 1);
            StartMenu.Children.Add(cbFieldSize);
            for (int i = 3; i < 16; i++) cbFieldSize.Items.Add(i);
            cbFieldSize.SelectionChanged += CbFieldSize_SelectionChanged;

            cbWinSize = new ComboBox
            {
                FontSize = 30,
                SelectedIndex = 0,
                Background = Brushes.WhiteSmoke,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(cbWinSize, 3);
            Grid.SetColumn(cbWinSize, 1);
            StartMenu.Children.Add(cbWinSize);
            cbWinSize.Items.Add(3);

            btnStart = new Button
            {
                Background = Brushes.LightGreen
            };
            btnStart.Content = "Старт!";
            btnStart.FontSize = 40;
            Grid.SetRow(btnStart, 4);
            Grid.SetColumn(btnStart, 0);
            Grid.SetColumnSpan(btnStart, 2);
            StartMenu.Children.Add(btnStart);
            btnStart.Click += Start_Click;

            FieldUpdated(StartMenu);
        }

        // Создание игрового поля
        private void CreateGameField(int size)
        {
            SizeField = size;
            GameField = new Grid();
            for (int i = 0; i < size; i++)
            {
                GameField.RowDefinitions.Add(new RowDefinition());
                GameField.ColumnDefinitions.Add(new ColumnDefinition());
            }
            // Контекстное меню для выхода в главное меню
            ContextMenu contextMenu = new ContextMenu();
            MenuItem item = new MenuItem { Header = "В главное меню" };
            contextMenu.Items.Add(item);
            GameField.ContextMenu = contextMenu;

            item.Click += ContextMenuItem_Click;

            // Создание кнопок для игрового поля
            int fontSize = 270 / size;
            buttons = new Button[size][];
            for (int i = 0; i < size; i++)
            {
                buttons[i] = new Button[size];
                for (int j = 0; j < size; j++)
                {
                    buttons[i][j] = new Button
                    {
                        FontSize = fontSize,
                        Background = Brushes.LightBlue
                    };
                    Grid.SetRow(buttons[i][j], i);
                    Grid.SetColumn(buttons[i][j], j);
                    GameField.Children.Add(buttons[i][j]);

                    buttons[i][j].Click += Field_Click;
                }
            }
            FieldUpdated(GameField);
        }
    }
}