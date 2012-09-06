using System;
using System.Configuration;
using System.ServiceModel.Configuration;
using System.ServiceModel;

namespace Gems.UIWPF
{
    //class WCFHelperClient : EvmsServiceClient
    //{
    //    public WCFHelperClient()
    //    {
    //        var endpointAddress = Endpoint.Address;
           
    //        EndpointAddressBuilder newEndpointAddress = new EndpointAddressBuilder(endpointAddress);
    //        newEndpointAddress.Uri = new Uri( ConfigHelper.GetEndpointAddress());
    //        this.Endpoint.Address = newEndpointAddress.ToEndpointAddress();
    //    }
    //}

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

    class AdminHelper : AdministrationClient
    {
        public AdminHelper()
        {
            var endpointAddress = Endpoint.Address;

            EndpointAddressBuilder newEndpointAddress = new EndpointAddressBuilder(endpointAddress);
            newEndpointAddress.Uri = new Uri(ConfigHelper.GetEndpointAddress());
            this.Endpoint.Address = newEndpointAddress.ToEndpointAddress();
        }
    }

    class NotifHelper : NotificationsClient
    {
        public NotifHelper()
        {
            var endpointAddress = Endpoint.Address;

            EndpointAddressBuilder newEndpointAddress = new EndpointAddressBuilder(endpointAddress);
            newEndpointAddress.Uri = new Uri(ConfigHelper.GetEndpointAddress());
            this.Endpoint.Address = newEndpointAddress.ToEndpointAddress();
        }
    }

    class EventHelper : EventClient
    {
        public EventHelper()
        {
            var endpointAddress = Endpoint.Address;

            EndpointAddressBuilder newEndpointAddress = new EndpointAddressBuilder(endpointAddress);
            newEndpointAddress.Uri = new Uri(ConfigHelper.GetEndpointAddress());
            this.Endpoint.Address = newEndpointAddress.ToEndpointAddress();
        }
    }

    class ProgrammeHelper : ProgrammeClient
    {
        public ProgrammeHelper()
        {
            var endpointAddress = Endpoint.Address;

            EndpointAddressBuilder newEndpointAddress = new EndpointAddressBuilder(endpointAddress);
            newEndpointAddress.Uri = new Uri(ConfigHelper.GetEndpointAddress());
            this.Endpoint.Address = newEndpointAddress.ToEndpointAddress();
        }
    }

    class RoleHelper : RoleClient
    {
        public RoleHelper()
        {
            var endpointAddress = Endpoint.Address;

            EndpointAddressBuilder newEndpointAddress = new EndpointAddressBuilder(endpointAddress);
            newEndpointAddress.Uri = new Uri(ConfigHelper.GetEndpointAddress());
            this.Endpoint.Address = newEndpointAddress.ToEndpointAddress();
        }
    }

    class FacilityHelper : FacilityClient
    {
        public FacilityHelper()
        {
            var endpointAddress = Endpoint.Address;

            EndpointAddressBuilder newEndpointAddress = new EndpointAddressBuilder(endpointAddress);
            newEndpointAddress.Uri = new Uri(ConfigHelper.GetEndpointAddress());
            this.Endpoint.Address = newEndpointAddress.ToEndpointAddress();
        }
    }

    class FacilityBookingsHelper : FacilityBookingsClient
    {
        public FacilityBookingsHelper()
        {
            var endpointAddress = Endpoint.Address;

            EndpointAddressBuilder newEndpointAddress = new EndpointAddressBuilder(endpointAddress);
            newEndpointAddress.Uri = new Uri(ConfigHelper.GetEndpointAddress());
            this.Endpoint.Address = newEndpointAddress.ToEndpointAddress();
        }
    }

    class EventItemsHelper : EventItemsClient
    {
        public EventItemsHelper()
        {
            var endpointAddress = Endpoint.Address;

            EndpointAddressBuilder newEndpointAddress = new EndpointAddressBuilder(endpointAddress);
            newEndpointAddress.Uri = new Uri(ConfigHelper.GetEndpointAddress());
            this.Endpoint.Address = newEndpointAddress.ToEndpointAddress();
        }
    }

    class ServiceContactHelper : ServiceContactClient
    {
        public ServiceContactHelper()
        {
            var endpointAddress = Endpoint.Address;

            EndpointAddressBuilder newEndpointAddress = new EndpointAddressBuilder(endpointAddress);
            newEndpointAddress.Uri = new Uri(ConfigHelper.GetEndpointAddress());
            this.Endpoint.Address = newEndpointAddress.ToEndpointAddress();
        }
    }

    class RegistrationHelper : RegistrationClient
    {
        public RegistrationHelper()
        {
            var endpointAddress = Endpoint.Address;

            EndpointAddressBuilder newEndpointAddress = new EndpointAddressBuilder(endpointAddress);
            newEndpointAddress.Uri = new Uri(ConfigHelper.GetEndpointAddress());
            this.Endpoint.Address = newEndpointAddress.ToEndpointAddress();
        }
    }

    class TasksHelper : TasksClient
    {
        public TasksHelper()
        {
            var endpointAddress = Endpoint.Address;

            EndpointAddressBuilder newEndpointAddress = new EndpointAddressBuilder(endpointAddress);
            newEndpointAddress.Uri = new Uri(ConfigHelper.GetEndpointAddress());
            this.Endpoint.Address = newEndpointAddress.ToEndpointAddress();
        }
    }

    class BudgetHelper : BudgetClient
    {
        public BudgetHelper()
        {
            var endpointAddress = Endpoint.Address;

            EndpointAddressBuilder newEndpointAddress = new EndpointAddressBuilder(endpointAddress);
            newEndpointAddress.Uri = new Uri(ConfigHelper.GetEndpointAddress());
            this.Endpoint.Address = newEndpointAddress.ToEndpointAddress();
        }
    }

}
