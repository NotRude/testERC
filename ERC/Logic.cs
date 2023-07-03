using System.Linq;

namespace ERC
{
    public static class Logic
    {
        public static double GetConsumVolume(double relevantData, double pastData) => relevantData - pastData;
        public static double GetBill(double consumVolume, double tariff)
            => consumVolume * tariff;
        public static double GetNorm(double personsCount, double norm)
            => personsCount * norm;
        public static double GetWarmWaterEnergy(double warmWaterVol)
        {
            Tariff tariff;
            var db = new AppContext();
            tariff = db.Tariffs.OrderBy(t => t.Id).Last();
            return warmWaterVol * tariff.WarmWaterEnergyNorm;
        }

    }
}
