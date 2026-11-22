using BeautySalon.Domain.Data;

namespace BeautySalon.Domain.Tests;

/// <summary>
/// Тесты, в соответствии с заданием.
/// </summary>
public class QueriesTests(TestDataFixture fixture) : IClassFixture<TestDataFixture>
{
    /// <summary>
    /// Вывести информацию о всех мастерах, стаж работы которых не менее 5 лет.
    /// </summary>
    /// <param name="lastName"> Фамилия мастера </param>
    /// <param name="expected"> Флаг того, что стаж работы не менее 5 лет </param>
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
        var result = fixture.Masters
            .Where(m => m.ExperienceYears >= 5)
            .Any(m => m.LastName == lastName);

        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Вывести информацию о всех окошках 💅 выбранного мастера.
    /// (под окошком подразумевается промежуток времени между двумя записями на услугу)
    /// </summary>
    /// <param name="lastName"> Фамилия мастера </param>
    /// <param name="expectedWindowsCount"> Количество окошек </param>
    [Theory]
    [InlineData("Сидорова", 4)]
    [InlineData("Кузнецов", 1)]
    [InlineData("Смирнов", 3)]
    public void Test_2_MasterIdleWindows(string lastName, int expectedWindowsCount)
    {
        var masterAppointments = fixture.Appointments
            .Where(a => a.Master.LastName == lastName)
            .OrderBy(a => a.AppointmentDate)
            .ToList();

        var windowsLengths = masterAppointments
            .Zip(masterAppointments.Skip(1),
                (current, next) => next.AppointmentDate - current.EndAppointmentDate)
            .Where(window => window > TimeSpan.Zero)
            .ToList();

        Assert.Equal(expectedWindowsCount, windowsLengths.Count);
    }

    /// <summary>
    /// Вывести топ 5 наиболее популярных услуг.
    /// </summary>
    [Fact]
    public void Test_3_TopFivePopularServices()
    {
        var popularServices = fixture.Appointments
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

    /// <summary>
    /// Вывести информацию о количестве повторных записей клиентов за последний месяц. 
    /// </summary>
    [Fact]
    public void Test_4_RepeatAppointmentsLastMonth()
    {
        var repeats = fixture.Appointments
            .Where(a =>
            a.AppointmentDate >= fixture.BaseDate.AddMonths(-1) &&
            a.AppointmentDate <= fixture.BaseDate)
            .GroupBy(a => a.Client)
            .Where(g => g.Count() > 1)
            .Select(g => (ClientName: g.Key.FullName, Count: g.Count() - 1))
            .OrderBy(x => x.ClientName)
            .ToList();

        var expected = new List<(string ClientName, int Count)>
        {
            ("Иванов Иван Иванович", 2),
            ("Петрова Анна Сергеевна", 1)
        };

        Assert.Equal(expected, repeats);
    }

    /// <summary>
    /// Вывести информацию о клиентах, записанных к нескольким мастерам, упорядочить по дате рождения.
    /// </summary>
    [Fact]
    public void Test_5_ClientsWithMultipleMastersOrderedByBirthDate()
    {
        var result = fixture.Appointments
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