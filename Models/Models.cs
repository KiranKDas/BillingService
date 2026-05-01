using System;

namespace BillingService.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public int PatientId { get; set; }
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } // OPEN, PAID, VOID, REFUND
        public DateTime CreatedAt { get; set; }
    }

    public class CreateBillDto
    {
        public int PatientId { get; set; }
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }
}