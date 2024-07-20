using System.Diagnostics.CodeAnalysis;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using MatrimonyApiService.AddressCQRS;
using MatrimonyApiService.AddressCQRS.Command;
using MatrimonyApiService.AddressCQRS.Event;
using MatrimonyApiService.AddressCQRS.Query;
using MatrimonyApiService.Auth;
using MatrimonyApiService.Chat;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Services;
using MatrimonyApiService.Commons.Validations;
using MatrimonyApiService.MatchRequest;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Message;
using MatrimonyApiService.Preference;
using MatrimonyApiService.Profile;
using MatrimonyApiService.ProfileView;
using MatrimonyApiService.Report;
using MatrimonyApiService.Staff;
using MatrimonyApiService.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace MatrimonyApiService;

[ExcludeFromCodeCoverage]
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
        builder.Services.AddHttpContextAccessor();

        #region Context

        builder.Services.AddDbContext<MatrimonyContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        );

        builder.Services.AddDbContext<EventStoreDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("eventStore"));
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        );

        #endregion

        #region Repos

        builder.Services.AddScoped<IBaseRepo<AddressCQRS.Address>, AddressRepo>();
        builder.Services.AddScoped<IBaseRepo<User.User>, UserRepo>();
        builder.Services.AddScoped<IBaseRepo<Profile.Profile>, ProfileRepo>();
        builder.Services.AddScoped<IBaseRepo<Staff.Staff>, StaffRepo>();
        builder.Services.AddScoped<IBaseRepo<ProfileView.ProfileView>, ProfileViewRepo>();
        builder.Services.AddScoped<IBaseRepo<Preference.Preference>, PreferenceRepo>();
        builder.Services.AddScoped<IBaseRepo<MatchRequest.MatchRequest>, MatchRequestRepo>();
        builder.Services.AddScoped<IBaseRepo<Message.Message>, MessageRepo>();
        builder.Services.AddScoped<IBaseRepo<Membership.Membership>, MembershipRepo>();
        builder.Services.AddScoped<IBaseRepo<Report.Report>, ReportRepo>();

        builder.Services.AddScoped<IEventStore, EventStore>();

        #endregion

        #region Logger

        builder.Services.AddLogging(l => l.AddLog4Net());

        #endregion

        #region Services

        builder.Services.AddMediatR(options => { options.RegisterServicesFromAssemblies(typeof(Program).Assembly); });

        builder.Services.AddScoped<IProfileViewService, ProfileViewService>();
        builder.Services.AddScoped<IProfileService, ProfileService>();
        builder.Services.AddScoped<IPreferenceService, PreferenceService>();
        builder.Services.AddScoped<IMembershipService, MembershipService>();
        builder.Services.AddScoped<IMessageService, MessageService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IMatchRequestService, MatchRequestService>();
        builder.Services.AddScoped<IBaseService<Report.Report, ReportDto>, ReportService>();

        builder.Services.AddScoped<ICommandHandler<CreateAddressCommand>, CreateAddressCommandHandler>();
        builder.Services.AddScoped<ICommandHandler<UpdateAddressCommand>, UpdateAddressCommandHandler>();
        builder.Services.AddScoped<ICommandHandler<DeleteAddressCommand>, DeleteAddressCommandHandler>();
        builder.Services.AddScoped<IQueryHandler<GetAddressByIdQuery, AddressDto>, GetAddressByIdQueryHandler>();
        builder.Services
            .AddScoped<IQueryHandler<GetAllAddressesQuery, List<AddressDto>>, GetAllAddressesQueryHandler>();
        builder.Services.AddScoped<CustomControllerValidator>();
        builder.Services.AddScoped<OtpService>();

        builder.Services.AddScoped<NewMessageService>();
        builder.Services.AddScoped<ChatService>();
        builder.Services.AddScoped<EmailService>();
        builder.Services.AddScoped<EventProducerService>();

        #endregion

        #region AuthConfig

        builder.Services.AddKeycloakAuthorization(builder.Configuration);
        builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
        builder.Services.AddAuthorization(option =>
        {
            option.AddPolicy("ChatPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
            });
            option.AddPolicy("AdminPolicy", policy => policy.RequireRealmRoles("admin"));
            option.AddPolicy("UserPolicy", policy => policy.RequireRealmRoles("user"));
        });

        #endregion

        #region Cors

        builder.Services.AddCors(opts =>
        {
            opts.AddPolicy("AllowAll",
                corsPolicyBuilder => { corsPolicyBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
        });

        #endregion

        builder.Services.AddSignalR();

        var app = builder.Build();

        #region ChatHub

        app.UseCors("AllowAll");
        app.MapHub<ChatHub>("/chatHub");
        
        #endregion
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();
        app.UseAuthorization();
        app.UseAuthentication();


        app.MapControllers();

        app.Run();
    }
}