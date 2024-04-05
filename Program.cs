using prometoFoodTrucksBackend.Services;
using prometoFoodTrucksBackend.Services.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<AppServices>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PasswordService>();

builder.Services.AddDbContext<DataContext>(Options => Options.UseSqlServer(connectionString));


builder.Services.AddCors(options => options.AddPolicy("BlogPolicy", 
builder => {
    builder.WithOrigins("http://localhost:5244")
    .AllowAnyHeader()
    .AllowAnyMethod();
}
));







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

app.UseCors("BlogPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
