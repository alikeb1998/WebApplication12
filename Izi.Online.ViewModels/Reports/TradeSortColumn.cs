using System.ComponentModel;

namespace Izi.Online.ViewModels.Reports
{
    public enum TradeSortColumn
    {
        [Description("تاریخ")]
        Date,
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

        [Description("ارزش خالص")]
        PureValue,
    }
}