using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remembr.Models.Share
{
    public class BaseModel
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
    }
}
