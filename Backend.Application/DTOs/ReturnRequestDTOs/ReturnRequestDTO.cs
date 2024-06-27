using Backend.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Backend.Application.DTOs.ReturnRequestDTOs
{
    public class ReturnRequestDTO
    {
        [Required]
        public int RequestorId { get; set; }
        public int AcceptorId { get; set; }
        [Required]
        public int AssignmentId { get; set; }
        public DateTime ReturnedDate { get; set; }
        [Required]
        public ReturnRequestState State { get; set; }
    }
}
