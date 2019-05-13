using CrmLeadImport.leadsExcel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrmLeadImport
{
    public partial class CrmServiceWrapper
    {
        public OrganizationServiceProxy organizationServiceProxy
        {
            get;
            set;
        }

        public CrmServiceWrapper(Uri serviceUri, ClientCredentials credentials)
        {
            this.organizationServiceProxy = new OrganizationServiceProxy(serviceUri, null, credentials, null);
        }
        public List<Guid> ImportLeadsThreads(List<Export> test)
        {
            try
            {
                List<Entity> lead = new List<Entity>();
                List<Guid> rGuid = new List<Guid>();

                foreach (var r in test)
                {
                    Entity entity = new Entity("lead");
                    entity.Attributes["subject"] = r.Subject;
                    entity.Attributes["firstname"] = r.FirstName;
                    entity.Attributes["lastname"] = r.LastName;
                    entity.Attributes["companyname"] = r.CompanyName;
                    entity.Attributes["numberofemployees"] = Convert.ToInt32(r.NumberOfEmployees);
                    entity.Attributes["revenue"] = Convert.ToDecimal(r.Revenue);
                    lead.Add(entity);
                }

                Parallel.ForEach(lead, (r) =>
                {
                    Console.WriteLine(String.Format("Thread: {0}, RecData: {1}",
                    Thread.CurrentThread.ManagedThreadId, this.organizationServiceProxy.Create(r)));
                });
                return rGuid;
            }
            catch
            {
                Console.WriteLine();
                organizationServiceProxy.Dispose();
                return null;
            }
        }
    }
} 

