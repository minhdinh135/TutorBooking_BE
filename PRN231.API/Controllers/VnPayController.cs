using Microsoft.AspNetCore.Mvc;
using PRN231.Constant;
using PRN231.Models.DTOs.Request;
using PRN231.Models.DTOs.Response;
using PRN231.Services;
using System.Net;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VnPayController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IVnPayService _vnPayService;

        public VnPayController(IHttpContextAccessor httpContextAccessor, IVnPayService vnPayService)
        {
            _httpContextAccessor = httpContextAccessor;
            _vnPayService = vnPayService;
        }

        [HttpPost("Pay")]
        public async Task<ActionResult<ApiResponse>> CreatePayment([FromBody] CreatePaymentRequest createPaymentRequest)
        {
            try
            {
                string payload = _vnPayService.CreatePayment(createPaymentRequest);

                return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, payload));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }

        [HttpGet("Result")]
        public async Task<ActionResult<ApiResponse>> GetPaymentResult()
        {
            var context = _httpContextAccessor.HttpContext;

            try
            {
                int result = _vnPayService.GetPaymentResult();

                string orderId = context.Request.Query["vnp_TxnRef"];
                string amount = context.Request.Query["vnp_Amount"];
                string orderInfo = context.Request.Query["vnp_OrderInfo"];
                string payDate = context.Request.Query["vnp_PayDate"];

                CreatePaymentResponse createPaymentResponse = new CreatePaymentResponse
                {
                    OrderId = orderId,
                    Amount = amount,
                    OrderInfo = orderInfo,
                    PayDate = payDate
                };

                return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, createPaymentResponse));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }

        }
    }
}
