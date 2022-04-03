using HandMadeApi.Auth;
using HandMadeApi.Models.StoreDatabase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddCors(options => {
    options.AddPolicy("AllowSpecificOrigin",
        builder => {
            builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
});
var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.Authority = domain;
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

builder.Services.AddAuthorization(options => {
    options.AddPolicy("read:products", policy => policy.Requirements.Add(new HasScopeRequirement("read:products", domain)));
    options.AddPolicy("post:products", policy => policy.Requirements.Add(new HasScopeRequirement("post:products", domain)));
    options.AddPolicy("read:category", policy => policy.Requirements.Add(new HasScopeRequirement("read:category", domain)));
    options.AddPolicy("post:category", policy => policy.Requirements.Add(new HasScopeRequirement("post:category", domain)));
    options.AddPolicy("update:product", policy => policy.Requirements.Add(new HasScopeRequirement("update:product", domain)));
    options.AddPolicy("delete:product", policy => policy.Requirements.Add(new HasScopeRequirement("delete:product", domain)));
    options.AddPolicy("update:category", policy => policy.Requirements.Add(new HasScopeRequirement("update:category", domain)));
    options.AddPolicy("delete:category", policy => policy.Requirements.Add(new HasScopeRequirement("delete:category", domain)));
    options.AddPolicy("read:clients", policy => policy.Requirements.Add(new HasScopeRequirement("read:clients", domain)));
    options.AddPolicy("post:client", policy => policy.Requirements.Add(new HasScopeRequirement("post:client", domain)));
    options.AddPolicy("update:client", policy => policy.Requirements.Add(new HasScopeRequirement("update:client", domain)));
    options.AddPolicy("delete:client", policy => policy.Requirements.Add(new HasScopeRequirement("delete:client", domain)));
    options.AddPolicy("read:orders", policy => policy.Requirements.Add(new HasScopeRequirement("read:orders", domain)));
    options.AddPolicy("post:orders", policy => policy.Requirements.Add(new HasScopeRequirement("post:orders", domain)));
    options.AddPolicy("update:orders", policy => policy.Requirements.Add(new HasScopeRequirement("update:orders", domain)));
    options.AddPolicy("delete:orders", policy => policy.Requirements.Add(new HasScopeRequirement("delete:orders", domain)));
    options.AddPolicy("read:rate", policy => policy.Requirements.Add(new HasScopeRequirement("read:rate", domain)));
    options.AddPolicy("post:rate", policy => policy.Requirements.Add(new HasScopeRequirement("post:rate", domain)));
    options.AddPolicy("update:rate", policy => policy.Requirements.Add(new HasScopeRequirement("update:rate", domain)));
    options.AddPolicy("delete:rate", policy => policy.Requirements.Add(new HasScopeRequirement("delete:rate", domain)));
    options.AddPolicy("read:store", policy => policy.Requirements.Add(new HasScopeRequirement("read:store", domain)));
    options.AddPolicy("update:store", policy => policy.Requirements.Add(new HasScopeRequirement("update:store", domain)));
    options.AddPolicy("post:store", policy => policy.Requirements.Add(new HasScopeRequirement("post:store", domain)));
    options.AddPolicy("delete:store", policy => policy.Requirements.Add(new HasScopeRequirement("delete:store", domain)));
});
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
//connection string
builder.Services.AddDbContext<StoreContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:connectionString"]));
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseCors("AllowSpecificOrigin");


app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
