using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfClient
{
    /// <summary>
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {

            _ = PostRequest();
                    
        }

        public async Task<bool> PostRequest()
        {
           // string base64 = ImageConverterBase.ConvertToBase(File_base);
            //WebRequest request = WebRequest.Create("http://localhost:5000/api/Flowers/add");
            var applic = Application.Current as IGetConfiguration;
            var serv_url = applic.Configuration.GetSection("ServerUrl").Value;
            WebRequest request = WebRequest.Create($"{serv_url}Account/add");
            {
                request.Method = "POST";
                request.ContentType = "application/json";

            };
            float.TryParse(tbPrice.Text, out float resparse);
            string json = JsonConvert.SerializeObject(new
            {
                Name = tbName.Text.ToString(),
                Description=tbDetails.Text.ToString(),
                Price = resparse,
                Image = "6.jpg"
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
                    MessageBox.Show(responces);
                                        
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
