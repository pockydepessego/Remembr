using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remembr.Models
{
    internal class Periodicidade
    {
        public required string IDTarefaOriginal { get; set;} 
        public required DateTime DataLimite { get; set; }
        public string[]? IDChildTarefas { get; set; }


    }
}
