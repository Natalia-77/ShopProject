using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfClient
{
    /// <summary>
    /// Логика взаимодействия для BasketWindow.xaml
    /// </summary>
    public partial class BasketWindow : Window
    {
        public List<OrderList> OrdList { get; set; }
        public BasketWindow(List<OrderList> ol)
        {
            InitializeComponent();
     
            OrdList = new List<OrderList>(ol);
           
            dgOrder.ItemsSource = OrdList;
        }

        private void btnMakeOrder_Click(object sender, RoutedEventArgs e)
        {
            MakeOrderWindow mow = new MakeOrderWindow();
            mow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mow.Owner = this;
            mow.ShowDialog();
        }
    }
}
