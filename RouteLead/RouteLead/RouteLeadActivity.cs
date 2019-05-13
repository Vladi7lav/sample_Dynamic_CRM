using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Crm.Sdk.Messages;

namespace LeadRouting
{
    public sealed class LeadRouting : CodeActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            IWorkflowContext workflowcontext = context.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(workflowcontext.InitiatingUserId);

            // Set the Counters 
            int currentLeadCount = 0;
            int lowLeadCount = -1;
            Guid lowUserId = Guid.Empty;
            Guid leadId = workflowcontext.PrimaryEntityId;
            // Create the Query 
            QueryExpression userQuery = new QueryExpression();
            userQuery.EntityName = "systemuser";
            userQuery.ColumnSet = new ColumnSet();
            userQuery.ColumnSet.AllColumns = true;
            // Gets the list of users 
            EntityCollection bec = service.RetrieveMultiple(userQuery);

            foreach (Entity e in bec.Entities)
            {
                #region lead query expression 
                QueryExpression qe = new QueryExpression();
                qe.EntityName = "lead";
                qe.ColumnSet = new ColumnSet();
                qe.ColumnSet.AllColumns = true;
                qe.Criteria = new FilterExpression();
                qe.Criteria.FilterOperator = LogicalOperator.And;
                ConditionExpression ce = new ConditionExpression("ownerid", ConditionOperator.Equal, e.Attributes["systemuserid"]);
                qe.Criteria.Conditions.Add(ce);
                EntityCollection ecLead = service.RetrieveMultiple(qe);
                #endregion
                currentLeadCount = ecLead.Entities.Count;
                //if the first user, the user is marked the lowest
                if (lowLeadCount == -1)
                {
                    lowLeadCount = currentLeadCount;
                    lowUserId = new Guid(e.Attributes["systemuserid"].ToString());
                }
                // if the number of leads is lowest, the current user is marked lowest
                if (currentLeadCount < lowLeadCount)
                {
                    lowLeadCount = currentLeadCount;
                    lowUserId = new Guid(e.Attributes["systemuserid"].ToString());
                }
            }

            if (lowUserId != Guid.Empty)
            {
                AssignRequest assignRequest = new AssignRequest()
                {
                    Assignee = new EntityReference("systemuser", lowUserId),
                    Target = new EntityReference("lead", leadId)
                };
                AssignResponse assignResponse = (AssignResponse)service.Execute(assignRequest);
            }
        }
        [RequiredArgument]
        [Input("Distribute Leads Evenly")]
        [ReferenceTarget("lead")]
        public InArgument<EntityReference> Lead { get; set; }
    }
}
