using Backend.Domain.Enum;

namespace Backend.Domain.Entities
{
    public class ReturnRequest : BaseEntity
    {
        public int RequestorId { get; set; }
        public User Requestor { get; set; }
        public int AcceptorId { get; set; }
        public User Acceptor { get; set; }
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
        public DateTime ReturnedDate { get; set; }
        public ReturnRequestState State { get; set; }
    }
}
