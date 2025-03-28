using MyWebApi.Configurations;
using MyWebApi.Repositories;
using EmailSettings = MyWebApi.Configurations.EmailSettings;

var builder = WebApplication.CreateBuilder(args);

// ✅ MongoDB Configuration
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Register services
builder.Services.AddSingleton<PersonService>();
// ✅ Register Services
builder.Services.AddSingleton<PersonRepository>();
builder.Services.AddSingleton<EmailService>();

// ✅ Add Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
