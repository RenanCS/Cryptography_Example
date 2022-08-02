namespace Cryptography.Library.Interface
{
    public interface ICryptography
    {
        public string Encrypt(string data);
        public string Decrypt(string data);
    }
}
