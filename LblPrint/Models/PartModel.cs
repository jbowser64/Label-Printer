﻿using System;
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
    public class PartModel()
    {
        public string? Material { get; set; }
        public string? Material_description { get; set; }
        public string? Bin { get; set; }

        [DataType(DataType.Date)]
        public DateTime FIFO { get; set; }
    }
}
        
    
    

