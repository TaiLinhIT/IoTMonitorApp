using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.Dto.PaymentDto;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _dbContext;

        public PaymentService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            var payments = await _dbContext.Payments.ToListAsync();
            return payments.Select(p => MapToDto(p));
        }

        public async Task<PaymentDto> GetByIdAsync(int id)
        {
            var payment = await _dbContext.Payments.FindAsync(id);
            return payment == null ? null : MapToDto(payment);
        }

        public async Task<PaymentDto> CreateAsync(PaymentDto dto)
        {
            var payment = new Payment
            {
                OrderId = dto.OrderId,
                PaymentMethod = dto.PaymentMethod,
                Amount = dto.Amount,
                PaymentDate = dto.PaymentDate,
                TransactionId = dto.TransactionId,
                Status = dto.Status,
                Slug = dto.Slug,
                CreatedDate = DateTime.UtcNow,
                IsDelete = dto.IsDelete
            };

            _dbContext.Payments.Add(payment);
            await _dbContext.SaveChangesAsync();

            dto.Id = payment.Id;
            return dto;
        }

        public async Task<bool> UpdateAsync(PaymentDto dto)
        {
            var payment = await _dbContext.Payments.FindAsync(dto.Id);
            if (payment == null) return false;

            payment.OrderId = dto.OrderId;
            payment.PaymentMethod = dto.PaymentMethod;
            payment.Amount = dto.Amount;
            payment.PaymentDate = dto.PaymentDate;
            payment.TransactionId = dto.TransactionId;
            payment.Status = dto.Status;
            payment.Slug = dto.Slug;
            payment.UpdatedDate = DateTime.UtcNow;
            payment.IsDelete = dto.IsDelete;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var payment = await _dbContext.Payments.FindAsync(id);
            if (payment == null) return false;

            _dbContext.Payments.Remove(payment);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<PaymentDto>> GetByOrderIdAsync(int orderId)
        {
            var payments = await _dbContext.Payments
                .Where(p => p.OrderId == orderId)
                .ToListAsync();

            return payments.Select(p => MapToDto(p));
        }

        public async Task<bool> UpdatePaymentStatusAsync(int paymentId, string status)
        {
            var payment = await _dbContext.Payments.FindAsync(paymentId);
            if (payment == null) return false;

            payment.Status = status;
            payment.UpdatedDate = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private PaymentDto MapToDto(Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                PaymentMethod = payment.PaymentMethod,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                TransactionId = payment.TransactionId,
                Status = payment.Status,
                Slug = payment.Slug,
                CreatedDate = payment.CreatedDate,
                UpdatedDate = payment.UpdatedDate,
                IsDelete = payment.IsDelete
            };
        }
    }
}
