using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using iTextSharp.text.pdf.codec.wmf;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Asn1.Crmf;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using Interaction = DOTP_BE.ViewModel.Interaction;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using IronBarCode;
using cbQRWriter = IronBarCode.QRCodeWriter;

namespace DOTP_BE.Repositories
{
    public class TransactionRepo : ITransaction
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        string hashValue = "";
        string signatureString = "";
        string key = "";
        string merchantId = "";
        string sessionId = "";
        string sessionVersion = "";
        string successIndicator = "";
        string orderId = "";
        string Vdescription = "";
        string url = "";
        StringContent data;
        public TransactionRepo(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }
        public async Task<List<Transaction>> getTransactionList()
        {
            var result = await _context.Transactions.ToListAsync();
            return result;
        }
        public async Task<List<Transaction>> getTransactionListByTransactionNumber(int TransactionNumber)
        {
            var result = await _context.Transactions.Where(s => s.TransactionId == TransactionNumber).ToListAsync();
            return result;
        }
        public async Task<Transaction> getTransactionById(int id)
        {
            var Transaction = await _context.Transactions.Where(s => s.TransactionId == id).FirstOrDefaultAsync();
            return Transaction;
        }
        public bool Create (TransactionVM TransactionVM)
        {
            var Transaction = new Transaction()
            {
                Transaction_Id = TransactionVM.Transaction_Id,
                ChalenNumber = TransactionVM.ChalenNumber,
                NRC_Number = TransactionVM.NRC_Number,
                RegistrationFees = TransactionVM.RegistrationFees,
                CertificateFees = TransactionVM.CertificateFees,
                PartOneFees = TransactionVM.PartOneFees,
                PartTwoFees = TransactionVM.PartTwoFees,
                TriangleFees = TransactionVM.TriangleFees,
                ModifiedCharges = TransactionVM.ModifiedCharges,
                TotalCars = TransactionVM.TotalCars,
                Total_WithoutCertificate = TransactionVM.Total_WithoutCertificate,
                Total = TransactionVM.Total,
                //IsDeleted = false,
                //IsAccpected = TransactionVM.IsAccpected,
                //IsRejected = TransactionVM.IsRejected,
                //IsPaid = TransactionVM.IsPaid,
                //IsPrinted = TransactionVM.IsPrinted,
                Status = TransactionVM.Status,
                AccpectedBy = TransactionVM.AccpectedBy,
                AccpectedAt = TransactionVM.AccpectedAt,
                PrintedAt = TransactionVM.PrintedAt

            };
            _context.Transactions.Add(Transaction);
            _context.SaveChanges();
            return true;
        }
        public bool Update(int id, TransactionVM TransactionVM)
        {
            var Transaction = _context.Transactions.Find(id);
            if (Transaction != null)
            {
                Transaction.Transaction_Id = TransactionVM.Transaction_Id;
                Transaction.ChalenNumber = TransactionVM.ChalenNumber;
                Transaction.NRC_Number = TransactionVM.NRC_Number;
                Transaction.RegistrationFees = TransactionVM.RegistrationFees;
                Transaction.CertificateFees = TransactionVM.CertificateFees;
                Transaction.PartOneFees = TransactionVM.PartOneFees;
                Transaction.PartTwoFees = TransactionVM.PartTwoFees;
                Transaction.TriangleFees = TransactionVM.TriangleFees;
                Transaction.ModifiedCharges = TransactionVM.ModifiedCharges;
                Transaction.TotalCars = TransactionVM.TotalCars;
                Transaction.Total_WithoutCertificate = TransactionVM.Total_WithoutCertificate;
                Transaction.Total = TransactionVM.Total;
                //Transaction.IsDeleted = TransactionVM.IsDeleted;
                //Transaction.IsAccpected = TransactionVM.IsAccpected;
                //Transaction.IsRejected = TransactionVM.IsRejected;
                //Transaction.IsPaid = TransactionVM.IsPaid;
                //Transaction.IsPrinted = TransactionVM.IsPrinted;
                Transaction.Status = TransactionVM.Status;
                Transaction.AccpectedBy = TransactionVM.AccpectedBy;
                Transaction.AccpectedAt = TransactionVM.AccpectedAt;
                Transaction.PrintedAt = TransactionVM.PrintedAt;

                _context.Transactions.Update(Transaction);
                _context.SaveChanges();
                return true;
            };
            return false;
        }
        public void Delete(int id)
        {
            var Transaction = _context.Transactions.Find(id);
            if (Transaction != null)
            {
                _context.Transactions.Remove(Transaction);
                _context.SaveChanges();
            }

        }


        //for Visa/Master payment mwl-10-03-23
        #region visamaster transaction (Hosted Checkout)
        public async Task<CheckoutSessionModel> UpdateAfterMasterPayment(string chaleanNumber)
        {

            var tData = await _context.Transactions.Where(s => s.ChalenNumber == chaleanNumber).FirstOrDefaultAsync();
            var VehicleData = await _context.Vehicles.Where(s => s.ChalenNumber == chaleanNumber).FirstOrDefaultAsync();
            Vdescription = VehicleData.FormMode;

            decimal totalAmount = tData.Total_WithoutCertificate;

            merchantId = _configuration.GetValue<string>("MPGS_Setting:MasterCard_MerchantID");
            orderId = tData.ChalenNumber;

            var result = GetMasterCardCheckoutSession(totalAmount);

            string sessionStr = JsonSerializer.Serialize(result); // extra line for data return if we need more Cols data

            CheckoutSessionModel cSM = new CheckoutSessionModel()
            {
                SessionId = result.Result.session.id,
                Version = result.Result.session.version,
                SuccessIndicator = result.Result.successIndicator,
                MerchantId = merchantId,
                OrderId = orderId,
                SsUrl = _configuration.GetValue<string>("MPGS_Setting:MasterCard_Url"),
                Amount = totalAmount,
                data = data,
                Description = Vdescription
                //version = 
            };

            return cSM;
        }

        public async Task<MasterCardRes> GetMasterCardCheckoutSession(decimal Tamount)
        {
            HttpClient _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_httpClient.DefaultRequestHeaders.Add("content-type", "application/json");
            //_httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            _httpClient.DefaultRequestHeaders.Add("Authorization", GetMasterCardAuthorization());


            //string url = _configuration.GetValue<string>("MPGS_Setting:MasterCard_Url") +
            //           $"/api/rest/version/45/merchant/{merchantId}/session";

            string url = _configuration.GetValue<string>("MPGS_Setting:MasterCard_Url") +
                       $"/api/rest/version/57/merchant/{merchantId}/session";

            MasterCardReq mcr = new MasterCardReq()
            {
                apiOperation = "CREATE_CHECKOUT_SESSION",

                //apiOperation = "INITIATE_CHECKOUT",
                //apiPassword = _configuration.GetValue<string>("MPGS_Setting:MasterCard_APIKey"),
                //apiUsername = "merchant." + merchantId ,
                //merchant = merchantId,

                interaction = new Interaction()
                {
                    operation = "PURCHASE",
                    //merchant = new Merchant()
                    //{
                    //    name = "Postal Office Kiosk"  /// Ball
                    //}
                },

                order = new Order()
                {
                    id = orderId,
                    amount = Tamount,
                    currency = "MMK",
                    //description = Vdescription
                }
            };
            var json = JsonSerializer.Serialize(mcr);
            data = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, data);
                var resBody = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                {
                    var res = JsonSerializer.Deserialize<MasterCardRes>(resBody);

                    sessionId = res.session.id;
                    sessionVersion = res.session.version;
                    successIndicator = res.successIndicator;
                    return res;
                }
                else
                {
                    Debug.WriteLine("Bad Request");
                    Debug.WriteLine(resBody);
                    Debug.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error ");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                return null;
            }

        }

        public string GetMasterCardAuthorization()
        {
            string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(
                "merchant." + _configuration.GetValue<string>("MPGS_Setting:MasterCard_MerchantID") + ":" +
                _configuration.GetValue<string>("MPGS_Setting:MasterCard_APIKey")));

            return "Basic " + credentials;
        }

        public async Task<MasterCardCheckTransactionResponse> MPGSResult(string OrderID)
        {
            //var VehicleData = await _context.Vehicles.Where(s => s.ChalenNumber == OrderID).FirstOrDefaultAsync();
            HttpClient _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", GetMasterCardAuthorization());

            merchantId = _configuration.GetValue<string>("MPGS_Setting:MasterCard_MerchantID");
            string url = _configuration.GetValue<string>("MPGS_Setting:MasterCard_Url") +
                        $"/api/rest/version/57/merchant/{merchantId}/order/{OrderID}";


                var response = await _httpClient.GetAsync(url);
                var resBody = await response.Content.ReadAsStringAsync();

                //if (response.StatusCode == HttpStatusCode.OK)
                //{
                    var res = JsonSerializer.Deserialize<MasterCardCheckTransactionResponse>(resBody);
                    //if (res.result.ToLower() == "success")
                    //{
                        return res;
                    //}

        }

        #endregion

        #region MasterPay With SimplePay

        //public async Task<string> MasterSimplePay(MPGS_SimplePay mPGS_SimplePay)
        //{
        //    merchantId = _configuration.GetValue<string>("MPGS_Setting:MasterCard_MerchantID");
        //    var tData = await _context.Transactions.Where(s => s.ChalenNumber == mPGS_SimplePay.chalenNumber).FirstOrDefaultAsync();
        //    var version = 62;
        //    orderId = mPGS_SimplePay.chalenNumber;
        //    var TrId = "1234502";
        //    var cardNo = mPGS_SimplePay.cardNumber.Trim();
        //    var sofexpmonth = mPGS_SimplePay.expMonth.Trim();
        //    var sofexpyear = mPGS_SimplePay.expYear.Trim();
        //    var sofsecuritycode = mPGS_SimplePay.securityCode.Trim();
        //    var softype = "CARD";
        //    var operation = "PAY";
        //    var oamount = mPGS_SimplePay.amount;
        //    var ocurrency = "MMK";

        //    //url = _configuration.GetValue<string>("MPGS_Setting:MasterCard_Url") +
        //    //           $"/api/rest/version/62/merchant/{merchantId}/order/{orderId}/transaction/{TrId}";

        //    var datatopost = new
        //    {
        //        apiOperation = operation,
        //        order = new
        //        {
        //            amount = oamount,
        //            currency = ocurrency
        //        },
        //        sourceOfFunds = new
        //        {
        //            type = softype,
        //            provided = new
        //            {
        //                card = new
        //                {
        //                    number = cardNo,
        //                    expiry = new { month = sofexpmonth, year = sofexpyear },
        //                    securityCode = sofsecuritycode
        //                }
        //            }
        //        }
        //    };

        //    string ssurl = $"/api/rest/version/62/merchant/{merchantId}/order/{orderId}/transaction/{TrId}";

        //    string baseurl = _configuration.GetValue<string>("MPGS_Setting:MasterCard_Url");

        //    var client = new RestClient(baseurl);
        //    var request = new RestRequest(ssurl, Method.Put);

        //    var res = reqestAndresponse(datatopost, client, request);

        //    /*Session["response"] = res*/
        //    ;

        //    return res;
        //}


        //public string reqestAndresponse(object datatopost, RestClient client, RestRequest request)
        //{
        //    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 |
        //                                                    System.Net.SecurityProtocolType.Tls12;
        //    var encodedcredential = generateEncodedCredential();
        //    request.RequestFormat = DataFormat.Json;
        //    request.AddHeader("content-type", "application/json");
        //    request.AddHeader("charset", "UTF-8");
        //    request.AddHeader("cache-control", "no-cache");
        //    request.AddHeader("Authorization", encodedcredential);
        //    request.AddBody(datatopost);
        //    RestSharp.RestResponse response = client.Execute(request);
        //    return response.Content.ToString();

        //}

        //public string generateEncodedCredential()
        //{
        //    string merchant = "merchant." + merchantId;
        //    string authData = string.Format("{0}:{1}", merchant, _configuration.GetValue<string>("MPGS_Setting:MasterCard_APIKey"));
        //    string encodedcredential = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

        //    encodedcredential = "Basic " + encodedcredential;
        //    return encodedcredential;
        //}
        #endregion

        //for MPU payment mwl-10-03-23
        #region MPUPayment
        public async Task<MPUPaymentReqVM> MPUPayMentTransaction(string ChalenNumber)
        {

            var Transaction = await _context.Transactions.Where(s => s.ChalenNumber == ChalenNumber).FirstOrDefaultAsync();
            var MPUAmount = Transaction.Total_WithoutCertificate;
            var Vehicle = await _context.Vehicles.Where(s => s.ChalenNumber == ChalenNumber && s.Transaction_Id == Transaction.Transaction_Id)
                .FirstOrDefaultAsync();
            if (Transaction != null)
            {
                Transaction.Transaction_Id = Transaction.Transaction_Id;
                signatureString = getsignatureString(ChalenNumber, MPUAmount, Vehicle.FormMode, "any test");
                hashValue = getHMAC(signatureString, _configuration.GetValue<string>("MPUPayment:MPU_Key"));
            }
            MPUPaymentReqVM mPUPaymentReqVM = new MPUPaymentReqVM()
            {

                Version = "2.0",
                merchantID = _configuration["MPUPayment:MPU_MerchantID"],
                invoiceNo = ChalenNumber,
                amount = getAmount(MPUAmount),
                currencyCode = 104,
                productDesc = Vehicle.FormMode,
                hashValue = hashValue,
                FrontendURL = _configuration.GetValue<string>("MPUPayment:MPU_FrontEndUrl")
            };

            return mPUPaymentReqVM;
        }
        public string getHMAC(string signatureString, string secretKey)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secretKey);
            HMACSHA1 hmac = new HMACSHA1(keyByte);
            byte[] messageBytes = encoding.GetBytes(signatureString);
            byte[] hashmessage = hmac.ComputeHash(messageBytes);
            return ByteArrayToHexString(hashmessage); //Convert Byte to String.
        }
        public string ByteArrayToHexString(byte[] Bytes)
        {
            StringBuilder Result = new StringBuilder(Bytes.Length * 2);
            string HexAlphabet = "0123456789ABCDEF";
            foreach (byte B in Bytes)
            {
                Result.Append(HexAlphabet[(int)(B >> 4)]);
                Result.Append(HexAlphabet[(int)(B & 0xF)]);
            }
            return Result.ToString();
        }

        public string getsignatureString(string ChalenNumber, decimal TotAmount, string FormMode, string userDefined2)
        {
            string amount = getAmount(TotAmount);
            string SString = "";

            SString = _configuration["MPUPayment:MPU_MerchantID"] + ChalenNumber + "PaidTransaction" + amount + "104" + FormMode
                       + userDefined2 + "userDefined3";

            return SString;
        }

        public string getAmount(decimal Amount)
        {
            string TAmount = Amount.ToString();
            string[] amountArray = TAmount.Split('.');
            string formattedAmount = amountArray[0] + "00";
            int zerosCountToAdd = 12 - formattedAmount.Length;
            string zerosToAdd = "";
            for (int i = 0; i < zerosCountToAdd; i++)
            {
                zerosToAdd += "0";
            }
            TAmount = zerosToAdd + formattedAmount;

            return TAmount;
        }

        //get MPU Request Setting 

        public Object GetMPUPaymentReq(string ChalenNumber, string FormMode, decimal TotalAmount, string xashValue)
        {
            var data = new MPUPaymentReqVM()
            {
                Version = "2.0",
                merchantID = _configuration["MPUPayment:MPU_MerchantID"],
                invoiceNo = ChalenNumber,
                amount = getAmount(TotalAmount),
                currencyCode = 104,
                categoryCode = FormMode,
                hashValue = xashValue
            };

            return data;
        }

        #endregion

        //For CB Pay
        #region CBpayment  THA    

        public async Task<CBPaymentReqVM> GetCBvmRequest(string ChalenNumber)
        {
            CBPaymentReqVM cbVM = new CBPaymentReqVM();
            var Transaction = await _context.Transactions.Where(s => s.ChalenNumber == ChalenNumber).FirstOrDefaultAsync();

            if (Transaction != null)
            {
                var Amount = Transaction.Total_WithoutCertificate;
                var Vehicle = await _context.Vehicles.Where(s => s.ChalenNumber == ChalenNumber && s.Transaction_Id == Transaction.Transaction_Id)
                    .FirstOrDefaultAsync();
                Transaction.Transaction_Id = Transaction.Transaction_Id;
                signatureString = getsignatureString(ChalenNumber, Transaction.Total_WithoutCertificate, Vehicle.FormMode, "any test");
                hashValue = getHMAC(signatureString, _configuration.GetValue<string>("CBPayment:CB_AuthenToken"));
                cbVM = new CBPaymentReqVM()
                {
                    //Version = "2.0",
                    invoiceNo = ChalenNumber,
                    merchantID = _configuration.GetSection("CBPayment:CB_MerchantID").Value,
                    subMerchantId = _configuration.GetSection("CBPayment:CB_Sub_MarchantId").Value,
                    terminalId = _configuration.GetSection("CBPayment:CB_TerminalId").Value,
                    amount = getAmount(Amount),
                    currency = _configuration.GetSection("CBPayment:Currency").Value,
                    productDesc = Vehicle.FormMode,
                    hashValue = hashValue,
                    qrURL = _configuration.GetValue<string>("CBPayment:CB_QrGenerate_URL"),
                    checkURL = _configuration.GetValue<string>("CBPayment:CB_CheckTransaction_URL"),
                    aut_Token = _configuration.GetValue<string>("CBPayment:CB_AuthenToken")
                };
            }

            return cbVM;
        }

        public async Task<CBPaymentReqVM> GetQrString_FromCBBank(string ChalenNumber)
        {
            HttpClient _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authen-Token", _configuration.GetSection("CBPayment:CB_AuthenToken").Value);

            string url = _configuration.GetSection("CBPayment:CB_QrGenerate_URL").Value;

            CBPaymentReqVM cbVM = new CBPaymentReqVM();
            var Transaction = await _context.Transactions.Where(s => s.ChalenNumber == ChalenNumber).FirstOrDefaultAsync();
            if (Transaction != null)
            {
                var Amount = Transaction.Total_WithoutCertificate;
                var Vehicle = await _context.Vehicles.Where(s => s.ChalenNumber == ChalenNumber && s.Transaction_Id == Transaction.Transaction_Id)
                    .FirstOrDefaultAsync();

                CBPayQrRequest reqBody = new CBPayQrRequest()
                {
                    reqId = ChalenNumber,
                    merId = _configuration.GetSection("CBPayment:CB_MerchantID").Value,
                    subMerId = _configuration.GetSection("CBPayment:CB_Sub_MarchantId").Value,
                    terminalId = _configuration.GetSection("CBPayment:CB_TerminalId").Value,
                    transAmount = Amount.ToString(), //getAmount(Amount),
                    transCurrency = _configuration.GetSection("CBPayment:Currency").Value,
                    ref1 = Vehicle.FormMode

                };

                var json = JsonSerializer.Serialize(reqBody);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    var response = await _httpClient.PostAsync(url, data);
                    var resBody = await response.Content.ReadAsStringAsync();
                    var res = JsonSerializer.Deserialize<CBPayQrResponse>(resBody);
                    if (res != null && res.code == "0000" && res.merDqrCode != null)
                    {
                        cbVM.invoiceNo = reqBody.reqId;
                        cbVM.TransactionRefNo = res.transRef;
                        cbVM.amount = Amount.ToString();
                        //cbVM.qrCodeData = res.merDqrCode;
                        qrGenerator(res.merDqrCode);
                        cbVM.qrPath = _configuration.GetSection("Qr_Path").Value;


                        Debug.WriteLine(resBody);
                    }
                    else
                    {
                        Debug.WriteLine(resBody);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                }
            }


            return cbVM;
        }

        public async Task<CBPayCheckResponse> CheckTransaction_FromCBBank(string transactionRefNo)
        {
            HttpClient _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authen-Token", _configuration.GetSection("CBPayment:CB_AuthenToken").Value);

            string url = _configuration.GetSection("CBPayment:CB_CheckTransaction_URL").Value;

            CBPayCheckResponse cbResponse = new CBPayCheckResponse();
            CBPayCheckTransactionRequest reqBody = new CBPayCheckTransactionRequest()
            {
                merId = _configuration.GetSection("CBPayment:CB_MerchantID").Value,
                transRef = transactionRefNo
            };
            var json = JsonSerializer.Serialize(reqBody);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(url, data);
                var resBody = await response.Content.ReadAsStringAsync();

                var res = JsonSerializer.Deserialize<CBPayCheckResponse>(resBody);
                if (res.code == "0000" && res.transAmount != null && (res.transStatus == "P" || res.transStatus == "S"))
                {
                    cbResponse.transStatus = GetLongTransactionStatus(res.transStatus);
                    cbResponse.code = res.code;
                    cbResponse.msg = res.msg;
                    cbResponse.bankTransId = res.bankTransId;
                    cbResponse.transAmount = res.transAmount;
                    cbResponse.transCurrency = res.transCurrency;

                    Debug.WriteLine(resBody);
                }
                else
                {
                    cbResponse.transStatus = GetLongTransactionStatus(res.transStatus);
                    Debug.WriteLine(resBody);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                //return false;
            }
            return cbResponse;
        }

        private void qrGenerator(string qrData)
        {
            ////simple code
            //GeneratedBarcode Qrcode = QRCodeWriter.CreateQrCode(textBox1.Text);
            //Qrcode.SaveAsPng("QrCode.png");

            string floderPath = _configuration.GetSection("Qr_Floder_Path").Value;
            if (!Directory.Exists(floderPath))
                Directory.CreateDirectory(floderPath);

            string savePath = _configuration.GetSection("Upload_FolderPath").Value + _configuration.GetSection("Qr_Path").Value;
            GeneratedBarcode Qrcode = cbQRWriter.CreateQrCode(qrData);
            Qrcode.SaveAsPng(savePath);
        }
        public string GetLongTransactionStatus(string shortCode)
        {
            string str = "";
            switch (shortCode)
            {
                case "P": str = "Pending"; break;
                case "S": str = "Success"; break;
                case "E": str = "Expired"; break;
                case "C": str = "Cancelled"; break;
                case "L": str = "Over limit"; break;
                case "AP": str = "Approved"; break;
                case "RS": str = "Ready to Settle"; break;
                case "SE": str = "Settled"; break;
                case "VO": str = "Voided"; break;
                case "DE": str = "Declined"; break;
                case "FA": str = "Failed"; break;
                case "RE": str = "Refund Pending"; break;
                case "RR": str = "Refund Ready"; break;
                case "RF": str = "Refunded"; break;
                case "PR": str = "Payment Gateway receive"; break;
                default: str = shortCode; break;
            }
            return str;
        }

        #endregion
    }
}

