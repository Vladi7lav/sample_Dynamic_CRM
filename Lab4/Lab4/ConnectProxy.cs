using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    class ConnectProxy : IDisposable
    {
        public OrganizationServiceProxy OrganizationServiceProxy { get; }

        public ConnectProxy(Uri serviceUri, ClientCredentials credentials)
        {
            this.OrganizationServiceProxy = new OrganizationServiceProxy(serviceUri, null, credentials, null);
        }

        public void Dispose()
        {
            this.OrganizationServiceProxy.Dispose();
        }
    }
}
