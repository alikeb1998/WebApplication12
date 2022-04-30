using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.IO;
using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Services.Services
{
    public class InitialReleaseService : IInitialReleaseService
    {
        public string Token { get; set; }

        public IExternalInitialReleaseService _externalInitialReleaseService { get; }
        public InitialReleaseService(IExternalInitialReleaseService externalInitialReleaseService)
        {
            _externalInitialReleaseService = externalInitialReleaseService;
        }
        public async Task<ResultModel<List<Offers>>> OpenOffers()
        {
            var offers = await _externalInitialReleaseService.Open();
            var res = offers.Model.Offers.Select(x => new Offers()
            {
                IsActive = x.IsActive,
                Name = x.Name,
                PriceMax = x.PriceMax,
                PriceMin = x.PriceMin,
                QuantityMax = x.QuantityMax,
                QuantityMin = x.QuantityMin,
                ReleaseDate = x.OfferStartsAt
            }).ToList();
            return new ResultModel<List<Offers>>(res, offers.StatusCode == 200, offers.Message, offers.Model.Offers.Count > 0 ? offers.StatusCode : 204);
        }

        public async Task<ResultModel<List<ActiveOrder>>> ActiveOrders()
        {
            var actives = await _externalInitialReleaseService.Actives();
            var res = actives.Model.ActiveIoOrders.Select(x => new ActiveOrder() {
                BookDate = x.CreatedAt,
                ExequtedPercent = x.Quantity - x.ExecutedQ / 100,
                Quantity = x.Quantity,
                Name = x.Offer.Name,
                Price = x.Price,
                StateText = x.State,

            }).ToList();
            return new ResultModel<List<ActiveOrder>>(res, actives.StatusCode == 200, actives.Message, actives.Model.ActiveIoOrders.Count > 0 ? actives.StatusCode : 204);
        }

        public Task<ResultModel<bool>> Delete(int id)
        {
            var res = _externalInitialReleaseService.Delete(id);
            return null;
        }
    }
}
