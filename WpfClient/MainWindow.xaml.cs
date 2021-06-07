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

            Products = new ObservableCollection<Products>
            {
                new Products { 
                    Name="Олівці",
                    Description = "Різнокольорові олівці відомої фірми Марко (Чехія). Набір 24 кольори. Яскраві, приємні для сприйняття",
                    Price=35, 
                    Image="C:\\Users\\Ba2H\\source\\repos\\WWWShopTest\\WWWShopTest\\images\\01.jpg" },
                new Products { 
                    Name="Набір",
                    Description = "Набір канцелярського приладдя для школярів. Включає кольорові олівці, фломастери, фарби акварельні, гуаші, лінійки, клей",
                    Price=305, 
                    Image="C:\\Users\\Ba2H\\source\\repos\\WWWShopTest\\WWWShopTest\\images\\02.jpg" },
                new Products { 
                    Name="Офісне приладдя",
                    Description = "Набір канцтоварів для офісу. До складу входять: олівці, ручки, підставка, блокноти, калькулятор, лінійка",
                    Price=520, 
                    Image="C:\\Users\\Ba2H\\source\\repos\\WWWShopTest\\WWWShopTest\\images\\03.jpg" },
                new Products { 
                    Name="Фломастери",
                    Description = "Набір різнокольорових фломастерів чеської фірми Кох-і-нор. 36 фломастерів відмінної якості з екологічними барвниками",
                    Price=75, 
                    Image="C:\\Users\\Ba2H\\source\\repos\\WWWShopTest\\WWWShopTest\\images\\04.jpg" },
                new Products { 
                    Name="Палички",
                    Description = "Палички для лічби. Призначені для учнів дошкільного та молодшого шкільного віку. 40 штук",
                    Price=16, 
                    Image="C:\\Users\\Ba2H\\source\\repos\\WWWShopTest\\WWWShopTest\\images\\05.jpg" }
            };
            productList.ItemsSource = Products;
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



        ///private void Btn_get_token(object sender, RoutedEventArgs e)
        ///{
        ///    //MessageBox.Show("yes");
        ///    var app = App.Current as IGetConfiguration;
        ///    var serverUrl = app.Configuration.GetSection("ServerUrl").Value;
        ///    WebRequest request = WebRequest.Create($"{serverUrl}Account");
        ///    {
        ///        request.Method = "POST";
        ///        request.ContentType = "application/json";
        ///    };
        ///    string json = JsonConvert.SerializeObject(new
        ///    {
        ///        Email = "user@gmail.com",
        ///        Password = "qwerty"
        ///    });
        ///    byte[] bytes = Encoding.UTF8.GetBytes(json);
        ///
        ///    using (Stream stream = request.GetRequestStreamAsync().Result)
        ///    {
        ///        stream.Write(bytes, 0, bytes.Length);
        ///    }
        ///    try
        ///    {
        ///        var responce = request.GetResponseAsync().Result;
        ///        using var stream = new StreamReader( responce.GetResponseStream());
        ///      
        ///        var res = stream.ReadToEnd();
        ///        var errors = JsonConvert.DeserializeObject<AuthToken>(res);
        ///        MessageBox.Show(errors.Token);
        ///        
        ///    }
        ///    catch (Exception ex)
        ///    {
        ///        MessageBox.Show(ex.Message.ToString());
        ///        
        ///    }
        ///}
    }
}
