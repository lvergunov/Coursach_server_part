using Library.Entities;

namespace Library.Repository
{
    public interface ICarRepository : ICommonRepository<Car>
    {
        List<Car> Filter(float? lowCost, float? highCost, string? carBody, string? manufacturer, string name, bool? active);

        List<Car> FindCarByActivity(bool activity);

        List<Car> FindByCarBody(string body);

        List<Car> FindByCarManufacturer(string manufacturer);

        List<Car> ReadByModelName(string modelName);

        List<Car> ReadByPriceInRate(float lowerCost, float higherCost);
    }
}
