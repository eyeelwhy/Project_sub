using api_docker.ModelDb;

internal class Program
{
    public static DockerShopContext context { get; } = new DockerShopContext();
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.UseUrls("http://*:80");
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors();

        var app = builder.Build();
        app.UseCors(c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:8080"));
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
    }
}