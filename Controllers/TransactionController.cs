using DOTP_BE.Interfaces;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransaction _repo;
        public TransactionController(ITransaction repo)
        {
            _repo = repo;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var Transaction = await _repo.getTransactionList();
            return Ok(Transaction);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById([FromRoute] int id)
        {
            var Transaction = await _repo.getTransactionById(id);
            if (Transaction == null)
            {
                return BadRequest();
            }
            return Ok(Transaction);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TransactionVM TransactionVM)
        {
            var Transaction =  _repo.Create(TransactionVM);
            if (Transaction)
            {
                return Ok();
            }
            return BadRequest("Transaction already exists.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionVM TransactionVM)
        {
            var OUpdate =  _repo.Update(id, TransactionVM);
            if (OUpdate)
            {
                return Ok();
            }
            return BadRequest("Transaction already exists.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionById([FromRoute] int id)
        {
            _repo.Delete(id);
            return Ok();
        }

        //MPGS Response Controller

        [HttpGet("[action]/{chalenNumber}")]
        public async Task<IActionResult> getMPGSRes([FromRoute] string chalenNumber)
        {
            var res = await _repo.UpdateAfterMasterPayment(chalenNumber);
            //res = JsonConvert.DeserializeObject<List<MasterCardRes>>(res);
            return Ok(res);
        }

        //MPGS Result Controller
        [HttpGet("[action]/{orderID}")]
        public async Task<IActionResult> getMPGSResults(string orderID)
        {
            var res = await _repo.MPGSResult(orderID);
            return Ok(res);
        }

        //[HttpPost("getResponseFromMPGS_SimplePay")]
        //public async Task<IActionResult> getResponseFromMPGS_SimplePay(MPGS_SimplePay mPGS_SimplePay)
        //{
        //    var res = await _repo.MasterSimplePay(mPGS_SimplePay);
        //    return Ok();
        //}

        //payment_(MPU)
        [HttpGet("MPU_Payment_Transaction/{chalenNumber}")]
        public async Task<IActionResult> MPU_Payment_Transaction(string chalenNumber)
        {
            var res = await _repo.MPUPayMentTransaction(chalenNumber);
            return Ok(res);
        }

        #region CBPAY THA
        [HttpGet("[action]/{chalenNumber}")]
        public async Task<IActionResult> CB_GetQR_Transaction(string chalenNumber)
        {
            var res = await _repo.GetQrString_FromCBBank(chalenNumber);
            return Ok(res);
        }
        // [HttpPost("CB_Check_Transaction")]  // [HttpGet("[action]/{transactionRefNo}")]
        [HttpGet("[action]/{transactionRefNo}")]
        public async Task<IActionResult> CB_Check_Transaction(string transactionRefNo)
        {
            var res = await _repo.CheckTransaction_FromCBBank(transactionRefNo);
            return Ok(res);
        }
        #endregion
    }
}
