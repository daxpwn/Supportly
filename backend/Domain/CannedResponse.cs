namespace Domain
{
    // Gotovi (šablonski) odgovori
    public class CannedResponse
    {
        public short Id { get; set; }      // SMALLINT
        public string Title { get; set; }
        public string Body { get; set; }
        public int CreatedBy { get; set; }

        public virtual User Creator { get; set; }
    }
}
