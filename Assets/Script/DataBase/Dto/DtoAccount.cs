using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonix.DB
{
    [Serializable]
    public class DtoAccount : DtoBase
    {
        public string nickname;
        public int gold;
        public int diamond;
    }
}
