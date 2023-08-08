global using LIN.Developer;
global using LIN.Developer.Services;
global using LIN.Developer.Types.Enumerations;
global using LIN.Developer.Types.Models;
global using LIN.Shared.Enumerations;
global using LIN.Shared.Responses;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using LIN.Shared.Tools;
global using LIN.Developer.Data.Query;
global using LIN.Shared.Validations;

try
{

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSignalR();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAnyOrigin",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
    });




    // Add services to the container.
    string sqlConnection = "";

#if SOMEE
    sqlConnection = builder.Configuration["ConnectionStrings:Somee"] ?? "";
#elif AZURE
    sqlConnection = builder.Configuration["ConnectionStrings:Azure"] ?? "";
#endif

    Conexión.SetStringConnection(sqlConnection);


    try
    {
        // SQL Server
        builder.Services.AddDbContext<LIN.Developer.Data.Context>(options =>
        {
            options.UseSqlServer(sqlConnection);
        });
    }
    catch (Exception ex)
    {
        ServerLogger.LogError("Error" + ex.Message);
    }




    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHttpContextAccessor();


    var app = builder.Build();


    try
    {
        // Si la base de datos no existe
        using var scope = app.Services.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<LIN.Developer.Data.Context>();
        var res = dataContext.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        ServerLogger.LogError("Error" + ex.Message);
    }




    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
    }


    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseCors("AllowAnyOrigin");

    app.UseAuthorization();

    app.MapControllers();

    // Inicia las conexiones
    _ = Conexión.StartConnections();

    // Estado del servidor
    ServerLogger.OpenDate = DateTime.Now;

    app.Run();
}
catch (Exception ex)
{
    ServerLogger.LogError("");
}
