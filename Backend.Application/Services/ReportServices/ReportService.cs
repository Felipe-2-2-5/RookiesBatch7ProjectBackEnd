using AutoMapper;
using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.IRepositories;
using Backend.Application.Services.AssetServices;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Backend.Domain.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace Backend.Application.Services.ReportServices
{
    public class ReportService : IReportService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<AssetDTO> _validator;
        private readonly IReportRepository _reportRepository;

        public ReportService (  IMapper mapper, IValidator<AssetDTO> validator, IReportRepository reportRepository)
        {
            _mapper = mapper;
            _validator = validator;
            _reportRepository = reportRepository;
        }

        //get report by category and state 
        public async Task<List<AssetReportDto>> GetAssetReportAsync(string sortColumn, string sortDirection)
        {
            return await _reportRepository.GetAssetReportAsync(sortColumn, sortDirection);
        }
    }
}
