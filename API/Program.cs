using API.Handlers;
using API.Infra;
using API.Services;

var builder = WebApplication.CreateBuilder(args);

// Rule: Consistent subdomain names should be used for your APIs
// Rule: Consistent subdomain names should be used for your client developer portal
// Configure this on DNS

// Add services to the container.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<HashHandlerOptions>(builder.Configuration);
builder.Services.AddSingleton<IHashHandler, HashHandler>();

builder.Services.Configure<TokenHandlerOptions>(builder.Configuration);
builder.Services.AddSingleton<ITokenHandler, TokenHandler>();
builder.Services.ConfigureJWT(builder.Configuration["JWTSecret"]);

builder.Services.AddSingleton<IData, Data>();
builder.Services.AddSingleton<ICollectionAccessor, CollectionAccessor>();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.Run();
