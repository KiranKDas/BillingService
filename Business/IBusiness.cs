using BillingService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillingService.Business
{ 
    public interface IBusiness
    {
        Task<IEnumerable<Bill>> GetBillsAsync();
        Task<Bill> GetBillByIdAsync(int billId);
        Task CreateBillAsync(Bill bill);
        Task UpdateBillAsync(Bill bill);

        Task CancelBillAsync(int billId, string cancellationPolicy);

    }
}