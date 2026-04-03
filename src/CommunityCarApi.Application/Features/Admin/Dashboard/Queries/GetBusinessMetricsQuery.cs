using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs.Admin;
using MediatR;

namespace CommunityCarApi.Application.Features.Admin.Dashboard.Queries;

public class GetBusinessMetricsQuery : IRequest<Result<BusinessMetricsDto>>
{
    public int MonthsBack { get; set; } = 6;
}
