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
                TextVolCW.Text = string.Format("{0:0.##}", bill.ColdWater);
                TextPriceCW.Text = string.Format("{0:0.##}", bill.ColdWaterBill);
                TextVolWWV.Text = string.Format("{0:0.##}", bill.WarmWaterVol);
                TextPriceWWV.Text = string.Format("{0:0.##}", bill.WarmWaterVolBill);
                TextVolWWE.Text = string.Format("{0:0.##}", bill.WarmWaterEnergy);
                TextPriceWWE.Text = string.Format("{0:0.##}", bill.WarmWaterEnergyBill);
                TextVolElD.Text = string.Format("{0:0.##}", bill.ElectricityDay);
                TextPriceElD.Text = string.Format("{0:0.##}", bill.ElectricityDayBill);
                TextVolElN.Text = string.Format("{0:0.##}", bill.ElectricityNight);
                TextPriceElN.Text = string.Format("{0:0.##}", bill.ElectricityNightBill);
                TextVolElFull.Text = string.Format("{0:0.##}", bill.Electricity);
                TextPriceElFull.Text = string.Format("{0:0.##}", bill.ElectricityBill);
                var fullPrice = bill.ColdWaterBill + bill.WarmWaterVolBill + bill.WarmWaterEnergyBill + bill.ElectricityBill;
                TextFullPrice.Text = string.Format("{0:0.##}", fullPrice);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();          
        }
    }
}
