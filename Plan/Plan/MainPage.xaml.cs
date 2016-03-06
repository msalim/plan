using System;
using System.Collections.Generic;
using System.Windows.Input;
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
using Windows.UI.Xaml.Shapes;
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
        public List<CalEvent> Events {get; set;}
        public static bool ProMode {get; set;}
        public string ResultText {get; set;}
        public static int SaveYear {get; set;}
        public static int SaveMonth {get; set;}
        public static int SaveDay {get; set;}
        public static int OrigYear {get; set;}
        public static int OrigMonth {get; set;}
        public static int OrigDay {get; set;}
        public static bool StringInput {get; set;}

        public MainPage()
        {
            Offset = 0;
            ProMode = false;
            Events = new List<CalEvent>();
            this.InitializeComponent();
            this.InitializeDate(); 
            this.DisplayPanels();
        }
        private void InitializeDate()
        {
            DateTime CurrentTime = DateTime.Now;
            int year = CurrentTime.Year;
            int month = CurrentTime.Month;
            int day = CurrentTime.Day + Offset;

        OrigYear = year;
        OrigMonth = month;
        OrigDay = 29; // demo purposes only

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
            SaveYear = year;
            SaveMonth = month;
            SaveDay = day;

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
        private void InitializeProMode()
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
            public int StartI { get; set; }
            public int EndI { get; set; }

            // constructors
            public CalEvent(string name, DateTime time, TimeSpan duration, int startI, int endI)
            {
                this.EventName = name;
                this.EventTime = time;
                this.Duration = duration;
                this.StartI = startI;
                this.EndI = endI;
            }
        }
        // ink class https://channel9.msdn.com/Shows/Inside-Windows-Platform/Leverage-Inking-in-your-UWP-Apps
        private void Refresh()
        {   
            SetTime = false;
            ResultText = "";
            CleanBoard();
            this.InitializeDate();
            this.DisplayPanels();
            this.CalendarInk.InkPresenter.StrokeContainer.Clear();
        }
        private void DisplayPanels()
        {
            Events.ForEach(delegate(CalEvent e)
            {
                EnableBox(e);
            });
        }
        private void EnableBox(CalEvent e)
        {
            DateTime start = new DateTime(SaveYear, SaveMonth, SaveDay, 0, 0, 0);
            DateTime end = new DateTime(SaveYear, SaveMonth, SaveDay, 23, 59, 59).AddDays(6);
            int lowbound = start.CompareTo(e.EventTime.Add(e.Duration)); // start 7days < end time
            int upbound = end.CompareTo(e.EventTime); // start time < end 7days
            if (lowbound <= 0 && upbound >= 0)
            {
                int first = e.StartI;
                int last = e.EndI;
                string eventName = e.EventName;
                addTextbox(eventName, first);
                addRectangles(first, last);
            }
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
                    if (!ProMode) EventText.Text = "Select time for: " + recognizedText;
                    SetTime = true;
                    if (ProMode)
                    {
                        RegisterEvent();
                        Refresh();
                    }
                }
                catch
                {
                    EventText.Text = "Please try again.";
                }
            }
        }
        private void RegisterEvent()
        {
            Rect strokes = CalendarInk.InkPresenter.StrokeContainer.BoundingRect;
            int top = (int)strokes.Top;
            int bottom = (int)strokes.Bottom;
            int left = (int)strokes.Left;
            int right = (int)strokes.Right;

            int i;
            bool startFound = false;
            int startI = -1;
            int endI = -1;
            int startDay = -1;
            Double startHourDiv2 = -1;
            int endDay = -1;
            Double endHourDiv2 = -1;
            
            for (i = 0; i < 336; i++)
            { //20 is padding
                if (left <= ((i/48)*150)+150+20 && ((i/48)*150)+20 <= right && top <= ((i%48)*38)+37+20  && ((i%48)*38)+20 <= bottom)
                {    
                    if (!startFound)
                    {
                        startI = i;
                        startDay = i/48;
                        startHourDiv2 = i%48;
                        startFound = true;
                    }
                }
                else
                    if (startFound)
                    {
                        endI = (i-1);
                        endDay = (i-1)/48;
                        endHourDiv2 = (i-1)%48;
                        break;
                    }
            }

            if (startDay == -1)
            {
                EventText.Text = "Please select time again.";
                this.CalendarInk.InkPresenter.StrokeContainer.Clear();
                return;
            }
            DateTime eventTime = new DateTime(SaveYear, SaveMonth, SaveDay).AddDays(startDay).AddHours(startHourDiv2/2);
            DateTime endTime = new DateTime(SaveYear, SaveMonth, SaveDay).AddDays(endDay).AddHours(endHourDiv2/2);
            TimeSpan duration = endTime.Subtract(eventTime);
            string eventName = ResultText;
            CalEvent result = new CalEvent(eventName, eventTime, duration, startI, endI);
            this.Events.Add(result);
            EventText.Text = "Event registered: " + ResultText;
            
            Refresh();

        }
        private void DeleteEvent()
        {
            Rect strokes = CalendarInk.InkPresenter.StrokeContainer.BoundingRect;
            int top = (int)strokes.Top;
            int bottom = (int)strokes.Bottom;
            int left = (int)strokes.Left;
            int right = (int)strokes.Right;

            int i;
            bool startFound = false;
            int startI = -1;
            int endI = -1;
            int startDay = -1;
            Double startHourDiv2 = -1;
            int endDay = -1;
            Double endHourDiv2 = -1;
            
            for (i = 0; i < 336; i++)
            { //20 is padding
                if (left <= ((i/48)*150)+150+20 && ((i/48)*150)+20 <= right && top <= ((i%48)*38)+37+20  && ((i%48)*38)+20 <= bottom)
                {    
                    if (!startFound)
                    {
                        startI = i;
                        startDay = i/48;
                        startHourDiv2 = i%48;
                        startFound = true;
                    }
                }
                else
                    if (startFound)
                    {
                        endI = i-1;
                        endDay = i-1/48;
                        endHourDiv2 = i-1%48;
                        break;
                    }
            }

            if (startDay == -1)
            {
                EventText.Text = "Please select time again.";
                this.CalendarInk.InkPresenter.StrokeContainer.Clear();
                return;
            }

            CalEvent toDelete = null;
            
            DateTime start = new DateTime(SaveYear, SaveMonth, SaveDay, 0, 0, 0);
            DateTime end = new DateTime(SaveYear, SaveMonth, SaveDay, 23, 59, 59).AddDays(6);            int count = Events.Count;
            for (i = 0; i < count; i++)
            {
                CalEvent e = Events[i];
                int lowbound = start.CompareTo(e.EventTime.Add(e.Duration)); // start 7days < end time
                int upbound = end.CompareTo(e.EventTime); // start time < end 7days
                if (lowbound <= 0 && upbound >= 0)
                {

                    if (startI <= e.EndI ||  endI <= e.StartI)
                    toDelete = e;
                }
            }
            if (toDelete != null) Events.Remove(toDelete);
            
            Refresh();
        }



    // EVENT HANDLERS

        // when mouse is over the box, gives emphasis to box
        private void EventText_PointerEntered(object sender, RoutedEventArgs e)
        {
            EventText.Background  = new SolidColorBrush(Colors.White);
            // EventText.BorderBrush = new SolidColorBrush(Colors.Gray);
            // EventText.BorderThickness = new Thickness(1);
        }
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
            EventText.Text = "";
        }
        private void PreviousWeek_Click(object sender, RoutedEventArgs e)
        {
            Offset = Offset-7;
            Refresh(); 
        }
        private void NextWeek_Click(object sender, RoutedEventArgs e)
        {
            Offset = Offset+7;
            Refresh();
        }
        private void ProModeButton_Click(object sender, RoutedEventArgs e)
        {
            ProMode = !ProMode;
            InitializeProMode();
        }
        private void Rectangle_Tapped(object sender, RoutedEventArgs e)
        {
            // Rectangle rectangle = sender as Rectangle;
            // if (rectangle.Fill == new SolidColorBrush(Colors.LightGray)) rectangle.Fill = new SolidColorBrush(Colors.Red);
            // rectangle.Fill = new SolidColorBrush(Colors.LightGray);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {   
            if (!SetTime)
            {
                DeleteEvent();
                EventText.Text = "Deleted.";
                return;
            }
            SetTime = false;
            ResultText = "";
            this.CalendarInk.InkPresenter.StrokeContainer.Clear();
            EventText.Text = "Cancelled.";
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
        private void EventText_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                var s = EventText.Text;
                ResultText = s;
                // figure out dependency property

                EventText.Text = s;
                SetTime = true;
            }
        }
       
        // to optimize
        private void addTextbox(string eventName, int first)
        {
            if(first == 0) t0.Text = eventName;
            else if(first == 1) t1.Text = eventName;
            else if(first == 2) t2.Text = eventName;
            else if(first == 3) t3.Text = eventName;
            else if(first == 4) t4.Text = eventName;
            else if(first == 5) t5.Text = eventName;
            else if(first == 6) t6.Text = eventName;
            else if(first == 7) t7.Text = eventName;
            else if(first == 8) t8.Text = eventName;
            else if(first == 9) t9.Text = eventName;
            else if(first == 10) t10.Text = eventName;
            else if(first == 11) t11.Text = eventName;
            else if(first == 12) t12.Text = eventName;
            else if(first == 13) t13.Text = eventName;
            else if(first == 14) t14.Text = eventName;
            else if(first == 15) t15.Text = eventName;
            else if(first == 16) t16.Text = eventName;
            else if(first == 17) t17.Text = eventName;
            else if(first == 18) t18.Text = eventName;
            else if(first == 19) t19.Text = eventName;
            else if(first == 20) t20.Text = eventName;
            else if(first == 21) t21.Text = eventName;
            else if(first == 22) t22.Text = eventName;
            else if(first == 23) t23.Text = eventName;
            else if(first == 24) t24.Text = eventName;
            else if(first == 25) t25.Text = eventName;
            else if(first == 26) t26.Text = eventName;
            else if(first == 27) t27.Text = eventName;
            else if(first == 28) t28.Text = eventName;
            else if(first == 29) t29.Text = eventName;
            else if(first == 30) t30.Text = eventName;
            else if(first == 31) t31.Text = eventName;
            else if(first == 32) t32.Text = eventName;
            else if(first == 33) t33.Text = eventName;
            else if(first == 34) t34.Text = eventName;
            else if(first == 35) t35.Text = eventName;
            else if(first == 36) t36.Text = eventName;
            else if(first == 37) t37.Text = eventName;
            else if(first == 38) t38.Text = eventName;
            else if(first == 39) t39.Text = eventName;
            else if(first == 40) t40.Text = eventName;
            else if(first == 41) t41.Text = eventName;
            else if(first == 42) t42.Text = eventName;
            else if(first == 43) t43.Text = eventName;
            else if(first == 44) t44.Text = eventName;
            else if(first == 45) t45.Text = eventName;
            else if(first == 46) t46.Text = eventName;
            else if(first == 47) t47.Text = eventName;
            else if(first == 48) t48.Text = eventName;
            else if(first == 49) t49.Text = eventName;
            else if(first == 50) t50.Text = eventName;
            else if(first == 51) t51.Text = eventName;
            else if(first == 52) t52.Text = eventName;
            else if(first == 53) t53.Text = eventName;
            else if(first == 54) t54.Text = eventName;
            else if(first == 55) t55.Text = eventName;
            else if(first == 56) t56.Text = eventName;
            else if(first == 57) t57.Text = eventName;
            else if(first == 58) t58.Text = eventName;
            else if(first == 59) t59.Text = eventName;
            else if(first == 60) t60.Text = eventName;
            else if(first == 61) t61.Text = eventName;
            else if(first == 62) t62.Text = eventName;
            else if(first == 63) t63.Text = eventName;
            else if(first == 64) t64.Text = eventName;
            else if(first == 65) t65.Text = eventName;
            else if(first == 66) t66.Text = eventName;
            else if(first == 67) t67.Text = eventName;
            else if(first == 68) t68.Text = eventName;
            else if(first == 69) t69.Text = eventName;
            else if(first == 70) t70.Text = eventName;
            else if(first == 71) t71.Text = eventName;
            else if(first == 72) t72.Text = eventName;
            else if(first == 73) t73.Text = eventName;
            else if(first == 74) t74.Text = eventName;
            else if(first == 75) t75.Text = eventName;
            else if(first == 76) t76.Text = eventName;
            else if(first == 77) t77.Text = eventName;
            else if(first == 78) t78.Text = eventName;
            else if(first == 79) t79.Text = eventName;
            else if(first == 80) t80.Text = eventName;
            else if(first == 81) t81.Text = eventName;
            else if(first == 82) t82.Text = eventName;
            else if(first == 83) t83.Text = eventName;
            else if(first == 84) t84.Text = eventName;
            else if(first == 85) t85.Text = eventName;
            else if(first == 86) t86.Text = eventName;
            else if(first == 87) t87.Text = eventName;
            else if(first == 88) t88.Text = eventName;
            else if(first == 89) t89.Text = eventName;
            else if(first == 90) t90.Text = eventName;
            else if(first == 91) t91.Text = eventName;
            else if(first == 92) t92.Text = eventName;
            else if(first == 93) t93.Text = eventName;
            else if(first == 94) t94.Text = eventName;
            else if(first == 95) t95.Text = eventName;
            else if(first == 96) t96.Text = eventName;
            else if(first == 97) t97.Text = eventName;
            else if(first == 98) t98.Text = eventName;
            else if(first == 99) t99.Text = eventName;
            else if(first == 100) t100.Text = eventName;
            else if(first == 101) t101.Text = eventName;
            else if(first == 102) t102.Text = eventName;
            else if(first == 103) t103.Text = eventName;
            else if(first == 104) t104.Text = eventName;
            else if(first == 105) t105.Text = eventName;
            else if(first == 106) t106.Text = eventName;
            else if(first == 107) t107.Text = eventName;
            else if(first == 108) t108.Text = eventName;
            else if(first == 109) t109.Text = eventName;
            else if(first == 110) t110.Text = eventName;
            else if(first == 111) t111.Text = eventName;
            else if(first == 112) t112.Text = eventName;
            else if(first == 113) t113.Text = eventName;
            else if(first == 114) t114.Text = eventName;
            else if(first == 115) t115.Text = eventName;
            else if(first == 116) t116.Text = eventName;
            else if(first == 117) t117.Text = eventName;
            else if(first == 118) t118.Text = eventName;
            else if(first == 119) t119.Text = eventName;
            else if(first == 120) t120.Text = eventName;
            else if(first == 121) t121.Text = eventName;
            else if(first == 122) t122.Text = eventName;
            else if(first == 123) t123.Text = eventName;
            else if(first == 124) t124.Text = eventName;
            else if(first == 125) t125.Text = eventName;
            else if(first == 126) t126.Text = eventName;
            else if(first == 127) t127.Text = eventName;
            else if(first == 128) t128.Text = eventName;
            else if(first == 129) t129.Text = eventName;
            else if(first == 130) t130.Text = eventName;
            else if(first == 131) t131.Text = eventName;
            else if(first == 132) t132.Text = eventName;
            else if(first == 133) t133.Text = eventName;
            else if(first == 134) t134.Text = eventName;
            else if(first == 135) t135.Text = eventName;
            else if(first == 136) t136.Text = eventName;
            else if(first == 137) t137.Text = eventName;
            else if(first == 138) t138.Text = eventName;
            else if(first == 139) t139.Text = eventName;
            else if(first == 140) t140.Text = eventName;
            else if(first == 141) t141.Text = eventName;
            else if(first == 142) t142.Text = eventName;
            else if(first == 143) t143.Text = eventName;
            else if(first == 144) t144.Text = eventName;
            else if(first == 145) t145.Text = eventName;
            else if(first == 146) t146.Text = eventName;
            else if(first == 147) t147.Text = eventName;
            else if(first == 148) t148.Text = eventName;
            else if(first == 149) t149.Text = eventName;
            else if(first == 150) t150.Text = eventName;
            else if(first == 151) t151.Text = eventName;
            else if(first == 152) t152.Text = eventName;
            else if(first == 153) t153.Text = eventName;
            else if(first == 154) t154.Text = eventName;
            else if(first == 155) t155.Text = eventName;
            else if(first == 156) t156.Text = eventName;
            else if(first == 157) t157.Text = eventName;
            else if(first == 158) t158.Text = eventName;
            else if(first == 159) t159.Text = eventName;
            else if(first == 160) t160.Text = eventName;
            else if(first == 161) t161.Text = eventName;
            else if(first == 162) t162.Text = eventName;
            else if(first == 163) t163.Text = eventName;
            else if(first == 164) t164.Text = eventName;
            else if(first == 165) t165.Text = eventName;
            else if(first == 166) t166.Text = eventName;
            else if(first == 167) t167.Text = eventName;
            else if(first == 168) t168.Text = eventName;
            else if(first == 169) t169.Text = eventName;
            else if(first == 170) t170.Text = eventName;
            else if(first == 171) t171.Text = eventName;
            else if(first == 172) t172.Text = eventName;
            else if(first == 173) t173.Text = eventName;
            else if(first == 174) t174.Text = eventName;
            else if(first == 175) t175.Text = eventName;
            else if(first == 176) t176.Text = eventName;
            else if(first == 177) t177.Text = eventName;
            else if(first == 178) t178.Text = eventName;
            else if(first == 179) t179.Text = eventName;
            else if(first == 180) t180.Text = eventName;
            else if(first == 181) t181.Text = eventName;
            else if(first == 182) t182.Text = eventName;
            else if(first == 183) t183.Text = eventName;
            else if(first == 184) t184.Text = eventName;
            else if(first == 185) t185.Text = eventName;
            else if(first == 186) t186.Text = eventName;
            else if(first == 187) t187.Text = eventName;
            else if(first == 188) t188.Text = eventName;
            else if(first == 189) t189.Text = eventName;
            else if(first == 190) t190.Text = eventName;
            else if(first == 191) t191.Text = eventName;
            else if(first == 192) t192.Text = eventName;
            else if(first == 193) t193.Text = eventName;
            else if(first == 194) t194.Text = eventName;
            else if(first == 195) t195.Text = eventName;
            else if(first == 196) t196.Text = eventName;
            else if(first == 197) t197.Text = eventName;
            else if(first == 198) t198.Text = eventName;
            else if(first == 199) t199.Text = eventName;
            else if(first == 200) t200.Text = eventName;
            else if(first == 201) t201.Text = eventName;
            else if(first == 202) t202.Text = eventName;
            else if(first == 203) t203.Text = eventName;
            else if(first == 204) t204.Text = eventName;
            else if(first == 205) t205.Text = eventName;
            else if(first == 206) t206.Text = eventName;
            else if(first == 207) t207.Text = eventName;
            else if(first == 208) t208.Text = eventName;
            else if(first == 209) t209.Text = eventName;
            else if(first == 210) t210.Text = eventName;
            else if(first == 211) t211.Text = eventName;
            else if(first == 212) t212.Text = eventName;
            else if(first == 213) t213.Text = eventName;
            else if(first == 214) t214.Text = eventName;
            else if(first == 215) t215.Text = eventName;
            else if(first == 216) t216.Text = eventName;
            else if(first == 217) t217.Text = eventName;
            else if(first == 218) t218.Text = eventName;
            else if(first == 219) t219.Text = eventName;
            else if(first == 220) t220.Text = eventName;
            else if(first == 221) t221.Text = eventName;
            else if(first == 222) t222.Text = eventName;
            else if(first == 223) t223.Text = eventName;
            else if(first == 224) t224.Text = eventName;
            else if(first == 225) t225.Text = eventName;
            else if(first == 226) t226.Text = eventName;
            else if(first == 227) t227.Text = eventName;
            else if(first == 228) t228.Text = eventName;
            else if(first == 229) t229.Text = eventName;
            else if(first == 230) t230.Text = eventName;
            else if(first == 231) t231.Text = eventName;
            else if(first == 232) t232.Text = eventName;
            else if(first == 233) t233.Text = eventName;
            else if(first == 234) t234.Text = eventName;
            else if(first == 235) t235.Text = eventName;
            else if(first == 236) t236.Text = eventName;
            else if(first == 237) t237.Text = eventName;
            else if(first == 238) t238.Text = eventName;
            else if(first == 239) t239.Text = eventName;
            else if(first == 240) t240.Text = eventName;
            else if(first == 241) t241.Text = eventName;
            else if(first == 242) t242.Text = eventName;
            else if(first == 243) t243.Text = eventName;
            else if(first == 244) t244.Text = eventName;
            else if(first == 245) t245.Text = eventName;
            else if(first == 246) t246.Text = eventName;
            else if(first == 247) t247.Text = eventName;
            else if(first == 248) t248.Text = eventName;
            else if(first == 249) t249.Text = eventName;
            else if(first == 250) t250.Text = eventName;
            else if(first == 251) t251.Text = eventName;
            else if(first == 252) t252.Text = eventName;
            else if(first == 253) t253.Text = eventName;
            else if(first == 254) t254.Text = eventName;
            else if(first == 255) t255.Text = eventName;
            else if(first == 256) t256.Text = eventName;
            else if(first == 257) t257.Text = eventName;
            else if(first == 258) t258.Text = eventName;
            else if(first == 259) t259.Text = eventName;
            else if(first == 260) t260.Text = eventName;
            else if(first == 261) t261.Text = eventName;
            else if(first == 262) t262.Text = eventName;
            else if(first == 263) t263.Text = eventName;
            else if(first == 264) t264.Text = eventName;
            else if(first == 265) t265.Text = eventName;
            else if(first == 266) t266.Text = eventName;
            else if(first == 267) t267.Text = eventName;
            else if(first == 268) t268.Text = eventName;
            else if(first == 269) t269.Text = eventName;
            else if(first == 270) t270.Text = eventName;
            else if(first == 271) t271.Text = eventName;
            else if(first == 272) t272.Text = eventName;
            else if(first == 273) t273.Text = eventName;
            else if(first == 274) t274.Text = eventName;
            else if(first == 275) t275.Text = eventName;
            else if(first == 276) t276.Text = eventName;
            else if(first == 277) t277.Text = eventName;
            else if(first == 278) t278.Text = eventName;
            else if(first == 279) t279.Text = eventName;
            else if(first == 280) t280.Text = eventName;
            else if(first == 281) t281.Text = eventName;
            else if(first == 282) t282.Text = eventName;
            else if(first == 283) t283.Text = eventName;
            else if(first == 284) t284.Text = eventName;
            else if(first == 285) t285.Text = eventName;
            else if(first == 286) t286.Text = eventName;
            else if(first == 287) t287.Text = eventName;
            else if(first == 288) t288.Text = eventName;
            else if(first == 289) t289.Text = eventName;
            else if(first == 290) t290.Text = eventName;
            else if(first == 291) t291.Text = eventName;
            else if(first == 292) t292.Text = eventName;
            else if(first == 293) t293.Text = eventName;
            else if(first == 294) t294.Text = eventName;
            else if(first == 295) t295.Text = eventName;
            else if(first == 296) t296.Text = eventName;
            else if(first == 297) t297.Text = eventName;
            else if(first == 298) t298.Text = eventName;
            else if(first == 299) t299.Text = eventName;
            else if(first == 300) t300.Text = eventName;
            else if(first == 301) t301.Text = eventName;
            else if(first == 302) t302.Text = eventName;
            else if(first == 303) t303.Text = eventName;
            else if(first == 304) t304.Text = eventName;
            else if(first == 305) t305.Text = eventName;
            else if(first == 306) t306.Text = eventName;
            else if(first == 307) t307.Text = eventName;
            else if(first == 308) t308.Text = eventName;
            else if(first == 309) t309.Text = eventName;
            else if(first == 310) t310.Text = eventName;
            else if(first == 311) t311.Text = eventName;
            else if(first == 312) t312.Text = eventName;
            else if(first == 313) t313.Text = eventName;
            else if(first == 314) t314.Text = eventName;
            else if(first == 315) t315.Text = eventName;
            else if(first == 316) t316.Text = eventName;
            else if(first == 317) t317.Text = eventName;
            else if(first == 318) t318.Text = eventName;
            else if(first == 319) t319.Text = eventName;
            else if(first == 320) t320.Text = eventName;
            else if(first == 321) t321.Text = eventName;
            else if(first == 322) t322.Text = eventName;
            else if(first == 323) t323.Text = eventName;
            else if(first == 324) t324.Text = eventName;
            else if(first == 325) t325.Text = eventName;
            else if(first == 326) t326.Text = eventName;
            else if(first == 327) t327.Text = eventName;
            else if(first == 328) t328.Text = eventName;
            else if(first == 329) t329.Text = eventName;
            else if(first == 330) t330.Text = eventName;
            else if(first == 331) t331.Text = eventName;
            else if(first == 332) t332.Text = eventName;
            else if(first == 333) t333.Text = eventName;
            else if(first == 334) t334.Text = eventName;
            else if(first == 335) t335.Text = eventName;
            
        }
        private void addRectangles(int first, int last)
        {
            SolidColorBrush col = new SolidColorBrush(Colors.LightGray);
            if(first <= 0 && 0 <= last) r0.Fill = col;
            if(first <= 1 && 1 <= last) r1.Fill = col;
            if(first <= 2 && 2 <= last) r2.Fill = col;
            if(first <= 3 && 3 <= last) r3.Fill = col;
            if(first <= 4 && 4 <= last) r4.Fill = col;
            if(first <= 5 && 5 <= last) r5.Fill = col;
            if(first <= 6 && 6 <= last) r6.Fill = col;
            if(first <= 7 && 7 <= last) r7.Fill = col;
            if(first <= 8 && 8 <= last) r8.Fill = col;
            if(first <= 9 && 9 <= last) r9.Fill = col;
            if(first <= 10 && 10 <= last) r10.Fill = col;
            if(first <= 11 && 11 <= last) r11.Fill = col;
            if(first <= 12 && 12 <= last) r12.Fill = col;
            if(first <= 13 && 13 <= last) r13.Fill = col;
            if(first <= 14 && 14 <= last) r14.Fill = col;
            if(first <= 15 && 15 <= last) r15.Fill = col;
            if(first <= 16 && 16 <= last) r16.Fill = col;
            if(first <= 17 && 17 <= last) r17.Fill = col;
            if(first <= 18 && 18 <= last) r18.Fill = col;
            if(first <= 19 && 19 <= last) r19.Fill = col;
            if(first <= 20 && 20 <= last) r20.Fill = col;
            if(first <= 21 && 21 <= last) r21.Fill = col;
            if(first <= 22 && 22 <= last) r22.Fill = col;
            if(first <= 23 && 23 <= last) r23.Fill = col;
            if(first <= 24 && 24 <= last) r24.Fill = col;
            if(first <= 25 && 25 <= last) r25.Fill = col;
            if(first <= 26 && 26 <= last) r26.Fill = col;
            if(first <= 27 && 27 <= last) r27.Fill = col;
            if(first <= 28 && 28 <= last) r28.Fill = col;
            if(first <= 29 && 29 <= last) r29.Fill = col;
            if(first <= 30 && 30 <= last) r30.Fill = col;
            if(first <= 31 && 31 <= last) r31.Fill = col;
            if(first <= 32 && 32 <= last) r32.Fill = col;
            if(first <= 33 && 33 <= last) r33.Fill = col;
            if(first <= 34 && 34 <= last) r34.Fill = col;
            if(first <= 35 && 35 <= last) r35.Fill = col;
            if(first <= 36 && 36 <= last) r36.Fill = col;
            if(first <= 37 && 37 <= last) r37.Fill = col;
            if(first <= 38 && 38 <= last) r38.Fill = col;
            if(first <= 39 && 39 <= last) r39.Fill = col;
            if(first <= 40 && 40 <= last) r40.Fill = col;
            if(first <= 41 && 41 <= last) r41.Fill = col;
            if(first <= 42 && 42 <= last) r42.Fill = col;
            if(first <= 43 && 43 <= last) r43.Fill = col;
            if(first <= 44 && 44 <= last) r44.Fill = col;
            if(first <= 45 && 45 <= last) r45.Fill = col;
            if(first <= 46 && 46 <= last) r46.Fill = col;
            if(first <= 47 && 47 <= last) r47.Fill = col;
            if(first <= 48 && 48 <= last) r48.Fill = col;
            if(first <= 49 && 49 <= last) r49.Fill = col;
            if(first <= 50 && 50 <= last) r50.Fill = col;
            if(first <= 51 && 51 <= last) r51.Fill = col;
            if(first <= 52 && 52 <= last) r52.Fill = col;
            if(first <= 53 && 53 <= last) r53.Fill = col;
            if(first <= 54 && 54 <= last) r54.Fill = col;
            if(first <= 55 && 55 <= last) r55.Fill = col;
            if(first <= 56 && 56 <= last) r56.Fill = col;
            if(first <= 57 && 57 <= last) r57.Fill = col;
            if(first <= 58 && 58 <= last) r58.Fill = col;
            if(first <= 59 && 59 <= last) r59.Fill = col;
            if(first <= 60 && 60 <= last) r60.Fill = col;
            if(first <= 61 && 61 <= last) r61.Fill = col;
            if(first <= 62 && 62 <= last) r62.Fill = col;
            if(first <= 63 && 63 <= last) r63.Fill = col;
            if(first <= 64 && 64 <= last) r64.Fill = col;
            if(first <= 65 && 65 <= last) r65.Fill = col;
            if(first <= 66 && 66 <= last) r66.Fill = col;
            if(first <= 67 && 67 <= last) r67.Fill = col;
            if(first <= 68 && 68 <= last) r68.Fill = col;
            if(first <= 69 && 69 <= last) r69.Fill = col;
            if(first <= 70 && 70 <= last) r70.Fill = col;
            if(first <= 71 && 71 <= last) r71.Fill = col;
            if(first <= 72 && 72 <= last) r72.Fill = col;
            if(first <= 73 && 73 <= last) r73.Fill = col;
            if(first <= 74 && 74 <= last) r74.Fill = col;
            if(first <= 75 && 75 <= last) r75.Fill = col;
            if(first <= 76 && 76 <= last) r76.Fill = col;
            if(first <= 77 && 77 <= last) r77.Fill = col;
            if(first <= 78 && 78 <= last) r78.Fill = col;
            if(first <= 79 && 79 <= last) r79.Fill = col;
            if(first <= 80 && 80 <= last) r80.Fill = col;
            if(first <= 81 && 81 <= last) r81.Fill = col;
            if(first <= 82 && 82 <= last) r82.Fill = col;
            if(first <= 83 && 83 <= last) r83.Fill = col;
            if(first <= 84 && 84 <= last) r84.Fill = col;
            if(first <= 85 && 85 <= last) r85.Fill = col;
            if(first <= 86 && 86 <= last) r86.Fill = col;
            if(first <= 87 && 87 <= last) r87.Fill = col;
            if(first <= 88 && 88 <= last) r88.Fill = col;
            if(first <= 89 && 89 <= last) r89.Fill = col;
            if(first <= 90 && 90 <= last) r90.Fill = col;
            if(first <= 91 && 91 <= last) r91.Fill = col;
            if(first <= 92 && 92 <= last) r92.Fill = col;
            if(first <= 93 && 93 <= last) r93.Fill = col;
            if(first <= 94 && 94 <= last) r94.Fill = col;
            if(first <= 95 && 95 <= last) r95.Fill = col;
            if(first <= 96 && 96 <= last) r96.Fill = col;
            if(first <= 97 && 97 <= last) r97.Fill = col;
            if(first <= 98 && 98 <= last) r98.Fill = col;
            if(first <= 99 && 99 <= last) r99.Fill = col;
            if(first <= 100 && 100 <= last) r100.Fill = col;
            if(first <= 101 && 101 <= last) r101.Fill = col;
            if(first <= 102 && 102 <= last) r102.Fill = col;
            if(first <= 103 && 103 <= last) r103.Fill = col;
            if(first <= 104 && 104 <= last) r104.Fill = col;
            if(first <= 105 && 105 <= last) r105.Fill = col;
            if(first <= 106 && 106 <= last) r106.Fill = col;
            if(first <= 107 && 107 <= last) r107.Fill = col;
            if(first <= 108 && 108 <= last) r108.Fill = col;
            if(first <= 109 && 109 <= last) r109.Fill = col;
            if(first <= 110 && 110 <= last) r110.Fill = col;
            if(first <= 111 && 111 <= last) r111.Fill = col;
            if(first <= 112 && 112 <= last) r112.Fill = col;
            if(first <= 113 && 113 <= last) r113.Fill = col;
            if(first <= 114 && 114 <= last) r114.Fill = col;
            if(first <= 115 && 115 <= last) r115.Fill = col;
            if(first <= 116 && 116 <= last) r116.Fill = col;
            if(first <= 117 && 117 <= last) r117.Fill = col;
            if(first <= 118 && 118 <= last) r118.Fill = col;
            if(first <= 119 && 119 <= last) r119.Fill = col;
            if(first <= 120 && 120 <= last) r120.Fill = col;
            if(first <= 121 && 121 <= last) r121.Fill = col;
            if(first <= 122 && 122 <= last) r122.Fill = col;
            if(first <= 123 && 123 <= last) r123.Fill = col;
            if(first <= 124 && 124 <= last) r124.Fill = col;
            if(first <= 125 && 125 <= last) r125.Fill = col;
            if(first <= 126 && 126 <= last) r126.Fill = col;
            if(first <= 127 && 127 <= last) r127.Fill = col;
            if(first <= 128 && 128 <= last) r128.Fill = col;
            if(first <= 129 && 129 <= last) r129.Fill = col;
            if(first <= 130 && 130 <= last) r130.Fill = col;
            if(first <= 131 && 131 <= last) r131.Fill = col;
            if(first <= 132 && 132 <= last) r132.Fill = col;
            if(first <= 133 && 133 <= last) r133.Fill = col;
            if(first <= 134 && 134 <= last) r134.Fill = col;
            if(first <= 135 && 135 <= last) r135.Fill = col;
            if(first <= 136 && 136 <= last) r136.Fill = col;
            if(first <= 137 && 137 <= last) r137.Fill = col;
            if(first <= 138 && 138 <= last) r138.Fill = col;
            if(first <= 139 && 139 <= last) r139.Fill = col;
            if(first <= 140 && 140 <= last) r140.Fill = col;
            if(first <= 141 && 141 <= last) r141.Fill = col;
            if(first <= 142 && 142 <= last) r142.Fill = col;
            if(first <= 143 && 143 <= last) r143.Fill = col;
            if(first <= 144 && 144 <= last) r144.Fill = col;
            if(first <= 145 && 145 <= last) r145.Fill = col;
            if(first <= 146 && 146 <= last) r146.Fill = col;
            if(first <= 147 && 147 <= last) r147.Fill = col;
            if(first <= 148 && 148 <= last) r148.Fill = col;
            if(first <= 149 && 149 <= last) r149.Fill = col;
            if(first <= 150 && 150 <= last) r150.Fill = col;
            if(first <= 151 && 151 <= last) r151.Fill = col;
            if(first <= 152 && 152 <= last) r152.Fill = col;
            if(first <= 153 && 153 <= last) r153.Fill = col;
            if(first <= 154 && 154 <= last) r154.Fill = col;
            if(first <= 155 && 155 <= last) r155.Fill = col;
            if(first <= 156 && 156 <= last) r156.Fill = col;
            if(first <= 157 && 157 <= last) r157.Fill = col;
            if(first <= 158 && 158 <= last) r158.Fill = col;
            if(first <= 159 && 159 <= last) r159.Fill = col;
            if(first <= 160 && 160 <= last) r160.Fill = col;
            if(first <= 161 && 161 <= last) r161.Fill = col;
            if(first <= 162 && 162 <= last) r162.Fill = col;
            if(first <= 163 && 163 <= last) r163.Fill = col;
            if(first <= 164 && 164 <= last) r164.Fill = col;
            if(first <= 165 && 165 <= last) r165.Fill = col;
            if(first <= 166 && 166 <= last) r166.Fill = col;
            if(first <= 167 && 167 <= last) r167.Fill = col;
            if(first <= 168 && 168 <= last) r168.Fill = col;
            if(first <= 169 && 169 <= last) r169.Fill = col;
            if(first <= 170 && 170 <= last) r170.Fill = col;
            if(first <= 171 && 171 <= last) r171.Fill = col;
            if(first <= 172 && 172 <= last) r172.Fill = col;
            if(first <= 173 && 173 <= last) r173.Fill = col;
            if(first <= 174 && 174 <= last) r174.Fill = col;
            if(first <= 175 && 175 <= last) r175.Fill = col;
            if(first <= 176 && 176 <= last) r176.Fill = col;
            if(first <= 177 && 177 <= last) r177.Fill = col;
            if(first <= 178 && 178 <= last) r178.Fill = col;
            if(first <= 179 && 179 <= last) r179.Fill = col;
            if(first <= 180 && 180 <= last) r180.Fill = col;
            if(first <= 181 && 181 <= last) r181.Fill = col;
            if(first <= 182 && 182 <= last) r182.Fill = col;
            if(first <= 183 && 183 <= last) r183.Fill = col;
            if(first <= 184 && 184 <= last) r184.Fill = col;
            if(first <= 185 && 185 <= last) r185.Fill = col;
            if(first <= 186 && 186 <= last) r186.Fill = col;
            if(first <= 187 && 187 <= last) r187.Fill = col;
            if(first <= 188 && 188 <= last) r188.Fill = col;
            if(first <= 189 && 189 <= last) r189.Fill = col;
            if(first <= 190 && 190 <= last) r190.Fill = col;
            if(first <= 191 && 191 <= last) r191.Fill = col;
            if(first <= 192 && 192 <= last) r192.Fill = col;
            if(first <= 193 && 193 <= last) r193.Fill = col;
            if(first <= 194 && 194 <= last) r194.Fill = col;
            if(first <= 195 && 195 <= last) r195.Fill = col;
            if(first <= 196 && 196 <= last) r196.Fill = col;
            if(first <= 197 && 197 <= last) r197.Fill = col;
            if(first <= 198 && 198 <= last) r198.Fill = col;
            if(first <= 199 && 199 <= last) r199.Fill = col;
            if(first <= 200 && 200 <= last) r200.Fill = col;
            if(first <= 201 && 201 <= last) r201.Fill = col;
            if(first <= 202 && 202 <= last) r202.Fill = col;
            if(first <= 203 && 203 <= last) r203.Fill = col;
            if(first <= 204 && 204 <= last) r204.Fill = col;
            if(first <= 205 && 205 <= last) r205.Fill = col;
            if(first <= 206 && 206 <= last) r206.Fill = col;
            if(first <= 207 && 207 <= last) r207.Fill = col;
            if(first <= 208 && 208 <= last) r208.Fill = col;
            if(first <= 209 && 209 <= last) r209.Fill = col;
            if(first <= 210 && 210 <= last) r210.Fill = col;
            if(first <= 211 && 211 <= last) r211.Fill = col;
            if(first <= 212 && 212 <= last) r212.Fill = col;
            if(first <= 213 && 213 <= last) r213.Fill = col;
            if(first <= 214 && 214 <= last) r214.Fill = col;
            if(first <= 215 && 215 <= last) r215.Fill = col;
            if(first <= 216 && 216 <= last) r216.Fill = col;
            if(first <= 217 && 217 <= last) r217.Fill = col;
            if(first <= 218 && 218 <= last) r218.Fill = col;
            if(first <= 219 && 219 <= last) r219.Fill = col;
            if(first <= 220 && 220 <= last) r220.Fill = col;
            if(first <= 221 && 221 <= last) r221.Fill = col;
            if(first <= 222 && 222 <= last) r222.Fill = col;
            if(first <= 223 && 223 <= last) r223.Fill = col;
            if(first <= 224 && 224 <= last) r224.Fill = col;
            if(first <= 225 && 225 <= last) r225.Fill = col;
            if(first <= 226 && 226 <= last) r226.Fill = col;
            if(first <= 227 && 227 <= last) r227.Fill = col;
            if(first <= 228 && 228 <= last) r228.Fill = col;
            if(first <= 229 && 229 <= last) r229.Fill = col;
            if(first <= 230 && 230 <= last) r230.Fill = col;
            if(first <= 231 && 231 <= last) r231.Fill = col;
            if(first <= 232 && 232 <= last) r232.Fill = col;
            if(first <= 233 && 233 <= last) r233.Fill = col;
            if(first <= 234 && 234 <= last) r234.Fill = col;
            if(first <= 235 && 235 <= last) r235.Fill = col;
            if(first <= 236 && 236 <= last) r236.Fill = col;
            if(first <= 237 && 237 <= last) r237.Fill = col;
            if(first <= 238 && 238 <= last) r238.Fill = col;
            if(first <= 239 && 239 <= last) r239.Fill = col;
            if(first <= 240 && 240 <= last) r240.Fill = col;
            if(first <= 241 && 241 <= last) r241.Fill = col;
            if(first <= 242 && 242 <= last) r242.Fill = col;
            if(first <= 243 && 243 <= last) r243.Fill = col;
            if(first <= 244 && 244 <= last) r244.Fill = col;
            if(first <= 245 && 245 <= last) r245.Fill = col;
            if(first <= 246 && 246 <= last) r246.Fill = col;
            if(first <= 247 && 247 <= last) r247.Fill = col;
            if(first <= 248 && 248 <= last) r248.Fill = col;
            if(first <= 249 && 249 <= last) r249.Fill = col;
            if(first <= 250 && 250 <= last) r250.Fill = col;
            if(first <= 251 && 251 <= last) r251.Fill = col;
            if(first <= 252 && 252 <= last) r252.Fill = col;
            if(first <= 253 && 253 <= last) r253.Fill = col;
            if(first <= 254 && 254 <= last) r254.Fill = col;
            if(first <= 255 && 255 <= last) r255.Fill = col;
            if(first <= 256 && 256 <= last) r256.Fill = col;
            if(first <= 257 && 257 <= last) r257.Fill = col;
            if(first <= 258 && 258 <= last) r258.Fill = col;
            if(first <= 259 && 259 <= last) r259.Fill = col;
            if(first <= 260 && 260 <= last) r260.Fill = col;
            if(first <= 261 && 261 <= last) r261.Fill = col;
            if(first <= 262 && 262 <= last) r262.Fill = col;
            if(first <= 263 && 263 <= last) r263.Fill = col;
            if(first <= 264 && 264 <= last) r264.Fill = col;
            if(first <= 265 && 265 <= last) r265.Fill = col;
            if(first <= 266 && 266 <= last) r266.Fill = col;
            if(first <= 267 && 267 <= last) r267.Fill = col;
            if(first <= 268 && 268 <= last) r268.Fill = col;
            if(first <= 269 && 269 <= last) r269.Fill = col;
            if(first <= 270 && 270 <= last) r270.Fill = col;
            if(first <= 271 && 271 <= last) r271.Fill = col;
            if(first <= 272 && 272 <= last) r272.Fill = col;
            if(first <= 273 && 273 <= last) r273.Fill = col;
            if(first <= 274 && 274 <= last) r274.Fill = col;
            if(first <= 275 && 275 <= last) r275.Fill = col;
            if(first <= 276 && 276 <= last) r276.Fill = col;
            if(first <= 277 && 277 <= last) r277.Fill = col;
            if(first <= 278 && 278 <= last) r278.Fill = col;
            if(first <= 279 && 279 <= last) r279.Fill = col;
            if(first <= 280 && 280 <= last) r280.Fill = col;
            if(first <= 281 && 281 <= last) r281.Fill = col;
            if(first <= 282 && 282 <= last) r282.Fill = col;
            if(first <= 283 && 283 <= last) r283.Fill = col;
            if(first <= 284 && 284 <= last) r284.Fill = col;
            if(first <= 285 && 285 <= last) r285.Fill = col;
            if(first <= 286 && 286 <= last) r286.Fill = col;
            if(first <= 287 && 287 <= last) r287.Fill = col;
            if(first <= 288 && 288 <= last) r288.Fill = col;
            if(first <= 289 && 289 <= last) r289.Fill = col;
            if(first <= 290 && 290 <= last) r290.Fill = col;
            if(first <= 291 && 291 <= last) r291.Fill = col;
            if(first <= 292 && 292 <= last) r292.Fill = col;
            if(first <= 293 && 293 <= last) r293.Fill = col;
            if(first <= 294 && 294 <= last) r294.Fill = col;
            if(first <= 295 && 295 <= last) r295.Fill = col;
            if(first <= 296 && 296 <= last) r296.Fill = col;
            if(first <= 297 && 297 <= last) r297.Fill = col;
            if(first <= 298 && 298 <= last) r298.Fill = col;
            if(first <= 299 && 299 <= last) r299.Fill = col;
            if(first <= 300 && 300 <= last) r300.Fill = col;
            if(first <= 301 && 301 <= last) r301.Fill = col;
            if(first <= 302 && 302 <= last) r302.Fill = col;
            if(first <= 303 && 303 <= last) r303.Fill = col;
            if(first <= 304 && 304 <= last) r304.Fill = col;
            if(first <= 305 && 305 <= last) r305.Fill = col;
            if(first <= 306 && 306 <= last) r306.Fill = col;
            if(first <= 307 && 307 <= last) r307.Fill = col;
            if(first <= 308 && 308 <= last) r308.Fill = col;
            if(first <= 309 && 309 <= last) r309.Fill = col;
            if(first <= 310 && 310 <= last) r310.Fill = col;
            if(first <= 311 && 311 <= last) r311.Fill = col;
            if(first <= 312 && 312 <= last) r312.Fill = col;
            if(first <= 313 && 313 <= last) r313.Fill = col;
            if(first <= 314 && 314 <= last) r314.Fill = col;
            if(first <= 315 && 315 <= last) r315.Fill = col;
            if(first <= 316 && 316 <= last) r316.Fill = col;
            if(first <= 317 && 317 <= last) r317.Fill = col;
            if(first <= 318 && 318 <= last) r318.Fill = col;
            if(first <= 319 && 319 <= last) r319.Fill = col;
            if(first <= 320 && 320 <= last) r320.Fill = col;
            if(first <= 321 && 321 <= last) r321.Fill = col;
            if(first <= 322 && 322 <= last) r322.Fill = col;
            if(first <= 323 && 323 <= last) r323.Fill = col;
            if(first <= 324 && 324 <= last) r324.Fill = col;
            if(first <= 325 && 325 <= last) r325.Fill = col;
            if(first <= 326 && 326 <= last) r326.Fill = col;
            if(first <= 327 && 327 <= last) r327.Fill = col;
            if(first <= 328 && 328 <= last) r328.Fill = col;
            if(first <= 329 && 329 <= last) r329.Fill = col;
            if(first <= 330 && 330 <= last) r330.Fill = col;
            if(first <= 331 && 331 <= last) r331.Fill = col;
            if(first <= 332 && 332 <= last) r332.Fill = col;
            if(first <= 333 && 333 <= last) r333.Fill = col;
            if(first <= 334 && 334 <= last) r334.Fill = col;
            if(first <= 335 && 335 <= last) r335.Fill = col;
        }
        private void CleanBoard()
        {
            r0.Fill = new SolidColorBrush(Colors.Transparent);
            r1.Fill = new SolidColorBrush(Colors.Transparent);
            r2.Fill = new SolidColorBrush(Colors.Transparent);
            r3.Fill = new SolidColorBrush(Colors.Transparent);
            r4.Fill = new SolidColorBrush(Colors.Transparent);
            r5.Fill = new SolidColorBrush(Colors.Transparent);
            r6.Fill = new SolidColorBrush(Colors.Transparent);
            r7.Fill = new SolidColorBrush(Colors.Transparent);
            r8.Fill = new SolidColorBrush(Colors.Transparent);
            r9.Fill = new SolidColorBrush(Colors.Transparent);
            r10.Fill = new SolidColorBrush(Colors.Transparent);
            r11.Fill = new SolidColorBrush(Colors.Transparent);
            r12.Fill = new SolidColorBrush(Colors.Transparent);
            r13.Fill = new SolidColorBrush(Colors.Transparent);
            r14.Fill = new SolidColorBrush(Colors.Transparent);
            r15.Fill = new SolidColorBrush(Colors.Transparent);
            r16.Fill = new SolidColorBrush(Colors.Transparent);
            r17.Fill = new SolidColorBrush(Colors.Transparent);
            r18.Fill = new SolidColorBrush(Colors.Transparent);
            r19.Fill = new SolidColorBrush(Colors.Transparent);
            r20.Fill = new SolidColorBrush(Colors.Transparent);
            r21.Fill = new SolidColorBrush(Colors.Transparent);
            r22.Fill = new SolidColorBrush(Colors.Transparent);
            r23.Fill = new SolidColorBrush(Colors.Transparent);
            r24.Fill = new SolidColorBrush(Colors.Transparent);
            r25.Fill = new SolidColorBrush(Colors.Transparent);
            r26.Fill = new SolidColorBrush(Colors.Transparent);
            r27.Fill = new SolidColorBrush(Colors.Transparent);
            r28.Fill = new SolidColorBrush(Colors.Transparent);
            r29.Fill = new SolidColorBrush(Colors.Transparent);
            r30.Fill = new SolidColorBrush(Colors.Transparent);
            r31.Fill = new SolidColorBrush(Colors.Transparent);
            r32.Fill = new SolidColorBrush(Colors.Transparent);
            r33.Fill = new SolidColorBrush(Colors.Transparent);
            r34.Fill = new SolidColorBrush(Colors.Transparent);
            r35.Fill = new SolidColorBrush(Colors.Transparent);
            r36.Fill = new SolidColorBrush(Colors.Transparent);
            r37.Fill = new SolidColorBrush(Colors.Transparent);
            r38.Fill = new SolidColorBrush(Colors.Transparent);
            r39.Fill = new SolidColorBrush(Colors.Transparent);
            r40.Fill = new SolidColorBrush(Colors.Transparent);
            r41.Fill = new SolidColorBrush(Colors.Transparent);
            r42.Fill = new SolidColorBrush(Colors.Transparent);
            r43.Fill = new SolidColorBrush(Colors.Transparent);
            r44.Fill = new SolidColorBrush(Colors.Transparent);
            r45.Fill = new SolidColorBrush(Colors.Transparent);
            r46.Fill = new SolidColorBrush(Colors.Transparent);
            r47.Fill = new SolidColorBrush(Colors.Transparent);
            r48.Fill = new SolidColorBrush(Colors.Transparent);
            r49.Fill = new SolidColorBrush(Colors.Transparent);
            r50.Fill = new SolidColorBrush(Colors.Transparent);
            r51.Fill = new SolidColorBrush(Colors.Transparent);
            r52.Fill = new SolidColorBrush(Colors.Transparent);
            r53.Fill = new SolidColorBrush(Colors.Transparent);
            r54.Fill = new SolidColorBrush(Colors.Transparent);
            r55.Fill = new SolidColorBrush(Colors.Transparent);
            r56.Fill = new SolidColorBrush(Colors.Transparent);
            r57.Fill = new SolidColorBrush(Colors.Transparent);
            r58.Fill = new SolidColorBrush(Colors.Transparent);
            r59.Fill = new SolidColorBrush(Colors.Transparent);
            r60.Fill = new SolidColorBrush(Colors.Transparent);
            r61.Fill = new SolidColorBrush(Colors.Transparent);
            r62.Fill = new SolidColorBrush(Colors.Transparent);
            r63.Fill = new SolidColorBrush(Colors.Transparent);
            r64.Fill = new SolidColorBrush(Colors.Transparent);
            r65.Fill = new SolidColorBrush(Colors.Transparent);
            r66.Fill = new SolidColorBrush(Colors.Transparent);
            r67.Fill = new SolidColorBrush(Colors.Transparent);
            r68.Fill = new SolidColorBrush(Colors.Transparent);
            r69.Fill = new SolidColorBrush(Colors.Transparent);
            r70.Fill = new SolidColorBrush(Colors.Transparent);
            r71.Fill = new SolidColorBrush(Colors.Transparent);
            r72.Fill = new SolidColorBrush(Colors.Transparent);
            r73.Fill = new SolidColorBrush(Colors.Transparent);
            r74.Fill = new SolidColorBrush(Colors.Transparent);
            r75.Fill = new SolidColorBrush(Colors.Transparent);
            r76.Fill = new SolidColorBrush(Colors.Transparent);
            r77.Fill = new SolidColorBrush(Colors.Transparent);
            r78.Fill = new SolidColorBrush(Colors.Transparent);
            r79.Fill = new SolidColorBrush(Colors.Transparent);
            r80.Fill = new SolidColorBrush(Colors.Transparent);
            r81.Fill = new SolidColorBrush(Colors.Transparent);
            r82.Fill = new SolidColorBrush(Colors.Transparent);
            r83.Fill = new SolidColorBrush(Colors.Transparent);
            r84.Fill = new SolidColorBrush(Colors.Transparent);
            r85.Fill = new SolidColorBrush(Colors.Transparent);
            r86.Fill = new SolidColorBrush(Colors.Transparent);
            r87.Fill = new SolidColorBrush(Colors.Transparent);
            r88.Fill = new SolidColorBrush(Colors.Transparent);
            r89.Fill = new SolidColorBrush(Colors.Transparent);
            r90.Fill = new SolidColorBrush(Colors.Transparent);
            r91.Fill = new SolidColorBrush(Colors.Transparent);
            r92.Fill = new SolidColorBrush(Colors.Transparent);
            r93.Fill = new SolidColorBrush(Colors.Transparent);
            r94.Fill = new SolidColorBrush(Colors.Transparent);
            r95.Fill = new SolidColorBrush(Colors.Transparent);
            r96.Fill = new SolidColorBrush(Colors.Transparent);
            r97.Fill = new SolidColorBrush(Colors.Transparent);
            r98.Fill = new SolidColorBrush(Colors.Transparent);
            r99.Fill = new SolidColorBrush(Colors.Transparent);
            r100.Fill = new SolidColorBrush(Colors.Transparent);
            r101.Fill = new SolidColorBrush(Colors.Transparent);
            r102.Fill = new SolidColorBrush(Colors.Transparent);
            r103.Fill = new SolidColorBrush(Colors.Transparent);
            r104.Fill = new SolidColorBrush(Colors.Transparent);
            r105.Fill = new SolidColorBrush(Colors.Transparent);
            r106.Fill = new SolidColorBrush(Colors.Transparent);
            r107.Fill = new SolidColorBrush(Colors.Transparent);
            r108.Fill = new SolidColorBrush(Colors.Transparent);
            r109.Fill = new SolidColorBrush(Colors.Transparent);
            r110.Fill = new SolidColorBrush(Colors.Transparent);
            r111.Fill = new SolidColorBrush(Colors.Transparent);
            r112.Fill = new SolidColorBrush(Colors.Transparent);
            r113.Fill = new SolidColorBrush(Colors.Transparent);
            r114.Fill = new SolidColorBrush(Colors.Transparent);
            r115.Fill = new SolidColorBrush(Colors.Transparent);
            r116.Fill = new SolidColorBrush(Colors.Transparent);
            r117.Fill = new SolidColorBrush(Colors.Transparent);
            r118.Fill = new SolidColorBrush(Colors.Transparent);
            r119.Fill = new SolidColorBrush(Colors.Transparent);
            r120.Fill = new SolidColorBrush(Colors.Transparent);
            r121.Fill = new SolidColorBrush(Colors.Transparent);
            r122.Fill = new SolidColorBrush(Colors.Transparent);
            r123.Fill = new SolidColorBrush(Colors.Transparent);
            r124.Fill = new SolidColorBrush(Colors.Transparent);
            r125.Fill = new SolidColorBrush(Colors.Transparent);
            r126.Fill = new SolidColorBrush(Colors.Transparent);
            r127.Fill = new SolidColorBrush(Colors.Transparent);
            r128.Fill = new SolidColorBrush(Colors.Transparent);
            r129.Fill = new SolidColorBrush(Colors.Transparent);
            r130.Fill = new SolidColorBrush(Colors.Transparent);
            r131.Fill = new SolidColorBrush(Colors.Transparent);
            r132.Fill = new SolidColorBrush(Colors.Transparent);
            r133.Fill = new SolidColorBrush(Colors.Transparent);
            r134.Fill = new SolidColorBrush(Colors.Transparent);
            r135.Fill = new SolidColorBrush(Colors.Transparent);
            r136.Fill = new SolidColorBrush(Colors.Transparent);
            r137.Fill = new SolidColorBrush(Colors.Transparent);
            r138.Fill = new SolidColorBrush(Colors.Transparent);
            r139.Fill = new SolidColorBrush(Colors.Transparent);
            r140.Fill = new SolidColorBrush(Colors.Transparent);
            r141.Fill = new SolidColorBrush(Colors.Transparent);
            r142.Fill = new SolidColorBrush(Colors.Transparent);
            r143.Fill = new SolidColorBrush(Colors.Transparent);
            r144.Fill = new SolidColorBrush(Colors.Transparent);
            r145.Fill = new SolidColorBrush(Colors.Transparent);
            r146.Fill = new SolidColorBrush(Colors.Transparent);
            r147.Fill = new SolidColorBrush(Colors.Transparent);
            r148.Fill = new SolidColorBrush(Colors.Transparent);
            r149.Fill = new SolidColorBrush(Colors.Transparent);
            r150.Fill = new SolidColorBrush(Colors.Transparent);
            r151.Fill = new SolidColorBrush(Colors.Transparent);
            r152.Fill = new SolidColorBrush(Colors.Transparent);
            r153.Fill = new SolidColorBrush(Colors.Transparent);
            r154.Fill = new SolidColorBrush(Colors.Transparent);
            r155.Fill = new SolidColorBrush(Colors.Transparent);
            r156.Fill = new SolidColorBrush(Colors.Transparent);
            r157.Fill = new SolidColorBrush(Colors.Transparent);
            r158.Fill = new SolidColorBrush(Colors.Transparent);
            r159.Fill = new SolidColorBrush(Colors.Transparent);
            r160.Fill = new SolidColorBrush(Colors.Transparent);
            r161.Fill = new SolidColorBrush(Colors.Transparent);
            r162.Fill = new SolidColorBrush(Colors.Transparent);
            r163.Fill = new SolidColorBrush(Colors.Transparent);
            r164.Fill = new SolidColorBrush(Colors.Transparent);
            r165.Fill = new SolidColorBrush(Colors.Transparent);
            r166.Fill = new SolidColorBrush(Colors.Transparent);
            r167.Fill = new SolidColorBrush(Colors.Transparent);
            r168.Fill = new SolidColorBrush(Colors.Transparent);
            r169.Fill = new SolidColorBrush(Colors.Transparent);
            r170.Fill = new SolidColorBrush(Colors.Transparent);
            r171.Fill = new SolidColorBrush(Colors.Transparent);
            r172.Fill = new SolidColorBrush(Colors.Transparent);
            r173.Fill = new SolidColorBrush(Colors.Transparent);
            r174.Fill = new SolidColorBrush(Colors.Transparent);
            r175.Fill = new SolidColorBrush(Colors.Transparent);
            r176.Fill = new SolidColorBrush(Colors.Transparent);
            r177.Fill = new SolidColorBrush(Colors.Transparent);
            r178.Fill = new SolidColorBrush(Colors.Transparent);
            r179.Fill = new SolidColorBrush(Colors.Transparent);
            r180.Fill = new SolidColorBrush(Colors.Transparent);
            r181.Fill = new SolidColorBrush(Colors.Transparent);
            r182.Fill = new SolidColorBrush(Colors.Transparent);
            r183.Fill = new SolidColorBrush(Colors.Transparent);
            r184.Fill = new SolidColorBrush(Colors.Transparent);
            r185.Fill = new SolidColorBrush(Colors.Transparent);
            r186.Fill = new SolidColorBrush(Colors.Transparent);
            r187.Fill = new SolidColorBrush(Colors.Transparent);
            r188.Fill = new SolidColorBrush(Colors.Transparent);
            r189.Fill = new SolidColorBrush(Colors.Transparent);
            r190.Fill = new SolidColorBrush(Colors.Transparent);
            r191.Fill = new SolidColorBrush(Colors.Transparent);
            r192.Fill = new SolidColorBrush(Colors.Transparent);
            r193.Fill = new SolidColorBrush(Colors.Transparent);
            r194.Fill = new SolidColorBrush(Colors.Transparent);
            r195.Fill = new SolidColorBrush(Colors.Transparent);
            r196.Fill = new SolidColorBrush(Colors.Transparent);
            r197.Fill = new SolidColorBrush(Colors.Transparent);
            r198.Fill = new SolidColorBrush(Colors.Transparent);
            r199.Fill = new SolidColorBrush(Colors.Transparent);
            r200.Fill = new SolidColorBrush(Colors.Transparent);
            r201.Fill = new SolidColorBrush(Colors.Transparent);
            r202.Fill = new SolidColorBrush(Colors.Transparent);
            r203.Fill = new SolidColorBrush(Colors.Transparent);
            r204.Fill = new SolidColorBrush(Colors.Transparent);
            r205.Fill = new SolidColorBrush(Colors.Transparent);
            r206.Fill = new SolidColorBrush(Colors.Transparent);
            r207.Fill = new SolidColorBrush(Colors.Transparent);
            r208.Fill = new SolidColorBrush(Colors.Transparent);
            r209.Fill = new SolidColorBrush(Colors.Transparent);
            r210.Fill = new SolidColorBrush(Colors.Transparent);
            r211.Fill = new SolidColorBrush(Colors.Transparent);
            r212.Fill = new SolidColorBrush(Colors.Transparent);
            r213.Fill = new SolidColorBrush(Colors.Transparent);
            r214.Fill = new SolidColorBrush(Colors.Transparent);
            r215.Fill = new SolidColorBrush(Colors.Transparent);
            r216.Fill = new SolidColorBrush(Colors.Transparent);
            r217.Fill = new SolidColorBrush(Colors.Transparent);
            r218.Fill = new SolidColorBrush(Colors.Transparent);
            r219.Fill = new SolidColorBrush(Colors.Transparent);
            r220.Fill = new SolidColorBrush(Colors.Transparent);
            r221.Fill = new SolidColorBrush(Colors.Transparent);
            r222.Fill = new SolidColorBrush(Colors.Transparent);
            r223.Fill = new SolidColorBrush(Colors.Transparent);
            r224.Fill = new SolidColorBrush(Colors.Transparent);
            r225.Fill = new SolidColorBrush(Colors.Transparent);
            r226.Fill = new SolidColorBrush(Colors.Transparent);
            r227.Fill = new SolidColorBrush(Colors.Transparent);
            r228.Fill = new SolidColorBrush(Colors.Transparent);
            r229.Fill = new SolidColorBrush(Colors.Transparent);
            r230.Fill = new SolidColorBrush(Colors.Transparent);
            r231.Fill = new SolidColorBrush(Colors.Transparent);
            r232.Fill = new SolidColorBrush(Colors.Transparent);
            r233.Fill = new SolidColorBrush(Colors.Transparent);
            r234.Fill = new SolidColorBrush(Colors.Transparent);
            r235.Fill = new SolidColorBrush(Colors.Transparent);
            r236.Fill = new SolidColorBrush(Colors.Transparent);
            r237.Fill = new SolidColorBrush(Colors.Transparent);
            r238.Fill = new SolidColorBrush(Colors.Transparent);
            r239.Fill = new SolidColorBrush(Colors.Transparent);
            r240.Fill = new SolidColorBrush(Colors.Transparent);
            r241.Fill = new SolidColorBrush(Colors.Transparent);
            r242.Fill = new SolidColorBrush(Colors.Transparent);
            r243.Fill = new SolidColorBrush(Colors.Transparent);
            r244.Fill = new SolidColorBrush(Colors.Transparent);
            r245.Fill = new SolidColorBrush(Colors.Transparent);
            r246.Fill = new SolidColorBrush(Colors.Transparent);
            r247.Fill = new SolidColorBrush(Colors.Transparent);
            r248.Fill = new SolidColorBrush(Colors.Transparent);
            r249.Fill = new SolidColorBrush(Colors.Transparent);
            r250.Fill = new SolidColorBrush(Colors.Transparent);
            r251.Fill = new SolidColorBrush(Colors.Transparent);
            r252.Fill = new SolidColorBrush(Colors.Transparent);
            r253.Fill = new SolidColorBrush(Colors.Transparent);
            r254.Fill = new SolidColorBrush(Colors.Transparent);
            r255.Fill = new SolidColorBrush(Colors.Transparent);
            r256.Fill = new SolidColorBrush(Colors.Transparent);
            r257.Fill = new SolidColorBrush(Colors.Transparent);
            r258.Fill = new SolidColorBrush(Colors.Transparent);
            r259.Fill = new SolidColorBrush(Colors.Transparent);
            r260.Fill = new SolidColorBrush(Colors.Transparent);
            r261.Fill = new SolidColorBrush(Colors.Transparent);
            r262.Fill = new SolidColorBrush(Colors.Transparent);
            r263.Fill = new SolidColorBrush(Colors.Transparent);
            r264.Fill = new SolidColorBrush(Colors.Transparent);
            r265.Fill = new SolidColorBrush(Colors.Transparent);
            r266.Fill = new SolidColorBrush(Colors.Transparent);
            r267.Fill = new SolidColorBrush(Colors.Transparent);
            r268.Fill = new SolidColorBrush(Colors.Transparent);
            r269.Fill = new SolidColorBrush(Colors.Transparent);
            r270.Fill = new SolidColorBrush(Colors.Transparent);
            r271.Fill = new SolidColorBrush(Colors.Transparent);
            r272.Fill = new SolidColorBrush(Colors.Transparent);
            r273.Fill = new SolidColorBrush(Colors.Transparent);
            r274.Fill = new SolidColorBrush(Colors.Transparent);
            r275.Fill = new SolidColorBrush(Colors.Transparent);
            r276.Fill = new SolidColorBrush(Colors.Transparent);
            r277.Fill = new SolidColorBrush(Colors.Transparent);
            r278.Fill = new SolidColorBrush(Colors.Transparent);
            r279.Fill = new SolidColorBrush(Colors.Transparent);
            r280.Fill = new SolidColorBrush(Colors.Transparent);
            r281.Fill = new SolidColorBrush(Colors.Transparent);
            r282.Fill = new SolidColorBrush(Colors.Transparent);
            r283.Fill = new SolidColorBrush(Colors.Transparent);
            r284.Fill = new SolidColorBrush(Colors.Transparent);
            r285.Fill = new SolidColorBrush(Colors.Transparent);
            r286.Fill = new SolidColorBrush(Colors.Transparent);
            r287.Fill = new SolidColorBrush(Colors.Transparent);
            r288.Fill = new SolidColorBrush(Colors.Transparent);
            r289.Fill = new SolidColorBrush(Colors.Transparent);
            r290.Fill = new SolidColorBrush(Colors.Transparent);
            r291.Fill = new SolidColorBrush(Colors.Transparent);
            r292.Fill = new SolidColorBrush(Colors.Transparent);
            r293.Fill = new SolidColorBrush(Colors.Transparent);
            r294.Fill = new SolidColorBrush(Colors.Transparent);
            r295.Fill = new SolidColorBrush(Colors.Transparent);
            r296.Fill = new SolidColorBrush(Colors.Transparent);
            r297.Fill = new SolidColorBrush(Colors.Transparent);
            r298.Fill = new SolidColorBrush(Colors.Transparent);
            r299.Fill = new SolidColorBrush(Colors.Transparent);
            r300.Fill = new SolidColorBrush(Colors.Transparent);
            r301.Fill = new SolidColorBrush(Colors.Transparent);
            r302.Fill = new SolidColorBrush(Colors.Transparent);
            r303.Fill = new SolidColorBrush(Colors.Transparent);
            r304.Fill = new SolidColorBrush(Colors.Transparent);
            r305.Fill = new SolidColorBrush(Colors.Transparent);
            r306.Fill = new SolidColorBrush(Colors.Transparent);
            r307.Fill = new SolidColorBrush(Colors.Transparent);
            r308.Fill = new SolidColorBrush(Colors.Transparent);
            r309.Fill = new SolidColorBrush(Colors.Transparent);
            r310.Fill = new SolidColorBrush(Colors.Transparent);
            r311.Fill = new SolidColorBrush(Colors.Transparent);
            r312.Fill = new SolidColorBrush(Colors.Transparent);
            r313.Fill = new SolidColorBrush(Colors.Transparent);
            r314.Fill = new SolidColorBrush(Colors.Transparent);
            r315.Fill = new SolidColorBrush(Colors.Transparent);
            r316.Fill = new SolidColorBrush(Colors.Transparent);
            r317.Fill = new SolidColorBrush(Colors.Transparent);
            r318.Fill = new SolidColorBrush(Colors.Transparent);
            r319.Fill = new SolidColorBrush(Colors.Transparent);
            r320.Fill = new SolidColorBrush(Colors.Transparent);
            r321.Fill = new SolidColorBrush(Colors.Transparent);
            r322.Fill = new SolidColorBrush(Colors.Transparent);
            r323.Fill = new SolidColorBrush(Colors.Transparent);
            r324.Fill = new SolidColorBrush(Colors.Transparent);
            r325.Fill = new SolidColorBrush(Colors.Transparent);
            r326.Fill = new SolidColorBrush(Colors.Transparent);
            r327.Fill = new SolidColorBrush(Colors.Transparent);
            r328.Fill = new SolidColorBrush(Colors.Transparent);
            r329.Fill = new SolidColorBrush(Colors.Transparent);
            r330.Fill = new SolidColorBrush(Colors.Transparent);
            r331.Fill = new SolidColorBrush(Colors.Transparent);
            r332.Fill = new SolidColorBrush(Colors.Transparent);
            r333.Fill = new SolidColorBrush(Colors.Transparent);
            r334.Fill = new SolidColorBrush(Colors.Transparent);
            r335.Fill = new SolidColorBrush(Colors.Transparent);
            t0.Text = "";
            t1.Text = "";
            t2.Text = "";
            t3.Text = "";
            t4.Text = "";
            t5.Text = "";
            t6.Text = "";
            t7.Text = "";
            t8.Text = "";
            t9.Text = "";
            t10.Text = "";
            t11.Text = "";
            t12.Text = "";
            t13.Text = "";
            t14.Text = "";
            t15.Text = "";
            t16.Text = "";
            t17.Text = "";
            t18.Text = "";
            t19.Text = "";
            t20.Text = "";
            t21.Text = "";
            t22.Text = "";
            t23.Text = "";
            t24.Text = "";
            t25.Text = "";
            t26.Text = "";
            t27.Text = "";
            t28.Text = "";
            t29.Text = "";
            t30.Text = "";
            t31.Text = "";
            t32.Text = "";
            t33.Text = "";
            t34.Text = "";
            t35.Text = "";
            t36.Text = "";
            t37.Text = "";
            t38.Text = "";
            t39.Text = "";
            t40.Text = "";
            t41.Text = "";
            t42.Text = "";
            t43.Text = "";
            t44.Text = "";
            t45.Text = "";
            t46.Text = "";
            t47.Text = "";
            t48.Text = "";
            t49.Text = "";
            t50.Text = "";
            t51.Text = "";
            t52.Text = "";
            t53.Text = "";
            t54.Text = "";
            t55.Text = "";
            t56.Text = "";
            t57.Text = "";
            t58.Text = "";
            t59.Text = "";
            t60.Text = "";
            t61.Text = "";
            t62.Text = "";
            t63.Text = "";
            t64.Text = "";
            t65.Text = "";
            t66.Text = "";
            t67.Text = "";
            t68.Text = "";
            t69.Text = "";
            t70.Text = "";
            t71.Text = "";
            t72.Text = "";
            t73.Text = "";
            t74.Text = "";
            t75.Text = "";
            t76.Text = "";
            t77.Text = "";
            t78.Text = "";
            t79.Text = "";
            t80.Text = "";
            t81.Text = "";
            t82.Text = "";
            t83.Text = "";
            t84.Text = "";
            t85.Text = "";
            t86.Text = "";
            t87.Text = "";
            t88.Text = "";
            t89.Text = "";
            t90.Text = "";
            t91.Text = "";
            t92.Text = "";
            t93.Text = "";
            t94.Text = "";
            t95.Text = "";
            t96.Text = "";
            t97.Text = "";
            t98.Text = "";
            t99.Text = "";
            t100.Text = "";
            t101.Text = "";
            t102.Text = "";
            t103.Text = "";
            t104.Text = "";
            t105.Text = "";
            t106.Text = "";
            t107.Text = "";
            t108.Text = "";
            t109.Text = "";
            t110.Text = "";
            t111.Text = "";
            t112.Text = "";
            t113.Text = "";
            t114.Text = "";
            t115.Text = "";
            t116.Text = "";
            t117.Text = "";
            t118.Text = "";
            t119.Text = "";
            t120.Text = "";
            t121.Text = "";
            t122.Text = "";
            t123.Text = "";
            t124.Text = "";
            t125.Text = "";
            t126.Text = "";
            t127.Text = "";
            t128.Text = "";
            t129.Text = "";
            t130.Text = "";
            t131.Text = "";
            t132.Text = "";
            t133.Text = "";
            t134.Text = "";
            t135.Text = "";
            t136.Text = "";
            t137.Text = "";
            t138.Text = "";
            t139.Text = "";
            t140.Text = "";
            t141.Text = "";
            t142.Text = "";
            t143.Text = "";
            t144.Text = "";
            t145.Text = "";
            t146.Text = "";
            t147.Text = "";
            t148.Text = "";
            t149.Text = "";
            t150.Text = "";
            t151.Text = "";
            t152.Text = "";
            t153.Text = "";
            t154.Text = "";
            t155.Text = "";
            t156.Text = "";
            t157.Text = "";
            t158.Text = "";
            t159.Text = "";
            t160.Text = "";
            t161.Text = "";
            t162.Text = "";
            t163.Text = "";
            t164.Text = "";
            t165.Text = "";
            t166.Text = "";
            t167.Text = "";
            t168.Text = "";
            t169.Text = "";
            t170.Text = "";
            t171.Text = "";
            t172.Text = "";
            t173.Text = "";
            t174.Text = "";
            t175.Text = "";
            t176.Text = "";
            t177.Text = "";
            t178.Text = "";
            t179.Text = "";
            t180.Text = "";
            t181.Text = "";
            t182.Text = "";
            t183.Text = "";
            t184.Text = "";
            t185.Text = "";
            t186.Text = "";
            t187.Text = "";
            t188.Text = "";
            t189.Text = "";
            t190.Text = "";
            t191.Text = "";
            t192.Text = "";
            t193.Text = "";
            t194.Text = "";
            t195.Text = "";
            t196.Text = "";
            t197.Text = "";
            t198.Text = "";
            t199.Text = "";
            t200.Text = "";
            t201.Text = "";
            t202.Text = "";
            t203.Text = "";
            t204.Text = "";
            t205.Text = "";
            t206.Text = "";
            t207.Text = "";
            t208.Text = "";
            t209.Text = "";
            t210.Text = "";
            t211.Text = "";
            t212.Text = "";
            t213.Text = "";
            t214.Text = "";
            t215.Text = "";
            t216.Text = "";
            t217.Text = "";
            t218.Text = "";
            t219.Text = "";
            t220.Text = "";
            t221.Text = "";
            t222.Text = "";
            t223.Text = "";
            t224.Text = "";
            t225.Text = "";
            t226.Text = "";
            t227.Text = "";
            t228.Text = "";
            t229.Text = "";
            t230.Text = "";
            t231.Text = "";
            t232.Text = "";
            t233.Text = "";
            t234.Text = "";
            t235.Text = "";
            t236.Text = "";
            t237.Text = "";
            t238.Text = "";
            t239.Text = "";
            t240.Text = "";
            t241.Text = "";
            t242.Text = "";
            t243.Text = "";
            t244.Text = "";
            t245.Text = "";
            t246.Text = "";
            t247.Text = "";
            t248.Text = "";
            t249.Text = "";
            t250.Text = "";
            t251.Text = "";
            t252.Text = "";
            t253.Text = "";
            t254.Text = "";
            t255.Text = "";
            t256.Text = "";
            t257.Text = "";
            t258.Text = "";
            t259.Text = "";
            t260.Text = "";
            t261.Text = "";
            t262.Text = "";
            t263.Text = "";
            t264.Text = "";
            t265.Text = "";
            t266.Text = "";
            t267.Text = "";
            t268.Text = "";
            t269.Text = "";
            t270.Text = "";
            t271.Text = "";
            t272.Text = "";
            t273.Text = "";
            t274.Text = "";
            t275.Text = "";
            t276.Text = "";
            t277.Text = "";
            t278.Text = "";
            t279.Text = "";
            t280.Text = "";
            t281.Text = "";
            t282.Text = "";
            t283.Text = "";
            t284.Text = "";
            t285.Text = "";
            t286.Text = "";
            t287.Text = "";
            t288.Text = "";
            t289.Text = "";
            t290.Text = "";
            t291.Text = "";
            t292.Text = "";
            t293.Text = "";
            t294.Text = "";
            t295.Text = "";
            t296.Text = "";
            t297.Text = "";
            t298.Text = "";
            t299.Text = "";
            t300.Text = "";
            t301.Text = "";
            t302.Text = "";
            t303.Text = "";
            t304.Text = "";
            t305.Text = "";
            t306.Text = "";
            t307.Text = "";
            t308.Text = "";
            t309.Text = "";
            t310.Text = "";
            t311.Text = "";
            t312.Text = "";
            t313.Text = "";
            t314.Text = "";
            t315.Text = "";
            t316.Text = "";
            t317.Text = "";
            t318.Text = "";
            t319.Text = "";
            t320.Text = "";
            t321.Text = "";
            t322.Text = "";
            t323.Text = "";
            t324.Text = "";
            t325.Text = "";
            t326.Text = "";
            t327.Text = "";
            t328.Text = "";
            t329.Text = "";
            t330.Text = "";
            t331.Text = "";
            t332.Text = "";
            t333.Text = "";
            t334.Text = "";
            t335.Text = "";
        }

    }
}
