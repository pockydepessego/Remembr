using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Remembr.Models.Share;

namespace Remembr.Models
{
    public class Perfil : BaseModel
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public string? Password { get; set; } = null;
        public required BitmapImage Fotografia { get; set; }


    }
}
