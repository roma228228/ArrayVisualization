using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArrayVisualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[] _array = Array.Empty<int>();
        private bool _isInitialized = false;


        public MainWindow()
        {
            InitializeComponent();
            this.Background = new SolidColorBrush(Color.FromRgb(200, 255, 200)); // Нежно-зеленый цвет
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _isInitialized = true;
        }

        private void GenerateArrayButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ArraySizeTextBox.Text, out int size) && size > 0)
            {
                DrawGraph(GenerateValueForGraph.Generate(size), "Первоначальный массив");
            }
            else
            {
                MessageBox.Show("Введите положительное целое число для размерности массива.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

            if (!_isInitialized) return;

            if (GenerateValueForGraph._array.Length == 0)
            {
                MessageBox.Show("Сначала сгенерируйте массив.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (OriginalRadioButton.IsChecked == true)
            {
                DrawGraph(GenerateValueForGraph._array, "Первоначальный массив");
            }
            else if (EvenRadioButton.IsChecked == true)
            {
                var evenArray = GenerateValueForGraph._array.Where(x => x % 2 == 0).ToArray();
                DrawGraph(evenArray, "Четные числа");
            }
            else if (OddRadioButton.IsChecked == true)
            {
                var oddArray = GenerateValueForGraph._array.Where(x => x % 2 != 0).ToArray();
                DrawGraph(oddArray, "Нечетные числа");
            }
        }

        private void DrawGraph(int[] array, string title)
        {
            GraphCanvas.Children.Clear();

            TextBlock titleBlock = new TextBlock
            {
                Text = title,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.DarkGreen,
                Margin = new Thickness(10)
            };
            Canvas.SetLeft(titleBlock, 10);
            Canvas.SetTop(titleBlock, 10);
            GraphCanvas.Children.Add(titleBlock);

            if (array.Length == 0)
            {
                TextBlock noDataBlock = new TextBlock
                {
                    Text = "Нет данных для отображения",
                    FontSize = 16,
                    Foreground = Brushes.Red,
                    Margin = new Thickness(10)
                };
                Canvas.SetLeft(noDataBlock, 10);
                Canvas.SetTop(noDataBlock, 50);
                GraphCanvas.Children.Add(noDataBlock);
                return;
            }

            double canvasWidth = GraphCanvas.ActualWidth;
            double canvasHeight = GraphCanvas.ActualHeight;

            double barWidth = canvasWidth / array.Length;
            double maxElement = array.Max();

            for (int i = 0; i < array.Length; i++)
            {
                double barHeight = (array[i] / maxElement) * (canvasHeight - 50);

                Rectangle bar = new Rectangle
                {
                    Width = barWidth - 5,
                    Height = barHeight,
                    Fill = Brushes.LightGreen,
                    Stroke = Brushes.DarkGreen,
                    StrokeThickness = 1
                };

                Canvas.SetLeft(bar, i * barWidth);
                Canvas.SetTop(bar, canvasHeight - barHeight);
                GraphCanvas.Children.Add(bar);

                TextBlock valueBlock = new TextBlock
                {
                    Text = array[i].ToString(),
                    FontSize = 12,
                    Foreground = Brushes.Black
                };
                Canvas.SetLeft(valueBlock, i * barWidth + (barWidth - 5) / 2 - 10);
                Canvas.SetTop(valueBlock, canvasHeight - barHeight - 20);
                GraphCanvas.Children.Add(valueBlock);
            }
        }
    }

    public static class GenerateValueForGraph // класс для генерации чисел
    {
        public static int[] _array;
        public static int[] Generate(int size)
        {
            _array = new int[size];
            Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                _array[i] = random.Next(1, 100); // Заполнение случайными числами
            }

            return _array;
        }
    }
}