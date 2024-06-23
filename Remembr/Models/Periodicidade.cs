using Remembr.Models.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remembr.Models
{
    public class Periodicidade : BaseModel
    {
        public required string IDTarefaOriginal { get; set;} 
        public required DateTime DataOriginal { get; set; }
        public required DateTime DataLimite { get; set; }
        public required int Tipo { get; set; }
        /* 0: desativada
         * 1: diária
         * 2: semanal
         * 3: mensal
         * 4: anual
         */
        public required int intervaloRepeticao { get; set; }
        /* de y em y dias, semanas, meses */
        public bool[]? DiasSemana { get; set; } = [false, false, false, false, false, false, false];
        /* dias da semana para tipo 2, começa na segunda */
        
        public int? tipoMensal { get; set; }
        /* 0: dia original, de y em y meses
         * 1: dia da semana x da zª semana do mês, todos os y em y meses
         * 2: dia da semana x da última semana do mês, todos os y em y meses
         */
        public int? tipoAnual { get; set; }
        /* 0: dia original, de y em y anos
         * 1: dia da semana x da zª semana do mês original, todos os y em y anos
         * 2: dia da semana x da última semana do mês original, todos os y em y anos
         */

        public string[]? IDChildTarefas { get; set; }

        public void copy(Periodicidade p)
        {
            IDTarefaOriginal = p.IDTarefaOriginal;
            DataOriginal = p.DataOriginal;
            DataLimite = p.DataLimite;
            Tipo = p.Tipo;
            intervaloRepeticao = p.intervaloRepeticao;
            DiasSemana = p.DiasSemana;
            tipoMensal = p.tipoMensal;
            tipoAnual = p.tipoAnual;
            IDChildTarefas = p.IDChildTarefas;
        }

    }
}
