using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonix.DB
{
    [Serializable]
    public class DtoUserInfo : DtoBase
    {
        public string nickname;
        public string inDate;
        public string subscriptionType;
    }
}
