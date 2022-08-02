using System.Security.Cryptography;

namespace Cryptography.Library
{
    public class HybridEncryption
    {
        private readonly AesGCMEncryption _aes = new AesGCMEncryption();

        public EncryptedPacket EncryptData(byte[] original, NewRSA rsaParams,
                                           NewDigitalSignature digitalSignature)
        {
            var sessionKey = GenerateRandomNumber(32);
            var Iv = GenerateRandomNumber(12);

            (byte[] ciphereText, byte[] tag) encrypted =
                _aes.Encrypt(original, sessionKey, Iv, null);


            var encryptedPacket = new EncryptedPacket
            {
                Tag = Convert.ToBase64String(encrypted.tag),
                EncryptedData = Convert.ToBase64String(encrypted.ciphereText),
                Iv = Convert.ToBase64String(Iv),
                EncryptedSessionKey = Convert.ToBase64String(sessionKey)
            };

            return encryptedPacket;
        }

        public byte[] DecryptData(EncryptedPacket encryptedPacket, NewRSA rsaParams,
                                  NewDigitalSignature digitalSignature)
        {
            var decryptedData = _aes.Decrypt(Convert.FromBase64String(encryptedPacket.EncryptedData),
                                             Convert.FromBase64String(encryptedPacket.EncryptedSessionKey),
                                             Convert.FromBase64String(encryptedPacket.Iv),
                                             Convert.FromBase64String(encryptedPacket.Tag),
                                             null);

            return decryptedData;
        }



        public static byte[] GenerateRandomNumber(int length)
        {
            return RandomNumberGenerator.GetBytes(length);
        }
    }
}
