using Cryptography.Library;
using Cryptography.Library.Helper;
using Newtonsoft.Json;
using System.Text;

namespace Cryptography.Filters
{
    public class IOMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly HybridEncryption hybrid = new HybridEncryption();
        private static readonly NewRSA rsaParams = new NewRSA();
        private static readonly NewDigitalSignature digitalSignature = new NewDigitalSignature();

        public IOMiddleware(RequestDelegate next)
        {
            this.CheckKey();

            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            await LogRequest(context.Request);

            await LogResponseAndInvokeNext(context);
        }

        private async Task LogRequest(HttpRequest request)
        {
            using (var bodyReader = new StreamReader(request.Body))
            {
                string body = await bodyReader.ReadToEndAsync();

                if (!string.IsNullOrEmpty(body))
                {
                    //var newBody = DecryptBlock(body);
                    var newBody = DecryptBlockSimple(body);

                    request.Body = new MemoryStream(Encoding.UTF8.GetBytes(newBody));
                    request.Headers.ContentType = "application/json";
                }

            }
        }

        private async Task LogResponseAndInvokeNext(HttpContext context)
        {
            var newBody = await FormatterResponseAsync(context);
            await context.Response.WriteAsync(newBody);

        }

        private async Task<string> FormatterResponseAsync(HttpContext context)
        {
            var newBody = "";
            using (var buffer = new MemoryStream())
            {
                var responseCopy = context.Response;
                //replace the context response with our buffer
                var stream = context.Response.Body;
                context.Response.Body = buffer;

                //invoke the rest of the pipeline
                await _next.Invoke(context);

                //reset the buffer and read out the contents
                buffer.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(buffer);
                using (var bufferReader = new StreamReader(buffer))
                {
                    string body = await bufferReader.ReadToEndAsync();

                    //newBody = EncryptStream(body);
                    newBody = EncryptStreamSimple(body);

                    //reset read to start of stream
                    buffer.Seek(0, SeekOrigin.Begin);

                    //reset stream
                    buffer.SetLength(0);

                    //copy our content to the original stream and put it back
                    await buffer.CopyToAsync(stream);
                    context.Response.Body = stream;

                    // Sempre retorna um text/plain
                    context.Response.ContentType = "text/plain";

                }
            }

            return newBody;
        }

        private static string EncryptStream(string original)
        {
            var encryptedBlock = hybrid.EncryptData(Encoding.UTF8.GetBytes(original), rsaParams, digitalSignature);
            var encryptedBlockJson = JsonConvert.SerializeObject(encryptedBlock);
            return encryptedBlockJson;
        }

        private static string EncryptStreamSimple(string original)
        {
            var encryptedBlock = rsaParams.Encrypt(original);
            var encryptedBlockJson = Convert.ToBase64String(encryptedBlock);
            return encryptedBlockJson;
        }

        private static string DecryptBlock(string responseBody)
        {
            EncryptedPacket encryptedBlock = JsonConvert.DeserializeObject<EncryptedPacket>(responseBody);
            var decrpyted = hybrid.DecryptData(encryptedBlock, rsaParams, digitalSignature);
            var decrpytedString = Encoding.UTF8.GetString(decrpyted);

            return decrpytedString;
        }

        private static string DecryptBlockSimple(string responseBody)
        {
            var decrpyted = rsaParams.Decrypt(Convert.FromBase64String(responseBody));
            var decrpytedString = Encoding.UTF8.GetString(decrpyted);
            return decrpytedString;
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
            WriteReadKey.GetPublicKeyFromPemFile(pathPublicKey, rsaParams.rsa);
            WriteReadKey.GetPrivateKeyFromPemFile(pathPrivateKey, rsaParams.rsa);
        }
    }
}
