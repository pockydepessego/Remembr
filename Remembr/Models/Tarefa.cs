using Remembr.Models.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remembr.Models
{
    internal class Tarefa : BaseModel
    {
        public required string Titulo { get; set; }
        public required DateTime CreationTime { get; set; }
        public required DateTime DataInicio { get; set; }
        public required bool FullDia { get; set; }
        public required Prioridade Prio { get; set; }
        public required int Estado { get; set; }
        /*  0: por iniciar
         *  1: em execução
         *  2: terminada
         * -1: apagada
         */

        public DateTime? DataFim { get; set; } = null;
        public string? idPeriodicidade { get; set; } = null;
        public Alerta? AlertaAtraso { get; set; }
        public Alerta? AlertaAntecipacao { get; set; }
        public bool IsTarefaOriginal { get; set; } = true;

    }
}
