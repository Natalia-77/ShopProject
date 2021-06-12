using Newtonsoft.Json;
using ShopProject.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
        public string  _token { get; set; }
        public int _id { get; set; }
        public Products prod { get; set; }
        public AdminWindow(string token )
        {
           InitializeComponent();
            _token = token;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            Webrequest();            
        }
        public void Webrequest()
        {
            var app = Application.Current as IGetConfiguration;
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
                dgSimple.ItemsSource = list;
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().ToString());
            }

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow add= new AddProductWindow(_token);
            //MessageBox.Show(_token);
            add.ShowDialog();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Window_Loaded(sender, e);
        }

        public async Task<bool> DeleteRequest()
        {
            //WebRequest request = WebRequest.Create($"http://localhost:5000/api/Flowers/{_Id}");
            var applic = Application.Current as IGetConfiguration;
            var serv_url = applic.Configuration.GetSection("ServerUrl").Value;

            WebRequest request = WebRequest.Create($"{serv_url}Account/{_id}");
            {
                request.Method = "DELETE";
                request.ContentType = "application/json";
                request.PreAuthenticate = true;
                request.Headers.Add("Authorization", $"Bearer {_token} ");

                try
                {
                    await request.GetResponseAsync();
                    return true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                    return false;
                }

            };
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgSimple.SelectedItem != null)
            {
                if (dgSimple.SelectedItem is Products)
                {
                    var prod_view = dgSimple.SelectedItem as Products;
                    int id = prod_view.Id;
                    _id = id;
                    MessageBox.Show(_id.ToString());
                }
            }
            _ = DeleteRequest();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgSimple.SelectedItem != null)
            {
                if (dgSimple.SelectedItem is Products)
                {
                    var prod_view = dgSimple.SelectedItem as Products;
                    int id = prod_view.Id;
                    _id = id;
                    MessageBox.Show(_id.ToString());
                }

                EditProductWindow edit = new EditProductWindow(_id, _token);
                edit.ShowDialog();
            }
           
        }

        
    }
}
