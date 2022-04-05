namespace HandMadeApi.Models.DTO.NewFolder
{
    public class PostOrderDto
    {
        public string UserID { get; set; }
        public string? Phone { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Note { get; set; }
        public bool? Paid { get; set; } = false;

    }
}
