using LiteDB;

namespace LetMeKnow.Entities
{
    public class State
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}