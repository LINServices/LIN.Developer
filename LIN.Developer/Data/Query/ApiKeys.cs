﻿using LIN.Types.Developer.Enumerations;
using LIN.Types.Developer.Models;

namespace LIN.Developer.Data.Query;


public static class ApiKeys
{


    /// <summary>
    /// (Query) Obtiene una api key
    /// </summary>
    /// <param name="key">String de la llave</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<ApiKeyDataModel> ReadBy(string key, Conexión context)
    {

        var query = (from A in context.DataBase.ApiKeys
                    where A.Key == key && A.Status != ApiKeyStatus.Deleted
                    select A).Take(1);

        return query;
    }



    /// <summary>
    /// (Query) Obtiene una api key
    /// </summary>
    /// <param name="key">ID de la llave</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<ApiKeyDataModel> ReadBy(int id, Conexión context)
    {

        var query = (from A in context.DataBase.ApiKeys
                    where A.ID == id && A.Status != ApiKeyStatus.Deleted
                    select A).Take(1);

        return query;

    }



    /// <summary>
    /// (Query) Obtiene las llaves asociadas a un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<ApiKeyDataModel> ReadAll(int id, Conexión context)
    {

        var query = from AK in context.DataBase.ApiKeys
                    where AK.ProjectID == id && AK.Status != ApiKeyStatus.Deleted
                    select AK;

        return query;

    }



    /// <summary>
    /// (Query) Obtiene el ID del proyecto al cual esta anclado una llave
    /// </summary>
    /// <param name="key">llave</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<int> GetProjectID(string key, Conexión context)
    {

        var query = (from K in context.DataBase.ApiKeys
                     where K.Key == key && K.Status == ApiKeyStatus.Actived
                     select K.ProjectID).Take(1);

        return query;
    }



}