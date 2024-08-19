using Restoranas.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerProject
{
    public class NoLogger : ILoggerService
    {
        public void Log(string message)
        {
        }
    }
}
