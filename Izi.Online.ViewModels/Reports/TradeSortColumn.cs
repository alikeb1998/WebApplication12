using System.ComponentModel;

namespace Izi.Online.ViewModels.Reports
{
    public enum TradeSortColumn
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