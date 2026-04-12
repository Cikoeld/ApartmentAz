namespace ApartmentAz.BLL.DTOs.Dashboard;

public class DashboardChartDataDto
{
    public List<string> ListingsPerDayLabels { get; set; } = [];
    public List<int> ListingsPerDayValues { get; set; } = [];
    public List<string> RegistrationsPerMonthLabels { get; set; } = [];
    public List<int> RegistrationsPerMonthValues { get; set; } = [];
}
