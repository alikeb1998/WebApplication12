using Iz.Online.OmsModels.ResponsModels.Instruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BestLimitsView = Izi.Online.ViewModels.Instruments.BestLimit.BestLimits;
namespace Izi.Online.ViewModels.Instruments.BestLimit
{
    public static class VolumeProccessor
    {
        public static BestLimitsView ProccessVolume(BestLimitsView bestLimits)
        {
            var totalBuys = bestLimits.orderRow1.volumeBestBuy +
                            bestLimits.orderRow2.volumeBestBuy +
                            bestLimits.orderRow3.volumeBestBuy +
                            bestLimits.orderRow4.volumeBestBuy +
                            bestLimits.orderRow5.volumeBestBuy +
                            bestLimits.orderRow6.volumeBestBuy;

            var totalSells = bestLimits.orderRow1.volumeBestSale +
                      bestLimits.orderRow2.volumeBestSale +
                      bestLimits.orderRow3.volumeBestSale +
                      bestLimits.orderRow4.volumeBestSale +
                      bestLimits.orderRow5.volumeBestSale +
                      bestLimits.orderRow6.volumeBestSale;

            bestLimits.orderRow1.BuyWeight = bestLimits.orderRow1.priceBestBuy != 0 ? PercentProccessor(totalBuys, bestLimits.orderRow1.volumeBestBuy) : 0;
            bestLimits.orderRow2.BuyWeight = bestLimits.orderRow2.priceBestBuy != 0 ? PercentProccessor(totalBuys, bestLimits.orderRow2.volumeBestBuy) : 0;
            bestLimits.orderRow3.BuyWeight = bestLimits.orderRow3.priceBestBuy != 0 ? PercentProccessor(totalBuys, bestLimits.orderRow3.volumeBestBuy) : 0;
            bestLimits.orderRow4.BuyWeight = bestLimits.orderRow4.priceBestBuy != 0 ? PercentProccessor(totalBuys, bestLimits.orderRow4.volumeBestBuy) : 0;
            bestLimits.orderRow5.BuyWeight = bestLimits.orderRow5.priceBestBuy != 0 ? PercentProccessor(totalBuys, bestLimits.orderRow5.volumeBestBuy) : 0;
            bestLimits.orderRow6.BuyWeight = bestLimits.orderRow6.priceBestBuy != 0 ? PercentProccessor(totalBuys, bestLimits.orderRow6.volumeBestBuy) : 0;

            bestLimits.orderRow1.SellWeight = bestLimits.orderRow1.priceBestSale != 0 ? PercentProccessor(totalSells, bestLimits.orderRow1.volumeBestSale) : 0;
            bestLimits.orderRow2.SellWeight = bestLimits.orderRow2.priceBestSale != 0 ? PercentProccessor(totalSells, bestLimits.orderRow2.volumeBestSale) : 0;
            bestLimits.orderRow3.SellWeight = bestLimits.orderRow3.priceBestSale != 0 ? PercentProccessor(totalSells, bestLimits.orderRow3.volumeBestSale) : 0;
            bestLimits.orderRow4.SellWeight = bestLimits.orderRow4.priceBestSale != 0 ? PercentProccessor(totalSells, bestLimits.orderRow4.volumeBestSale) : 0;
            bestLimits.orderRow5.SellWeight = bestLimits.orderRow5.priceBestSale != 0 ? PercentProccessor(totalSells, bestLimits.orderRow5.volumeBestSale) : 0;
            bestLimits.orderRow6.SellWeight = bestLimits.orderRow6.priceBestSale != 0 ? PercentProccessor(totalSells, bestLimits.orderRow6.volumeBestSale) : 0;

            //if (bestLimits.orderRow1.priceBestBuy == detail.PriceMax)
            //{
            //    bestLimits.IsBuyQueue = true;
            //    bestLimits.IsSellQueue = false;
            //}
            //else if (bestLimits.orderRow1.priceBestSale == detail.PriceMin)
            //{
            //    bestLimits.IsSellQueue = true;
            //    bestLimits.IsBuyQueue = false;
            //}

            return bestLimits;
        }
        public static double PercentProccessor(double a, double b)
        {
            if (a == 0) return 0;
            var res = (a - b) / a * 100;
            return 100 - res;
        }
    }
}
