namespace EventSourcing.Application.Commands;

using System;
using EventSourcing.Domain.Seedwork;

// Command as a record
public record RequestMonthlyReportGenerationCommand(Guid ReportId, int Year, int Month);

// Interface for the command handler
public interface IRequestMonthlyReportGenerationCommandHandler
{
    public Task<Result> Handle(RequestMonthlyReportGenerationCommand command);
}

// Implementation of the command handler
public class RequestMonthlyReportGenerationCommandHandler : IRequestMonthlyReportGenerationCommandHandler
{
    public Task<Result> Handle(RequestMonthlyReportGenerationCommand command) =>
        // Implement the logic to handle the command here
        // Example:
        // GenerateMonthlyReport(command.ReportId, command.Year, command.Month);
        Task.FromResult(Result.Ok());
}
