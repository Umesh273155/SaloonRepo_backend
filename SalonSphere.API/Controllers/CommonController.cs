
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonSphere.Application.Interfaces;

namespace SalonSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CommonController : ControllerBase
    {
        private readonly ICommonService _commonService;
        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpGet("servicecategories")]
        public async Task<IActionResult> GetServiceCategories()
        {
            try
            {
                var categories = await _commonService.GetServiceCategoriesAsync();

                return Ok(new
                {
                    Success = true,
                    Message = "Service categories fetched successfully.",
                    Data = categories
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }


        [HttpGet("designations")]
        public async Task<IActionResult> GetDesignations()
        {
            try
            {
                var response = await _commonService.GetDesignationsAsync();

                if (!response.Success)
                    return BadRequest(response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}

