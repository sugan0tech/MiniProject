using System.Text;
using MatrimonyApiService.Address;
using MatrimonyApiService.Auth;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.MatchRequest;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Message;
using MatrimonyApiService.Preference;
using MatrimonyApiService.Profile;
using MatrimonyApiService.ProfileView;
using MatrimonyApiService.Report;
using MatrimonyApiService.Staff;
using MatrimonyApiService.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddLogging(l => l.AddLog4Net());
        builder.Services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        }); // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddLogging(l => l.AddLog4Net());
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
        builder.Services.AddScoped<IBaseRepo<MatchRequest.MatchRequest>, MatchRequestRepo>();
        builder.Services.AddScoped<IBaseRepo<Message.Message>, MessageRepo>();
        builder.Services.AddScoped<IBaseRepo<Membership.Membership>, MembershipRepo>();
        builder.Services.AddScoped<IBaseRepo<Report.Report>, ReportRepo>();

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
        builder.Services.AddScoped<IMatchRequestService, MatchRequestRequestService>();
        builder.Services.AddScoped<IBaseService<Report.Report, ReportDto>, ReportService>();

        #endregion

        #region AuthConfig

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey:JWT"]))
                };
            });
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policyBuilder => policyBuilder.RequireRole(Role.Admin.ToString()));
            options.AddPolicy("UserPolicy",
                policyBuilder => policyBuilder.RequireRole(Role.User.ToString(), Role.Admin.ToString()));
        });

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