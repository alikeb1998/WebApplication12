using Iz.Online.Entities;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using Izi.Online.ViewModels.SignalR;
using WatchList = Izi.Online.ViewModels.Instruments.WatchList;

namespace Iz.Online.Reopsitory.IRepository
{
    public interface IInstrumentsRepository : IBaseRepository
    {
        Task<ResultModel<List<Instruments>>> GetInstrumentsList();
        Task<ResultModel<List<InstrumentBourse>>> GetInstrumentBourse();
        Task<ResultModel<List<InstrumentSector>>> GetInstrumentSector();
        Task<ResultModel<List<InstrumentSubSector>>> GetInstrumentSubSectors();
        Task<ResultModel<bool>> AddInstrumentBourse(InstrumentBourse model);
        Task<ResultModel<bool>> AddInstrumentSector(InstrumentSector model);
        Task<ResultModel<bool>> AddInstrumentSubSectors(InstrumentSubSector model);
        Task<ResultModel<bool>> AddInstrument(Instrument model, int sectorId, int subSectorId, int bourseId, long tick, float buyCommissionRate, float sellCommissionRate);
        Task<ResultModel<bool>> UpdateInstruments(Instrument model, int sectorId, int subSectorId, int bourseId, long tick, float buyCommissionRate, float sellCommissionRate);
        Task<ResultModel<bool>> AddCommentToInstrument(AddCommentForInstrument model);

        Task<ResultModel<string>> GetInstrumentComment(GetInstrumentComment model);
        InstrumentList InstrumentData(int instrumentId);
        InstrumentList InstrumentData(string nsc);
        List<InstrumentList> InstrumentData();
        int GetLocalInstrumentIdFromOmsId(int omsId);
        void CustomerSelectInstrument(SelectInstrumentInput model);
        Task<List<string>> GetInstrumentHubs(string NscCode);
        void CleareCache();
    }
}
