using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Thread.Sleep(2000);
            InitializeComponent();
            
        }

        private void Btn_get_token(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("yes");
            var app = App.Current as IGetConfiguration;
            var serverUrl = app.Configuration.GetSection("ServerUrl").Value;
            WebRequest request = WebRequest.Create($"{serverUrl}Account");
            {
                request.Method = "POST";
                request.ContentType = "application/json";
            };
            string json = JsonConvert.SerializeObject(new
            {
                Email = "user@gmail.com",
                Password = "qwerty"
            });
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            using (Stream stream = request.GetRequestStreamAsync().Result)
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            try
            {
                var responce = request.GetResponseAsync().Result;
                using var stream = new StreamReader( responce.GetResponseStream());
              
                var res = stream.ReadToEnd();
                var errors = JsonConvert.DeserializeObject<AuthToken>(res);
                MessageBox.Show(errors.Token);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                
            }
        }
    }
}
