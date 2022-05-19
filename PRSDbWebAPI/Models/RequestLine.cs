using System.Text.Json.Serialization;

namespace PRSDbWebAPI.Models {

    public class RequestLine {

        public int Id { get; set; } = 0;
        public int RequestId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;

        [JsonIgnore]
        public virtual Request? Request { get; set; }
        public virtual Product? Product { get; set; }
    }
}
