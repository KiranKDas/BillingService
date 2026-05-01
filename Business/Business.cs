using BillingService.Data;
using BillingService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillingService.Business
{
    public class Business : IBusiness
    {
        private readonly BillingDbContext _context;

        public Business(BillingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bill>> GetBillsAsync()
        {
            return await _context.Bills.ToListAsync();
        }

        public async Task<Bill> GetBillByIdAsync(int billId)
        {
            return await _context.Bills.FindAsync(billId);
        }

        public async Task CreateBillAsync(Bill bill)
        {
            await _context.Bills.AddAsync(bill);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBillAsync(Bill bill)
        {
            _context.Bills.Update(bill);
            await _context.SaveChangesAsync();
        }
    }
}