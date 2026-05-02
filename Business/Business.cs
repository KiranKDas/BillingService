﻿using BillingService.Data;
using BillingService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BillingService.Business
{
    public class Business : IBusiness
    {
        private readonly BillingDbContext _context;
        private readonly Monitoring.BillingMetrics _metrics;

        public Business(BillingDbContext context, Monitoring.BillingMetrics metrics)
        {
            _context = context;
            _metrics = metrics;
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
            var sw = Stopwatch.StartNew();

            decimal taxRate = 0.05m;
            decimal taxAmount = bill.Amount * taxRate;
            bill.Amount += taxAmount;

            bill.Status = "OPEN";
            bill.CreatedAt = DateTime.UtcNow;

            await _context.Bills.AddAsync(bill);
            await _context.SaveChangesAsync();

            sw.Stop();
            _metrics.BillCreationLatencyMs.Record(sw.ElapsedMilliseconds);
            _metrics.BillsCreatedTotal.Add(1);
        }

        public async Task UpdateBillAsync(Bill bill)
        {
            var existingBill = await _context.Bills.FindAsync(bill.BillId);
            if (existingBill == null) throw new KeyNotFoundException("Bill not found.");
    
            if (existingBill.Status == "PAID")
            {
                throw new InvalidOperationException("Cannot edit a bill after it is PAID. Please process a refund or adjustment instead.");
            }

            existingBill.Amount = bill.Amount;
            existingBill.Status = bill.Status;

            _context.Bills.Update(existingBill);
            await _context.SaveChangesAsync();
        }

        public async Task CancelBillAsync(int billId, string cancellationPolicy)
        {
            var bill = await _context.Bills.FindAsync(billId);
            if (bill == null) throw new KeyNotFoundException("Bill not found.");

            if (bill.Status == "PAID")
                throw new InvalidOperationException("Cannot cancel a paid bill. Initiate a refund instead.");

            if (cancellationPolicy == "MORE_THAN_2_HOURS") {
                bill.Status = "VOID";
                bill.Amount = 0;
            } else if (cancellationPolicy == "LESS_THAN_2_HOURS") {
                bill.Amount *= 0.5m; // 50% cancellation fee
            }

            _context.Bills.Update(bill);
            await _context.SaveChangesAsync();
        }
    }
}