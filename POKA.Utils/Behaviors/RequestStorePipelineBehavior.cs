using Microsoft.Extensions.Logging;
using POKA.Utils.Repositories;
using POKA.Utils.Attributes;
using POKA.Utils.Entities;

namespace POKA.Utils.Behaviors
{
    public class RequestStorePipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<RequestStorePipelineBehavior<TRequest, TResponse>> _logger;
        private readonly IRequestRepository _requestRepository;

        public RequestStorePipelineBehavior(IRequestRepository requestRepository, ILogger<RequestStorePipelineBehavior<TRequest, TResponse>> logger)
        {
            _requestRepository = requestRepository;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            RequestEntity? requestEntity = null;

            try
            {
                var requestCopy = Clone(request);
                requestEntity = await this._requestRepository.InitializeAsync(requestCopy, cancellationToken);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
            }

            try
            {
                var result = await next();

                try
                {
                    requestEntity.Success();

                    await this._requestRepository.UpdateAsync(requestEntity.Id, requestEntity, cancellationToken);
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, ex.Message);
                }

                return result;
            }
            catch (Exception ex)
            {
                try
                {
                    requestEntity.Fail(ex.ToString());

                    await this._requestRepository.UpdateAsync(requestEntity.Id, requestEntity, cancellationToken);
                }
                catch (Exception _ex)
                {
                    this._logger.LogError(_ex, _ex.Message);
                }

                throw;
            }
        }

        public IBaseRequest Clone(IBaseRequest request)
        {
            var propertiesToMask =  request
                                        .GetType()
                                        .GetProperties()
                                        .Where(
                                            l => l.GetCustomAttributes<MaskedInRequestStoreAttribute>()
                                                  .Any()
                                        )
                                        .Select(l => new { property = l, maskValue = l.GetCustomAttribute<MaskedInRequestStoreAttribute>().MaskValue })
                                        .ToList();

            var jsonString = JsonConvert.SerializeObject(request);
            var result = JsonConvert.DeserializeObject(jsonString, request.GetType()) as IBaseRequest;

            foreach (var item in propertiesToMask)
            {
                item.property.SetValue(result, item.maskValue);
            }

            return result;
        }
    }
}
