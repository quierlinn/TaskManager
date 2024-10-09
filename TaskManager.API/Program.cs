using Microsoft.EntityFrameworkCore;
using TaskManager.DataAccess;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

builder.Services.AddDbContext<TaskManagerDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(TaskManagerDbContext)));
});

app.UseHttpsRedirection();
app.Run();
