using System.ComponentModel;

namespace Izi.Online.ViewModels.Reports
{
    public enum TradeSortTypes
    {
        [Description("نماد")]
        Symbol,
        [Description("سمت")]
        Side,
        [Description("تعداد")]
        Count,
        [Description("قیمت")]
        Price,
        [Description("وضعیت")]
        State,
        [Description("تاریخ")]
        Date, 
        [Description("ارزش خالص")]
        PureValue,
    }
}