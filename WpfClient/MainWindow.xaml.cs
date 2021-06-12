using Newtonsoft.Json;
using ShopProject.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
using WpfClient.Models;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public Products orders { get; set; }
        public List<Products> ord { get; set; }
        public float summ_order { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            ord = new List<Products>();
            var app = App.Current as IGetConfiguration;
            var serverUrl = app.Configuration.GetSection("ServerUrl").Value;

            using (WebClient client = new WebClient())
            {
                client.DownloadDataCompleted += asyncWeb_DownloadDataCompleted;
                Uri url = new Uri($"{serverUrl}Account/show");
                client.DownloadDataAsync(url);
            }
           
        }


        public void asyncWeb_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            try
            {
                string res = Encoding.Default.GetString(e.Result);
                var list = JsonConvert.DeserializeObject<List<Products>>(res);
                productList.ItemsSource = list;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().ToString());
            }

        }

        private void productList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Products prod = (Products)productList.SelectedItem;
            tbName.Text = prod.Name;
            tbPrice.Text = prod.Price.ToString();
            tbDescription.Text = prod.Description;
            Uri fileUri = new Uri(prod.Image);
            imgPhoto.Source = new BitmapImage(fileUri);
            orders = prod;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void btnContact_Click(object sender, RoutedEventArgs e)
        {
            ContactUsWindow cuw = new ContactUsWindow();
            cuw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            cuw.Owner = this;
            cuw.ShowDialog();
        }

        private void btnBasket_Click(object sender, RoutedEventArgs e)
        {
            BasketWindow bw = new BasketWindow(ord, summ_order);
            bw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            bw.Owner = this;
            bw.ShowDialog();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            loginWindow.Owner = this;
            loginWindow.ShowDialog();
        }

        private void btnAddToBasket_Click(object sender, RoutedEventArgs e)
        {
            ord.Add(orders);
            summ_order += orders.Price;
        }
    }
}
