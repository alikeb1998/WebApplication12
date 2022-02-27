﻿using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.OmsModels.ResponsModels.Order;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.Reports;
using Izi.Online.ViewModels.ShareModels;
using ActiveOrder = Izi.Online.ViewModels.Orders.ActiveOrder;
using AddOrderResult = Izi.Online.ViewModels.Orders.AddOrderResult;
using CanceledOrder = Izi.Online.ViewModels.Orders.CanceledOrder;
using CancelOrder = Izi.Online.ViewModels.Orders.CancelOrder;
using UpdatedOrder = Izi.Online.ViewModels.Orders.UpdatedOrder;
using UpdateOrder = Izi.Online.ViewModels.Orders.UpdateOrder;
using AllOrder = Izi.Online.ViewModels.Orders.AllOrder;

namespace Iz.Online.Services.IServices
{
    public interface IOrderServices
    {
        ResultModel<AddOrderResult> Add(AddOrderModel addOrderModel);
        ResultModel<List<ActiveOrder>> AllActive();
        ResultModel<OrderReport> AllActivePaged(OrderFilter filter);
        ResultModel<AllOrderReport> AllSortedOrder(AllOrderCustomFilter filter);
        ResultModel<UpdatedOrder> Update(UpdateOrder model);
        ResultModel<CanceledOrder> Cancel(CancelOrder model);
        
    }
}
