using Iz.Online.Entities;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using WatchList = Izi.Online.ViewModels.Instruments.WatchList;

namespace Iz.Online.Reopsitory.IRepository
{
    public interface IInstrumentsRepository : IBaseRepository
    {
        ResultModel<List<Instruments>> GetInstrumentsList();
        ResultModel<List<InstrumentBourse>> GetInstrumentBourse();
        ResultModel<List<InstrumentSector>> GetInstrumentSector();
        ResultModel<List<InstrumentSubSector>> GetInstrumentSubSectors();
        ResultModel<bool> AddInstrumentBourse(InstrumentBourse model);
        ResultModel<bool> AddInstrumentSector(InstrumentSector model);
        ResultModel<bool> AddInstrumentSubSectors(InstrumentSubSector model);
        ResultModel<bool> AddInstrument(Instrument model, int sectorId, int subSectorId, int bourseId, long tick, float buyCommissionRate, float sellCommissionRate);
        ResultModel<bool> UpdateInstruments(Instrument model, int sectorId, int subSectorId, int bourseId, long tick, float buyCommissionRate, float sellCommissionRate);
        //ResultModel<long> GetInstrumentId(int InstrumentId);

        ResultModel<bool> AddCommentToInstrument(AddCommentForInstrument model);
      
        ResultModel<string> GetInstrumentComment(GetInstrumentComment model);
        InstrumentList InstrumentData(int instrumentId);
        List<InstrumentList> InstrumentData();
    }
}
