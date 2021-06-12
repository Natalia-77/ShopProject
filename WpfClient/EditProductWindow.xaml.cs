using Microsoft.Win32;
using Newtonsoft.Json;
using ShopProject.Entities;
using ShopProject.UIHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace WpfClient
{
    /// <summary>
    /// Логика взаимодействия для EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        public Products product { get; set; }
        public string newPhoto { get; set; }
        public int _id { get; set; }
        public string _toren_edit { get; set; }
        public EditProductWindow(int id, string token_edit)
        {
            InitializeComponent();
            _id = id;
            _toren_edit = token_edit;

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
                var item = list.FirstOrDefault(y => y.Id == _id);
                product = item;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().ToString());
            }

        }
        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;" +
                "*.PNG|All files (*.*)|*.*";

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    newPhoto = dlg.FileName;
                }
                catch
                {
                    MessageBox.Show("Неможливо відкрити!");
                }
            }
        }

        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            _ = PutRequest();
            Close();
        }

        public async Task<bool> PutRequest()
        {

            if (newPhoto != null)
            {
                string base64 = ImageHelper.ImageConvertToBase64(newPhoto);
                product.Image = base64;
            }


            if (!string.IsNullOrEmpty(tbName.Text))
                product.Name = tbName.Text.ToString();

            if (!string.IsNullOrEmpty(tbDetails.Text))
                product.Description = tbDetails.Text.ToString();

            if (!string.IsNullOrEmpty(tbPrice.Text))
                product.Price = float.Parse(tbPrice.Text);


            var applic = Application.Current as IGetConfiguration;
            var serv_url = applic.Configuration.GetSection("ServerUrl").Value;

            WebRequest request = WebRequest.Create($"{serv_url}Account/{_id}");
            {
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.PreAuthenticate = true;
                request.Headers.Add("Authorization", $"Bearer {_toren_edit} ");
            };


            string json = JsonConvert.SerializeObject(new
            {
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Image
            });

            byte[] bytes = Encoding.UTF8.GetBytes(json);

            using (Stream stream = await request.GetRequestStreamAsync())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
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

        }
    }
}
