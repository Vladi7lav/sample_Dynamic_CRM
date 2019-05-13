using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace AutoNumber
{
    public class AutoNumber : IPlugin
    {         
        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                IPluginExecutionContext _context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var _service = serviceFactory.CreateOrganizationService(_context.UserId);

                if (_context.InputParameters.Contains("Target") && _context.InputParameters["Target"] is Entity)
                {
                    Entity lead = (Entity)_context.InputParameters["Target"];
                    decimal maxCnt;
                    string fetch = @" <fetch distinct='false' mapping='logical' aggregate='true'> <entity name='lead'> 
                                    <attribute name='new_autonumber' alias='new_autonumber_max' aggregate='max' /> 
                                    </entity> </fetch>";
                    Entity max = _service.RetrieveMultiple(new FetchExpression(fetch)).Entities.FirstOrDefault();
                    maxCnt = max.Contains("new_autonumber") ? (decimal)max["new_autonumber"] : -1;
                    
                    lead.Attributes["new_autonumber"] = ++maxCnt;
                    
                                                           
                }

            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                throw e;
            }
        }
    }
}
