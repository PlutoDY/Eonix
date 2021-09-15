using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonix.DB
{
    [Serializable]
    public class BoAccount : BoBase
    {
        public string nickname;
        public int gold;
        public int diamond;

        public BoAccount(DtoAccount dtoAccount)
        { 
            nickname = dtoAccount.nickname;
            gold = dtoAccount.gold;
            diamond = dtoAccount.diamond;
        }
    }
}
