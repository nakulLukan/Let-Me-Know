using LiteDB;

namespace LetMeKnow.Entities
{
    public class District
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }

        [BsonRef(nameof(LetMeKnow.Entities.State))]
        public State State { get; set; }
    }
}
