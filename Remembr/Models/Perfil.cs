using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remembr_.Models
{
    internal class Perfil
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }

        public required string Password { get; set; }
        public required Fotografia Fotografia { get; set; }
    }
}
