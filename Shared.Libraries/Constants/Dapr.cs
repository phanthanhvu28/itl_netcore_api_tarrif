using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Libraries.Constants
{
    public class Dapr
    {
        public const string PubSubName = "DaprPubSub";
        public const string DaprHttp = "Dapr:Http";
        public const string DaprGrpc = "Dapr:Grpc";

        public static readonly Dictionary<string, string> DefaultStateStoreMetadata = new()
        {
            { "ttlInSeconds", "6000" }
        };
    }
}
