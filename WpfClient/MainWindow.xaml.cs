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
        public ObservableCollection<Products> Products { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            var app = App.Current as IGetConfiguration;
            var serverUrl = app.Configuration.GetSection("ServerUrl").Value;

            using (WebClient client = new WebClient())
            {
                client.DownloadDataCompleted += asyncWeb_DownloadDataCompleted;
                Uri url = new Uri($"{serverUrl}Account/show");
                client.DownloadDataAsync(url);
            }

            //Products = new ObservableCollection<Products>
            //{
            //    new Products { 
            //        Name="Олівці",
            //        Description = "Різнокольорові олівці відомої фірми Марко (Чехія). Набір 24 кольори. Яскраві, приємні для сприйняття",
            //        Price=35, 
            //       // Image="D:\\ШАГ\\0 Repository\\ShopProject\\ShopProject\\Photos\\01.jpg"
            //        },
            //    new Products { 
            //        Name="Набір",
            //        Description = "Набір канцелярського приладдя для школярів. Включає кольорові олівці, фломастери, фарби акварельні, гуаші, лінійки, клей",
            //        Price=305,
            //        //Image="D:\\ШАГ\\0 Repository\\ShopProject\\ShopProject\\Photos\\02.jpg"
            //       },
            //    new Products { 
            //        Name="Офісне приладдя",
            //        Description = "Набір канцтоварів для офісу. До складу входять: олівці, ручки, підставка, блокноти, калькулятор, лінійка",
            //        Price=520,
            //        //Image="D:\\ШАГ\\0 Repository\\ShopProject\\ShopProject\\Photos\\03.jpg"
            //       },
            //    new Products { 
            //        Name="Фломастери",
            //        Description = "Набір різнокольорових фломастерів чеської фірми Кох-і-нор. 36 фломастерів відмінної якості з екологічними барвниками",
            //        Price=75,
            //        //Image="D:\\ШАГ\\0 Repository\\ShopProject\\ShopProject\\Photos\\04.jpg"
            //        },
            //    new Products { 
            //        Name="Палички",
            //        Description = "Палички для лічби. Призначені для учнів дошкільного та молодшого шкільного віку. 40 штук",
            //        Price=16,
            //        //Image="D:\\ШАГ\\0 Repository\\ShopProject\\ShopProject\\Photos\\05.jpg"
            //        }
            //};
            //productList.ItemsSource = Products;
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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void btnContact_Click(object sender, RoutedEventArgs e)
        {
            ContactUsWindow cuw = new ContactUsWindow();
            cuw.ShowDialog();
        }

        private void btnBasket_Click(object sender, RoutedEventArgs e)
        {
            BasketWindow bw = new BasketWindow();
            bw.ShowDialog();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }

        
    }
}
