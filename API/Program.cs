using System.Text;
using API.Data;
using API.Entities;
using API.Middleware;
using API.Services;
using API.Services.AttendanceRecordService;
using API.Services.CandidateAuthService;
using API.Services.CandidateService;
using API.Services.CohortService;
using API.Services.EmailService;
using API.Services.TrainingProgramService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            options.JsonSerializerOptions.MaxDepth = 64; // You can adjust the depth limit as needed.
        });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put Bearer + your token in the box below",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme, Array.Empty<string>()
        }
    });
});

// Add and Configure Database context service

// builder.Services.AddDbContext<AppDbContext>(
//     opt => 
//     {
//         opt.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),    new MySqlServerVersion(new Version(8, 0, 21)));
//         // opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
//     });

builder.Services.AddDbContext<AppDbContext>(opt =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    });

builder.Services.AddIdentity<User, IdentityRole>(opt => 
{
    opt.User.RequireUniqueEmail = true;
})
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


// builder.Services.AddIdentityCore<User>(opt => 
// {
//     opt.User.RequireUniqueEmail = true;
// })
//     .AddRoles<IdentityRole>()
//     .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration["JWTSettings:TokenKey"]))
        };
    });

// builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICandidateAuthService, CandidateAuthService>();
builder.Services.AddAuthorization();

builder.Services.AddScoped<TokenService>();

builder.Services.AddScoped<ICandidateService, CandidateService>();

builder.Services.AddScoped<ICohortService, CohortService>();

builder.Services.AddScoped<ITrainingProgramService, TrainingProgramService>();

builder.Services.AddScoped<IAttendanceRecordService, AttendanceRecordService>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.Configure<EmailSmtpSettings>(builder.Configuration.GetSection("EmailSmtpSettings"));

builder.Services.Configure<List<string>>(builder.Configuration.GetSection("ValidEmailDomains"));

//builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(MappingProfile));



// Configure redis cahing service
// builder.Services.AddStackExchangeRedisCache(options =>
// {
//     options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
// });


var app = builder.Build();


// Use the custom exception middleware
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthorization();

app.MapControllers();


app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000"); // ("https://example.com", "https://another-allowed-origin.com")
});


// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI(c =>
//     {
//         c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
//     });
// }


// Allow swagger for both development and production environment
app.UseSwagger();
app.UseSwaggerUI();
// app.UseSwaggerUI(c =>
// {
//     c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
// });


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseAuthentication();



var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

try
{
    await context.Database.MigrateAsync();
    await DbInitializer.Initialize(context, userManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "A problem occured during migration.");    
}

app.Run();
