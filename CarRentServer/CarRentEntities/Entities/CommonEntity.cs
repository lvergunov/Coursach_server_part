using System.Text.Json.Serialization;

namespace Library.Entities
{
    public abstract class CommonEntity : IComparable<CommonEntity>
    {
        public CommonEntity(long id) { 
            Id = id;
        }

        [JsonPropertyName("id")]
        public long Id { get; }

        public int CompareTo(CommonEntity? other)
        {
            if (other is null) throw new ArgumentException("Incorrect value");
            return Id.CompareTo(other.Id);
        }
    }
}