using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mood.Models
{
    public class MeetUp
    {
        [Key]
        public int Id_meetup { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Type { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Description { get; set; }
        [Column(TypeName = "int")]
        public int companyId_company { get; set; }
        public DateTime StartofSelection { get; set; }
        public DateTime FinishofSelection { get; set; }
        public float lng { get; set; }
        public float lat { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Adress { get; set; }
        public string Cost { get; set; }

    }
}
