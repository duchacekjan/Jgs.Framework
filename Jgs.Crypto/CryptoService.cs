using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using resx = Jgs.Crypto.Resources.Resources;

namespace Jgs.Crypto
{
    public class CryptoService
    {
        private readonly CspParameters m_cspp = new CspParameters();
        private readonly Operation m_operation;
        private RSACryptoServiceProvider m_rsa;
        private string m_source;
        private string m_target;
        private DataType m_dataType;
        private const string KeyName = "{8B3445C2-9A1C-44EF-A5D1-74A152982D3C}";

        private enum Operation
        {
            Encrypt,
            Decrypt
        }

        private CryptoService(Operation operation)
        {
            m_operation = operation;
            SetPassword(null);
        }

        public static CryptoService Encrypt()
        {
            return new CryptoService(Operation.Encrypt);
        }

        public static CryptoService Decrypt()
        {
            return new CryptoService(Operation.Decrypt);
        }

        public CryptoService File(string sourceFile)
        {
            m_source = sourceFile;
            m_dataType = DataType.File;
            return this;
        }

        public CryptoService Text(string sourceText)
        {
            m_source = sourceText;
            m_dataType = DataType.Text;
            return this;
        }

        public CryptoService WithPassword(string password)
        {
            SetPassword(password);

            return this;
        }

        public CryptoService ToFile(string targetFile)
        {
            m_target = targetFile;
            return this;
        }

        public CryptoServiceResult Result => GetResult();

        private CryptoServiceResult GetResult()
        {
            CryptoServiceResult result;
            switch (m_dataType)
            {
                case DataType.File:
                    var fResult = ExecuteFile(m_source, m_target, out var errorMessageF);
                    result = fResult
                        ? CryptoServiceResult.Ok(m_target)
                        : CryptoServiceResult.Error(errorMessageF);
                    break;
                case DataType.Text:
                    var tResult = ExecuteText(m_source, out var target, out var errorMessageT);
                    result = tResult
                        ? CryptoServiceResult.Ok(target)
                        : CryptoServiceResult.Error(errorMessageT);
                    break;
                default:
                    result = CryptoServiceResult.Error(resx.UnknownDataType);
                    break;
            }

            return result;
        }

        private bool ExecuteText(string source, out string destination, out string errorMessage)
        {
            var result = false;
            errorMessage = resx.UnknownOperation;
            destination = string.Empty;

            switch (m_operation)
            {
                case Operation.Encrypt:
                    result = EncryptText(source, out destination, out errorMessage);
                    break;
                case Operation.Decrypt:
                    result = DecryptText(source, out destination, out errorMessage);
                    break;
            }

            return result;
        }

        private bool DecryptText(string source, out string destination, out string errorMessage)
        {
            return ExecuteTextSafe(source, out destination, (s) =>
            {
#if NET48
                using (var sourceStream = PrepareStreamForDecryption(ToStream(s), out var blockSize, out var transform))
                {
                    using (var destStream = ToStream(""))
                    {
                        return ProcessCryptoStream(sourceStream, destStream, blockSize, transform);
                    }
                }
#else
                using var sourceStream = PrepareStreamForDecryption(ToStream(s), out var blockSize, out var transform);
                using var destStream = ToStream("");

                return ProcessCryptoStream(sourceStream, destStream, blockSize, transform);
#endif
            }, out errorMessage);
        }

        private bool EncryptText(string source, out string destination, out string errorMessage)
        {
            return ExecuteTextSafe(source, out destination, (s) =>
            {
#if NET48
                using (var destStream = PrepareStreamForEncryption(ToStream(""), out var blockSize, out var transform))
                {
                    using (var sourceStream = ToStream(s))
                    {
                        return ProcessCryptoStream(sourceStream, destStream, blockSize, transform);
                    }
                }
#else
                using var destStream = PrepareStreamForEncryption(ToStream(""), out var blockSize, out var transform);
                using var sourceStream = ToStream(s);

                return ProcessCryptoStream(sourceStream, destStream, blockSize, transform);
#endif
            }, out errorMessage);
        }

        private bool ExecuteFile(string source, string destination, out string errorMessage)
        {
            var result = false;
            errorMessage = resx.UnknownOperation;
            switch (m_operation)
            {
                case Operation.Encrypt:
                    result = EncryptFile(source, destination, out errorMessage);
                    break;
                case Operation.Decrypt:
                    result = DecryptFile(source, destination, out errorMessage);
                    break;
            }

            return result;
        }

        private bool DecryptFile(string source, string destination, out string errorMessage)
        {
            return ExecuteFileSafe(source, destination, (s, d) =>
            {
#if NET48
                using (var sourceStream = PrepareStreamForDecryption(ToFileStream(source, FileMode.Open), out var blockSize, out var transform))
                {
                    using (var destStream = new FileStream(destination, FileMode.Create))
                    {
                        ProcessCryptoStream(sourceStream, destStream, blockSize, transform);
                    }
                }
#else
                using var sourceStream = PrepareStreamForDecryption(ToFileStream(source, FileMode.Open), out var blockSize, out var transform);
                using var destStream = new FileStream(destination, FileMode.Create);
                ProcessCryptoStream(sourceStream, destStream, blockSize, transform);
#endif
            }, out errorMessage);
        }

        private bool EncryptFile(string source, string destination, out string errorMessage)
        {
            return ExecuteFileSafe(source, destination, (s, d) =>
            {
#if NET48
                using (var destStream = PrepareStreamForEncryption(ToFileStream(d, FileMode.Create), out var blockSize, out var transform))
                {
                    using (var sourceStream = ToFileStream(s, FileMode.Open))
                    {
                        ProcessCryptoStream(sourceStream, destStream, blockSize, transform);
                    }
                }
#else
                using var destStream = PrepareStreamForEncryption(ToFileStream(d, FileMode.Create), out var blockSize, out var transform);
                using var sourceStream = ToFileStream(s, FileMode.Open);

                ProcessCryptoStream(sourceStream, destStream, blockSize, transform);
#endif
            }, out errorMessage);
        }

        private static bool ExecuteFileSafe(string source, string destination, Action<string, string> action, out string errorMessage)
        {
            var result = false;
            errorMessage = string.Empty;
            try
            {
                if (System.IO.File.Exists(source))
                {
                    var path = Path.GetDirectoryName(destination);
                    if (!string.IsNullOrEmpty(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    action?.Invoke(source, destination);
                    result = true;
                }
                else
                {
                    errorMessage = resx.FileNotFound;
                }
            }
            catch (Exception e)
            {
                if (e.HResult == 87)
                {
                    errorMessage = resx.IncorrectPassword;
                }
                else
                {
                    errorMessage = e.Message;
                }
            }

            return result;
        }

        private static bool ExecuteTextSafe(string source, out string destination, Func<string, string> func, out string errorMessage)
        {
            var result = false;
            errorMessage = string.Empty;
            destination = string.Empty;
            try
            {
                if (func != null)
                {
                    destination = func.Invoke(source);
                    result = true;
                }
            }
            catch (Exception e)
            {
                if (e.HResult == 87)
                {
                    errorMessage = resx.IncorrectPassword;
                }
                else
                {
                    errorMessage = e.Message;
                }
            }

            return result;
        }

        private void SetPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                password = KeyName;
            }

            m_cspp.KeyContainerName = password;
            m_rsa = new RSACryptoServiceProvider(m_cspp)
            {
                PersistKeyInCsp = true
            };
        }

        private Stream PrepareStreamForDecryption(Stream source, out int blockSize, out ICryptoTransform transform)
        {
            var aes = Aes.Create();

            var LenK = new byte[4];
            var LenIV = new byte[4];
            source.Read(LenK, 0, 4);
            source.Read(LenIV, 0, 4);
            int lenK = BitConverter.ToInt32(LenK, 0);
            int lenIV = BitConverter.ToInt32(LenIV, 0);

            var keyEncrypted = new byte[lenK];
            var iV = new byte[lenIV];
            source.Read(keyEncrypted, 0, lenK);
            source.Read(iV, 0, lenIV);

            var keyDecrypted = m_rsa.Decrypt(keyEncrypted, false);

            blockSize = aes.BlockSize;
            transform = aes.CreateDecryptor(keyDecrypted, iV);
            return source;
        }

        private Stream PrepareStreamForEncryption(Stream destination, out int blockSize, out ICryptoTransform transform)
        {
            var aes = Aes.Create();
            var keyEncrypted = m_rsa.Encrypt(aes.Key, false);

            var lKey = keyEncrypted.Length;
            var lenK = BitConverter.GetBytes(lKey);
            var lIV = aes.IV.Length;
            var lenIV = BitConverter.GetBytes(lIV);

            destination.Write(lenK, 0, 4);
            destination.Write(lenIV, 0, 4);
            destination.Write(keyEncrypted, 0, lKey);
            destination.Write(aes.IV, 0, lIV);

            blockSize = aes.BlockSize;
            transform = aes.CreateEncryptor();
            return destination;
        }

        private static FileStream ToFileStream(string fileName, FileMode mode)
        {
            return new FileStream(fileName, mode);
        }

        private MemoryStream ToStream(string text, Encoding encoding = null)
        {
            var result = new MemoryStream();
            byte[] data;
            switch (m_operation)
            {
                case Operation.Encrypt:
                case Operation.Decrypt when string.IsNullOrEmpty(text):
                    data = (encoding ?? Encoding.UTF8).GetBytes(text);
                    break;
                case Operation.Decrypt:
                    data = Convert.FromBase64String(text);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(null, nameof(Operation));
            }
            result.Write(data, 0, data.Length);
            result.Seek(0, SeekOrigin.Begin);

            return result;
        }

        private string FromStream(Stream stream, Encoding encoding = null)
        {
            byte[] bytes;
            stream.Seek(0, SeekOrigin.Begin);
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            var result = string.Empty;
            switch (m_operation)
            {
                case Operation.Encrypt:
                    result = Convert.ToBase64String(bytes);
                    break;
                case Operation.Decrypt:
                    result = (encoding ?? Encoding.UTF8).GetString(bytes);
                    break;
                default:
                    break;
            }

            return result;
        }

#if NET48
        private string ProcessCryptoStream(Stream from, Stream to, int blockSize, ICryptoTransform transform)
        {
            var result = string.Empty;
            using (var cryptoStream = new CryptoStream(to, transform, CryptoStreamMode.Write))
            {
                var count = 0;
                var blockSizeBytes = blockSize / 8;
                var data = new byte[blockSizeBytes];
                var bytesRead = 0;

                do
                {
                    count = from.Read(data, 0, blockSizeBytes);
                    cryptoStream.Write(data, 0, count);
                    bytesRead += blockSizeBytes;
                }
                while (count > 0);
                cryptoStream.FlushFinalBlock();
                if (to is MemoryStream ms)
                {
                    result = FromStream(ms);
                }

                return result;
            }
        }
#else
        private string ProcessCryptoStream(Stream from, Stream to, int blockSize, ICryptoTransform transform)
        {
            var result = string.Empty;
            using var cryptoStream = new CryptoStream(to, transform, CryptoStreamMode.Write);
            var count = 0;
            var blockSizeBytes = blockSize / 8;
            var data = new byte[blockSizeBytes];
            var bytesRead = 0;

            do
            {
                count = from.Read(data, 0, blockSizeBytes);
                cryptoStream.Write(data, 0, count);
                bytesRead += blockSizeBytes;
            }
            while (count > 0);
            cryptoStream.FlushFinalBlock();
            if (to is MemoryStream ms)
            {
                result = FromStream(ms);
            }

            return result;
        }
#endif
    }
}
