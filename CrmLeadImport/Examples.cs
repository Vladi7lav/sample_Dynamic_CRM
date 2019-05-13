using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmLeadImport
{
    class Examples
    {   //Как узнаь значение типа поля
        RetrieveAttributeRequest c = new RetrieveAttributeRequest() { EntityLogicalName = "lead", LogicalName = "preferredcontactmethodcode" };

        var result = (RetrieveAttributeResponse)a.organizationServiceProxy.Execute(c);
        var attributeMeth = (PicklistAttributeMetadata)result.AttributeMetadata;
        var optionSetValue = attributeMeth.OptionSet.Options.FirstOrDefault(p => p.Label.UserLocalizedLabel.Label.ToLower() == "mail")?.Value;

        Entity lead = new Entity("lead");
        lead.Attributes["firstname"] = "Дмитрий1222";
        lead.Attributes["lastname"] = "Федоренко2";
        lead.Attributes["preferredcontactmethodcode"] = optionSetValue.HasValue ? new OptionSetValue(optionSetValue.Value) : null;    

    }
}
