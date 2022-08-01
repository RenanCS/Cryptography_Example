using System.Security.Cryptography;
using System.Text;

namespace Cryptography.Library
{
    public class HybridEncryption
    {
        private readonly AesGCMEncryption _aes = new AesGCMEncryption();

        public static byte[] ComputeHMACSha256(byte[] toBeHashed, byte[] hmacKey)
        {
            using (var hmacSha256 = new HMACSHA256(hmacKey))
            {
                return hmacSha256.ComputeHash(toBeHashed);
            }
        }

        public EncryptedPacket EncryptData(byte[] original, NewRSA rsaParams,
                                           NewDigitalSignature digitalSignature)
        {
            // Create AES session key.
            var sessionKey = GenerateRandomNumber(32);
            var Iv = GenerateRandomNumber(12);
            var additionalData = Encoding.UTF8.GetBytes("teste_adicional_info");

            // Encrypt data with AES-GCM
            (byte[] ciphereText, byte[] tag) encrypted =
                _aes.Encrypt(original, sessionKey, Iv, additionalData);


            var encryptedPacket = new EncryptedPacket
            {
                Tag = Convert.ToBase64String(encrypted.tag),
                EncryptedData = Convert.ToBase64String(encrypted.ciphereText),
                Iv = Convert.ToBase64String(Iv),
                EncryptedSessionKey = Convert.ToBase64String(sessionKey)

            };

            //// Dica => Criptografa a session key com chave pública de quem disponibilizou
            //encryptedPacket.EncryptedSessionKey = rsaParams.Encrypt(sessionKey);

            //encryptedPacket.SignatureHMAC =
            //    ComputeHMACSha256(
            //        Combine(encryptedPacket.EncryptedData, encryptedPacket.Iv),
            //        sessionKey);

            //encryptedPacket.Signature =
            //    digitalSignature.SignData(encryptedPacket.SignatureHMAC);

            return encryptedPacket;
        }

        public byte[] DecryptData(EncryptedPacket encryptedPacket, NewRSA rsaParams,
                                  NewDigitalSignature digitalSignature)
        {
            var additionalData = Encoding.UTF8.GetBytes("teste_adicional_info");
            var decryptedData = _aes.Decrypt(Convert.FromBase64String(encryptedPacket.EncryptedData),
                                             Convert.FromBase64String(encryptedPacket.EncryptedSessionKey),
                                             Convert.FromBase64String(encryptedPacket.Iv),
                                             Convert.FromBase64String(encryptedPacket.Tag),
                                             additionalData);

            return decryptedData;
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length];

            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);

            return ret;
        }

        private static bool Compare(byte[] array1, byte[] array2)
        {
            var result = array1.Length == array2.Length;

            for (var i = 0; i < array1.Length && i < array2.Length; ++i)
            {
                result &= array1[i] == array2[i];
            }

            return result;
        }

        public static byte[] GenerateRandomNumber(int length)
        {
            return RandomNumberGenerator.GetBytes(length);

            // OLD => 
            //using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            //{
            //    var randomNumber = new byte[length];
            //    randomNumberGenerator.GetBytes(randomNumber);

            //    return randomNumber;
            //}
        }
    }
}
