using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using System.Net;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentials = new ClientCredentials
            {
                Windows = { ClientCredential = new NetworkCredential("Administrator", "Pass@word99") }
            };
            Uri serviceUri = new Uri("http://crm-train.columbus.ru:5555/CRM2016/XRMServices/2011/Organization.svc");
        }
    }
}
