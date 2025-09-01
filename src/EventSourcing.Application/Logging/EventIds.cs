namespace EventSourcing.Application.Logging;


/// <summary>
/// Contains EventId constants for structured logging.
/// </summary>
public static class EventIds
{
    // General events
    public const int HandlingMontlyReport = 1000;
    public const int MonthlyReportGenerated = 1001;
    public const int Last = MonthlyReportGenerated;
}
