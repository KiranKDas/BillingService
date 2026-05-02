﻿using System;

namespace BillingService.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public int PatientId { get; set; }
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public required string Status { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}