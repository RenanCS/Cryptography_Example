using Cryptography.Library.Interface;

namespace Cryptography.Library.Factory
{
    public abstract class FactoryCrypt
    {
        public abstract ICryptography GetFactoryCrypt();

        public string Encrypt(string data)
        {
            var type = this.GetFactoryCrypt();
            return type.Encrypt(data);
        }

        public string Decrypt(string data)
        {
            var type = this.GetFactoryCrypt();
            return type.Decrypt(data);
        }

    }
}
