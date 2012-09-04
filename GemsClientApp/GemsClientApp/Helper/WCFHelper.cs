using System;
using System.Configuration;
using System.ServiceModel.Configuration;
using System.ServiceModel;

namespace Gems.UIWPF
{
    class WCFHelperClient : EvmsServiceClient
    {
        public WCFHelperClient()
        {
            var endpointAddress = Endpoint.Address;
           
            EndpointAddressBuilder newEndpointAddress = new EndpointAddressBuilder(endpointAddress);
            newEndpointAddress.Uri = new Uri( ConfigHelper.GetEndpointAddress());
            this.Endpoint.Address = newEndpointAddress.ToEndpointAddress();
        }
    }

    class GuestHelper : GuestClient
    {
        public GuestHelper()
        {
            var endpointAddress = Endpoint.Address;

            EndpointAddressBuilder newEndpointAddress = new EndpointAddressBuilder(endpointAddress);
            newEndpointAddress.Uri = new Uri(ConfigHelper.GetEndpointAddress());
            this.Endpoint.Address = newEndpointAddress.ToEndpointAddress();
        }
    }
}
