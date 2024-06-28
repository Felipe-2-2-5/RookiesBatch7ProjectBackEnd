using Backend.Domain.Enum;

namespace Backend.Application.Services.ReturnRequestServices
{
    public interface IReturnRequestService
    {
        Task CreateRequest(int assignmentId, string createName, int createId, Role role);
    }
}