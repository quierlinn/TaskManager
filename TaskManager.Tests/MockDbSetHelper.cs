using Microsoft.EntityFrameworkCore;
using Moq;

namespace TaskManager.Tests;

public static class MockDbSetHelper
{
    public static Mock<DbSet<T>> GetMockDbSet<T>(List<T> data) where T : class
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        mockSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(data.Add);
        mockSet.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>(t => data.Remove(t));
        return mockSet;
    }
}