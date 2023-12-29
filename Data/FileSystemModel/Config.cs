using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.FileSystemModel
{
    public class Config
    {
        public string broker_ip { get; set; } = "broker";
        public int broker_port { get; set; } = 1883;
    }
}
