using Cryptography.Library.Interface;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography.Library
{
    public class HybridEncryption : ICryptography
    {
        private readonly AesGCMEncryption _aes = new AesGCMEncryption();
        public string Encrypt(string original)
        {
            var encryptedBlock = EncryptData(Encoding.UTF8.GetBytes(original));
            var encryptedBlockJson = JsonConvert.SerializeObject(encryptedBlock);
            return encryptedBlockJson;
        }

        public string Decrypt(string data)
        {
            EncryptedPacket encryptedBlock = JsonConvert.DeserializeObject<EncryptedPacket>(data);
            var decrpyted = DecryptData(encryptedBlock);
            var decrpytedString = Encoding.UTF8.GetString(decrpyted);
            return decrpytedString;
        }

        public EncryptedPacket EncryptData(byte[] original)
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

        public byte[] DecryptData(EncryptedPacket encryptedPacket)
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
