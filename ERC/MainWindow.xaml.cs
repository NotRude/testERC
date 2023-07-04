using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace ERC
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<int, int> daysPersonPairs = new Dictionary<int, int>();
        private int daysCount = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        public MainWindow()
        {
            AppContext db = new AppContext();
            db.Database.EnsureCreated();
            if (!db.Tariffs.Any())
            {
                var tariff = new Tariff()
                {
                    Date = DateTime.Now,
                    ColdWater = 35.78,
                    WarmWaterVol = 35.78,
                    WarmWaterEnergy = 998.96,
                    Electricity = 4.28,
                    ElectricityDay = 4.9,
                    ElectricityNight = 2.31,
                    ColdWaterNorm = 4.85,
                    ElectricityNorm = 164,
                    WarmWaterVolNorm = 4.1,
                    WarmWaterEnergyNorm = 0.05349
                };
                db.Tariffs.Add(tariff);
            }
            var dbMD = db.MetersDatas.ToList<MetersData>();
            if (!db.MetersDatas.Any())
            {
                db.MetersDatas.Add(MetersData.GetMetersData(0, 0, 0, 0, 0));
            }
            db.MetersDatas.RemoveRange(db.MetersDatas);
            db.Bills.RemoveRange(db.Bills);
            db.SaveChanges();
            
            InitializeComponent();
            
            for(var i = 1; i <= daysCount; i++)
            {
                ComboBox.Items.Add(i);
            }
            ComboBox.SelectedIndex = ComboBox.Items.Count - 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double d;
            var tryParse =  double.TryParse(TextCW.Text, out d)
                && double.TryParse(TextWW.Text, out d)
                && double.TryParse(TextElD.Text, out d)
                && double.TryParse(TextElN.Text, out d);
            if (!tryParse)
            {
                MessageBox.Show("Введены не корректные данные!");
                return;
            }
            var personCount = 0.0;
            if (TextPC.Visibility == Visibility.Visible)
            {
                if (ComboBox.Items.Count > 0)
                {
                    MessageBox.Show("Заполните количество человек и дней");
                    return;
                }
                foreach(var p in daysPersonPairs)
                {
                    personCount += p.Value * p.Key;
                }
                personCount = personCount / daysCount;
            }           
            var coldWater = double.Parse(TextCW.Text);
            var warmWater = double.Parse(TextWW.Text);
            var electricityDay = double.Parse(TextElD.Text);
            var electricityNight = double.Parse(TextElN.Text);  
            
            MetersData metersData = MetersData.GetMetersData(personCount, coldWater, warmWater, electricityDay, electricityNight);
            var db = new AppContext();
            var pastMetersData1 = db.MetersDatas.ToList<MetersData>();
            var pastMetersData = db.MetersDatas.OrderBy(md => md.Id).Last();
            var bill = Bill.GetBill(metersData, pastMetersData);

            db.Bills.Add(bill);
            db.MetersDatas.Add(metersData);
            db.SaveChanges();
            new BillWindow().Show();
            Close();
        }

        private void CheckCW_Checked(object sender, RoutedEventArgs e)
        {
            TextCW.Visibility = Visibility.Visible;
            TextCW.Text = "0";
            if(CheckWW.IsChecked.Value  && CheckEl.IsChecked.Value)
            {
                Button_PC.Visibility = Visibility.Hidden;
                TextPC.Visibility = Visibility.Hidden;
                TextPC.Text = "1";
            }
        }

        private void CheckCW_Unchecked(object sender, RoutedEventArgs e)
        {
            TextCW.Visibility = Visibility.Hidden;
            TextCW.Text = "-1";
            Button_PC.Visibility = Visibility.Visible;
            TextPC.Visibility = Visibility.Visible;
            TextPC.Text = "0";
        }

        private void CheckWW_Checked(object sender, RoutedEventArgs e)
        {
            TextWW.Visibility = Visibility.Visible;
            TextWW.Text = "0";
            if (CheckCW.IsChecked.Value && CheckEl.IsChecked.Value)
            {
                Button_PC.Visibility = Visibility.Hidden;
                TextPC.Visibility = Visibility.Hidden;
                TextPC.Text = "1";
            }
        }

        private void CheckWW_Unchecked(object sender, RoutedEventArgs e)
        {
            TextWW.Visibility = Visibility.Hidden;
            TextWW.Text = "-1";
            Button_PC.Visibility = Visibility.Visible;
            TextPC.Visibility = Visibility.Visible;
            TextPC.Text = "0";
        }

        private void CheckEl_Checked(object sender, RoutedEventArgs e)
        {
            TextElD.Visibility = Visibility.Visible;
            TextElN.Visibility = Visibility.Visible;
            TextElD.Text = "0";
            TextElN.Text = "0";
            if (CheckWW.IsChecked.Value && CheckCW.IsChecked.Value)
            {
                Button_PC.Visibility = Visibility.Hidden;
                TextPC.Visibility = Visibility.Hidden;
                TextPC.Text = "1";
            }
        }

        private void CheckEl_Unchecked(object sender, RoutedEventArgs e)
        {
            TextElD.Visibility = Visibility.Hidden;
            TextElN.Visibility = Visibility.Hidden;
            TextElD.Text = "-1";
            TextElN.Text = "-1";
            Button_PC.Visibility = Visibility.Visible;
            TextPC.Visibility = Visibility.Visible;
            TextPC.Text = "0";
        }

        private void Button_PC_Click(object sender, RoutedEventArgs e)
        {
            if(!Int32.TryParse(TextPC.Text, out int n))
            {
                MessageBox.Show("Введены не корректные данные!");
                return;
            }
            var personCount = Int32.Parse(TextPC.Text);
            if (personCount < 1)
            {
                MessageBox.Show("Введите число человек");
                return;
            }
            var daysCount = ComboBox.SelectedIndex + 1;
            for(var i = 0; i < daysCount; i++)
            {
                ComboBox.Items.RemoveAt(ComboBox.Items.Count - 1);
            }
            ComboBox.SelectedIndex = ComboBox.Items.Count - 1;
            InfoPD.Text += "\n" + daysCount + "/" + personCount;
            if (daysPersonPairs.ContainsKey(personCount))
            {
                daysPersonPairs[personCount] += daysCount;
            }
            else
                daysPersonPairs.Add(personCount, daysCount);
            if(ComboBox.Items.Count == 0)
            {
                Button_PC.Visibility = Visibility.Hidden;
            }
        }

        private void Guid_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Шаг1: Расставьте галочки на показатели, которые имеют счётчики. \n Шаг2: Если кнопка \"Учесть\" активна, впишите в поле \"Число человек/дней\" число человек и выбирите число дней из выпадающего списка. Повторяйте действие пока не кончится количество дней. \n Шаг3: Заполните оставшиеся поля.\n Шаг4: Нажмите на \"Передать\".");
        }
    }
}
