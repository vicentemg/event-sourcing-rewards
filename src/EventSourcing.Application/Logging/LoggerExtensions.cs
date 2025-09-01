namespace EventSourcing.Application.Logging;

using EventSourcing.Application.Queries;
using Microsoft.Extensions.Logging;

internal static partial class LoggerExtensions
{
    [LoggerMessage(EventId = EventIds.HandlingMontlyReport, Level = LogLevel.Information, Message = "Handling GetMonthlyReportQuery for Year: {Year}, Month: {Month}")]
    internal static partial void LogHandlingQuery(this ILogger<GetMonthlyReportQueryHandler> logger, int year, int month);

    [LoggerMessage(EventId = EventIds.MonthlyReportGenerated, Level = LogLevel.Information, Message = "Monthly report generated for {Year}/{Month}: {Summary}")]
    internal static partial void LogReportGenerated(this ILogger<GetMonthlyReportQueryHandler> logger, int year, int month, string summary);
}
