namespace OnlineStoreAPI.Dto
{
    public class OrderFilterDto
    {
        public string CustomerEmail { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
