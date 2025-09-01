namespace EventSourcing.Application.Queries;

using Microsoft.Extensions.Logging;
using EventSourcing.Domain.Seedwork;
using EventSourcing.Application.Logging;

public record MonthlyReportDto(int Year, int Month, string Summary);

// Query record
public record GetMonthlyReportQuery(int Year, int Month);

// Query handler interface
public interface IGetMonthlyReportQueryHandler
{
    public Result<MonthlyReportDto> Handle(GetMonthlyReportQuery query);
}

// Implementation of the query handler
public class GetMonthlyReportQueryHandler(ILogger<GetMonthlyReportQueryHandler> logger) : IGetMonthlyReportQueryHandler
{
    public Result<MonthlyReportDto> Handle(GetMonthlyReportQuery query)
    {
        logger.LogHandlingQuery(query.Year, query.Month);

        var summary = $"Report for {query.Month}/{query.Year}";
        var report = new MonthlyReportDto(query.Year, query.Month, summary);

        logger.LogReportGenerated(query.Year, query.Month, summary);

        return Result.Ok(report);
    }
}
