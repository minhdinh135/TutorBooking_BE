using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRN231.Constant;
using PRN231.Models;
using PRN231.Models.DTOs.Request;
using PRN231.Models.DTOs.Response;
using PRN231.Repository.Interfaces;
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
        private readonly IGenericRepository<User> _userRepo;
        private readonly UserManager<User> _manager;

        public VnPayController(IHttpContextAccessor httpContextAccessor, IVnPayService vnPayService,
            IGenericRepository<User> userRepo, UserManager<User> manager)
        {
            _httpContextAccessor = httpContextAccessor;
            _vnPayService = vnPayService;
            _userRepo = userRepo;
            _manager = manager;
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

        [HttpPost("PayUserCredit")]
        public async Task<ActionResult<ApiResponse>> CreatePaymentForUserCredit([FromBody] CreatePaymentUserRequest createPaymentRequest)
        {
            try
            {
                string payload = _vnPayService.CreatePaymentForUserCredit(createPaymentRequest);

                return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, payload));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }

        [HttpGet("Result/{Id}")]
        public async Task<ActionResult<ApiResponse>> GetPaymentResultUser(int Id)
        {
            var context = _httpContextAccessor.HttpContext;

            try
            {
                int result = _vnPayService.GetPaymentResult();

                string orderId = context.Request.Query["vnp_TxnRef"];
                string amount = context.Request.Query["vnp_Amount"];
                string orderInfo = context.Request.Query["vnp_OrderInfo"];
                string payDate = context.Request.Query["vnp_PayDate"];
                //string userId = context.Request.Query["vnp_Billing_Email"];

                var user = await _userRepo.Get(Id);
                var roleList = await _manager.GetRolesAsync(user);
                var role = roleList.FirstOrDefault() ?? "";
                if (user != null && result == 0){
                    user.Credit = user.Credit + decimal.Parse(amount)/100;
                    await _userRepo.Update(user);
                }
                if(role == "Tutor")
                {
                    return Redirect("http://localhost:3000/ProfileTutor");
                }
                return Redirect("http://localhost:3000/Profile");
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
                string userId = context.Request.Query["vnp_Billing_Email"];

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
