using ShopProject.Entities;
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
        public List<Products> _products { get; set; }
        public float _sum { get; set; }
        public BasketWindow(List<Products> products, float sum)
        {
            InitializeComponent();
            _products = products;
            _sum = sum;
            dgOrder.ItemsSource = _products;
            tbSumm.Text = sum.ToString();

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
