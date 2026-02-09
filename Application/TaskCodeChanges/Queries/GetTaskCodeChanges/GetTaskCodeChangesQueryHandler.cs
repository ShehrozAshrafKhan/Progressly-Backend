using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TaskCodeChanges.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskCodeChanges.Queries.GetTaskCodeChanges
{
    public class GetTaskCodeChangesQueryHandler : IRequestHandler<GetTaskCodeChangesQuery, Response<List<TaskCodeChangesDTO>>>
    {
        private readonly IApplicationDbContext _context;

        public GetTaskCodeChangesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<TaskCodeChangesDTO>>> Handle(GetTaskCodeChangesQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var changes = await _context.TaskCodeChanges.Where(x=>x.TaskId==query.TaskId).Select(x=>new TaskCodeChangesDTO
                {
                    EntryType = x.EntryType,
                    NewExtension = x.NewExtension,
                    NewFileName = x.NewFileName,
                    NewFilePath = x.NewFilePath,
                    OldExtension = x.OldExtension,
                    OldFileName = x.OldFileName,
                    OldFilePath = x.OldFilePath,
                    TagId = x.TagId,
                    TaskCodeChangeId=x.TaskCodeChangeId,
                    TaskId= x.TaskId
                }).ToListAsync(cancellationToken);
                if (changes.Count == 0 || changes == null)
                {
                    return new Response<List<TaskCodeChangesDTO>>() { result = Result.Failure(new List<string>() { "File changing not found" }) };
                }
                return new Response<List<TaskCodeChangesDTO>>() { result = Result.Success(), data = changes };
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<List<TaskCodeChangesDTO>>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
