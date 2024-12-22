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
using Microsoft.Data.SqlClient;


namespace LblPrint.Models
{
    public class PartModel
    {
        readonly List<GetData> partslist = new List<GetData>();
        public void GetPartData(string x)
        {
            try
            {
                string connectString = "";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(connectString);
                con.Open();
                string query = $"Select * from material where material = {x}";
                Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(query, con);
                Microsoft.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    GetData parts = new GetData();
                    parts.Material = reader.GetString(1);
                    parts.Material_description = reader.GetString(2);
                    parts.Bin = reader.GetString(3);  
                    
                    partslist.Add(parts);
                }
            }

            catch (Exception ex)
            {S

            }
        }
    }
    public class GetData()
    {
        public string? Material { get; set; }
        public string? Material_description { get; set; }
        public string? Bin { get; set; }


        public static void LabelAPI()
        {
            byte[] zpl = Encoding.UTF8.GetBytes("^xa^cfa,50^fo100,100^fdHello World^fs^xz");

            // adjust print density (8dpmm), label width (4 inches), label height (6 inches), and label index (0) as necessary
            // The comment below was the in the original API for Labelary and is unsupported. 
            // var request = (HttpWebRequest)WebRequest.Create("http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/");

            var url = "http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/";
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Method = "POST";
            // request.Accept = "application/pdf"; // omit this line to get PNG images back
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = zpl.Length;

            var requestStream = request.GetRequestStream();
            requestStream.Write(zpl, 0, zpl.Length);
            requestStream.Close();

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                var fileStream = File.Create("label.PNG"); // change file name for PNG images
                responseStream.CopyTo(fileStream);
                responseStream.Close();
                fileStream.Close();
            }

            catch (WebException e)
            {
                Console.WriteLine("Error: {0}", e.Status);
            }

            /*
             * Potential use of API later on if does not work. 
             var url = "http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/";
var zpl = Encoding.UTF8.GetBytes("^xa^cfa,50^fo100,100^fdHello World^fs^xz");

using var client = new HttpClient();
var request = new HttpRequestMessage(HttpMethod.Post, url);
request.Content = new ByteArrayContent(zpl);
request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

try
{
  var response = await client.SendAsync(request);
  response.EnsureSuccessStatusCode();
  // Handle successful response here
}
catch (HttpRequestException e)
{
  Console.WriteLine("Error: {0}", e.Status);
}
             */
        }
    }
}
        
    
    

