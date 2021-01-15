namespace Jgs.Crypto
{
    public class CryptoServiceResult
    {
        private CryptoServiceResult(bool result, string errorMessage, string data)
        {
            Result = result;
            ErrorMessage = errorMessage;
            Data = data;
        }

        public static CryptoServiceResult Ok(string data = null)
        {
            return new CryptoServiceResult(true, string.Empty, data);
        }

        public static CryptoServiceResult Error(string errorMessage)
        {
            return new CryptoServiceResult(false, errorMessage, null);
        }

        public string ErrorMessage { get; }

        public bool Result { get; }

        public string Data { get; }
    }
}
