using Duende.Bff;
using Duende.Bff.Yarp;
using Microsoft.IdentityModel.Tokens;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddReverseProxy()
    .AddBffExtensions();

//builder.LoadFromMemory(
//    new[]
//    {
//        new RouteConfig()
//        {
//            RouteId = "api_user",
//            ClusterId = "cluster1",

//            Match = new()
//            {
//                Path = "/user_api/{**catch-all}"
//            }
//        }.WithAccessToken(TokenType.User).WithAntiforgeryCheck(),
//        new RouteConfig()
//        {
//            RouteId = "api_client",
//            ClusterId = "cluster1",

//            Match = new()
//            {
//                Path = "/client_api/{**catch-all}"
//            }
//        }.WithAccessToken(TokenType.Client).WithAntiforgeryCheck(),
//        new RouteConfig()
//        {
//            RouteId = "api_anon",
//            ClusterId = "cluster1",

//            Match = new()
//            {
//                Path = "/anon_api/{**catch-all}"
//            }
//        }.WithAntiforgeryCheck(),
//        new RouteConfig()
//        {
//            RouteId = "api_optional_user",
//            ClusterId = "cluster1",

//            Match = new()
//            {
//                Path = "/optional_user_api/{**catch-all}"
//            }
//        }.WithAntiforgeryCheck()//.WithOptionalUserAccessToken().WithAntiforgeryCheck()
//    },
//    new[]
//    {
//        new ClusterConfig
//        {
//            ClusterId = "cluster1",

//            Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
//            {
//                { "destination1", new() { Address = "https://localhost:5010" } },
//            }
//        }
//    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add YARP with BFF extensions
builder.Services
    .AddBff();

builder.Services.AddAuthentication("token")
    .AddJwtBearer("token", options =>
    {
        options.Authority = builder.Configuration["Authority"];
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false,
            ValidTypes = new[] { "at+jwt" },
            NameClaimType = "name",
            RoleClaimType = "role"
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapReverseProxy();

app.Run();
