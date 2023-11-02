try
{

    // Crear el constructor.
    var builder = WebApplication.CreateBuilder(args);

    // Servicios.
    builder.Services.AddSignalR();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHttpContextAccessor();

    // CORS.
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

    // Conexión SQL.
    {

        // Conexión SQL.
        string sqlConnection = string.Empty;

        // Obtiene la cadena de conexión.
        sqlConnection = builder.Configuration["ConnectionStrings:Somee"] ?? "";

        // Establecer.
        Conexión.SetStringConnection(sqlConnection);

    }

    // Llave de la app en Identity.
    LIN.Access.Auth.Build.SetAuth(builder.Configuration["lin:app"] ?? "");
   

    // Crea la aplicación.
    var app = builder.Build();

    // Asegura la creación de la base de datos.
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


    // Uso de Swagger.
    app.UseSwagger();
    app.UseSwaggerUI();

    // Otras políticas.
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
catch
{
}