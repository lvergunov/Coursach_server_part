using Library.Entities;

namespace Library.Json.Reader
{
    public interface ICarReader : IJsonReader<Car>
    {
        List<Car> ReadByPriceBetween(float lowCost, float highCost);

        List<Car> ReadByCarBody(string carBody);

        List<Car> ReadByMark(string mark);

        List<Car> ReadByModelName(string name);

        List<Car> ReadCarsByActivity(bool active = true);

        List<Car> Filter(float? lowCost, float? highCost, string? carBody, string? manufacturer, string? name, bool? active);
    }
}
