namespace Cryptography.Library
{
    public class EncryptedPacket
    {
        // Sessão criada criptografada com a chave pública
        public byte[] EncryptedSessionKey { get; set; }
        public byte[] EncryptedData { get; set; }
        // Iv sinônimos para NONCE
        public byte[] Iv { get; set; }
        // Tag equivale ao HMAC original 
        public byte[] Tag { get; set; }
        // Armazena a hash autenticado do dados criptografados (sessionkey + EncryptedData + Iv)
        public byte[] SignatureHMAC { get; set; }
        public byte[] Signature { get; set; }
    }
}
