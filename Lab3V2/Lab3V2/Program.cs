using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Lab3V2
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

            //Задание 3.1 (метод QueryExpressionExample)
            //ConnectProxy con = new ConnectProxy(serviceUri, credentials);
            //EntityCollection contacts = QueryExpressionExample(con.OrganizationServiceProxy);
            //con.Dispose();
            //foreach (var c in contacts.Entities)
            //{
            //    Console.WriteLine(c.Attributes["fullname"].ToString());
            //}

            //Задание 3.3 (Использование FetchXML запроса)
            //string fetch = @"<fetch mapping='logical'> 
            //                        <entity name='lead'> 
            //                            <attribute name='fullname'/> 
            //                            <filter type='and'> 
            //                            <condition attribute='jobtitle' operator='eq' value='Менеджер по закупкам'/> 
            //                            </filter> 
            //                        </entity> 
            //                 </fetch>";
            //ConnectProxy con = new ConnectProxy(serviceUri, credentials);
            //EntityCollection contactlist = (EntityCollection)con.OrganizationServiceProxy.RetrieveMultiple(new FetchExpression(fetch));
            //con.Dispose();
            //foreach (var contact in contactlist.Entities)
            //{
            //    Console.WriteLine(contact.Attributes["fullname"].ToString());
            //}

            //Задание 3.4 (Использование фильтрованных представлений)
            //string strConSQL = "Data Source = localhost; Initial Catalog = AdventureWorksCycles_MSCRM; Integrated Security = True";
            //Пока что не понятно к какой базе подключаться
            //ConnectSQL con = new ConnectSQL(strConSQL);
            //string sqlQuery = @"SELECT name, address1_city, address1_stateorprovince 
            //                    FROM filteredaccount    
            //                    WHERE name like '%" + "" + "%'";    //ADD SEARCH TEXT!
            //SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, con.SqlCon);
            //DataTable dt = new DataTable();
            //adapter.Fill(dt);
            //con.Dispose();

            //Задание 3.5 (Использование фильтрованных представлений)
            ConnectProxy con = new ConnectProxy(serviceUri, credentials);

            List<string> notSearh = new List<string>();
            notSearh.Add("Roman Kopaev");
            notSearh.Add("Ivan Ivanov");

            EntityCollection accounts = (EntityCollection)GetAllAcctAttributes(con.OrganizationServiceProxy);
            EntityCollection users = (EntityCollection)GetAllUserAttributes(con.OrganizationServiceProxy, notSearh);
            int AllUsers = users.Entities.Count;
            int AllAccounts = accounts.Entities.Count;
            if (AllUsers != 0 && AllAccounts != 0)
            {        
                int maxUser = users.Entities.Count - 1;
                int cntUser = 0;                
                foreach (Entity acc in accounts.Entities)
                {
                    if (cntUser > maxUser) cntUser = 0;
                    AssignRequest assignRequest = new AssignRequest()
                    {
                        Assignee = new EntityReference("systemuser", users.Entities[cntUser].Id),
                        Target = new EntityReference("account", acc.Id)
                    };
                    try
                    {
                        AssignResponse assignResponse = (AssignResponse)con.OrganizationServiceProxy.Execute(assignRequest);
                        Console.WriteLine(users.Entities[cntUser].Attributes["fullname"] + " - " + acc.Attributes["name"] + " : " + assignResponse.ToString());
                    }
                    catch (Exception e) { Console.WriteLine(e.Message.ToString()); }                    
                    cntUser++;
                }
                con.Dispose();
            }
            else { Console.WriteLine("number of available users or organizations is 0"); }
            Console.Read();           
        }

        public static EntityCollection QueryExpressionExample(OrganizationServiceProxy con)
        {
            try
            {                                
                ColumnSet cset = new ColumnSet();
                cset.AddColumns("fullname", "parentcustomerid");
                                
                ConditionExpression condExp = new ConditionExpression();                
                condExp.AttributeName = "jobtitle";
                condExp.Operator = ConditionOperator.Equal;
                condExp.Values.Add("Purchasing Assistant");

                FilterExpression filterJobMan = new FilterExpression();
                filterJobMan.AddCondition(condExp);

                QueryExpression qContact = new QueryExpression("contact");
                qContact.ColumnSet = cset;
                qContact.Criteria.AddFilter(filterJobMan);

                EntityCollection contacts = con.RetrieveMultiple(qContact);
                return contacts;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                return null;
            }
        }

        public static EntityCollection GetAllAcctAttributes(OrganizationServiceProxy con)
        {
            try
            {
                ColumnSet cSet = new ColumnSet("name", "parentaccountid");
                QueryExpression qExp = new QueryExpression("account");
                qExp.ColumnSet = cSet;
                RetrieveMultipleRequest rmr = new RetrieveMultipleRequest();
                rmr.Query = qExp;
                RetrieveMultipleResponse response = (RetrieveMultipleResponse)con.Execute(rmr);
                return response.EntityCollection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                return null;
            }
        }
        public static EntityCollection GetAllUserAttributes(OrganizationServiceProxy con, List<string> excludedUsers = null)
        {
            try
            {
                ColumnSet cSet = new ColumnSet("fullname", "parentsystemuserid");

                ConditionExpression cExp = new ConditionExpression("isdisabled", ConditionOperator.Equal, new object[] { false });
                ConditionExpression excluded = excludedUsers!=null ? new ConditionExpression("fullname", ConditionOperator.NotIn, excludedUsers) : null;
                FilterExpression fExp = new FilterExpression(LogicalOperator.And);
                fExp.AddCondition(cExp);
                fExp.AddCondition(excluded);
                QueryExpression qExp = new QueryExpression("systemuser");
                qExp.ColumnSet = cSet;
                qExp.Criteria = fExp;
                RetrieveMultipleRequest rmr = new RetrieveMultipleRequest();
                rmr.Query = qExp;
                RetrieveMultipleResponse rResponse = (RetrieveMultipleResponse)con.Execute(rmr);
                return rResponse.EntityCollection;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                return null;
            }
        }
    }
}
