using API.Infra;
using API.Services;

var builder = WebApplication.CreateBuilder(args);

// Rule: Consistent subdomain names should be used for your APIs
// Rule: Consistent subdomain names should be used for your client developer portal
// Configure this on DNS

// Add services to the container.
builder.Services.AddSingleton<IData, Data>();
builder.Services.AddSingleton<ICollectionAccessor, CollectionAccessor>();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IPetRepository, PetRepository>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IPetService, PetService>();

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

app.Run();
