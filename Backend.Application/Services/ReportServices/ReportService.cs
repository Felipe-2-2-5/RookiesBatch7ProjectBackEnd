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
        private readonly IReportRepository _reportRepository;

        public ReportService ( IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        //get report by category and state 
        public async Task<PaginationResponse<AssetReportDto>> GetAssetReportAsync(string SortColumn, string SortDirection, int PageSize, int Page)
        {
            return await _reportRepository.GetAssetReportAsync(SortColumn, SortDirection, PageSize, Page);
        }
    }
}
