namespace ApartmentAz.BLL.DTOs;

public class AdminDashboardStatsDto
{
    public int TotalListings { get; set; }
    public int PendingListings { get; set; }
    public int ApprovedListings { get; set; }
    public int TotalUsers { get; set; }
    public int BannedUsers { get; set; }
}
