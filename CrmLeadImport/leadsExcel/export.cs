using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmLeadImport.leadsExcel
{
    public class Export
    {
        public string Subject { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string NumberOfEmployees { get; set; }
        public string Revenue { get; set; }
        public void piece(string line)
        {
            string[] parts = line.Split(';');
            Subject = parts[0];
            FirstName = parts[1];
            LastName = parts[2];
            CompanyName = parts[3];
            NumberOfEmployees = parts[4];
            Revenue = parts[5];
        }
               
        public static List<Export> ReadFile(FileStream path)
        {
            int i = 0;
            try
            {
                List<Export> res = new List<Export>();
                using (StreamReader sr = new StreamReader(path))
                {
                    
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                        if (i != 0)
                        {
                            Export p = new Export();
                            p.piece(line);
                            res.Add(p);
                        }
                        else
                            i++;
                    }
                    
                }
                return res;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                return null;
            }
        }
    }
}
