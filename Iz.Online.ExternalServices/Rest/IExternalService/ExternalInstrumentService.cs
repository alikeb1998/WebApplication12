﻿using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.OmsModels.ResponsModels.BestLimits;
using Iz.Online.OmsModels.ResponsModels.Instruments;
using Iz.Online.Reopsitory.IRepository;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using Instruments = Iz.Online.OmsModels.ResponsModels.Instruments.Instruments;
using InstrumentDetails = Iz.Online.OmsModels.ResponsModels.Instruments.InstrumentDetails;
using InstrumentModel = Iz.Online.OmsModels.InputModels.Instruments.Instrument;
using InstrumentPrice = Iz.Online.OmsModels.ResponsModels.Instruments.InstrumentPrice;

namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class ExternalInstrumentService : BaseService, IExternalInstrumentService
    {
        private readonly IInstrumentsRepository _instrumentsRepository;

        public ExternalInstrumentService(IInstrumentsRepository instrumentsRepository) : base(instrumentsRepository)
        {
            _instrumentsRepository = instrumentsRepository;
        }
        public bool UpdateInstrumentList(ViewBaseModel model)
        {
            var onDbInstrumentsList = _instrumentsRepository.GetInstrumentsList().Select(x => x.InstrumentId).ToList();
            var onDbInstrumentSector = _instrumentsRepository.GetInstrumentSector().Select(x => x.SectorId).ToList();
            var onDbInstrumentSubSectors = _instrumentsRepository.GetInstrumentSubSectors().Select(x => x.SubSectorId).ToList();
            var onDbInstrumentBourse = _instrumentsRepository.GetInstrumentBourse().Select(x => x.BourseId).ToList();

            var instruments = HttpGetRequest<Instruments>("order/instruments", model.Token);

            try
            {
                foreach (var instrument in instruments.instruments)
                {

                    if (!onDbInstrumentSector.Contains(instrument.sector.id))
                    {
                        _instrumentsRepository.AddInstrumentSector(instrument.sector);
                        onDbInstrumentSector.Add(instrument.sector.id);
                    }

                    if (!onDbInstrumentSubSectors.Contains(instrument.subSector.id))
                    {
                        _instrumentsRepository.AddInstrumentSubSectors(instrument.subSector);
                        onDbInstrumentSubSectors.Add(instrument.subSector.id);
                    }

                    if (!onDbInstrumentBourse.Contains(instrument.group.id))
                    {
                        _instrumentsRepository.AddInstrumentBourse(instrument.group);
                        onDbInstrumentBourse.Add(instrument.group.id);
                    }

                    if (!onDbInstrumentsList.Contains(instrument.Id))
                    {
                        _instrumentsRepository.AddInstrument(instrument);
                        onDbInstrumentsList.Add(instrument.Id);
                    }

                }

                return true;
            }
            catch (Exception e)
            {
                _instrumentsRepository.LogException(e);
                return false;
            }

        }

        public BestLimits BestLimits(SelectedInstrument model)
        {
           var result =  HttpGetRequest<BestLimits>($"rlc/best-limit/{model.InstrumentId}", model.Token);
           return result;
        }

        public InstrumentPrice Price(InstrumentModel model)
        {
            var result = HttpGetRequest<InstrumentPrice>($"rlc/price/{model.NscCode}", model.Authorization);
            return result;
        }

        public InstrumentDetails Details(InstrumentModel model)
        {
            var result = HttpGetRequest<InstrumentDetails>($"order/instrument/{model.InstrumentId}", model.Authorization);
            return result;
        }

        //public InstrumentStates States(SelectedInstrument model)
        //{
        //    var result = HttpGetRequest<InstrumentStates>($"order/instrument/{model.InstrumentId}", model.Authorization);
        //    return result;
        //}
    }
}
