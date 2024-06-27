using Backend.Application.IRepositories;

namespace Backend.Application.Services.ReturnRequestServices
{
    public class ReturnRequestService
    {
        private readonly IReturnRequestRepository _requestRepository;

        public ReturnRequestService(IReturnRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

    }
}
