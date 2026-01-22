using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pr_WPF
{
    /// <summary>
    /// Логика взаимодействия для DrawingPage.xaml
    /// </summary>
    public partial class DrawingPage : Page
    {
        private Line _line;
 private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
 {
     if (e.LeftButton == MouseButtonState.Pressed) { 
         _line = new Line();
         _line.Stroke = Brushes.Black;
         _line.StrokeThickness = 2;
         Point p = e.GetPosition(DrowingCanvas);
         _line.X1 = p.X;
         _line.Y1 = p.Y;
         _line.X2 = p.X;
         _line.Y2 = p.Y;
         DrowingCanvas.Children.Add(_line);
     }
 }

 private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
 {
     if(e.LeftButton == MouseButtonState.Pressed && _line!=null)
     {
         Point finalPoint = e.GetPosition(DrowingCanvas);
         _line.X2 = finalPoint.X;
         _line.Y2 = finalPoint.Y;
     }
 }

 private void DrawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
 {
     _line = null;
 }

        public DrawingPage()
        {
            InitializeComponent();
        }

        private void Click_Prev_Menu(object sender, RoutedEventArgs e)
        {
            MainWindow.mein_window.MainFrame.Navigate(new MainPage());
        }
    }
}
