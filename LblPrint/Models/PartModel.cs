using System;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Text;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using System.ComponentModel.DataAnnotations;



namespace LblPrint.Models
{ 
    public class PrintPartModel()
    {
        [Key]
        public string? Material { get; set; }
        public string? Description { get; set; }
        public string? Bin { get; set; }

        [DataType(DataType.Date)]
        public DateTime FIFO { get; set; }
    }

    public class ViewPartModel()
    {
        [Key]

        public string? Material { get; set; }
        public string? Description { get; set; }
        public string? Bin { get; set; }

        public string? PLnt { get; set; }

        public string? SLoc { get; set; }

        public string? ABC { get; set; }
        public int? On_Hand { get; set; }
        public DateOnly? Last_Count { get; set; }
        public int? Std_Cst { get; set; } 

    }
}
        
    
    

