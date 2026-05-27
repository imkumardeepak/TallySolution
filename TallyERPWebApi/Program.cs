using NLog.Web;
using Serilog;
using TallyERPWebApi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
	});



//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Configure NLog
builder.Logging.ClearProviders();
builder.Logging.AddNLog("nlog.config"); // Specify the NLog config file

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddResponseCompression();
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllPolicy", policy =>
	{
		policy.AllowAnyOrigin()    // Allow all origins
			  .AllowAnyMethod()    // Allow all HTTP methods (POST, GET, etc.)
			  .AllowAnyHeader();   // Allow all headers
	});
});

builder.Host.UseSerilog((ctx, lc) => lc
	.WriteTo.Console()
	.ReadFrom.Configuration(ctx.Configuration));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpClient<TallyService>();
builder.Services.AddHttpClient<PostTallyService>();
//builder.Services.AddHostedService<TallyBackgroundService>();

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
app.MapGet("/", () => "Welcome to the ASP.NET Core Application!");
app.UseResponseCompression();
app.UseDefaultFiles(); // Looks for index.html, default.html, etc.
app.UseStaticFiles();
app.UseCors("AllowAllPolicy");
app.Run();
