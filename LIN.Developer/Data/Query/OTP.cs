﻿using LIN.Types.Developer.Enumerations;
using LIN.Types.Developer.Models;

namespace LIN.Developer.Data.Query;


public static class OTP
{

    /// <summary>
    /// (Query) 
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<OTPDataModel> ReadAll(int id, Conexión context)
    {
        var time = DateTime.Now;
        var query = from OTP in context.DataBase.OTP
                    where OTP.ProfileID == id && OTP.Vencimiento > time && OTP.Estado == OTPStatus.actived
                    select OTP;

        return query;

    }






}