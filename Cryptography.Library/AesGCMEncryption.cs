using System.Security.Cryptography;

namespace Cryptography.Library
{
    public class AesGCMEncryption
    {

        public (byte[], byte[]) Encrypt(byte[] dataToEncrypt, byte[] sessionKey, byte[] Iv, byte[] associatedData)
        {
            // these will be filled during the encryption
            byte[] tag = new byte[16];
            byte[] ciphertext = new byte[dataToEncrypt.Length];

            using (AesGcm aesGcm = new AesGcm(sessionKey))
            {
                aesGcm.Encrypt(Iv, dataToEncrypt, ciphertext, tag, associatedData);
            }

            return (ciphertext, tag);
        }

        public byte[] Decrypt(byte[] cipherText, byte[] key, byte[] nonce, byte[] tag, byte[] associatedData)
        {
            byte[] decryptedData = new byte[cipherText.Length];

            using (AesGcm aesGcm = new AesGcm(key))
            {
                aesGcm.Decrypt(nonce, cipherText, tag, decryptedData, associatedData);
            }

            return decryptedData;
        }
    }
}
