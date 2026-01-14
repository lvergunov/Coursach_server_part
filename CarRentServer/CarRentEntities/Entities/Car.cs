using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Library.Entities
{
    public class Car : CommonEntity, IEquatable<Car>
    {
        [JsonPropertyName("model")]
        public CarModel Model { get; }

        [JsonPropertyName("price")]
        public float Price { get; }

        [JsonPropertyName("rate")]
        public float Rate { get; private set; }

        [JsonPropertyName("rates-number")]
        public uint NumberOfRates { get; private set; }

        public Car(long id, CarModel model, float price, float rate = 0.0f, uint numberOfRates = 0) : base(id)
        {
            Model = model;
            Price = price;
            Rate = rate;
            NumberOfRates = numberOfRates;
        }

        public Car RenewRate(List<float> rates)
        {
            this.Rate = rates.Sum() / rates.Count;
            NumberOfRates = (uint)rates.Count;
            return this;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Car);
        }

        public override string ToString()
        {
            return $"{Id}. {Model.Mark} {Model.Name}. Rent per hour {Price}. Total rate: {Rate}. Number of reviews {NumberOfRates}.";
        }

        public bool Equals(Car? other)
        {
            if (other == null) return false;
            return Id == other.Id;
        }
    }
}
