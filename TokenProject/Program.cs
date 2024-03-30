using DataProvider.DataContext;
using JWTManager.Abstract;
using JWTManager.Handler;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddDbContext<ApplicationDbContext>(p => p.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(c =>
{
    c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    c.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(c =>
{
    c.RequireHttpsMetadata = true;
    c.SaveToken = true;
    c.TokenValidationParameters = new TokenValidationParameters
    {
        ClockSkew = TimeSpan.Zero,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWTTokenHandler.JWT_SECURITY_KEY))
    };
});
builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
    policy =>
    {
        policy.WithOrigins("https://localhost:44396").AllowAnyMethod().AllowAnyHeader();
    }));

builder.Services.AddSingleton(typeof(IJWTTokenService), typeof(JWTTokenHandler));
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyTestService", Version = "1.0", Description = "JWT Testing Token .Net 6" });
//    c.AddSecurityDefinition("Bearer Authorization", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "Enter Bearer Token Authorization : `Bearer Generate JWT Token`",
//    });
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = JwtBearerDefaults.AuthenticationScheme
//                }
//            },
//            new string[] { }
//        }
//    });
//c.OperationFilter<CustomHeaderSwaggerAttribute>();
//var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//c.IncludeXmlComments(xmlPath);
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestService");
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("NgOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.UseEndpoints(c => c.MapControllers());

app.Run();
