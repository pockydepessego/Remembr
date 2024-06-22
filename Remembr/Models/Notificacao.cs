using Remembr.Models.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remembr.Models
{
    public class Notificacao : BaseModel
    {
        public required string Mensagem { get; set; }
        public required DateTime Data { get; set; }
        public required bool Lida { get; set; }
        public required string IDOriginal { get; set; }

        public required int Tipo { get; set; }
        /* 0: ??
         * 1: antecipação
         * 2: atraso
         */
    }
}
