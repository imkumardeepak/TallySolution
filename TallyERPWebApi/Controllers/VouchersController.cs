using Microsoft.AspNetCore.Mvc;
using TallyERPWebApi.Model;
using TallyERPWebApi.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TallyERPWebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VouchersController : ControllerBase
	{

		private readonly ILogger<VouchersController> _logger;
		private readonly TallyService _tallyService;
		private readonly PostTallyService _postTallyService;


		public VouchersController(ILogger<VouchersController> logger, TallyService tallyService, PostTallyService postTallyService)
		{
			_logger = logger;
			_tallyService = tallyService;
			_postTallyService = postTallyService;
		}
		// GET: api/<GetVouchersController>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				bool result = await _tallyService.GetTestConnection();
				if (!result)
				{
					_logger.LogWarning("Tally Server is not running");
					return NotFound(new ApiResponse<string>
					{
						Success = false,
						Message = "Tally Server is not running!!!"
					});
				}

				// Path to the XML file
				string xmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TallyXML", "GetVoucher.xml");

				// Check if the XML file exists
				if (!System.IO.File.Exists(xmlFilePath))
				{
					_logger.LogWarning("The specified XML file does not exist: {FilePath}", xmlFilePath);
					return NotFound(new ApiResponse<string>
					{
						Success = false,
						Message = "The specified XML file does not exist."
					});
				}

				// Get the current company from Tally
				List<Voucher> vouchers = await _tallyService.GetVoucherAsync(xmlFilePath);

				// Check if the result is valid
				if (vouchers.Count == 0)
				{
					_logger.LogWarning("The current company returned by Tally is null or empty.");
					return NotFound(new ApiResponse<List<Voucher>>
					{
						Success = false,
						Message = "No current company found in Tally.",
						Data = vouchers
					});
				}

				_logger.LogInformation("Successfully fetched current vouchers: {vouchers}", vouchers);

				// Return success response with company data
				return Ok(new ApiResponse<List<Voucher>>
				{
					Success = true,
					Message = "Current voucherd fetched successfully.",
					Data = vouchers
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while fetching company information from Tally.");
				return StatusCode(500, new ApiResponse<string>
				{
					Success = false,
					Message = "An internal server error occurred. Please try again later."
				});
			}
		}

		[HttpGet("GetVoucherType")]
		public async Task<IActionResult> GetVoucherType()
		{
			try
			{
				bool result = await _tallyService.GetTestConnection();
				if (!result)
				{
					_logger.LogWarning("Tally Server is not running");
					return NotFound(new ApiResponse<string>
					{
						Success = false,
						Message = "Tally Server is not running!!!"
					});
				}

				// Path to the XML file
				string xmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TallyXML", "GetVoucherType.xml");

				// Check if the XML file exists
				if (!System.IO.File.Exists(xmlFilePath))
				{
					_logger.LogWarning("The specified XML file does not exist: {FilePath}", xmlFilePath);
					return NotFound(new ApiResponse<string>
					{
						Success = false,
						Message = "The specified XML file does not exist."
					});
				}

				// Get the current company from Tally
				List<string> group = await _tallyService.GetVoucherType(xmlFilePath);

				// Check if the result is valid
				if (group.Count == 0)
				{
					_logger.LogWarning("The current company returned by Tally is null or empty.");
					return NotFound(new ApiResponse<string>
					{
						Success = false,
						Message = "No current company found in Tally."
					});
				}

				_logger.LogInformation("Successfully fetched current company: {CurrentCompany}", group);

				// Return success response with company data
				return Ok(new ApiResponse<List<string>>
				{
					Success = true,
					Message = "Current company fetched successfully.",
					Data = group
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while fetching company information from Tally.");
				return StatusCode(500, new ApiResponse<string>
				{
					Success = false,
					Message = "An internal server error occurred. Please try again later."
				});
			}
		}

		[HttpPost]
		public async Task<IActionResult> SaveVoucher(Voucher voucher)
		{
			try
			{
				bool result = await _tallyService.GetTestConnection();
				if (!result)
				{
					_logger.LogWarning("Tally Server is not running");
					return NotFound(new ApiResponse<string>
					{
						Success = false,
						Message = "Tally Server is not running!!!"
					});
				}

				// Path to the XML file
				string xmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TallyXML", "CreateVoucher.txt");

				// Check if the XML file exists
				if (!System.IO.File.Exists(xmlFilePath))
				{
					_logger.LogWarning("The specified XML file does not exist: {FilePath}", xmlFilePath);
					return NotFound(new ApiResponse<string>
					{
						Success = false,
						Message = "The specified XML file does not exist."
					});
				}
				// Get the current company from Tally
				string response = await _postTallyService.SaveVoucher(xmlFilePath, voucher);

				// Check if the result is valid
				if (string.IsNullOrEmpty(response))
				{
					_logger.LogWarning("The current company returned by Tally is null or empty.");
					return NotFound(new ApiResponse<string>
					{
						Success = false,
						Message = "No current company found in Tally."
					});
				}

				_logger.LogInformation("Successfully fetched current company: {CurrentCompany}", response);

				// Return success response with company data
				return Ok(new
				{
					Success = true,
					Message = "Current company fetched successfully.",
					Data = response
				});
			}
			catch (Exception ex)
			{

				_logger.LogError(ex, "An error occurred while fetching company information from Tally.");
				return StatusCode(500, new ApiResponse<string>
				{
					Success = false,
					Message = "An internal server error occurred. Please try again later."
				});
			}
		}
	}
}
