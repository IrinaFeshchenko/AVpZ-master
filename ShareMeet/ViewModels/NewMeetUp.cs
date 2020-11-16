using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShareMeet.ViewModels
{
    public class NewMeetUp
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int companyId_company { get; set; }
        public DateTime StartofSelection { get; set; }
        public DateTime FinishofSelection { get; set; }
        public float lng { get; set; }
        public float lat { get; set; }
        public string Adress { get; set; }
        public string Cost { get; set; }
    }
}
