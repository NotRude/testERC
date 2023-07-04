using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace ERC
{
    public class MetersData
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; private set; }
        public double PersonsCount { get; private set; }
        public double ColdWater { get; private set; }
        public double WarmWaterVol { get; private set; }
        public double ElectricityDay { get; private set; }
        public double ElectricityNight { get; private set; }

        public static MetersData GetMetersData(double personsCount, double coldWater, double warmWaterVol, double electricityDay, double electricityNight)
        {
            var metersData = new MetersData();
            metersData.Date = DateTime.Now;
            metersData.PersonsCount = personsCount;
            metersData.ColdWater = coldWater;
            metersData.WarmWaterVol = warmWaterVol;
            metersData.ElectricityDay = electricityDay;
            metersData.ElectricityNight = electricityNight;
            return metersData;
        }
    }

    public class Tariff
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double ColdWater { get; set; }
        public double WarmWaterVol { get; set; }
        public double WarmWaterEnergy { get; set; }
        public double Electricity { get; set; }
        public double ElectricityDay { get; set; }
        public double ElectricityNight { get; set; }
        public double ColdWaterNorm { get; set; }
        public double WarmWaterVolNorm { get; set; }
        public double WarmWaterEnergyNorm { get; set; }
        public double ElectricityNorm { get; set; }       
    }
 
    public class Bill
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; private set; }
        public double ColdWater { get; private set; }
        public double ColdWaterBill { get; private set; }
        public double WarmWaterVol { get; private set; }
        public double WarmWaterVolBill { get; private set; }
        public double WarmWaterEnergy { get; private set; }
        public double WarmWaterEnergyBill { get; private set; }
        public double Electricity { get; private set; }
        public double ElectricityBill { get; private set; }
        public double ElectricityDay { get; private set; }
        public double ElectricityDayBill { get; private set; }
        public double ElectricityNight { get; private set; }
        public double ElectricityNightBill { get; private set; }

        public static Bill GetBill(MetersData relevantMD, MetersData pastMD)
        {
            var bill = new Bill();
            Tariff tariff;
            var db = new AppContext();        
            tariff = db.Tariffs.OrderBy(t => t.Id).Last();
            bill.Date = DateTime.Now;

            if (relevantMD.ColdWater >= 0)
                bill.ColdWater = Logic.GetConsumVolume(relevantMD.ColdWater, pastMD.ColdWater);
            else
                bill.ColdWater = Logic.GetNorm(relevantMD.PersonsCount, tariff.ColdWaterNorm);

            bill.ColdWaterBill = Logic.GetBill(bill.ColdWater, tariff.ColdWater);

            if (relevantMD.WarmWaterVol >= 0)
                bill.WarmWaterVol = Logic.GetConsumVolume(relevantMD.WarmWaterVol, pastMD.WarmWaterVol);
            else
                bill.WarmWaterVol = Logic.GetNorm(relevantMD.PersonsCount, tariff.WarmWaterVolNorm);

            bill.WarmWaterVolBill = Logic.GetBill(bill.WarmWaterVol, tariff.WarmWaterVol);
            bill.WarmWaterEnergy = Logic.GetWarmWaterEnergy(bill.WarmWaterVol);
            bill.WarmWaterEnergyBill = Logic.GetBill(bill.WarmWaterEnergy, tariff.WarmWaterEnergy);
            if (relevantMD.ElectricityDay >= 0 && relevantMD.ElectricityNight >= 0)
            {
                bill.ElectricityDay = Logic.GetConsumVolume(relevantMD.ElectricityDay, pastMD.ElectricityDay);
                bill.ElectricityDayBill = Logic.GetBill(bill.ElectricityDay, tariff.ElectricityDay);
                bill.ElectricityNight = Logic.GetConsumVolume(relevantMD.ElectricityNight, pastMD.ElectricityNight);
                bill.ElectricityNightBill = Logic.GetBill(bill.ElectricityNight, tariff.ElectricityNight);
                bill.Electricity = bill.ElectricityDay + bill.ElectricityNight;
                bill.ElectricityBill = bill.ElectricityDayBill + bill.ElectricityNightBill;
            }
            else
            {
                bill.Electricity = Logic.GetNorm(relevantMD.PersonsCount, tariff.ElectricityNorm);
                bill.ElectricityBill = Logic.GetBill(bill.Electricity, tariff.Electricity);
            }
            return bill;
        }

    }
}
