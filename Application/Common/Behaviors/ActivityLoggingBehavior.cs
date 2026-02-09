using Application.Common.Interfaces;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{

    public class ActivityLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IActivityLogger _activityLogger;
        private readonly ICurrentUserService _currentUserService;

        public ActivityLoggingBehavior(IActivityLogger activityLogger, ICurrentUserService currentUserService)
        {
            _activityLogger = activityLogger;
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Execute the actual handler
            var response = await next();

            try
            {
                var userId = _currentUserService.UserId;
                var userName = _currentUserService.UserName;

                // Log activity
                await _activityLogger.LogAsync(
                    userId: userId,
                    userName: userName,
                    action: typeof(TRequest).Name,                 // e.g., CreateTaskCommand
                    entity: typeof(TRequest).Name,                 // can be refined if needed
                    details: JsonConvert.SerializeObject(request),
                    cancellationToken: cancellationToken
                );
            }
            catch
            {
                // Optional: ignore logging errors to avoid breaking the main request
            }

            return response;
        }
    }

}
