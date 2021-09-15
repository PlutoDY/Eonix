using System;


namespace Eonix.DB
{
    [Serializable]
    public class BoUser : BoBase
    {
        public BoStage boStage;
        public BoAccount boAccount;
        public BoHeroes boHeroes;
    }
}
