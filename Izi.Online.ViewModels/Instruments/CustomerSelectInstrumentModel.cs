using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Instruments
{
    public class CustomerSelectInstrumentModel
    {
        //int instrumentId, string token, string kafkaUserId
        public string NscCode { get; set; }
        public List<string> HubId { get; set; }
        public string KafkaUserId { get; set; }

    }
}