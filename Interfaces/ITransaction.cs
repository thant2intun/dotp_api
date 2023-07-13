using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface ITransaction
    {
        Task<List<Transaction>> getTransactionList();
        Task<List<Transaction>> getTransactionListByTransactionNumber(int TransactionNumber);
        Task<Transaction> getTransactionById(int id);
        bool Create(TransactionVM TransactionVM);
        void Delete(int id);
        bool Update(int id, TransactionVM TransactionVM);

        Task<CheckoutSessionModel> UpdateAfterMasterPayment(string chaleanNumber);

        //Task<string> MasterSimplePay(MPGS_SimplePay mPGS_SimplePay);

        Task<MasterCardCheckTransactionResponse> MPGSResult(string OrderID);

        Task<MPUPaymentReqVM> MPUPayMentTransaction(string ChalenNumber);

        Task<CBPaymentReqVM> GetQrString_FromCBBank(string ChalenNumber); //THA
        Task<CBPayCheckResponse> CheckTransaction_FromCBBank(string TransactionRefNo);  //THA


    }
}
