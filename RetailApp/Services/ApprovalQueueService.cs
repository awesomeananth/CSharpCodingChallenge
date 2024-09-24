using Microsoft.EntityFrameworkCore;
using RetailApp.Data;
using RetailApp.Dtos;
using RetailApp.Models;

namespace RetailApp.Services
{
    public class ApprovalQueueService
    {
        private readonly AppDbContext _context;

        public ApprovalQueueService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<IEnumerable<ApprovalQueue>>> GetApprovalQueueAsync()
        {
            var queueItems = await _context.ApprovalQueue
                .Include(aq => aq.Product)
                .OrderBy(aq => aq.RequestDate)
                .ToListAsync();

            return new ServiceResponse<IEnumerable<ApprovalQueue>> { Data = queueItems };
        }

        public async Task<ServiceResponse<ApprovalQueue>> ProcessApprovalAsync(int queueId, ApprovalDecisionDto decision)
        {
            var queueItem = await _context.ApprovalQueue.Include(aq => aq.Product)
                .FirstOrDefaultAsync(aq => aq.QueueId == queueId);

            if (queueItem == null)
            {
                return new ServiceResponse<ApprovalQueue> { Success = false, Message = "Queue item not found." };
            }

            if (decision.IsApproved)
            {
                switch (queueItem.RequestType)
                {
                    case "Create":
                        queueItem.Product.Status = "Active";
                        _context.Products.Add(queueItem.Product);
                        break;
                    case "Update":
                        queueItem.Product.UpdatedDate = DateTime.UtcNow;
                        _context.Products.Update(queueItem.Product);
                        break;
                    case "Delete":
                        _context.Products.Remove(queueItem.Product);
                        break;
                }
            }

            _context.ApprovalQueue.Remove(queueItem);
            await _context.SaveChangesAsync();

            return new ServiceResponse<ApprovalQueue> { Data = queueItem };
        }
    }
}
