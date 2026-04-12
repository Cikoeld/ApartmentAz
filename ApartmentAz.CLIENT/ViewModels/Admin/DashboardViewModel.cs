using ApartmentAz.CLIENT.Services;

namespace ApartmentAz.CLIENT.ViewModels.Admin;

public class DashboardViewModel
{
    // Stats
    public int TotalListings { get; set; }
    public int PendingListings { get; set; }
    public int ApprovedListings { get; set; }
    public int TotalUsers { get; set; }
    public int BannedUsers { get; set; }

    // Chart data (serialized to JSON for Chart.js)
    public List<string> ListingsPerDayLabels { get; set; } = [];
    public List<int> ListingsPerDayValues { get; set; } = [];
    public List<string> RegistrationsPerMonthLabels { get; set; } = [];
    public List<int> RegistrationsPerMonthValues { get; set; } = [];

    // Recent Activity
    public List<RecentPendingListingApiModel> RecentPendingListings { get; set; } = [];
    public List<RecentUserApiModel> RecentUsers { get; set; } = [];

    public bool LoadFailed { get; set; }
}
