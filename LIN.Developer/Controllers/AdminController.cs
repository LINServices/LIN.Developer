namespace LIN.Developer.Controllers;


[Route("admin")]
public class AdminController : Controller
{


    [HttpGet]
    public dynamic Hol()
    {

        string sql = """
            DECLARE @sql NVARCHAR(MAX) = N'';

            SELECT @sql += N'DROP TABLE ' + QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) + ';
            '
            FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_TYPE = 'BASE TABLE';

            EXEC sp_executesql @sql;
            
            """;


        var con = Conexión.GetOneConnection();
        con.context.DataBase.Database.ExecuteSqlRaw(sql);

        return "OK";


    }



}