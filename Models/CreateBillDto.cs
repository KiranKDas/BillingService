namespace BillingService.Models
{
    public class CreateBillDto
    {
        public int PatientId { get; set; }
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}