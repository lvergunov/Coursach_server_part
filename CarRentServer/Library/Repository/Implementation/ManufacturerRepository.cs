
using Library.Json.Implementation.Reader;

namespace Library.Repository.Implementation
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        public List<string> GetAllBodies()
        {
            return CarBodyReader.Instance.GetAll();
        }

        public List<string> GetAllManufacturers()
        {
            return ManufacturerReader.Instance.ReadAll();
        }
    }
}
