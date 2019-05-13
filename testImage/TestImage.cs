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

namespace testImage
{
    public class TestImage : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                IPluginExecutionContext _context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var _service = serviceFactory.CreateOrganizationService(_context.UserId);
                
                Entity PreImage = (Entity)_context.PreEntityImages["Image"];

                if (_context.InputParameters.Contains("Target") && _context.InputParameters["Target"] is Entity)
                {
                    Entity lead = (Entity)_context.InputParameters["Target"];

                    if (PreImage.Contains("mobilephone"))
                    {
                        lead.Attributes["new_autonumber"] = 10;
                    }
                    else
                    {
                        lead.Attributes["new_autonumber"] = 100;
                    }
                }
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                throw e;
            }
        }
    }
}
