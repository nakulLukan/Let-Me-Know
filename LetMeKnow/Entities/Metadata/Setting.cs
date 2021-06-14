using LiteDB;

namespace LetMeKnow.Entities
{
    public class Setting
    {
        [BsonId]
        public int Id { get; set; }

        [BsonRef(nameof(LetMeKnow.Entities.State))]
        public State State { get; set; }

        [BsonRef(nameof(LetMeKnow.Entities.District))]
        public District District { get; set; }

        public bool Is18Plus { get; set; }
        public bool Is40Plus { get; set; }
        public bool Is45Plus { get; set; }

        public Vaccine Vaccine { get; set; }
        public bool OnlyFree { get; set; }
        public bool SurfAppointments { get; set; }

        public bool Dose1Enabled { get; set; }
        public bool Dose2Enabled { get; set; }

        public int Frequency { get; set; }
    }
}
