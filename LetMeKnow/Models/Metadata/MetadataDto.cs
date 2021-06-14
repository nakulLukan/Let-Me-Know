namespace LetMeKnow.Models.Metadata
{
    public class MetadataDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public MetadataDto()
        {
        }

        public MetadataDto(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
