using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Infra.Utils
{
    public static class DateTimeUtils
    {
        private static string TIME_ZONE_BR = "E. South America Standard Time";

        public static System.DateTime ToTimeZoneBrazil(this System.DateTime dt)
        {
            try
            {
                System.TimeZoneInfo tz = System.TimeZoneInfo.FindSystemTimeZoneById(TIME_ZONE_BR);
                if (tz != null)
                {
                    System.TimeZoneInfo tzSource = TimeZoneInfo.Utc;
                    if (dt.Kind == DateTimeKind.Local)
                    {
                        tzSource = TimeZoneInfo.Local;
                    }
                    System.DateTime dataAjustada = TimeZoneInfo.ConvertTime(dt, tzSource, tz);

                    return dataAjustada;
                }
                else
                {
                    throw new Exception();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro na data", ex);
            }
        }

    }
}
