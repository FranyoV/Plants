using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PlantsAPI.Configuration;
using PlantsAPI.Data;
using PlantsAPI.Jobs;
using PlantsAPI.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.Text.Json.Serialization;
using Quartz;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<PlantsDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlantsDatabase")));




builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    var jobKey = new JobKey("SendMaintenanceEmailJob");
    q.AddJob<SendMaintenanceEmailJob>(opt => opt.WithIdentity(jobKey));
    q.AddTrigger(opts =>

        opts.ForJob(jobKey)
        .WithIdentity("SendMaintenanceEmailJob-trigger")
        .WithCronSchedule("0 * * * * ?"));

});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.AddScoped<INotificationService, NotificationService>();
//builder.Services.Configure<SendMaintenanceEmailJob>(builder.Configuration.GetSection("SendMaintenanceEmailJob"));


builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(options =>
{ 
 options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
   
});


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type=ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
  
    }) ;

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:SymmetricKey").Value)),
            ValidateIssuer = false,
            ValidateAudience = false

        };
    });


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200");
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.AllowAnyOrigin();
                      });

});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);


app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();



app.Run();
