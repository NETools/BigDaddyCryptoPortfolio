using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models.Dtos
{
    public class MessageBusNotification
    {
        public GenericMessageType GenericMessageType { get; set; }
        public string ChanneId { get; set; }
        public string MessageId { get; set; }
        public byte[] StructData { get; set; }
    }
}
