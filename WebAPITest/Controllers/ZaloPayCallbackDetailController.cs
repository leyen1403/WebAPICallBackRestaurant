using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Security.Cryptography;
using WebAPITest.Models;
using ZaloPay.Helper;
using ZaloPay.Helper.Crypto;

namespace WebAPITest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZaloPayCallbackDetailController : Controller
    {
        private readonly RestaurantGLBContext _context;

        public ZaloPayCallbackDetailController(RestaurantGLBContext restaurantGLB)
        {
            _context = restaurantGLB;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _context.WaitPayments.ToList();
            return Ok(result);
        }
        [HttpPost("callback")]
        public async Task<IActionResult> Callback([FromBody] ZaloPayCallbackModel callback)
        {
            var dataJson = callback.data;
            var mac = callback.mac;
            var type = callback.type;
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                ZaloPayCallbackDetail transaction = JsonConvert.DeserializeObject<ZaloPayCallbackDetail>(dataJson);
                if (transaction == null)
                {
                    result["return_code"] = -1;
                    result["return_message"] = "invalid transaction data";
                    return Ok(result);
                }
                string? key2 = _context.MerchantAccountsZalopays.FirstOrDefault(x => x.APP_ID == transaction.AppId.ToString())?.KEY2;
                if (key2 == null)
                {
                    result["return_code"] = -1;
                    result["return_message"] = "Key 2 not found";
                    return Ok(result);
                }
                string reqMac = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key2, dataJson);
                if (!string.Equals(reqMac, mac))
                {
                    result["return_code"] = -1;
                    result["return_message"] = "Invalid MAC";
                    return Ok(result);
                }
                var payment = await _context.WaitPayments.FirstOrDefaultAsync(x => x.TransactionUuid == transaction.AppTransId);
                if (payment == null)
                {
                    result["return_code"] = -1;
                    result["return_message"] = "Transaction not found";
                    return Ok(result);
                }
                if (payment.Status == "5")
                {
                    result["return_code"] = 1;
                    result["return_message"] = "Already processed";
                    return Ok(result);
                }
                payment.StatusOld = payment.Status;
                payment.Status = "5";
                payment.ModifileDate = DateTime.Now;
                await _context.ZaloPayCallbackDetails.AddAsync(transaction);
                await _context.SaveChangesAsync();

                result["return_code"] = 1;
                result["return_message"] = "success";
            }
            catch(Exception ex)
            {
                result["return_code"] = 0; // ZaloPay server sẽ callback lại (tối đa 3 lần)
                result["return_message"] = ex.Message;
            }
            return Ok(result);
        }

        [HttpPost("getlistmerchantbanks")]
        public async Task<IActionResult> GetListMerchantBanks()
        {
            try
            {
                var merchant = await _context.MerchantAccountsZalopays.FirstOrDefaultAsync();
                if (merchant == null)
                {
                    return BadRequest("No merchant account found.");
                }
                var appid = "554";
                var key1 = "8NdU5pG5R2spGHGhyO99HN1OhD8IQJBn";
                var getBankListUrl = "https://sbgateway.zalopay.vn/api/getlistmerchantbanks";
                var reqtime = Utils.GetTimeStamp().ToString();
                var param = new Dictionary<string, string>
                {
                    { "appid", appid },
                    { "reqtime", reqtime },
                    { "mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key1, $"{appid}|{reqtime}") }
                };
                var result = await HttpHelper.PostFormAsync<BankListResponse>(getBankListUrl, param);
                Console.WriteLine("returncode = {0}", result.returncode);
                Console.WriteLine("returnmessage = {0}", result.returnmessage);
                foreach (var entry in result.banks)
                {
                    var pmcid = entry.Key;
                    var banklist = entry.Value;
                    foreach (var bank in banklist)
                    {
                        Console.WriteLine("{0}. {1} - {2}", pmcid, bank.bankcode, bank.name);
                    }
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get bank list", error = ex.Message });
            }
        }
        [HttpPost("CheckOrderStatus")]
        public async Task<IActionResult> CheckOrderStatus([FromBody]string app_trans_id)
        {
            try
            {
                const string query_order_url = "https://sb-openapi.zalopay.vn/v2/query";
                var merchant = await _context.MerchantAccountsZalopays.FirstOrDefaultAsync();
                if (merchant == null)
                {
                    return BadRequest("No merchant account found.");
                }
                var appid = merchant.APP_ID;
                var key1 = merchant.KEY1;
                var data = $"{appid}|{app_trans_id}|{key1}";
                var mac = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key1, data);
                var param = new Dictionary<string, string>
                {
                    { "app_id", appid },
                    { "app_trans_id", app_trans_id },
                    { "mac", mac }
                };
                var result = await HttpHelper.PostFormAsync<ZaloPayQueryResponse>(query_order_url, param);
                return Ok(result);  
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal error", detail = ex.Message });
            }
        }
    }
}
