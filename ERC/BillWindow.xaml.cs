using System.Linq;
using System.Windows;


namespace ERC
{
    /// <summary>
    /// Логика взаимодействия для BillWindow.xaml
    /// </summary>
    public partial class BillWindow : Window
    {
        public BillWindow()
        {
            InitializeComponent();
            using(var db = new AppContext())
            {
                var bill = db.Bills.OrderBy(b => b.Id).Last();
                TextVolCW.Text = bill.ColdWater.ToString();
                TextPriceCW.Text = bill.ColdWaterBill.ToString();
                TextVolWWV.Text = bill.WarmWaterVol.ToString();
                TextPriceWWV.Text = bill.WarmWaterVolBill.ToString();
                TextVolWWE.Text = bill.WarmWaterEnergy.ToString();
                TextPriceWWE.Text = bill.WarmWaterEnergyBill.ToString();
                TextVolElD.Text = bill.ElectricityDay.ToString();
                TextPriceElD.Text = bill.ElectricityDayBill.ToString();
                TextVolElN.Text = bill.ElectricityNight.ToString();
                TextPriceElN.Text = bill.ElectricityNightBill.ToString();
                TextVolElFull.Text = bill.Electricity.ToString();
                TextPriceElFull.Text = bill.ElectricityBill.ToString();
                var fullPrice = bill.ColdWaterBill + bill.WarmWaterVolBill + bill.WarmWaterEnergyBill + bill.ElectricityBill;
                TextFullPrice.Text = fullPrice.ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();          
        }
    }
}
