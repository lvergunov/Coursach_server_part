using Library.Entities.Collections;
using System.Text.Json;

namespace Library.Json.Singletons
{
    public class ReviewFileSingleton : CommonSingleton
    {
        private static readonly ReviewFileSingleton instance = new ReviewFileSingleton();

        public static ReviewFileSingleton Instance { get { return instance; } }

        private ReviewFileSingleton() : base(FILE_PATH) { }

        protected override void InitializeFile()
        {
            string initialString = JsonSerializer.Serialize(new ReviewList());
            File.WriteAllText(fullFilePath, initialString);
        }

         private const string FILE_PATH = "Reviews.json";
    }
}
