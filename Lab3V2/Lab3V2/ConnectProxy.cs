using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Lab3V2
{
    class ConnectProxy : IDisposable
    {
        public OrganizationServiceProxy OrganizationServiceProxy
        {
            get;
        }
        //private OrganizationServiceContext organizationServiceContext
        //{
        //    get;
        //    set;
        //}

        public ConnectProxy(Uri serviceUri, ClientCredentials credentials)
        {
            this.OrganizationServiceProxy = new OrganizationServiceProxy(serviceUri, null, credentials, null);
            //this.organizationServiceContext = new OrganizationServiceContext(this.organizationServiceProxy);

        }

        public void Dispose()
        {
            this.OrganizationServiceProxy.Dispose();
        }
    }
}
