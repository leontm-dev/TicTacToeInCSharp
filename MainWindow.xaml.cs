using Microsoft.Win32.SafeHandles;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TicTacToeBot;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private (string Symbol, string Color) onPlaying = ("X", "#FFFFFF");
        private Difficulty Difficulty = Difficulty.None;
        private Button[] Buttons = Array.Empty<Button>();
        private TextBlock[] Texts = Array.Empty<TextBlock>();
        public List<Button> placedSymbols = new List<Button>();
        public MainWindow()
        {
            InitializeComponent();
            defaultComboBoxElement.IsSelected = true;
            ResetRaw();
            Texts = [text0, text1, text2, text3, text4, text5, text6, text7, text8];
            Buttons = [button0, button1, button2, button3, button4, button5, button6, button7, button8];
            placedSymbols = [];
        }
        public void ButtonClicked(object sender, RoutedEventArgs e)
        {
            ButtonClickedIntern(sender);
        }
        private void ButtonClickedIntern(object sender)
        {
            Button button = (Button)sender;
            TextBlock text = (TextBlock)button.FindName(button.Name.Replace("button", "text"));
            text.Text = onPlaying.Symbol;
            placedSymbols.Add(button);
            text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(onPlaying.Color));
            button.IsHitTestVisible = false;
            if (!CheckIfWon(text))
            {
                if (!CheckIfTie())
                {
                    ChangePlayer();
                }
            }
        }
        public void Reset(object sender, RoutedEventArgs e)
        {
            ResetRaw();
            defaultComboBoxElement.IsSelected = true;
        }
        private void ResetButtons()
        {
            onPlaying = ("X", "#FFFFFF");
            foreach (Button b in Buttons)
            {
                b.IsHitTestVisible = true;
            }
            foreach (TextBlock t in Texts)
            {
                t.Text = "";
                t.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        private void LockButtons()
        {
            for (int i = 0; i < 9; i++)
            {
                ((Button)button0.FindName($"button{i}")).IsHitTestVisible = false;
            }
        }
        private void ResetRaw()
        {
            ResetButtons();
            UpdateAnnouncer("Your turn Player (X), go ahead", onPlaying.Color);
        }
        private bool CheckAllSolutionsAbout(int id)
        {
            bool solution012 = TES(text0) && TES(text1) && TES(text2);
            bool solution036 = TES(text0) && TES(text3) && TES(text6);
            bool solution048 = TES(text0) && TES(text4) && TES(text8);
            bool solution147 = TES(text1) && TES(text4) && TES(text7);
            bool solution246 = TES(text2) && TES(text4) && TES(text6);
            bool solution258 = TES(text2) && TES(text5) && TES(text8);
            bool solution345 = TES(text3) && TES(text4) && TES(text5);
            bool solution678 = TES(text6) && TES(text7) && TES(text8);

            bool result = false;

            switch (id)
            {
                case 0:
                    result = (solution012 || solution036 || solution048);
                    break;
                case 1:
                    result = (solution012 || solution147);
                    break;
                case 2:
                    result = (solution012 || solution246 || solution258);
                    break;
                case 3:
                    result = (solution036 || solution345);
                    break;
                case 4:
                    result = (solution048 || solution147 || solution246 || solution345);
                    break;
                case 5:
                    result = (solution258 || solution345);
                    break;
                case 6:
                    result = (solution036 || solution246 || solution678);
                    break;
                case 7:
                    result = (solution147 || solution678);
                    break;
                case 8:
                    result = (solution048 || solution258 || solution678);
                    break;
            }

            return result;
        }
        private List<(decimal percentage, List<TextBlock> fields, List<bool> fieldsChecked)> GetByPercentage(decimal percentage)
        {

            (decimal percentage, List<TextBlock> fields, List<bool> fieldsChecked) solution012 = (0.0m, [text0, text1, text2], new List<bool>());
            (decimal percentage, List<TextBlock> fields, List<bool> fieldsChecked) solution036 = (0.0m, [text0, text3, text6], new List<bool>());
            (decimal percentage, List<TextBlock> fields, List<bool> fieldsChecked) solution048 = (0.0m, [text0, text4, text8], new List<bool>());
            (decimal percentage, List<TextBlock> fields, List<bool> fieldsChecked) solution147 = (0.0m, [text1, text4, text7], new List<bool>());
            (decimal percentage, List<TextBlock> fields, List<bool> fieldsChecked) solution246 = (0.0m, [text2, text4, text6], new List<bool>());
            (decimal percentage, List<TextBlock> fields, List<bool> fieldsChecked) solution258 = (0.0m, [text2, text5, text8], new List<bool>());
            (decimal percentage, List<TextBlock> fields, List<bool> fieldsChecked) solution345 = (0.0m, [text3, text4, text5], new List<bool>());
            (decimal percentage, List<TextBlock> fields, List<bool> fieldsChecked) solution678 = (0.0m, [text6, text7, text8], new List<bool>());
            List<(decimal percentage, List<TextBlock> fields, List<bool> fieldsChecked)> solutions = [solution012, solution036, solution048, solution147, solution246, solution258, solution345, solution678];

            for (int i = 0; i < solutions.Count; i++)
            {
                var solution = solutions[i];
                foreach (TextBlock field in solution.fields)
                {
                    if (TES(field, "X"))
                    {
                        solution.percentage += 0.3m;
                    }
                    solution.fieldsChecked.Add(TES(field, "X"));
                }
                solutions[i] = solution;
            }
            List<(decimal percentage, List<TextBlock> fields, List<bool> fieldsChecked)> results = new List<(decimal percentage, List<TextBlock> fields, List<bool> fieldsChecked)>();
            switch (percentage)
            {
                case 0.0m:
                    for (int i = 0; i < solutions.Count; i++)
                    {
                        var s = solutions[i];
                        if (s.percentage == 0.0m)
                        {
                            results.Add(s);
                        }
                    }
                    break;
                case 0.3m:
                    for (int i = 0; i < solutions.Count; i++)
                    {
                        var s = solutions[i];
                        if (s.percentage == 0.3m)
                        {
                            results.Add(s);
                        }
                    }
                    break;
                case 0.6m:
                    for (int i = 0; i < solutions.Count; i++)
                    {
                        var s = solutions[i];
                        if (s.percentage == 0.6m)
                        {
                            results.Add(s);
                        }
                    }
                    break;
                default:
                    break;
            }
            return results;
        }
        private void DoBestMove()
        { 
            if (placedSymbols.Count > 2)
            {
                AvoidEnemyWinning();
            }
            else
            {
                if (button4.IsHitTestVisible)
                {
                    ButtonClickedIntern(button4);
                } 
                else
                {
                    if (button1.IsHitTestVisible)
                    {
                        ButtonClickedIntern(button1);
                    } 
                    else if (button3.IsHitTestVisible)
                    {
                        ButtonClickedIntern(button3);
                    } 
                    else if (button6.IsHitTestVisible)
                    {
                        ButtonClickedIntern(button6);
                    }
                    else if (button8.IsHitTestVisible)
                    {
                        ButtonClickedIntern(button8);
                    }
                    else
                    {
                        HitTheNextButtonThatIsHittable();
                    }
                }
            }
        }
        private bool CheckIfWon(TextBlock fieldChoosen)
        {
            int id = Convert.ToInt32(fieldChoosen.Name.Replace("text", ""));
            if (CheckAllSolutionsAbout(id))
            {
                UpdateAnnouncer($"Player ({onPlaying.Symbol}) won the game 🏆", Colors.Gold.ToString());
                LockButtons();
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CheckIfTie()
        {
            if (TEN(text0) && TEN(text1) && TEN(text2) && TEN(text3) && TEN(text4) && TEN(text5) && TEN(text6) && TEN(text7) && TEN(text8))
            {
                UpdateAnnouncer("It is a tie, hit reset to start again...", Colors.Red.ToString());
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool TEN(TextBlock text)
        {
            return text.Text == "X" || text.Text == "O";
        }
        private bool TES(TextBlock text, string symbol = "")
        {
            if (symbol == "")
            {
                symbol = onPlaying.Symbol;
            }
            return text.Text == symbol;
        }
        private void ChangePlayer()
        {
            if (onPlaying.Symbol == "X")
            {
                onPlaying.Symbol = "O";
                onPlaying.Color = "#3C6E71";
                if (Difficulty != Difficulty.None)
                {
                    UpdateAnnouncer("Please wait for the bot, Player (X)", onPlaying.Color);
                    RunComputer(Difficulty);
                }
                else
                {
                    UpdateAnnouncer("Now you can play, Player (O)", onPlaying.Color);
                }
            }
            else
            {
                onPlaying.Symbol = "X";
                onPlaying.Color = "#FFFFFF";
                UpdateAnnouncer("Your turn Player (X), go...", onPlaying.Color);
            }
        }
        private void UpdateAnnouncer(string message, string color)
        {
            gameAnnouncements.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            gameAnnouncements.Text = message;
        }
        private void HitTheNextButtonThatIsHittable()
        {
            foreach (Button b in Buttons)
            {
                if (b.IsHitTestVisible)
                {
                    ButtonClickedIntern(b);
                    break;
                }
            }
        }
        private void AvoidEnemyWinning()
        {
            var results = GetByPercentage(0.6m);
            Debug.WriteLine(results);
            if (results.Count > 0)
            {
                var result = results[0];
                int firstFalse = -1;
                for (int i = 0; i < result.fieldsChecked.Count; i++)
                {
                    if (!result.fieldsChecked[i])
                    {
                        firstFalse = i;
                        break;
                    }
                }
                TextBlock missing = result.fields[firstFalse];
                ButtonClickedIntern(button0.FindName(missing.Name.Replace("text", "button")));
            }
        }
        public async void RunComputer(Difficulty mode)
        {
            if (mode != Difficulty.None && onPlaying.Symbol == "O")
            {
                await Task.Delay(3000);
                switch (mode)
                {
                    case Difficulty.Easy:
                        HitTheNextButtonThatIsHittable();
                        break;
                    case Difficulty.Medium:
                        double chanceMedium = new Random().NextDouble();
                        if (chanceMedium < 0.25)
                        {
                            DoBestMove();
                            Debug.WriteLine("Chose 0.25");
                        }
                        else
                        {
                            HitTheNextButtonThatIsHittable();
                            Debug.WriteLine("Chose 0.75");
                        }
                        break;
                    case Difficulty.Hard:
                        double chanceHard = new Random().NextDouble();
                        if (chanceHard < 0.75)
                        {
                            DoBestMove();
                            Debug.WriteLine("Chose 0.75");
                        }
                        else
                        {
                            HitTheNextButtonThatIsHittable();
                            Debug.WriteLine("Chose 0.25");
                        }
                        break;
                    case Difficulty.God:
                        double chanceGod = new Random().NextDouble();
                        if (chanceGod < 1.1)
                        {
                            DoBestMove();
                        }
                        else
                        {
                            HitTheNextButtonThatIsHittable();
                        }
                        break;
                    case Difficulty.Unmatched:
                        LockButtons();
                        UpdateAnnouncer($"WHAT A GAME, but you lost! The bot stays unmatched", Colors.Gold.ToString());
                        break;
                    default:
                        Console.WriteLine("Please check the Difficulty");
                        break;
                }
            }
        }

        private void DifficultyChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box != null && box.SelectedItem != null)
            {
                string item = box.SelectedValue.ToString().Split(": ")[1];
                switch (item)
                {
                    case "Easy":
                        Difficulty = Difficulty.Easy;
                        ResetRaw();
                        break;
                    case "Medium":
                        Difficulty = Difficulty.Medium;
                        ResetRaw();
                        break;
                    case "Hard":
                        Difficulty = Difficulty.Hard;
                        ResetRaw();
                        break;
                    case "God":
                        Difficulty = Difficulty.God;
                        ResetRaw();
                        break;
                    case "Unmatched":
                        ResetButtons();
                        Difficulty = Difficulty.Unmatched;
                        UpdateAnnouncer($"Another win by the UNMATCHED!! You lost!", Colors.Gold.ToString());
                        for (int i = 0; i < 9; i++)
                        {
                            ((Button)button0.FindName($"button{i}")).IsHitTestVisible = false;
                        }
                        break;
                    default:
                        Difficulty = Difficulty.None;
                        ResetRaw();
                        break;
                }
            }
        }
    }
}
