using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using risersoft.shared.cloud;
using risersoft.shared.portable.Models.Gst;
using Risersoft.API.GSTN.Public;
using Risersoft.API.GSTN;
using Newtonsoft.Json;
using System.Net;

namespace GSTNirvana.API.Console
{

	static class StartupModule
	{


		public static void Main()
		{

            //clsClientCredTokenProvider provider = new clsClientCredTokenProvider("3U#R62K9J7SR", "KXAFHS", "http://localhost:11626", true);
            clsClientCredTokenProvider provider = new clsClientCredTokenProvider("4AFKA8#EDAXA", "753RPM", "http://login.risersoft.com", true);
            var token = provider.AuthenticateAsync().Result;
			WebApiClientToken client = new WebApiClientToken(provider.ClientId, provider.TokenResponse.AccessToken);


            System.Console.WriteLine("1=Post Invoice, 2=Search GSTIN, 3=Convert");
            String selection = System.Console.ReadLine();
            string server = "http://www.gstnirvana.com";

            switch (selection)
            {
                case "1":
                    GstInvoiceInfo invoice = new GstInvoiceInfo();
                    invoice.INUM = "Test1";
                    invoice.IDT = DateTime.Now.Date;
                    invoice.CTIN = "33GSPXXXXXX1ZA";
                    invoice.GSTInvoiceType = "b2b";
                    invoice.Sply_Ty = "inter";
                    invoice.CampusID = 1;
                    invoice.inv_typ = "R";
                    invoice.DocType = "IS";
                    invoice.InvoiceItems = new List<GstInvoiceItemInfo>();
                    invoice.InvoiceItems.Add(new GstInvoiceItemInfo
                    {
                        Description = "Cotton",
                        BasicRate = 1000,
                        Hsn_Sc = "1000982",
                        Qty = 10,
                        Uqc = "Kg",
                        TXVAL = 10000,
                        RT = 5,
                        IAMT = 500,
                        TY = "G"
                    });


                    client.PrepareQueryString(server + "/api/data/invoice", new Dictionary<string, string>());
                    var _info = client.Post<GstInvoiceInfo, ResultInfo<int,HttpStatusCode>>(invoice);

                    break;
                case "2":
                    Dictionary<string, string> params2 = new Dictionary<string, string>();
                    params2.Add("GSTIN", "09AAACI1195H1ZK");
                    client.PrepareQueryString(server + "/api/data/gstreginfo", params2);
                    var _info2 = client.Get<ResultInfo<TaxPayerModel,HttpStatusCode>>();
                    string str2 = JsonConvert.SerializeObject(_info2);
                    System.Console.WriteLine(str2);

                    break;
                case "3":
                    var json = System.IO.File.ReadAllText("b2bin.json");
                    var client3 = new MxApiClient(server + "/api/convert");
                    string str3 = client3.Json2CSV(json, "gstr1", "b2b").Data;

                    System.Console.WriteLine(str3);
                    break;


            }

            System.Console.WriteLine("Press any key to end this program");
            System.Console.ReadKey(false);

		}

		//Dim provider As New clsClientCredTokenProvider("3U#R62K9J7SR", "KXAFHS", "http://localhost:11626", True)
		//Dim server As String = "http://test.dev:56492"

	}
}

