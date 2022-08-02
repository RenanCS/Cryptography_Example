using Cryptography.Library.Interface;

namespace Cryptography.Library.Factory
{
    public class FactoryAes : FactoryCrypt
    {
        private static ICryptography instance = null;

        public override ICryptography GetFactoryCrypt()
        {
            if (FactoryAes.instance == null)
            {
                FactoryAes.instance = new HybridEncryption();
            }

            return FactoryAes.instance;
        }
    }
}
