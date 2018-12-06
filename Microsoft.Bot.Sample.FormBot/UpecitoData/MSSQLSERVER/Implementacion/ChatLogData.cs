using System;
using System.Data;
using System.Data.SqlClient;
using Upecito.Data.Common;
using Upecito.Data.Implementation;
using Upecito.Data.Interface;
using Upecito.Model;

namespace Upecito.Data.MSSQLSERVER.Implementacion
{
    public class ChatLogData : BaseData, IChatLogData
    {
        public ChatLog Crear(ChatLog model)
        {
            ChatLog output = new ChatLog();

            try
            {
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_CREARCHATLOG, cnn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@pIdSesion", model.IdSesion);
                    cmd.Parameters.AddWithValue("@pIdAlumno", model.IdAlumno);
                    cmd.Parameters.AddWithValue("@pFecha", model.Fecha);
                    cmd.Parameters.AddWithValue("@pTexto", model.Texto);
                    cmd.Parameters.AddWithValue("@pIntencion", model.Intencion?? string.Empty);
                    cmd.Parameters.AddWithValue("@pFuente", model.Fuente);
                    cmd.Parameters.AddWithValue("@pTipo", model.Tipo ?? string.Empty);
                    cmd.Parameters.AddWithValue("@pContexto", model.Contexto ?? string.Empty);
                    cmd.Parameters.AddWithValue("@pParametros", model.Parametros ?? string.Empty);
                    cmd.Parameters.AddWithValue("@pConfianza", model.Confianza);

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            output = new ChatLog
                            {
                                IdChatLog = rd.GetInt32(rd.GetOrdinal("IDCHATLOG")),
                                IdSesion = rd.GetInt32(rd.GetOrdinal("IDSESION")),
                                IdAlumno = rd.GetInt32(rd.GetOrdinal("IDALUMNO")),
                                Fecha = rd.GetDateTime(rd.GetOrdinal("FECHA")),
                                Texto = rd.GetString(rd.GetOrdinal("TEXTO")),
                                Intencion = rd.GetString(rd.GetOrdinal("INTENCION")),
                                Fuente = rd.GetString(rd.GetOrdinal("FUENTE")),
                                Tipo = rd.GetString(rd.GetOrdinal("TIPO")),
                                Contexto = rd.GetString(rd.GetOrdinal("CONTEXTO")),
                                Parametros = rd.GetString(rd.GetOrdinal("PARAMETROS")),
                                Confianza = rd.GetDecimal(rd.GetOrdinal("CONFIANZA"))
                            };
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogError(ex);
            }

            return output;
        }
    }
}
