using Application.Common.Models;
using MediatR;
using Progressly.Application.DocumentStore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskCodeChanges.Queries.GetTaskCodeChangesFiles
{
    public class GetTaskCodeChangesFilesQuery:IRequest<Response<FileInfoDto>>
    {
        public Guid? TaskCodeChangeId { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
    }
}
