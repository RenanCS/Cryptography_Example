using Cryptography.Library.Helper;
using Cryptography.Library.Interface;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography.Library
{
    public class NewRSA : ICryptography
    {
        public RSA rsa;

        public NewRSA()
        {
            rsa = RSA.Create(2048);

            CheckKey();
        }

        private void CheckKey()
        {
            string pathPublicKey = "C:\\keys\\public.perm";
            string pathPrivateKey = "C:\\keys\\private.perm.";

            if (!File.Exists(pathPrivateKey))
            {
                throw new Exception("Não foi encontrado as chaves públicas e privadas");
            }

            // OBS: A importação da chave privada sempre tem que ser a última
            WriteReadKey.GetPublicKeyFromPemFile(pathPublicKey, rsa);
            WriteReadKey.GetPrivateKeyFromPemFile(pathPrivateKey, rsa);
        }

        public string Encrypt(string data)
        {
            var encryptedBlock = EncryptString(data);
            var encryptedBlockJson = Convert.ToBase64String(encryptedBlock);
            return encryptedBlockJson;
        }

        public string Decrypt(string data)
        {
            var decrpyted = Decrypt(Convert.FromBase64String(data));
            var decrpytedString = Encoding.UTF8.GetString(decrpyted);
            return decrpytedString;
        }

        public byte[] EncryptString(string dataToEncrypt)
        {
            return rsa.Encrypt(Encoding.UTF8.GetBytes(dataToEncrypt), RSAEncryptionPadding.OaepSHA256);
        }

        public byte[] EncryptByte(byte[] dataToEncrypt)
        {
            return rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.OaepSHA256);
        }

        public byte[] Decrypt(byte[] dataToDecrypt)
        {
            return rsa.Decrypt(dataToDecrypt, RSAEncryptionPadding.OaepSHA256);
        }

        public byte[] ExportPrivateKey(int numberOfIterations, string password)
        {
            byte[] encryptedPrivateKey = new byte[2000];

            PbeParameters keyParams = new PbeParameters(PbeEncryptionAlgorithm.Aes256Cbc, HashAlgorithmName.SHA256, numberOfIterations);
            encryptedPrivateKey = rsa.ExportEncryptedPkcs8PrivateKey(Encoding.UTF8.GetBytes(password), keyParams);

            return encryptedPrivateKey;
        }

        public void ImportEncryptedPrivateKey(byte[] encryptedKey, string password)
        {
            rsa.ImportEncryptedPkcs8PrivateKey(Encoding.UTF8.GetBytes(password), encryptedKey, out _);
        }

        public byte[] ExportPublicKey()
        {
            return rsa.ExportRSAPublicKey();
        }

        public void ImportPublicKey(byte[] publicKey)
        {
            rsa.ImportRSAPublicKey(publicKey, out _);
        }

        public void ImportSubjectPublicKeyInfo(byte[] publicKey)
        {
            rsa.ImportSubjectPublicKeyInfo(publicKey, out _);
        }


    }
}
