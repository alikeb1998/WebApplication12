using Iz.Online.OmsModels.ResponsModels.Instruments;
using Iz.Online.OmsModels.ResponsModels.User;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;


namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalInstrumentService
    {
        string Token { get; set; }
        static string NationalCode { get; set; }
        Task<bool> UpdateInstrumentList();
        Task<ResultModel<Izi.Online.ViewModels.Instruments.BestLimit.BestLimits>> BestLimits(string NscCode, long InstrumentId, string hubId);
        Task<ResultModel<InstrumentPriceDetails>> Price(string NscCode);
        Task<ResultModel<Details>> Details(long InstrumentId, string NscCode, string hubId);
        // InstrumentStates States(Instrument model);
        Task StartConsume();
        ResultModel<CustomerInfo> GetNationalCode(string token);
        void SetNationalCode(string nationalCode);
    }
}
