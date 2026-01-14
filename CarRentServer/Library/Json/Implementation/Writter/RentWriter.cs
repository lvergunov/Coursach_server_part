using Library.Entities;
using Library.Entities.Collections;
using Library.Exception;
using Library.Json.Singletons;
using Library.Json.Writter;
using System.Text.Json;

namespace Library.Json.Implementation.Writter
{
    public class RentWriter : IRentWriter
    {

        private static readonly RentWriter _instance = new RentWriter();

        public static RentWriter Instance { get { return _instance; } }

        private RentWriter() { }

        public void Update(long id, Rent entity)
        {
            string fileText = RentFileSingleton.Instance.ReadAllFile();
            RentList rentList = JsonSerializer.Deserialize<RentList>(fileText) ?? throw new ReadFileException("Cannot read file with rents.");
            rentList.Rents.Remove(rentList.Rents.Find(r => r.Id == id) ?? throw new ReadFileException($"Cannot read file with id {id}"));
            rentList.Rents.Add(entity);
            string newJsonText = JsonSerializer.Serialize(rentList);
            RentFileSingleton.Instance.RewriteFile(newJsonText);
        }

        public void Write(Rent entity)
        {
            string fileText = RentFileSingleton.Instance.ReadAllFile();
            RentList rentList = JsonSerializer.Deserialize<RentList>(fileText) ?? throw new ReadFileException("Cannot read file with rents.");
            rentList.Rents.Add(entity);
            string newText = JsonSerializer.Serialize(rentList, serializerOptions);
            RentFileSingleton.Instance.RewriteFile(newText);
        }

        public void ResetData()
        {
            RentFileSingleton.Instance.TearDownAll();
        }

        public void Delete(long id)
        {
            string allFileText = RentFileSingleton.Instance.ReadAllFile();
            RentList rentList = JsonSerializer.Deserialize<RentList>(allFileText) ?? throw new ReadFileException("Cannot read file with rents");
            Rent searchedRent = rentList.Rents.Find(c => c.Id == id) ?? throw new ReadFileException($"There is no rent with id {id}");
            rentList.Rents.Remove(searchedRent);
            string newFileText = JsonSerializer.Serialize(rentList, serializerOptions);
            RentFileSingleton.Instance.RewriteFile(newFileText);
        }

        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions() { WriteIndented = true };
    }
}
