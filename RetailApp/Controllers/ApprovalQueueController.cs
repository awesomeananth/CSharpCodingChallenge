using Microsoft.AspNetCore.Mvc;
using RetailApp.Services;
using RetailApp.Dtos;

namespace RetailApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApprovalQueueController : ControllerBase
    {
        private readonly ApprovalQueueService _approvalQueueService;

        public ApprovalQueueController(ApprovalQueueService approvalQueueService)
        {
            _approvalQueueService = approvalQueueService;
        }

        [HttpGet]
        public async Task<IActionResult> GetApprovalQueue()
        {
            try
            {
                var queueItems = await _approvalQueueService.GetApprovalQueueAsync();
                return Ok(queueItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{queueId}/approve")]
        public async Task<IActionResult> ApproveRequest(int queueId, [FromBody] ApprovalDecisionDto decision)
        {
            try
            {
                var result = await _approvalQueueService.ProcessApprovalAsync(queueId, decision);
                if (!result.Success)
                    return BadRequest(result.Message);

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
