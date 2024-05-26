using MatrimonyApiService.Address;
using MatrimonyApiService.Auth;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Match;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Message;
using MatrimonyApiService.Preference;
using MatrimonyApiService.Profile;
using MatrimonyApiService.ProfileView;
using MatrimonyApiService.Staff;
using MatrimonyApiService.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace MatrimonyApiService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        #region Context

        builder.Services.AddDbContext<MatrimonyContext>(optionsBuilder =>
            optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

        #endregion

        #region Repos

        builder.Services.AddScoped<IBaseRepo<Address.Address>, AddressRepo>();
        builder.Services.AddScoped<IBaseRepo<User.User>, UserRepo>();
        builder.Services.AddScoped<IBaseRepo<Profile.Profile>, ProfileRepo>();
        builder.Services.AddScoped<IBaseRepo<Staff.Staff>, StaffRepo>();
        builder.Services.AddScoped<IBaseRepo<ProfileView.ProfileView>, ProfileViewRepo>();
        builder.Services.AddScoped<IBaseRepo<Preference.Preference>, PreferenceRepo>();
        builder.Services.AddScoped<IBaseRepo<Match.Match>, MatchRepo>();
        builder.Services.AddScoped<IBaseRepo<Message.Message>, MessageRepo>();
        builder.Services.AddScoped<IBaseRepo<Membership.Membership>, MembershipRepo>();

        #endregion

        #region Logger

        builder.Services.AddLogging(l => l.AddLog4Net());

        #endregion
        #region Services

        builder.Services.AddScoped<IAddressService, AddressService>();
        builder.Services.AddScoped<IProfileViewService, ProfileViewService>();
        builder.Services.AddScoped<IProfileService, ProfileService>();
        builder.Services.AddScoped<IPreferenceService, PreferenceService>();
        builder.Services.AddScoped<IMembershipService, MembershipService>();
        builder.Services.AddScoped<IMessageService, MessageService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IMatchService, MatchService>();

        #endregion

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}