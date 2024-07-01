using Backend.Domain.Entities;
using Backend.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Backend.Application.DTOs.ReturnRequestDTOs
{
    public class ReturnRequestResponse
    {
        public int Id { get; set; }
        [Required]
        public int RequestorId { get; set; }
        public User? Requestor { get; set; }
        [Required]
        public int AcceptorId { get; set; }
        public User? Acceptor { get; set; }
        [Required]
        public int AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }
        [Required]
        public DateTime? ReturnedDate { get; set; }
        [Required]
        public ReturnRequestState State { get; set; }
    }
}
