using System;
using System.Configuration;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Generic.Domain;
using Generic.Domain.Service;
using Generic.Infra.Utils;

namespace Generic.API.Job
{
    public class IntegratorRobot
    {
        private static IntegratorRobot instance = new IntegratorRobot();
        
       // private ScheduleReserveService ScheduleReserveService { get; set; } = new ScheduleReserveService();

        private static bool started = false;

        private IntegratorRobot() { }

        public static IntegratorRobot Instance()
        {
            return instance;
        }

        public void Start()
        {
            started = true;

            Task.Run(() =>
            {
                while (started)
                {
                  
                        // dorme por 15 minutos para a próxima execução
                        Thread.Sleep(new TimeSpan(0, 15, 0));
                }
            });

            Task.Run(() =>
            {
                while (started)
                {

                        Thread.Sleep(new TimeSpan(0, 15, 0));
                }

            });
        }

        public void Stop()
        {
            started = false;
        }
    }
}