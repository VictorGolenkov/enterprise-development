namespace BeautySalon.Domain.Tests;

public class QueriesTests
{
    [Fact]
    public void Test_1_MastersWithExperienceFiveYearsOrMore()
    {
        var result = TestData.Masters
            .Where(m => m.ExperienceYears >= 5)
            .ToList();

        Assert.NotEmpty(result);
        Assert.All(result, m => Assert.True(m.ExperienceYears >= 5));
    }

    [Fact]
    public void Test_2_MasterIdleWindows()
    {
        var master = TestData.Masters[0];

        var masterAppointments = TestData.Appointments
            .Where(a => a.Master.Id == master.Id)
            .OrderBy(a => a.AppointmentDate)
            .ToList();

        var windows = new List<TimeSpan>();

        for (int i = 1; i < masterAppointments.Count; i++)
        {
            var previousEnd = masterAppointments[i - 1].AppointmentDate.Add(masterAppointments[i - 1].Service.Duration);
            var currentStart = masterAppointments[i].AppointmentDate;

            if (currentStart > previousEnd)
            {
                windows.Add(currentStart - previousEnd);
            }
        }

        Assert.NotEmpty(windows);
        Assert.All(windows, w => Assert.True(w > TimeSpan.Zero));

        Assert.Contains(TimeSpan.FromHours(2), windows);
    }

    [Fact]
    public void Test_3_TopFivePopularServices()
    {
        var result = TestData.Appointments
            .GroupBy(a => a.Service.Name)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => new { ServiceName = g.Key, Count = g.Count() })
            .ToList();

        Assert.NotEmpty(result);
        Assert.True(result.Count <= 5);

        Assert.Equal("Стрижка", result.First().ServiceName);
    }

    [Fact]
    public void Test_4_RepeatAppointmentsLastMonth()
    {
        var oneMonthAgo = DateTime.Today.AddMonths(-1);

        var result = TestData.Appointments
            .Where(a => a.IsRegularClient && a.AppointmentDate >= oneMonthAgo)
            .GroupBy(a => a.Client)
            .Select(g => new
            {
                ClientName = g.Key.FullName,
                RepeatCount = g.Count()
            })
            .ToList();

        Assert.NotEmpty(result);
        Assert.Contains(result, r => r.ClientName == "Иванов Иван Иванович");
        Assert.All(result, r => Assert.True(r.RepeatCount >= 1));
    }

    [Fact]
    public void Test_5_ClientsWithMultipleMastersOrderedByBirthDate()
    {
        var result = TestData.Appointments
            .GroupBy(a => a.Client)
            .Where(g => g.Select(a => a.Master).Distinct().Count() > 1)
            .Select(g => g.Key)
            .OrderBy(c => c.BirthDate)
            .ToList();

        Assert.NotEmpty(result);
        Assert.Contains(result, c => c.LastName == "Иванов");
        Assert.Contains(result, c => c.LastName == "Петрова");

        Assert.True(result.First().BirthDate <= result.Last().BirthDate);
    }
}