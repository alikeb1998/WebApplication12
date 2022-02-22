using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Reports
{
    public enum OrderSortTypes
    {
        [Description("انجام شده")]
        Percentage,
        [Description("نماد")]
        Symbol,
        [Description("تاریخ")]
        Date,
        [Description("سمت")]
        Side,
        [Description("حجم")]
        Volume,
        [Description("قیمت")]
        Price,
        [Description("نوع اعتبار")]
        Credit,
        [Description("تاریخ اعتبار")]
        CreditDate,
        [Description("وضعیت")]
        State,
        [Description("عملیات")]
        Proccess
    }
}
