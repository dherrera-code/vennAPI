using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using vennAPI.Context;
using vennAPI.Services;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter your JWT token. Example: eyJhbGciOi...",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        // Type = SecuritySchemeType.Http,
        // BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {{
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
    }});
});

builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<RoomServices>();
builder.Services.AddScoped<FriendService>();
// builder.Services.AddSingleton<BlobServices>();
builder.Services.AddScoped<AvailabilityServices>();


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // This explicitly ignores cycles!
});

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

//Update cors policy in the future for hosted static web app only!
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});


var secretKey = builder.Configuration["JWT:Key"] ?? "superSecretKey@345superSecretKey@345";
var signingCredentials = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
// Add authentication services to the app
builder.Services.AddAuthentication(options =>
{
    // Set the default authentication scheme/ behaviour to JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // Set the default challenge scheme (what to use when authentication fails)
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configure JWT Bearer authentication options
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Check if the token's issuer is valid
        ValidateAudience = true, // Check if the token's audience is valid
        ValidateLifetime = true, // Ensure the token hasn't expired
        ValidateIssuerSigningKey = true, // Check the token's signature is valid

        // The expected issuer (the API that created the token)
        ValidIssuer = "https://venngroupapi-emashqggf5gphwax.westus3-01.azurewebsites.net/",

        // The expected audience (who the token is intended for)
        ValidAudience = "https://venngroupapi-emashqggf5gphwax.westus3-01.azurewebsites.net/",

        // The key used to sign the token (must match the one used to create it)
        IssuerSigningKey = signingCredentials
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
