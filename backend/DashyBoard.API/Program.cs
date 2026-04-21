using DashyBoard.API.Authentication;
using System.Text;
using DashyBoard.API.Middleware;
using DashyBoard.Application;
using DashyBoard.Infrastructure;
using DashyBoard.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "DashyBoard API",
            Version = "v1",
            Description = "API for managing DashyBoard application",
        }
    );
    c.AddSecurityDefinition(
        "github",
        new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    // Token URL is set dynamically per-request by GitHubTokenUrlFilter
                    AuthorizationUrl = new Uri("https://github.com/login/oauth/authorize"),
                    TokenUrl = new Uri("https://placeholder/oauth/github/token"),
                    Scopes = new Dictionary<string, string>
                    {
                        { "read:user", "Read GitHub user profile" },
                        { "user:email", "Read GitHub user email" },
                        { "repo", "Check repository collaborator access" },
                    },
                },
            },
        }
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "github",
                    },
                },
                new[] { "read:user" }
            },
        }
    );
    c.DocumentFilter<GitHubTokenUrlFilter>();
});

// Add JWT Authentication
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)
            ),
        };
    });

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});

// Add GitHub OAuth token validation
builder.Services.AddHttpClient();
builder
    .Services.AddAuthentication("GitHub")
    .AddScheme<AuthenticationSchemeOptions, GitHubTokenAuthHandler>("GitHub", null);
builder.Services.AddAuthorization();

// Add Clean Architecture layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
var app = builder.Build();

// Ensure database directory and database are created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Only setup directories for SQLite (skip for InMemory in tests)
    if (context.Database.IsSqlite())
    {
        var connectionString = context.Database.GetConnectionString();
        if (connectionString != null)
        {
            var builder2 = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder(
                connectionString
            );
            var dbPath = builder2.DataSource;
            var dbDir = Path.GetDirectoryName(dbPath);
            if (!string.IsNullOrEmpty(dbDir))
            {
                Directory.CreateDirectory(dbDir);
            }
        }
    }

    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DashyBoard API v1");
        c.OAuthClientId(builder.Configuration["GitHub:ClientId"]);
        c.OAuthUsePkce();
        c.OAuthScopeSeparator(" ");
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
