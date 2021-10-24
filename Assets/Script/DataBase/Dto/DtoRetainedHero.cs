using LitJson;
using System;
using System.Linq;

namespace Eonix.DB
{
    [Serializable]
    public class DtoRetainedHero : DtoBase
    {
        public string heroID;
        public string inParty;
        public string heroLevel;
        public string heroCurrentExp;
        public string heroListIndex;
        public string heroPartyIndex;
        public string heroNumber;

        public DtoRetainedHero()
        {
            var boHeroes = GameManager.User.boHeroes?.boHeroes;
            
            if(boHeroes == null)
            {
                this.heroID = this.inParty = this.heroLevel = this.heroCurrentExp =
                    this.heroListIndex = this.heroPartyIndex = this.heroNumber = string.Empty;
                return;
            }

            int[] heroID = boHeroes.Select(_ => _.heroID).ToArray();
            bool[] inParty = boHeroes.Select(_ => _.inParty).ToArray();
            int[] heroLevel = boHeroes.Select(_ => _.heroLevel).ToArray();
            float[] currentExp = boHeroes.Select(_ => _.currentExp).ToArray();
            int[] heroListIndex = boHeroes.Select(_ => _.heroListIndex).ToArray();
            int[] heroPartyIndex = boHeroes.Select(_ => _.heroPartyIndex).ToArray();
            int[] heroNumber = boHeroes.Select(_ => _.boHeroInfo.sdHeroinfo.num).ToArray();

            this.heroID = JsonMapper.ToJson(heroID);
            this.inParty = JsonMapper.ToJson(inParty);
            this.heroLevel = JsonMapper.ToJson(heroLevel);
            this.heroCurrentExp = JsonMapper.ToJson(currentExp);
            this.heroListIndex = JsonMapper.ToJson(heroListIndex);
            this.heroPartyIndex = JsonMapper.ToJson(heroPartyIndex);
            this.heroNumber = JsonMapper.ToJson(heroNumber);
        }
    }
}
