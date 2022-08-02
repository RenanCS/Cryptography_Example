using Cryptography.Library.Factory;
using System.Text;

namespace Cryptography.Filters
{
    public class IOMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly FactoryAes factoryAes = new FactoryAes();
        private readonly FactoryPubPriKey factoryPubPriKey = new FactoryPubPriKey();

        public IOMiddleware(RequestDelegate next)
        {
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
                    //var newBody = factoryAes.Decrypt(body);
                    var newBody = factoryPubPriKey.Decrypt(body);

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

                    //newBody = factoryAes.Encrypt(body);
                    newBody = factoryPubPriKey.Encrypt(body);

                    buffer.Seek(0, SeekOrigin.Begin);

                    buffer.SetLength(0);

                    await buffer.CopyToAsync(stream);
                    context.Response.Body = stream;

                    // Sempre retorna um text/plain
                    context.Response.ContentType = "text/plain";

                }
            }

            return newBody;
        }
    }
}
