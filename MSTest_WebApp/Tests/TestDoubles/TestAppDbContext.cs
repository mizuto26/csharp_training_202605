using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Context;

namespace MSTest_WebApp.Tests.TestDoubles;

internal sealed class TestAppDbContext : AppDbContext
{
    public TestAppDbContext()
        : base(new DbContextOptionsBuilder<AppDbContext>().Options)
    {
    }

    public int SaveChangesCallCount { get; private set; }

    public override int SaveChanges()
    {
        SaveChangesCallCount++;
        return 1;
    }
}
