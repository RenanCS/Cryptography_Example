// See https://aka.ms/new-console-template for more information

using Cryptography.Library;
using Cryptography.Library.Helper;
using CryptographyCreatePem.Helper;

namespace Program
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NewRSA rsaParams = new NewRSA();

            string pathPublicKey = "C:\\keys\\public.perm";
            string pathPrivateKey = "C:\\keys\\private.perm.";

            if (!File.Exists(pathPrivateKey))
            {
                FileHelper.StringToFile(pathPublicKey, WriteReadKey.ExportPublicKey(rsaParams.rsa));
                FileHelper.StringToFile(pathPrivateKey, WriteReadKey.ExportPrivateKey(rsaParams.rsa));
            }

            WriteReadKey.GetPrivateKeyFromPemFile(pathPrivateKey, rsaParams.rsa);
            WriteReadKey.GetPublicKeyFromPemFile(pathPublicKey, rsaParams.rsa);
        }
    }
}


