using Monki.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.SetConfiguration();

// Build
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Swagger
if(app.Environment.IsDevelopment())
{
	app.UseCors("AllowFrontend");
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.Run();