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

    // Conexi�n SQL.
    {

        // Conexi�n SQL.
        string sqlConnection = string.Empty;

        // Obtiene la cadena de conexi�n.
        sqlConnection = builder.Configuration["ConnectionStrings:Somee"] ?? "";

        // Establecer.
        Conexi�n.SetStringConnection(sqlConnection);

    }

    // Llave de la app en Identity.
    LIN.Access.Auth.Build.SetAuth(builder.Configuration["lin:app"] ?? "");
   

    // Crea la aplicaci�n.
    var app = builder.Build();

    // Asegura la creaci�n de la base de datos.
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

    // Otras pol�ticas.
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseCors("AllowAnyOrigin");
    app.UseAuthorization();
    app.MapControllers();

    // Inicia las conexiones
    _ = Conexi�n.StartConnections();

    // Estado del servidor
    ServerLogger.OpenDate = DateTime.Now;

    app.Run();
}
catch
{
}