using CrmLeadImport.leadsExcel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmLeadImport.Lab1
{
    class ImportLeads
    {
        public Entity lead
        {
            get;
            set;
        }

        public List<Guid> rGuid
        {
            get;
            set;
        }
        public ImportLeads(OrganizationServiceProxy connect ,List<Export> test)
        {            
            try
            {
                lead = new Entity("lead");
                rGuid = new List<Guid>();
                foreach (var r in test)
                {
                    lead.Attributes["subject"] = r.Subject;
                    lead.Attributes["firstname"] = r.FirstName;
                    lead.Attributes["lastname"] = r.LastName;
                    lead.Attributes["companyname"] = r.CompanyName;
                    lead.Attributes["numberofemployees"] = Convert.ToInt32(r.NumberOfEmployees);
                    lead.Attributes["revenue"] = Convert.ToDecimal(r.Revenue);
                    //lead.Contains("revenue"); Проверка на существование
                    rGuid.Add(connect.Create(lead));
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                connect.Dispose();
            }
        }
    }
}
