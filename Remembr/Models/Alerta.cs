using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remembr.Models
{
    internal class Alerta
    {
        public required bool Email { get; set; }
        public required bool Windows { get; set; }
        public required TimeSpan Tempo { get; set; }
    }
}
