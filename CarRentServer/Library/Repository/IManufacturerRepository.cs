namespace Library.Repository
{
    public interface IManufacturerRepository
    {
        public List<string> GetAllManufacturers();
        public List<string> GetAllBodies();
    }
}
