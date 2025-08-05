using AutoMapper;
using AutoMapper.Execution;
using AutoMapper.QueryableExtensions;
using FluentResults;
using LibraryManagement.Application.Errors;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exception;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.Services;
using LibraryManagement.Domain.ValueObjects;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LibraryManagement.Infrastructure.QueryHandler
{
    public class BorrowingQueryService : IBorrowingQueryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BorrowingQueryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BorrowRecordResponse>> GetAllBorrowingRecord(string searchText, DateTime? dataTime)
        {
            IQueryable<BorrowingRecord> query = _context.BorrowingRecords.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(br => br.Book.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                                       || br.Book.Author.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                                       || br.Borrower.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                                       || br.Borrower.Email.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            }

            if (dataTime.HasValue)
            {
                query = query.Where(record => record.DateBorrowed.Date == dataTime.Value.Date);
            }

            return await query.ProjectTo<BorrowRecordResponse>(_mapper.ConfigurationProvider).ToListAsync();

           
        }

        public async Task<BorrowRecordResponse?> GetBorrowingRecordById(Guid recordId)
        {
            var borrowingRecord = await _context.BorrowingRecords
                .Where(br => br.Id == new BorrowingRecordId(recordId))
                .ProjectTo<BorrowRecordResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return borrowingRecord;
        }
    }
}
