using Library.Exception;

namespace Library.Json.Singletons
{
    public abstract class CommonSingleton
    {
        protected CommonSingleton(string fileName) {
            fullFilePath = DIRECTORY_PATH + fileName;
        }

        public string ReadAllFile()
        {
            try
            {
                CheckFileExistance();
                return File.ReadAllText(fullFilePath);
            }
            catch (IOException ex)
            {
                throw new ReadFileException("Error in reading file");
            }
        }

        public void RewriteFile(string text)
        {
            try
            {
                CheckFileExistance();
                File.WriteAllText(fullFilePath, text);
            }
            catch (IOException ex)
            {
                throw new WriteFileException("Error in writing file");
            }
        }

        public void TearDownAll() { 
            File.Delete(fullFilePath);
        }

        private void CheckFileExistance() 
        {
            if (!Directory.Exists(Path.GetDirectoryName(DIRECTORY_PATH))) 
            {
                Directory.CreateDirectory(DIRECTORY_PATH);
            }
            if (!File.Exists(fullFilePath))
            {
                File.Create(fullFilePath).Close();
                InitializeFile();
            }
        }

        private static readonly string DIRECTORY_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\CarRentServer\\Jsons\\";

        protected abstract void InitializeFile();

        protected string fullFilePath;
    }
}
