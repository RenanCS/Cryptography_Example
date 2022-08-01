namespace Cryptography.Library
{
    public class EncryptedPacket
    {
        // Sessão criada criptografada com a chave pública
        public string EncryptedSessionKey { get; set; }
        public string EncryptedData { get; set; }
        // Iv sinônimos para NONCE
        public string Iv { get; set; }
        // Tag equivale ao HMAC original 
        public string Tag { get; set; }
        // Armazena a hash autenticado do dados criptografados (sessionkey + EncryptedData + Iv)
        public string SignatureHMAC { get; set; }
        public string Signature { get; set; }
    }
}
