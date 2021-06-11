using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        //private string _login;
        //private string _password;
        public string _token_prop { get; set; }
        public LoginWindow()
        {
            InitializeComponent();
            //_login = tbEmail.Text;
            //_password = tbPassword.Text;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            
            var app = App.Current as IGetConfiguration;
            var serverUrl = app.Configuration.GetSection("ServerUrl").Value;
            WebRequest request = WebRequest.Create($"{serverUrl}Account");
            {
                request.Method = "POST";
                request.ContentType = "application/json";
            };
            string json = JsonConvert.SerializeObject(new
            {
                //Email = "user@gmail.com",
                Email = tbEmail.Text,
                //Password = "qwerty"
                Password = tbPassword.Text
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
                var tokens_answer = JsonConvert.DeserializeObject<AuthToken>(res);
               // MessageBox.Show(tokens_answer.Token);
                _token_prop = tokens_answer.Token;

                Close();
                AdminWindow adminWindow = new AdminWindow(_token_prop);
                adminWindow.ShowDialog();
            }
            catch (Exception)
            {
                // MessageBox.Show(ex.Message.ToString());
                MessageBox.Show("You are not admin!");
                
            }
        }
    }
}
