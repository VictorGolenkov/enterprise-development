namespace BeautySalon.Domain.Tests;

public class QueriesTests
{
    [Theory]
    [InlineData("Сидорова", true)]
    [InlineData("Кузнецов", false)]
    [InlineData("Смирнов", true)]
    [InlineData("Волкова", true)]
    [InlineData("Морозов", false)]
    [InlineData("Павлова", true)]
    [InlineData("Семенов", false)]
    [InlineData("Григорьева", true)]
    [InlineData("Николаев", true)]
    [InlineData("Алексеева", false)]
    public void Test_1_MastersWithExperienceFiveYearsOrMore(string lastName, bool expected)
    {
        var result = TestData.Masters
            .Where(m => m.ExperienceYears >= 5)
            .Any(m => m.LastName == lastName);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Сидорова", 4)]
    [InlineData("Кузнецов", 1)]
    [InlineData("Смирнов", 3)]
    public void Test_2_MasterIdleWindows(string lastName, int expectedWindowsCount)
    {
        var masterAppointments = TestData.Appointments
            .Where(a => a.Master.LastName == lastName)
            .OrderBy(a => a.AppointmentDate)
            .ToList();

        var windowsLengths = new List<TimeSpan>();
        for (int i = 0; i < masterAppointments.Count - 1; i++)
        {
            var endOfCurrentAppointment = masterAppointments[i].EndAppointmentDate;
            var startOfNextAppointment = masterAppointments[i + 1].AppointmentDate;

            TimeSpan window = startOfNextAppointment - endOfCurrentAppointment;

            if (window > TimeSpan.Zero)
            {
                windowsLengths.Add(window);
            }
        }

        Assert.Equal(expectedWindowsCount, windowsLengths.Count);
    }

    [Fact]
    public void Test_3_TopFivePopularServices()
    {
        var popularServices = TestData.Appointments
            .GroupBy(a => a.Service.Name)
            .Select(g => (ServiceName: g.Key, Count: g.Count()))
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.ServiceName)
            .Take(5)
            .ToList();

        var expected = new List<(string, int)>
    {
        ("Стрижка", 5),
        ("Маникюр", 3),
        ("Укладка", 2),
        ("Барбершоп-бритье", 1),
        ("Массаж лица", 1)
    };
        Assert.Equal(expected, popularServices);
    }

    [Fact]
    public void Test_4_RepeatAppointmentsLastMonth()
    {
        var repeats = TestData.Appointments
            .Where(a =>
            a.AppointmentDate >= DateTime.Today.AddMonths(-1) &&
            a.AppointmentDate <= DateTime.Today)
            .GroupBy(a => a.Client)
            .Where(g => g.Count() > 1)
            .Select(g => (ClientName: g.Key.FullName, Count: g.Count()))
            .OrderBy(x => x.ClientName)
            .ToList();

        var expected = new List<(string ClientName, int Count)>
        {
            ("Иванов Иван Иванович", 3),
            ("Петрова Анна Сергеевна", 2)
        };

        Assert.Equal(expected, repeats);
    }

    [Fact]
    public void Test_5_ClientsWithMultipleMastersOrderedByBirthDate()
    {
        var result = TestData.Appointments
            .GroupBy(a => a.Client, a => a.Master)
            .Where(g => g.Distinct().Count() > 1)
            .OrderBy(g => g.Key.BirthDate)
            .Select(g => g.Key.FullName)
            .ToList();

        var expected = new List<string>
            {
                "Петрова Анна Сергеевна",
                "Иванов Иван Иванович"
            };

        Assert.Equal(expected, result);
    }
}