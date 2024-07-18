using MatrimonyEventConsumer.Models;
using MatrimonyEventConsumer.Repos;
using MatrimonyEventConsumer.Services;
using Microsoft.EntityFrameworkCore;

namespace MatrimonyEventConsumer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddLogging(l => l.AddLog4Net());
        builder.Services.AddDbContext<ReplicaContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddScoped<IBaseRepo<Address>, AddressRepo>();
        builder.Services.AddHostedService<ConsumerService>();

        var app = builder.Build();

        app.UseCors();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
