using System.ComponentModel;

namespace Izi.Online.ViewModels.Reports
{
    public enum PortfolioSortColumn
    {
        [Description("نماد")]
        Symbol, 
        [Description("تعداد")]
        Count, 
        [Description("قیمت لحظه ای")]
        LivePrice, 
        [Description("میانگین خرید")]
        BuyAverage, 
        [Description("قیمت خرید")]
        BoughtPrice,
        [Description("ارزش ناخالص")]
        Value,
        [Description("سود/زیان")]
        ProfitOrLoss,
    }
}