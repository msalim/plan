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
        public static string ResultText {get; set;}
        public static int SaveYear {get; set;}
        public static int SaveMonth {get; set;}
        public static int SaveDay {get; set;}

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
            InitializeDate();
        }
        private void PreviousWeek_Click(object sender, RoutedEventArgs e)
        {
            Offset = Offset-7;
            InitializeDate(); 
        }

        private void ProModeButton_Click(object sender, RoutedEventArgs e)
        {
            ProMode = !ProMode;
            InitializeProMode();
        }
        
 // ink class https://channel9.msdn.com/Shows/Inside-Windows-Platform/Leverage-Inking-in-your-UWP-Apps

        private void Rectangle_Tapped(object sender, RoutedEventArgs e)
        {
            // Rectangle rectangle = sender as Rectangle;
            // if (rectangle.Fill == new SolidColorBrush(Colors.LightGray)) rectangle.Fill = new SolidColorBrush(Colors.Red);
            // rectangle.Fill = new SolidColorBrush(Colors.LightGray);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {   
            SetTime = false;
            ResultText = "";
            this.CalendarInk.InkPresenter.StrokeContainer.Clear();
            EventText.Text = "Cancelled.";
        }

        private void Refresh()
        {   
            SetTime = false;
            ResultText = "";
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

        private void addTextbox(string eventName, int first)
        {
            if(i=0) t0.Text = eventName;
            else if(i=1) t1.Text = eventName;
            else if(i=2) t2.Text = eventName;
            else if(i=3) t3.Text = eventName;
            else if(i=4) t4.Text = eventName;
            else if(i=5) t5.Text = eventName;
            else if(i=6) t6.Text = eventName;
            else if(i=7) t7.Text = eventName;
            else if(i=8) t8.Text = eventName;
            else if(i=9) t9.Text = eventName;
            else if(i=10) t10.Text = eventName;
            else if(i=11) t11.Text = eventName;
            else if(i=12) t12.Text = eventName;
            else if(i=13) t13.Text = eventName;
            else if(i=14) t14.Text = eventName;
            else if(i=15) t15.Text = eventName;
            else if(i=16) t16.Text = eventName;
            else if(i=17) t17.Text = eventName;
            else if(i=18) t18.Text = eventName;
            else if(i=19) t19.Text = eventName;
            else if(i=20) t20.Text = eventName;
            else if(i=21) t21.Text = eventName;
            else if(i=22) t22.Text = eventName;
            else if(i=23) t23.Text = eventName;
            else if(i=24) t24.Text = eventName;
            else if(i=25) t25.Text = eventName;
            else if(i=26) t26.Text = eventName;
            else if(i=27) t27.Text = eventName;
            else if(i=28) t28.Text = eventName;
            else if(i=29) t29.Text = eventName;
            else if(i=30) t30.Text = eventName;
            else if(i=31) t31.Text = eventName;
            else if(i=32) t32.Text = eventName;
            else if(i=33) t33.Text = eventName;
            else if(i=34) t34.Text = eventName;
            else if(i=35) t35.Text = eventName;
            else if(i=36) t36.Text = eventName;
            else if(i=37) t37.Text = eventName;
            else if(i=38) t38.Text = eventName;
            else if(i=39) t39.Text = eventName;
            else if(i=40) t40.Text = eventName;
            else if(i=41) t41.Text = eventName;
            else if(i=42) t42.Text = eventName;
            else if(i=43) t43.Text = eventName;
            else if(i=44) t44.Text = eventName;
            else if(i=45) t45.Text = eventName;
            else if(i=46) t46.Text = eventName;
            else if(i=47) t47.Text = eventName;
            else if(i=48) t48.Text = eventName;
            else if(i=49) t49.Text = eventName;
            else if(i=50) t50.Text = eventName;
            else if(i=51) t51.Text = eventName;
            else if(i=52) t52.Text = eventName;
            else if(i=53) t53.Text = eventName;
            else if(i=54) t54.Text = eventName;
            else if(i=55) t55.Text = eventName;
            else if(i=56) t56.Text = eventName;
            else if(i=57) t57.Text = eventName;
            else if(i=58) t58.Text = eventName;
            else if(i=59) t59.Text = eventName;
            else if(i=60) t60.Text = eventName;
            else if(i=61) t61.Text = eventName;
            else if(i=62) t62.Text = eventName;
            else if(i=63) t63.Text = eventName;
            else if(i=64) t64.Text = eventName;
            else if(i=65) t65.Text = eventName;
            else if(i=66) t66.Text = eventName;
            else if(i=67) t67.Text = eventName;
            else if(i=68) t68.Text = eventName;
            else if(i=69) t69.Text = eventName;
            else if(i=70) t70.Text = eventName;
            else if(i=71) t71.Text = eventName;
            else if(i=72) t72.Text = eventName;
            else if(i=73) t73.Text = eventName;
            else if(i=74) t74.Text = eventName;
            else if(i=75) t75.Text = eventName;
            else if(i=76) t76.Text = eventName;
            else if(i=77) t77.Text = eventName;
            else if(i=78) t78.Text = eventName;
            else if(i=79) t79.Text = eventName;
            else if(i=80) t80.Text = eventName;
            else if(i=81) t81.Text = eventName;
            else if(i=82) t82.Text = eventName;
            else if(i=83) t83.Text = eventName;
            else if(i=84) t84.Text = eventName;
            else if(i=85) t85.Text = eventName;
            else if(i=86) t86.Text = eventName;
            else if(i=87) t87.Text = eventName;
            else if(i=88) t88.Text = eventName;
            else if(i=89) t89.Text = eventName;
            else if(i=90) t90.Text = eventName;
            else if(i=91) t91.Text = eventName;
            else if(i=92) t92.Text = eventName;
            else if(i=93) t93.Text = eventName;
            else if(i=94) t94.Text = eventName;
            else if(i=95) t95.Text = eventName;
            else if(i=96) t96.Text = eventName;
            else if(i=97) t97.Text = eventName;
            else if(i=98) t98.Text = eventName;
            else if(i=99) t99.Text = eventName;
            else if(i=100) t100.Text = eventName;
            else if(i=101) t101.Text = eventName;
            else if(i=102) t102.Text = eventName;
            else if(i=103) t103.Text = eventName;
            else if(i=104) t104.Text = eventName;
            else if(i=105) t105.Text = eventName;
            else if(i=106) t106.Text = eventName;
            else if(i=107) t107.Text = eventName;
            else if(i=108) t108.Text = eventName;
            else if(i=109) t109.Text = eventName;
            else if(i=110) t110.Text = eventName;
            else if(i=111) t111.Text = eventName;
            else if(i=112) t112.Text = eventName;
            else if(i=113) t113.Text = eventName;
            else if(i=114) t114.Text = eventName;
            else if(i=115) t115.Text = eventName;
            else if(i=116) t116.Text = eventName;
            else if(i=117) t117.Text = eventName;
            else if(i=118) t118.Text = eventName;
            else if(i=119) t119.Text = eventName;
            else if(i=120) t120.Text = eventName;
            else if(i=121) t121.Text = eventName;
            else if(i=122) t122.Text = eventName;
            else if(i=123) t123.Text = eventName;
            else if(i=124) t124.Text = eventName;
            else if(i=125) t125.Text = eventName;
            else if(i=126) t126.Text = eventName;
            else if(i=127) t127.Text = eventName;
            else if(i=128) t128.Text = eventName;
            else if(i=129) t129.Text = eventName;
            else if(i=130) t130.Text = eventName;
            else if(i=131) t131.Text = eventName;
            else if(i=132) t132.Text = eventName;
            else if(i=133) t133.Text = eventName;
            else if(i=134) t134.Text = eventName;
            else if(i=135) t135.Text = eventName;
            else if(i=136) t136.Text = eventName;
            else if(i=137) t137.Text = eventName;
            else if(i=138) t138.Text = eventName;
            else if(i=139) t139.Text = eventName;
            else if(i=140) t140.Text = eventName;
            else if(i=141) t141.Text = eventName;
            else if(i=142) t142.Text = eventName;
            else if(i=143) t143.Text = eventName;
            else if(i=144) t144.Text = eventName;
            else if(i=145) t145.Text = eventName;
            else if(i=146) t146.Text = eventName;
            else if(i=147) t147.Text = eventName;
            else if(i=148) t148.Text = eventName;
            else if(i=149) t149.Text = eventName;
            else if(i=150) t150.Text = eventName;
            else if(i=151) t151.Text = eventName;
            else if(i=152) t152.Text = eventName;
            else if(i=153) t153.Text = eventName;
            else if(i=154) t154.Text = eventName;
            else if(i=155) t155.Text = eventName;
            else if(i=156) t156.Text = eventName;
            else if(i=157) t157.Text = eventName;
            else if(i=158) t158.Text = eventName;
            else if(i=159) t159.Text = eventName;
            else if(i=160) t160.Text = eventName;
            else if(i=161) t161.Text = eventName;
            else if(i=162) t162.Text = eventName;
            else if(i=163) t163.Text = eventName;
            else if(i=164) t164.Text = eventName;
            else if(i=165) t165.Text = eventName;
            else if(i=166) t166.Text = eventName;
            else if(i=167) t167.Text = eventName;
            else if(i=168) t168.Text = eventName;
            else if(i=169) t169.Text = eventName;
            else if(i=170) t170.Text = eventName;
            else if(i=171) t171.Text = eventName;
            else if(i=172) t172.Text = eventName;
            else if(i=173) t173.Text = eventName;
            else if(i=174) t174.Text = eventName;
            else if(i=175) t175.Text = eventName;
            else if(i=176) t176.Text = eventName;
            else if(i=177) t177.Text = eventName;
            else if(i=178) t178.Text = eventName;
            else if(i=179) t179.Text = eventName;
            else if(i=180) t180.Text = eventName;
            else if(i=181) t181.Text = eventName;
            else if(i=182) t182.Text = eventName;
            else if(i=183) t183.Text = eventName;
            else if(i=184) t184.Text = eventName;
            else if(i=185) t185.Text = eventName;
            else if(i=186) t186.Text = eventName;
            else if(i=187) t187.Text = eventName;
            else if(i=188) t188.Text = eventName;
            else if(i=189) t189.Text = eventName;
            else if(i=190) t190.Text = eventName;
            else if(i=191) t191.Text = eventName;
            else if(i=192) t192.Text = eventName;
            else if(i=193) t193.Text = eventName;
            else if(i=194) t194.Text = eventName;
            else if(i=195) t195.Text = eventName;
            else if(i=196) t196.Text = eventName;
            else if(i=197) t197.Text = eventName;
            else if(i=198) t198.Text = eventName;
            else if(i=199) t199.Text = eventName;
            else if(i=200) t200.Text = eventName;
            else if(i=201) t201.Text = eventName;
            else if(i=202) t202.Text = eventName;
            else if(i=203) t203.Text = eventName;
            else if(i=204) t204.Text = eventName;
            else if(i=205) t205.Text = eventName;
            else if(i=206) t206.Text = eventName;
            else if(i=207) t207.Text = eventName;
            else if(i=208) t208.Text = eventName;
            else if(i=209) t209.Text = eventName;
            else if(i=210) t210.Text = eventName;
            else if(i=211) t211.Text = eventName;
            else if(i=212) t212.Text = eventName;
            else if(i=213) t213.Text = eventName;
            else if(i=214) t214.Text = eventName;
            else if(i=215) t215.Text = eventName;
            else if(i=216) t216.Text = eventName;
            else if(i=217) t217.Text = eventName;
            else if(i=218) t218.Text = eventName;
            else if(i=219) t219.Text = eventName;
            else if(i=220) t220.Text = eventName;
            else if(i=221) t221.Text = eventName;
            else if(i=222) t222.Text = eventName;
            else if(i=223) t223.Text = eventName;
            else if(i=224) t224.Text = eventName;
            else if(i=225) t225.Text = eventName;
            else if(i=226) t226.Text = eventName;
            else if(i=227) t227.Text = eventName;
            else if(i=228) t228.Text = eventName;
            else if(i=229) t229.Text = eventName;
            else if(i=230) t230.Text = eventName;
            else if(i=231) t231.Text = eventName;
            else if(i=232) t232.Text = eventName;
            else if(i=233) t233.Text = eventName;
            else if(i=234) t234.Text = eventName;
            else if(i=235) t235.Text = eventName;
            else if(i=236) t236.Text = eventName;
            else if(i=237) t237.Text = eventName;
            else if(i=238) t238.Text = eventName;
            else if(i=239) t239.Text = eventName;
            else if(i=240) t240.Text = eventName;
            else if(i=241) t241.Text = eventName;
            else if(i=242) t242.Text = eventName;
            else if(i=243) t243.Text = eventName;
            else if(i=244) t244.Text = eventName;
            else if(i=245) t245.Text = eventName;
            else if(i=246) t246.Text = eventName;
            else if(i=247) t247.Text = eventName;
            else if(i=248) t248.Text = eventName;
            else if(i=249) t249.Text = eventName;
            else if(i=250) t250.Text = eventName;
            else if(i=251) t251.Text = eventName;
            else if(i=252) t252.Text = eventName;
            else if(i=253) t253.Text = eventName;
            else if(i=254) t254.Text = eventName;
            else if(i=255) t255.Text = eventName;
            else if(i=256) t256.Text = eventName;
            else if(i=257) t257.Text = eventName;
            else if(i=258) t258.Text = eventName;
            else if(i=259) t259.Text = eventName;
            else if(i=260) t260.Text = eventName;
            else if(i=261) t261.Text = eventName;
            else if(i=262) t262.Text = eventName;
            else if(i=263) t263.Text = eventName;
            else if(i=264) t264.Text = eventName;
            else if(i=265) t265.Text = eventName;
            else if(i=266) t266.Text = eventName;
            else if(i=267) t267.Text = eventName;
            else if(i=268) t268.Text = eventName;
            else if(i=269) t269.Text = eventName;
            else if(i=270) t270.Text = eventName;
            else if(i=271) t271.Text = eventName;
            else if(i=272) t272.Text = eventName;
            else if(i=273) t273.Text = eventName;
            else if(i=274) t274.Text = eventName;
            else if(i=275) t275.Text = eventName;
            else if(i=276) t276.Text = eventName;
            else if(i=277) t277.Text = eventName;
            else if(i=278) t278.Text = eventName;
            else if(i=279) t279.Text = eventName;
            else if(i=280) t280.Text = eventName;
            else if(i=281) t281.Text = eventName;
            else if(i=282) t282.Text = eventName;
            else if(i=283) t283.Text = eventName;
            else if(i=284) t284.Text = eventName;
            else if(i=285) t285.Text = eventName;
            else if(i=286) t286.Text = eventName;
            else if(i=287) t287.Text = eventName;
            else if(i=288) t288.Text = eventName;
            else if(i=289) t289.Text = eventName;
            else if(i=290) t290.Text = eventName;
            else if(i=291) t291.Text = eventName;
            else if(i=292) t292.Text = eventName;
            else if(i=293) t293.Text = eventName;
            else if(i=294) t294.Text = eventName;
            else if(i=295) t295.Text = eventName;
            else if(i=296) t296.Text = eventName;
            else if(i=297) t297.Text = eventName;
            else if(i=298) t298.Text = eventName;
            else if(i=299) t299.Text = eventName;
            else if(i=300) t300.Text = eventName;
            else if(i=301) t301.Text = eventName;
            else if(i=302) t302.Text = eventName;
            else if(i=303) t303.Text = eventName;
            else if(i=304) t304.Text = eventName;
            else if(i=305) t305.Text = eventName;
            else if(i=306) t306.Text = eventName;
            else if(i=307) t307.Text = eventName;
            else if(i=308) t308.Text = eventName;
            else if(i=309) t309.Text = eventName;
            else if(i=310) t310.Text = eventName;
            else if(i=311) t311.Text = eventName;
            else if(i=312) t312.Text = eventName;
            else if(i=313) t313.Text = eventName;
            else if(i=314) t314.Text = eventName;
            else if(i=315) t315.Text = eventName;
            else if(i=316) t316.Text = eventName;
            else if(i=317) t317.Text = eventName;
            else if(i=318) t318.Text = eventName;
            else if(i=319) t319.Text = eventName;
            else if(i=320) t320.Text = eventName;
            else if(i=321) t321.Text = eventName;
            else if(i=322) t322.Text = eventName;
            else if(i=323) t323.Text = eventName;
            else if(i=324) t324.Text = eventName;
            else if(i=325) t325.Text = eventName;
            else if(i=326) t326.Text = eventName;
            else if(i=327) t327.Text = eventName;
            else if(i=328) t328.Text = eventName;
            else if(i=329) t329.Text = eventName;
            else if(i=330) t330.Text = eventName;
            else if(i=331) t331.Text = eventName;
            else if(i=332) t332.Text = eventName;
            else if(i=333) t333.Text = eventName;
            else if(i=334) t334.Text = eventName;
            else if(i=335) t335.Text = eventName;
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
                    SetTime = true;
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
                if (left <= ((i/48)*176)+175+20 && ((i/48)*176)+20 <= right && top <= ((i%48)*38)+37+20  && ((i%48)*38)+20 <= bottom)
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
                        endI = i;
                        endDay = i/48;
                        endHourDiv2 = i%48;
                        break;
                    }
            }

            if (startDay == -1)
            {
                EventText.Text = "Please select time again.";
                this.CalendarInk.InkPresenter.StrokeContainer.Clear();
                
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
    }
}
