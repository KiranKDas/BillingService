﻿// Controllers/BillsController.cs
using BillingService.Models;
using BillingService.Business;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BillingService.Controllers
{
    [ApiController]
    [Route("v1/bills")]
    public class BillsController : ControllerBase
    {
        private readonly IBusiness _repository;

        public BillsController(IBusiness repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBill([FromBody] CreateBillDto createDto)
        {
            var bill = new Bill
            {
                PatientId = createDto.PatientId,
                AppointmentId = createDto.AppointmentId,
                Amount = createDto.Amount,
                Status = createDto.Status,
                CreatedAt = DateTime.UtcNow
            };
            await _repository.CreateBillAsync(bill);
            return CreatedAtAction(nameof(GetBillById), new { id = bill.BillId }, bill);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBillById(int id)
        {
            var bill = await _repository.GetBillByIdAsync(id);
            if (bill == null) return NotFound();
            return Ok(bill);
        }

        [HttpGet]
        public async Task<IActionResult> GetBills()
        {
            var bills = await _repository.GetBillsAsync();
            return Ok(bills);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelBill(int id)
        {
            var bill = await _repository.GetBillByIdAsync(id);
            if (bill == null) return NotFound();

            bill.Status = "VOID";
            await _repository.UpdateBillAsync(bill);
            return NoContent();
        }
    }
}