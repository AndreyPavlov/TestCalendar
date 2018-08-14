using System.Windows;
using System.Windows.Controls;

namespace TestCalendar
{
    /// <summary>
    /// Логика взаимодействия для CarouselControl.xaml
    /// Представляет собой прокручивающийся список с переходом на первый элемент при достижении конечного элементаы
    /// </summary>
    public partial class CarouselControl : UserControl
    {
        public CarouselControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Действие при нажатии кнопки Next
        /// Переход на следующий элемент 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = CarouselListBox.SelectedIndex;
            if (selectedIndex != -1)
            {
                if (CarouselListBox.Items.Count - 1 != selectedIndex)
                    CarouselListBox.SelectedIndex++;
                else
                    CarouselListBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Действие при нажатии кнопки Prev
        /// Переход на следующий элемент 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = CarouselListBox.SelectedIndex;
            if (selectedIndex != -1)
            {
                if (selectedIndex != 0)
                    CarouselListBox.SelectedIndex--;
                else
                    CarouselListBox.SelectedIndex = CarouselListBox.Items.Count - 1;

            }
        }

        /// <summary>
        /// Отцентровка выбранного элемента в поле отображения ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarouselListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxExtensions.ScrollIntoViewCentered(CarouselListBox, CarouselListBox.SelectedItem);
        }
    }
}
