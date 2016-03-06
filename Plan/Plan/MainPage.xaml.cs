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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Plan
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.InitializeDate();   
        }

        private void InitializeDate()
        {
            DateTime CurrentTime = DateTime.Now;
            int year = CurrentTime.Year;
            int month = CurrentTime.Month;
            int day = CurrentTime.Day;
            int hour = CurrentTime.Hour;
            int min = CurrentTime.Minute;
            int sec = CurrentTime.Second;
            string date = CurrentTime.DayOfWeek.ToString();
            
            // set month on top left
            string monthStr = DateTime.Now.ToString("MMMM");
            string yearStr = year.ToString();
            MonthText.Text = monthStr + " " + yearStr;
        }

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
            Color color = Color.FromArgb(255, 242, 242, 242);
            EventText.Background  = new SolidColorBrush(Colors.White);
            // EventText.BorderBrush = new SolidColorBrush(Colors.Transparent);
            // EventText.BorderThickness = new Thickness(1);
        }
        private void EventCanvas_Tapped(object sender, RoutedEventArgs e)
        {
            EventText.Focus(FocusState.Programmatic);
        }
        
        private void EventCanvas_DragEnter(object sender, RoutedEventArgs e)
        {
            EventText.Focus(FocusState.Unfocused);
        }

        



    }
}
