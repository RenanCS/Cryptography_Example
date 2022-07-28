namespace Cryptography.Services
{
    public class FileKey : IFileKey
    {

        private readonly string PathPublicKey = "C:\\keys\\public.perm";
        public string ReaderPublicKey()
        {
            return File.ReadAllText(PathPublicKey);
        }
    }
}
