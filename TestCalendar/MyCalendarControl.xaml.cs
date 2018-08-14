using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace TestCalendar
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// Кастомный календарь
    /// </summary>
    public partial class MyCalendarControl : System.Windows.Controls.UserControl
    {
        private List<string> months;
        private List<int> years;
        private CarouselControl yearCarouselControl, monthCarouselControl;
        public MyCalendarControl()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private string rangeDates = "12.12.2017 - 13.12.2018";

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            rangeOfDates.Text = rangeDates;
            MyCalendar.SelectedDate = Convert.ToDateTime(rangeDates.Split('-').First().Trim());
        }


        /// <summary>
        /// Действие при потере фокуса текстовым полем
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaskedTextBox_LostFocus(object sender, EventArgs e)
        {
            string enteredString = (sender as MaskedTextBox).Text;
            if (!string.IsNullOrEmpty(enteredString))
            {
                string[] splitDates = enteredString.Split('-');
                if (DateTime.TryParse(splitDates[0].Trim(), out DateTime startDate) && DateTime.TryParse(splitDates[1].Trim(), out DateTime endDate))
                {
                    if (DateTime.Compare(startDate, endDate) == -1)
                        rangeDates = enteredString;
                }
            }
        }

        /// <summary>
        /// Действие кнопки "Неделя" при нажатии
        /// Выбирается неделя до текущей даты в календаре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeekButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime currentDate = DateTime.Today;
            DateTime dayWeekBefore = currentDate.AddDays(-7);
            rangeDates = dayWeekBefore.ToShortDateString() + " - " + currentDate.ToShortDateString();
            rangeOfDates.Text = rangeDates;
            calendarExpander.IsExpanded = false;
        }

        /// <summary>
        /// Действие кнопки "Месяц" при нажатии
        /// Выбирается месяц до текущей даты в календаре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime currentDate = DateTime.Today;
            DateTime dayMonthBefore = currentDate.AddMonths(-1);
            rangeDates = dayMonthBefore.ToShortDateString() + " - " + currentDate.ToShortDateString();
            rangeOfDates.Text = rangeDates;
            calendarExpander.IsExpanded = false;
        }

        /// <summary>
        /// Действие кнопки "Год" при нажатии
        /// Выбирается неделя до текущей даты в календаре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YearButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime currentDate = DateTime.Today;
            DateTime dayYearBefore = currentDate.AddYears(-1);
            rangeDates = dayYearBefore.ToShortDateString() + " - " + currentDate.ToShortDateString();
            rangeOfDates.Text = rangeDates;
            calendarExpander.IsExpanded = false;
        }

        /// <summary>
        /// Действие при открытии объекта Expander
        /// Устанавливается отображение выделения диапазона дат из текстового поля объекта Expander
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalendarExpander_Expanded(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(rangeOfDates.Text))
            {
                string[] splitDates = rangeOfDates.Text.Split('-');
                if (DateTime.TryParse(splitDates[0].Trim(), out DateTime startDate) && DateTime.TryParse(splitDates[1].Trim(), out DateTime endDate))
                {
                    SetDate(startDate, endDate);
                    MyCalendar.DisplayDate = MyCalendar.SelectedDates[0];
                }
            }
        }

        /// <summary>
        /// Устанавливает выделение выбранных данных в календаре
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void SetDate(DateTime from, DateTime to)
        {
            MyCalendar.SelectedDate = null;
            DateTime dateToAdd = from;
            while (dateToAdd <= to)
            {
                MyCalendar.SelectedDates.Add(dateToAdd);
                dateToAdd = dateToAdd.AddDays(1);
            }
        }

        /// <summary>
        /// действие при изменение выбранной даты в календаре
        /// при выборе второй даты происходит изменение в текстовом поле объекта Expander
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var removedDate = e.RemovedItems;
            DateTime startDate, endDate;
            if (removedDate.Count == 1 && e.AddedItems.Count != 0)
            {
                if (DateTime.Compare((DateTime)removedDate[0], (DateTime)e.AddedItems[0]) == 1)
                {
                    endDate = (DateTime)removedDate[0];
                    startDate = (DateTime)e.AddedItems[0];
                }
                else
                {
                    startDate = (DateTime)removedDate[0];
                    endDate = (DateTime)e.AddedItems[0];
                }

                rangeDates = startDate.ToShortDateString() + " - " + endDate.ToShortDateString();
                rangeOfDates.Text = rangeDates;
                calendarExpander.IsExpanded = false;
            }
            else
                e.Handled = true;
        }

        /// <summary>
        /// Заполнение данных в объекте MonthCarouselControl 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthCarousel_Loaded(object sender, RoutedEventArgs e)
        {
            monthCarouselControl = e.Source as CarouselControl;
            months = new List<string> { "Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек" };
            monthCarouselControl.CarouselListBox.ItemsSource = months;
            if (!String.IsNullOrEmpty(rangeDates))
            {
                monthCarouselControl.CarouselListBox.SelectedIndex = Convert.ToDateTime(rangeDates.Split('-').First().Trim()).Month - 1;
            }
            else
            {
                monthCarouselControl.CarouselListBox.SelectedIndex = DateTime.Today.Month - 1;
            }
            monthCarouselControl.CarouselListBox.SelectionChanged += CarouselMonthListBox_SelectionChanged1;
        }

        /// <summary>
        /// Установление даты в календаре соответствуюшей выбору в объекте monthCarouselControl 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarouselMonthListBox_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            DateTime displayDate = MyCalendar.DisplayDate;
            string monthString = (string)e.AddedItems[0];
            int month = DateTime.ParseExact(monthString, "MMM", CultureInfo.CurrentCulture).Month;
            MyCalendar.DisplayDate = new DateTime(displayDate.Year, month, displayDate.Day);
        }

        /// <summary>
        /// Заполнение данныхв объекте YearCarouselControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YearCarousel_Loaded(object sender, RoutedEventArgs e)
        {
            yearCarouselControl = e.Source as CarouselControl;
            years = Enumerable.Range(1900, 200).ToList();
            yearCarouselControl.CarouselListBox.ItemsSource = years;
            if (!String.IsNullOrEmpty(rangeDates))
            {
                yearCarouselControl.CarouselListBox.SelectedItem = Convert.ToDateTime(rangeDates.Split('-').First().Trim()).Year;
            }
            else
            {
                yearCarouselControl.CarouselListBox.SelectedItem = DateTime.Today.Year;
            }
            yearCarouselControl.CarouselListBox.SelectionChanged += CarouselListBox_SelectionChanged;
        }

        /// <summary>
        /// Установление даты в календаре соответствуюшей выбору в объекте yearCarouselControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarouselListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime displayDate = MyCalendar.DisplayDate;
            MyCalendar.DisplayDate = new DateTime((int)e.AddedItems[0], displayDate.Month, displayDate.Day);

        }

        /// <summary>
        /// Установление значений в объектах yearCarouselControl и monthCarouselControl 
        /// в соответствии со значение текстового поля объекта Expander
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RangeOfDates_TextChanged(object sender, EventArgs e)
        {
            if (monthCarouselControl != null && yearCarouselControl != null)
            {
                DateTime firstDate = Convert.ToDateTime(rangeDates.Split('-').First().Trim());
                monthCarouselControl.CarouselListBox.SelectedIndex = firstDate.Month - 1;
                yearCarouselControl.CarouselListBox.SelectedItem = firstDate.Year;
            }

        }
    }
}

