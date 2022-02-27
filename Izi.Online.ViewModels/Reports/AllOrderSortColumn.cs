using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Reports
{
    public enum AllOrderSortColumn
    {

        [Description("تاریخ")]
        Date,

        [Description("نماد")]
        Symbol,

        [Description("سمت")]
        Side,

        [Description("قیمت")]
        Price,

        [Description("شماره سفارش")]
        Id,

        [Description("نوع اعتبار")]
        ValidityType,

        [Description("حجم معمله شده")]
        Volume,

        [Description("ارزش معامله شده")]
        VolumeValue,

        [Description("تاریخ اعتبار")]
        ValidityDate,

        [Description("وضعیت")]
        State,
    }
}
