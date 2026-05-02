﻿// Controllers/BillsController.cs
using BillingService.Models;
using BillingService.Business;
using Microsoft.AspNetCore.Mvc;
using BillingService.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillingService.Controllers
{
    [ApiController]
    [Route("v1/bills")]
    [RequireUserType]
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
                Status = createDto.Status
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBill(int id, [FromBody] CreateBillDto updateDto)
        {
            try
            {
                var bill = new Bill
                {
                    BillId = id,
                    Amount = updateDto.Amount,
                    Status = updateDto.Status
                };
                await _repository.UpdateBillAsync(bill);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelBill(int id, [FromQuery] string cancellationPolicy)
        {
            try
            {
                await _repository.CancelBillAsync(id, cancellationPolicy);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}