namespace ApartmentAz.BLL.DTOs.Dashboard;

public class AdminDashboardFullDto
{
    // Stats
    public int TotalListings { get; set; }
    public int PendingListings { get; set; }
    public int ApprovedListings { get; set; }
    public int TotalUsers { get; set; }
    public int BannedUsers { get; set; }

    // Charts
    public DashboardChartDataDto ChartData { get; set; } = new();

    // Recent Activity
    public List<RecentPendingListingDto> RecentPendingListings { get; set; } = [];
    public List<RecentUserDto> RecentUsers { get; set; } = [];
}
