using Newtonsoft.Json;
using ShopProject.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfClient.Models;

namespace WpfClient
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        //private EFContext _context = new EFContext();
       // private ObservableCollection<Products> _products = new ObservableCollection<Products>();
        //private string  _token { get; set; }
        public AdminWindow( )
        {
            InitializeComponent();
           // _token = token;
            var app = Application.Current as IGetConfiguration;
            var serverUrl = app.Configuration.GetSection("ServerUrl").Value;

            using (WebClient client = new WebClient())
            {
                client.DownloadDataCompleted += asyncWeb_DownloadDataCompleted;
                Uri url = new Uri($"{serverUrl}Account/show");
                client.DownloadDataAsync(url);
            }

            //============check============
           // MessageBox.Show(_token);
            //=============================


            //var list = _context.Products.AsQueryable()
            //    .Select(x => new Products()
            //    {
            //        Id = x.Id,
            //        Name = x.Name,
            //        Description = x.Description,
            //        Price = x.Price
            //    })
            //    .ToList();
            //_products = new ObservableCollection<Products>(list);
            //dgSimple.ItemsSource = _products;
        }

        public void asyncWeb_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            try
            {
                string res = Encoding.Default.GetString(e.Result);
                var list = JsonConvert.DeserializeObject<List<Products>>(res);
                dgSimple.ItemsSource = list;
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().ToString());
            }

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow add= new AddProductWindow();
            add.ShowDialog();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditProductWindow edit = new EditProductWindow();
            edit.ShowDialog();
        }
    }
}
