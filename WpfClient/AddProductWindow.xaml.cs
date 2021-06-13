using Microsoft.Win32;
using Newtonsoft.Json;
using ShopProject.UIHelper;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfClient
{
    /// <summary>
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public  string New_FileName { get; set; }
        public string _tok { get; set; }
        public AddProductWindow(string tok)
        {
            InitializeComponent();
            _tok = tok;

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
                    New_FileName = dlg.FileName;
                }
                catch
                {
                    MessageBox.Show("Неможливо відкрити!");
                }
            }
        }

        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {

            _ = PostRequest();
            Close();
                    
        }

        public async Task<bool> PostRequest()
        {
            string base64 = ImageHelper.ImageConvertToBase64(New_FileName);
            var applic = Application.Current as IGetConfiguration;
            var serv_url = applic.Configuration.GetSection("ServerUrl").Value;
            
            WebRequest request = WebRequest.Create($"{serv_url}Account/add");
            {
                request.Method = "POST";
                request.ContentType = "application/json";
                request.PreAuthenticate = true;
                request.Headers.Add("Authorization", $"Bearer {_tok} ");
            };
            float.TryParse(tbPrice.Text, out float resparse);
            string json = JsonConvert.SerializeObject(new
            {
                Name = tbName.Text.ToString(),
                Description=tbDetails.Text.ToString(),
                Price = resparse,
                Image = base64
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
            catch (WebException e)
            {

                using (WebResponse response = e.Response)
                {

                    HttpWebResponse web = (HttpWebResponse)response;

                    MessageBox.Show("Error-->>>" + web.StatusCode);
                    using var stream = e.Response.GetResponseStream();
                    using var reader = new StreamReader(stream);
                    var responces = reader.ReadToEnd();
                   // MessageBox.Show(responces);
                                        
                }
                return false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }

        }


    }
}
