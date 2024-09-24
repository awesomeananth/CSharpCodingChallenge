using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailApp.Models
{
    public class ApprovalQueue
    {
        public int QueueId { get; set; }
        public int ProductId { get; set; }

        [StringLength(10)]
        public string RequestType { get; set; }

        [StringLength(255)]
        public string RequestReason { get; set; }

        public DateTime RequestDate { get; set; }

        public Product Product { get; set; }
    }
}
