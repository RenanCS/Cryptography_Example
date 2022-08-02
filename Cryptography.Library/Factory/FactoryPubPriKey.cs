using Cryptography.Library.Interface;

namespace Cryptography.Library.Factory
{
    public class FactoryPubPriKey : FactoryCrypt
    {
        private static ICryptography instance = null;

        public override ICryptography GetFactoryCrypt()
        {
            if (FactoryPubPriKey.instance == null)
            {
                FactoryPubPriKey.instance = new NewRSA();
            }

            return FactoryPubPriKey.instance;
        }
    }
}
