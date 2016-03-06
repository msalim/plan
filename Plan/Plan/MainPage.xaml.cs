using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Popups;
using System.Diagnostics;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Plan
{
    /// <summary>
    /// An empty page that ` be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public static int Offset {get; set;}
        public static bool SetTime {get; set;}
        public static List<CalEvent> Events {get; set;}
        public static bool ProMode {get; set;}
        public static string ResultText {get; set;}
        public static List<int> ActivePanels {get; set;}

        public MainPage()
        {
            Offset = 0;
            ProMode = false;
            Events = new List<CalEvent>();
            this.InitializeComponent();
            this.initializeDate(); 
        }

        private void initializeDate()
        {
            DateTime CurrentTime = DateTime.Now;
            int year = CurrentTime.Year;
            int month = CurrentTime.Month;
            int day = CurrentTime.Day + Offset;
            int hour = CurrentTime.Hour;
            int min = CurrentTime.Minute;
            int sec = CurrentTime.Second;
            string date = CurrentTime.DayOfWeek.ToString();

           // find when monday starts
            int weekOffset = 0;
            if (date == "Monday") weekOffset = 0;
            else if (date == "Tuesday") weekOffset = -1;
            else if (date == "Wednesday") weekOffset = -2;
            else if (date == "Thursday") weekOffset = -3;
            else if (date == "Friday") weekOffset = -4;
            else if (date == "Saturday") weekOffset = -5;
            else if (date == "Sunday") weekOffset = -6;
            day = day + weekOffset;
            
            // adjust based on offset
            int[] result = adjustDate(year, month, day);
            year = result[0];
            month = result[1];
            day = result[2];

            // set month on top left
            string monthStr = "help";
            if (month == 1) monthStr = "January";
            else if (month == 2) monthStr = "February";
            else if (month == 3) monthStr = "March";
            else if (month == 4) monthStr = "April";
            else if (month == 5) monthStr = "May";
            else if (month == 6) monthStr = "June";
            else if (month == 7) monthStr = "July";
            else if (month == 8) monthStr = "August";
            else if (month == 9) monthStr = "September";
            else if (month == 10) monthStr = "October";
            else if (month == 11) monthStr = "November";
            else if (month == 12) monthStr = "December";


            string yearStr = year.ToString();
            MonthText.Text = monthStr + " " + yearStr;

            // change date on table
            int currentDate = day;
            for (int i = 0; i <= 6; i++)
            {
                if (i == 0) MondayText.Text = "Mon " + currentDate;
                if (i == 1) TuesdayText.Text = "Tue " + currentDate;
                if (i == 2) WednesdayText.Text = "Wed " + currentDate;
                if (i == 3) ThursdayText.Text = "Thu " + currentDate;
                if (i == 4) FridayText.Text = "Fri " + currentDate;
                if (i == 5) SaturdayText.Text = "Sat " + currentDate;
                if (i == 6) SundayText.Text = "Sun " + currentDate;
                
                currentDate = currentDate + 1;

                if (month == 1 || month == 3 || month == 5 || month == 7 ||
                    month == 8 || month == 10 || month == 12)
                {
                    if (currentDate > 31)
                    {
                        currentDate = currentDate - 31;
                    }
                }
                else if (month == 4 || month == 6 || month == 9 || month == 11)
                {
                    if (currentDate > 30)
                    {
                        currentDate = currentDate - 30;
                    }
                }
                else if (month == 2)
                {
                    if (year % 100 == 0 && currentDate > 28) currentDate = currentDate - 28;
                    else if (year % 4 == 0 && currentDate > 29) currentDate = currentDate - 29;
                    else if (currentDate > 28) currentDate = currentDate - 28;
                }
            }

        }

        private int[] adjustDate(int year, int month, int day)
        {
 
            while (true)
            {
                
                if (month == 1 || month == 3 || month == 5 || month == 7 ||
                    month == 8 || month == 10 || month == 12)
                {
                    if (day <= 0)
                    {

                        if (month == 8 || month == 12) day = day + 31;
                        else if (month == 3)
                        {
                            if ((year % 100) == 0) day = day + 28;
                            else if ((year % 4) == 0) day = day + 29;
                            else day = day + 28;
                        }
                        else day = day + 30;
                        month--;
                    }
                    else if (day <= 31)
                    {
                        break;
                    }
                    else
                    {
                        month++;
                        day = day - 31;
                    }
                }
                else if (month == 4 || month == 6 || month == 9 || month == 11)
                {
                    if (day <= 0)
                    {
                        month--;
                        day = day + 31;
                    }
                    else if (day <= 30)
                    {
                        break;
                    }
                    else
                    {
                        month++;
                        day = day - 30;
                    }
                }
                else if (month == 2)
                {
                    if (day <= 0)
                    {
                        month--;
                        day = day + 31;
                    }
                    else if (((year % 100) == 0) && day <= 28)
                    {
                        break;
                    }
                    else if (((year % 4) == 0) && day <= 29)
                    {
                        break;
                    }
                    else if (((year % 4) != 0) && day <= 28)
                    {
                        break;
                    }
                    else
                    {
                        month++;
                        if (year % 100 == 0) day = day - 28;
                        else if (year % 4 == 0) day = day - 29;
                        else day = day - 28;
                    }
                }
            if (month < 1 || month > 12)
            {
                if (month < 1)
                {
                    year--;
                    month = month + 12;
                }
                
                if (month > 12)
                {
                    year++;
                    month = month - 12;
                }

            }

            }

            
            int[] result = {year, month, day};
            return result;
        }

        private void initializeProMode()
        {
            if (ProMode) ProModeButton.Content = "On";
            else ProModeButton.Content = "Off";

        }

        public class CalEvent
        {
            // member variables
            public string EventName { get; set; }
            public DateTime EventTime { get; set; }
            public TimeSpan Duration { get; set; }

            // constructors
            public CalEvent(string name, int yr, int mo, int day,
                            int hr, int min, int sec, Double length)
            {
                this.EventName = name;
                this.EventTime = new DateTime(yr, mo, day, hr, min, sec);
                this.Duration = new TimeSpan();
                this.Duration = TimeSpan.FromMinutes(length);
            }
        }

        public class AddEvent
        {
            
        }

    // EVENT HANDLERS

        // when mouse is over the box, gives emphasis to box
        private void EventText_PointerEntered(object sender, RoutedEventArgs e)
        {
            EventText.Background  = new SolidColorBrush(Colors.White);
            // EventText.BorderBrush = new SolidColorBrush(Colors.Gray);
            // EventText.BorderThickness = new Thickness(1);
        }

        // when mouse left, returns state to normal
        private void EventText_PointerExited(object sender, RoutedEventArgs e)
        {
            Color color = Color.FromArgb(255, 250, 250, 250);
            EventText.Background  = new SolidColorBrush(Colors.White);
            // EventText.BorderBrush = new SolidColorBrush(Colors.Transparent);
            // EventText.BorderThickness = new Thickness(1);
        }
        private void EventCanvas_Tapped(object sender, RoutedEventArgs e)
        {
            EventText.Focus(FocusState.Programmatic);
        }
        
        private void NextWeek_Click(object sender, RoutedEventArgs e)
        {
            Offset = Offset+7;
            initializeDate();
        }

        private void PreviousWeek_Click(object sender, RoutedEventArgs e)
        {
            Offset = Offset-7;
            initializeDate(); 
        }

        private void ProModeButton_Click(object sender, RoutedEventArgs e)
        {
            ProMode = !ProMode;
            initializeProMode();
        }
        
 // ink class https://channel9.msdn.com/Shows/Inside-Windows-Platform/Leverage-Inking-in-your-UWP-Apps

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {   
            SetTime = false;
            ResultText = "";
            ActivePanels = null;
        }
        
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {   
            if (ProMode)
            {
                RecognizeCalendar();

            }
            else if (!SetTime) RecognizeCalendar();
            else RegisterEvent();
        }

        private async void RecognizeCalendar()
        {
            var InkRecognizer = new InkRecognizerContainer();
            if (InkRecognizer != null)
            {
                try
                {

                    var recognitionResults = await InkRecognizer.RecognizeAsync(this.CalendarInk.InkPresenter.StrokeContainer, InkRecognitionTarget.All);
                    string recognizedText = string.Join(" ", recognitionResults.Select(i => i.GetTextCandidates()[0]));
                    this.CalendarInk.InkPresenter.StrokeContainer.Clear();
                    ResultText = recognizedText;
                    if (!ProMode) EventText.Text = "Select time for: " + recognizedText+".";
                    SetTime = true;                }
                catch
                {
                    EventText.Text = "Please try again.";
                }
            }
        }

        private void RegisterEvent()
        {
            int Actives = getActivePanels();
            CalEvent result = new CalEvent();

        }

        private int getActivePanels()
        {
            List<int> actives = new List<int>();
            int i;
            for (i = 0; i < 336; i++)
            {
                r0.PointerEntered
            }
        }

    }
}
