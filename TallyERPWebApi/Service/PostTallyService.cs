using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Xml;
using TallyERPWebApi.Model;
using System.Net;

namespace TallyERPWebApi.Service
{
	public class PostTallyService
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly ILogger<PostTallyService> _logger;

		public PostTallyService(HttpClient httpClient, IConfiguration configuration, ILogger<PostTallyService> logger)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_logger = logger;
		}

		public async Task<string> SaveUom(string xmlFilePath, string uom)
		{
			try
			{
				// Validate Tally URL
				string tallyUrl = _configuration["TallySettings:TallyUrl"];
				if (string.IsNullOrWhiteSpace(tallyUrl))
				{
					throw new InvalidOperationException("Tally URL is not configured.");
				}

				// Validate XML file
				if (!File.Exists(xmlFilePath))
				{
					throw new FileNotFoundException("The specified XML file was not found.", xmlFilePath);
				}

				// Read the XML content from the file
				string xmlContent = await File.ReadAllTextAsync(xmlFilePath);
				string modifiedXmlContent = xmlContent.Replace("<unitname>", uom.ToUpper().Trim());

				// Create HTTP request
				var request = new HttpRequestMessage(HttpMethod.Post, tallyUrl)
				{
					Content = new StringContent(modifiedXmlContent, Encoding.UTF8, "text/xml")
				};

				// Send request and get response
				var response = await _httpClient.SendAsync(request);
				response.EnsureSuccessStatusCode();

				var responseContent = await response.Content.ReadAsStringAsync();

				return responseContent;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while communicating with Tally.");
				throw;
			}
		}

		public async Task<string> SaveStockGroup(string xmlFilePath, string stockgroup)
		{
			try
			{
				// Validate Tally URL
				string tallyUrl = _configuration["TallySettings:TallyUrl"];
				if (string.IsNullOrWhiteSpace(tallyUrl))
				{
					throw new InvalidOperationException("Tally URL is not configured.");
				}

				// Validate XML file
				if (!File.Exists(xmlFilePath))
				{
					throw new FileNotFoundException("The specified XML file was not found.", xmlFilePath);
				}

				// Read the XML content from the file
				string xmlContent = await File.ReadAllTextAsync(xmlFilePath);
				string modifiedXmlContent = xmlContent.Replace("<newstockgroup>", stockgroup.ToUpper().Trim());

				// Create HTTP request
				var request = new HttpRequestMessage(HttpMethod.Post, tallyUrl)
				{
					Content = new StringContent(modifiedXmlContent, Encoding.UTF8, "text/xml")
				};

				// Send request and get response
				var response = await _httpClient.SendAsync(request);
				response.EnsureSuccessStatusCode();

				var responseContent = await response.Content.ReadAsStringAsync();

				return responseContent;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while communicating with Tally.");
				throw;
			}
		}

		public async Task<string> SaveStockItem(string xmlFilePath, StockItem stockItem)
		{
			try
			{
				// Validate Tally URL
				string tallyUrl = _configuration["TallySettings:TallyUrl"];
				if (string.IsNullOrWhiteSpace(tallyUrl))
				{
					throw new InvalidOperationException("Tally URL is not configured.");
				}

				// Validate XML file
				if (!File.Exists(xmlFilePath))
				{
					throw new FileNotFoundException("The specified XML file was not found.", xmlFilePath);
				}

				string xmlContent = await File.ReadAllTextAsync(xmlFilePath);

				string modifiedXmlContent = xmlContent.Trim()
								.Replace("<itemname>", ConvertToHtmlEntities(stockItem.name.ToUpper().Trim()))
								.Replace("<itemunit>", stockItem.unit.ToUpper().Trim())
								.Replace("<itemgroup>", stockItem.category.ToUpper().Trim())
								.Replace("<itemopeningqnty>", Convert.ToInt32(stockItem.openingqnty).ToString())
								.Replace("<itemopeningrate>", Convert.ToInt32(stockItem.openingrate).ToString())
								.Replace("<itemalias>", stockItem.alias)
								.Replace("<itemhsncode>", stockItem.hsncode);


				// Create HTTP request
				var request = new HttpRequestMessage(HttpMethod.Post, tallyUrl)
				{
					Content = new StringContent(modifiedXmlContent, Encoding.UTF8, "text/xml")
				};

				// Send request and get response
				var response = await _httpClient.SendAsync(request);
				response.EnsureSuccessStatusCode();

				var responseContent = await response.Content.ReadAsStringAsync();

				return responseContent;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while communicating with Tally.");
				throw;
			}
		}
		public static string ConvertToHtmlEntities(string input)
		{
			return WebUtility.HtmlEncode(input);
		}

		public async Task<string> SaveLedeger(string xmlFilePath, Ledger ledger)
		{
			try
			{
				// Validate Tally URL
				string tallyUrl = _configuration["TallySettings:TallyUrl"];
				if (string.IsNullOrWhiteSpace(tallyUrl))
				{
					throw new InvalidOperationException("Tally URL is not configured.");
				}

				// Validate XML file
				if (!File.Exists(xmlFilePath))
				{
					throw new FileNotFoundException("The specified XML file was not found.", xmlFilePath);
				}

				string xmlContent = await File.ReadAllTextAsync(xmlFilePath);

				string modifiedXmlContent = xmlContent.Trim()
								.Replace("<name>", ledger.name.ToUpper().Trim())
								.Replace("<parent>", ledger.type.ToUpper().Trim())
								.Replace("<address>", ledger.address.ToUpper().Trim())
								.Replace("<city>", ledger.city.ToUpper().Trim())
								.Replace("<country>", ledger.country.ToString())
								.Replace("<state>", ledger.state.ToString())
								.Replace("<phoneno>", ledger.phoneno)
								.Replace("<pincode>", ledger.zipcode)
								.Replace("<gst>", ledger.gst);


				// Create HTTP request
				var request = new HttpRequestMessage(HttpMethod.Post, tallyUrl)
				{
					Content = new StringContent(modifiedXmlContent, Encoding.UTF8, "text/xml")
				};

				// Send request and get response
				var response = await _httpClient.SendAsync(request);
				response.EnsureSuccessStatusCode();

				var responseContent = await response.Content.ReadAsStringAsync();

				return responseContent;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while communicating with Tally.");
				throw;
			}
		}

		public async Task<string> SaveVoucher(string xmlFilePath, Voucher voucher)
		{
			try
			{
				// Validate Tally URL
				string tallyUrl = _configuration["TallySettings:TallyUrl"];
				if (string.IsNullOrWhiteSpace(tallyUrl))
				{
					throw new InvalidOperationException("Tally URL is not configured.");
				}

				// Validate XML file
				if (!File.Exists(xmlFilePath))
				{
					throw new FileNotFoundException("The specified XML file was not found.", xmlFilePath);
				}

				string xmlContent = await File.ReadAllTextAsync(xmlFilePath);
				string addmoreitem = null;

				foreach (var item in voucher.Items)
				{
					string itemxml = " <ALLINVENTORYENTRIES.LIST>\r\n       <STOCKITEMNAME><itemname></STOCKITEMNAME>\r\n       <GSTOVRDNINELIGIBLEITC>&#4; Not Applicable</GSTOVRDNINELIGIBLEITC>\r\n       <GSTOVRDNISREVCHARGEAPPL>&#4; Not Applicable</GSTOVRDNISREVCHARGEAPPL>\r\n       <GSTOVRDNTAXABILITY>Taxable</GSTOVRDNTAXABILITY>\r\n       <GSTSOURCETYPE>Company</GSTSOURCETYPE>\r\n       <HSNSOURCETYPE>Company</HSNSOURCETYPE>\r\n       <GSTOVRDNSTOREDNATURE/>\r\n       <GSTOVRDNTYPEOFSUPPLY>Goods</GSTOVRDNTYPEOFSUPPLY>\r\n       <GSTRATEINFERAPPLICABILITY>As per Masters/Company</GSTRATEINFERAPPLICABILITY>\r\n       <GSTHSNNAME>12345678</GSTHSNNAME>\r\n       <GSTHSNDESCRIPTION>GST DETAILS</GSTHSNDESCRIPTION>\r\n       <GSTHSNINFERAPPLICABILITY>As per Masters/Company</GSTHSNINFERAPPLICABILITY>\r\n       <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>\r\n       <ISGSTASSESSABLEVALUEOVERRIDDEN>No</ISGSTASSESSABLEVALUEOVERRIDDEN>\r\n       <STRDISGSTAPPLICABLE>No</STRDISGSTAPPLICABLE>\r\n       <CONTENTNEGISPOS>No</CONTENTNEGISPOS>\r\n       <ISLASTDEEMEDPOSITIVE>Yes</ISLASTDEEMEDPOSITIVE>\r\n       <ISAUTONEGATE>No</ISAUTONEGATE>\r\n       <ISCUSTOMSCLEARANCE>No</ISCUSTOMSCLEARANCE>\r\n       <ISTRACKCOMPONENT>No</ISTRACKCOMPONENT>\r\n       <ISTRACKPRODUCTION>No</ISTRACKPRODUCTION>\r\n       <ISPRIMARYITEM>No</ISPRIMARYITEM>\r\n       <ISSCRAP>No</ISSCRAP>\r\n       <RATE><itemrate></RATE>\r\n       <AMOUNT><itemsum></AMOUNT>\r\n       <ACTUALQTY> <itemqnty></ACTUALQTY>\r\n       <BILLEDQTY> <itemqnty></BILLEDQTY>\r\n       <BATCHALLOCATIONS.LIST>\r\n        <GODOWNNAME>WAREHOUSE 1</GODOWNNAME>\r\n        <BATCHNAME>Primary Batch</BATCHNAME>\r\n        <DESTINATIONGODOWNNAME>WAREHOUSE 1</DESTINATIONGODOWNNAME>\r\n        <INDENTNO>&#4; Not Applicable</INDENTNO>\r\n        <ORDERNO><ordernumber></ORDERNO>\r\n        <TRACKINGNUMBER>&#4; Not Applicable</TRACKINGNUMBER>\r\n        <DYNAMICCSTISCLEARED>No</DYNAMICCSTISCLEARED>\r\n        <AMOUNT><itemsum></AMOUNT>\r\n        <ACTUALQTY> <itemqnty></ACTUALQTY>\r\n        <BILLEDQTY> <itemqnty></BILLEDQTY>\r\n        <ORDERDUEDATE JD=\"45382\" P=\"1-Apr-24\">1-Apr-24</ORDERDUEDATE>\r\n        <ADDITIONALDETAILS.LIST>        </ADDITIONALDETAILS.LIST>\r\n        <VOUCHERCOMPONENTLIST.LIST>        </VOUCHERCOMPONENTLIST.LIST>\r\n       </BATCHALLOCATIONS.LIST>\r\n       <ACCOUNTINGALLOCATIONS.LIST>\r\n        <OLDAUDITENTRYIDS.LIST TYPE=\"Number\">\r\n         <OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>\r\n        </OLDAUDITENTRYIDS.LIST>\r\n        <LEDGERNAME><accoutgroup></LEDGERNAME>\r\n        <GSTCLASS>&#4; Not Applicable</GSTCLASS>\r\n        <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>\r\n        <LEDGERFROMITEM>No</LEDGERFROMITEM>\r\n        <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>\r\n        <ISPARTYLEDGER>No</ISPARTYLEDGER>\r\n        <GSTOVERRIDDEN>No</GSTOVERRIDDEN>\r\n        <ISGSTASSESSABLEVALUEOVERRIDDEN>No</ISGSTASSESSABLEVALUEOVERRIDDEN>\r\n        <STRDISGSTAPPLICABLE>No</STRDISGSTAPPLICABLE>\r\n        <STRDGSTISPARTYLEDGER>No</STRDGSTISPARTYLEDGER>\r\n        <STRDGSTISDUTYLEDGER>No</STRDGSTISDUTYLEDGER>\r\n        <CONTENTNEGISPOS>No</CONTENTNEGISPOS>\r\n        <ISLASTDEEMEDPOSITIVE>Yes</ISLASTDEEMEDPOSITIVE>\r\n        <ISCAPVATTAXALTERED>No</ISCAPVATTAXALTERED>\r\n        <ISCAPVATNOTCLAIMED>No</ISCAPVATNOTCLAIMED>\r\n        <AMOUNT><itemsum></AMOUNT>\r\n        <SERVICETAXDETAILS.LIST>        </SERVICETAXDETAILS.LIST>\r\n        <BANKALLOCATIONS.LIST>        </BANKALLOCATIONS.LIST>\r\n        <BILLALLOCATIONS.LIST>        </BILLALLOCATIONS.LIST>\r\n        <INTERESTCOLLECTION.LIST>        </INTERESTCOLLECTION.LIST>\r\n        <OLDAUDITENTRIES.LIST>        </OLDAUDITENTRIES.LIST>\r\n        <ACCOUNTAUDITENTRIES.LIST>        </ACCOUNTAUDITENTRIES.LIST>\r\n        <AUDITENTRIES.LIST>        </AUDITENTRIES.LIST>\r\n        <INPUTCRALLOCS.LIST>        </INPUTCRALLOCS.LIST>\r\n        <DUTYHEADDETAILS.LIST>        </DUTYHEADDETAILS.LIST>\r\n        <EXCISEDUTYHEADDETAILS.LIST>        </EXCISEDUTYHEADDETAILS.LIST>\r\n        <RATEDETAILS.LIST>        </RATEDETAILS.LIST>\r\n        <SUMMARYALLOCS.LIST>        </SUMMARYALLOCS.LIST>\r\n        <CENVATDUTYALLOCATIONS.LIST>        </CENVATDUTYALLOCATIONS.LIST>\r\n        <STPYMTDETAILS.LIST>        </STPYMTDETAILS.LIST>\r\n        <EXCISEPAYMENTALLOCATIONS.LIST>        </EXCISEPAYMENTALLOCATIONS.LIST>\r\n        <TAXBILLALLOCATIONS.LIST>        </TAXBILLALLOCATIONS.LIST>\r\n        <TAXOBJECTALLOCATIONS.LIST>        </TAXOBJECTALLOCATIONS.LIST>\r\n        <TDSEXPENSEALLOCATIONS.LIST>        </TDSEXPENSEALLOCATIONS.LIST>\r\n        <VATSTATUTORYDETAILS.LIST>        </VATSTATUTORYDETAILS.LIST>\r\n        <COSTTRACKALLOCATIONS.LIST>        </COSTTRACKALLOCATIONS.LIST>\r\n        <REFVOUCHERDETAILS.LIST>        </REFVOUCHERDETAILS.LIST>\r\n        <INVOICEWISEDETAILS.LIST>        </INVOICEWISEDETAILS.LIST>\r\n        <VATITCDETAILS.LIST>        </VATITCDETAILS.LIST>\r\n        <ADVANCETAXDETAILS.LIST>        </ADVANCETAXDETAILS.LIST>\r\n        <TAXTYPEALLOCATIONS.LIST>        </TAXTYPEALLOCATIONS.LIST>\r\n       </ACCOUNTINGALLOCATIONS.LIST>\r\n       <DUTYHEADDETAILS.LIST>       </DUTYHEADDETAILS.LIST>\r\n       <RATEDETAILS.LIST>\r\n        <GSTRATEDUTYHEAD>CGST</GSTRATEDUTYHEAD>\r\n        <GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>\r\n        <GSTRATE> 9</GSTRATE>\r\n       </RATEDETAILS.LIST>\r\n       <RATEDETAILS.LIST>\r\n        <GSTRATEDUTYHEAD>SGST/UTGST</GSTRATEDUTYHEAD>\r\n        <GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>\r\n        <GSTRATE> 9</GSTRATE>\r\n       </RATEDETAILS.LIST>\r\n       <RATEDETAILS.LIST>\r\n        <GSTRATEDUTYHEAD>IGST</GSTRATEDUTYHEAD>\r\n        <GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>\r\n        <GSTRATE> 18</GSTRATE>\r\n       </RATEDETAILS.LIST>\r\n       <RATEDETAILS.LIST>\r\n        <GSTRATEDUTYHEAD>Cess</GSTRATEDUTYHEAD>\r\n        <GSTRATEVALUATIONTYPE>&#4; Not Applicable</GSTRATEVALUATIONTYPE>\r\n       </RATEDETAILS.LIST>\r\n       <RATEDETAILS.LIST>\r\n        <GSTRATEDUTYHEAD>State Cess</GSTRATEDUTYHEAD>\r\n        <GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>\r\n       </RATEDETAILS.LIST>\r\n       <SUPPLEMENTARYDUTYHEADDETAILS.LIST>       </SUPPLEMENTARYDUTYHEADDETAILS.LIST>\r\n       <TAXOBJECTALLOCATIONS.LIST>       </TAXOBJECTALLOCATIONS.LIST>\r\n       <REFVOUCHERDETAILS.LIST>       </REFVOUCHERDETAILS.LIST>\r\n       <EXCISEALLOCATIONS.LIST>       </EXCISEALLOCATIONS.LIST>\r\n       <EXPENSEALLOCATIONS.LIST>       </EXPENSEALLOCATIONS.LIST>\r\n      </ALLINVENTORYENTRIES.LIST>";

					if (item != null)
					{
						string modifiedadditem = itemxml.Trim()
							  .Replace("<itemname>", item.StockItemName.ToUpper().Trim())
							  .Replace("<itemqnty>", item.ActualQty)
							  .Replace("<itemsum>", item.Amount)
							  .Replace("<ordernumber>", "ORDER" + DateTime.Now.ToString("HH:mm:ss").Replace("-", "").Replace("/", "").Replace(":", ""))
							  .Replace("<itemrate>", item.Rate)
							  .Replace("<trackingno>", "TRACK" + DateTime.Now.ToString("HH:mm:ss").Replace("-", "").Replace("/", "").Replace(":", ""))
							  .Replace("<accoutgroup>", voucher.AccountType);


						addmoreitem += modifiedadditem;

					}
				}

				string modifiedXmlContent = xmlContent.Trim()
				.Replace("<vouchertype>", voucher.VoucherType.ToUpper().Trim())
				.Replace("<date>", voucher.Date.Replace("-", ""))
				.Replace("<customername>", voucher.PartyName.ToUpper().Trim())
				.Replace("<documnetno>", "DOC" + DateTime.Now.ToString("HH:mm:ss").Replace("-", "").Replace("/", "").Replace(":", ""))
				.Replace("<ordernumber>", "ORDER" + DateTime.Now.ToString("HH:mm:ss").Replace("-", "").Replace("/", "").Replace(":", ""))
				.Replace("<vehicleno>", "MH01XX1234")
				.Replace("<trackingno>", "TRACK" + DateTime.Now.ToString("HH:mm:ss").Replace("-", "").Replace("/", "").Replace(":", ""))
				.Replace("<overallamunt>", voucher.overallamount)
				.Replace("<additem>", addmoreitem);

				// Create HTTP request
				var request = new HttpRequestMessage(HttpMethod.Post, tallyUrl)
				{
					Content = new StringContent(modifiedXmlContent, Encoding.UTF8, "text/xml")
				};

				// Send request and get response
				var response = await _httpClient.SendAsync(request);
				response.EnsureSuccessStatusCode();

				var responseContent = await response.Content.ReadAsStringAsync();

				return responseContent;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while communicating with Tally.");
				throw;
			}
		}
	}
}
