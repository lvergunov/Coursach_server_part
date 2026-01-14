using System.Text.Json.Serialization;

namespace Library.Entities
{
    public class Rent : CommonEntity
    {

        public Rent(long id, Car car, User user, DateTime startRent, DateTime endRent) : base(id) { 
            _car = car;
            CarId = car.Id;
            UserId = user.Id;
            _user = user;
            StartRent = startRent;
            EndRent = endRent;
        }

        [JsonConstructor]
        public Rent(long id, long carId, long userId, DateTime startRent, DateTime endRent) : base(id){
            CarId = carId;
            UserId = userId;
            StartRent = startRent;
            EndRent = endRent;
        }

        [JsonIgnore]
        public bool IsActive { get { 
                return DateTime.Now < StartRent ||
                    DateTime.Now > EndRent;
            } 
        }

        [JsonPropertyName("car-id")]
        public long CarId { get; }

        [JsonPropertyName("user-id")]
        public long UserId { get; }

        [JsonPropertyName("start-rent")]
        public DateTime StartRent { get; }

        [JsonPropertyName("end-rent")]
        public DateTime EndRent { get; }

        [JsonIgnore]
        public float FullPrice {
            get { 
                return (float)(EndRent - StartRent).TotalHours * _car.Price;
            }
        }

        public void SetInnerObjects(Car car, User user) { 
            _car = car;
            _user = user;
        }

        [JsonIgnore]
        private Car _car;
        [JsonIgnore]
        private User _user;
    }
}
