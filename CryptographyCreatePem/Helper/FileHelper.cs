namespace CryptographyCreatePem.Helper
{
    public class FileHelper
    {
        public static bool StringToFile(string path, string fileString)
        {
            try
            {

                File.WriteAllText(path, fileString);
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }
    }
}
