using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace Eonix.DB
{
    [Serializable]
    public class BoHero : BoBase
    {
        public BoHeroInfo boHeroInfo;
        public BoHeroStatInfo boHeroStatInfo;
        public List<BoHeroSkillInfo> boHeroSkillInfos = new List<BoHeroSkillInfo>();

        #region NoneBattleValue
        public int heroID;
        public bool inParty;
        public int heroLevel;
        public float heroCurrentExp;
        public float maxExp;
        public int heroListIndex;
        public int heroPartyIndex;
        #endregion

        public BoHero(int index)
        {
            var sd = GameManager.SD;

            boHeroInfo = new BoHeroInfo(sd.sdHeroInfos.Where(_ => _.num == index).SingleOrDefault());
            boHeroStatInfo = new BoHeroStatInfo(sd.sdHeroStatInfos.Where(_ => _.num == boHeroInfo.sdHeroinfo.heroInfoRef).SingleOrDefault());

            foreach(int skillNumber in boHeroInfo.sdHeroinfo.heroHasSkillList)
            {
                boHeroSkillInfos.Add(new BoHeroSkillInfo(sd.sdHeroSkillInfos.Where(_ => _.num == skillNumber).SingleOrDefault()));
            }
        }
    }

    [Serializable]
    public class BoHeroes
    {
        public List<BoHero> boHeroes;

        public BoHeroes() { }

        public BoHeroes(List<int> HeroIds) 
        {
            boHeroes ??= new List<BoHero>();

            var sdHeroInfo = GameManager.SD.sdHeroInfos;

            foreach(int heroId in HeroIds)
            {
                var heroID = Define.IndexSetter.idIndex++;
                var heroListIndex = Define.IndexSetter.listIndex++;

                var sdHero = GameManager.SD.sdHeroInfos.Where(_ => _.num == heroId).SingleOrDefault();

                BoHero boHero = new BoHero(heroId);

                boHero.heroID = heroID;
                boHero.inParty = false;
                boHero.heroLevel = 0;
                boHero.heroCurrentExp = 0;
                boHero.heroListIndex = heroListIndex;
                boHero.heroPartyIndex = -1;
                boHero.maxExp = GameManager.SD.sdMaxExpInfos[boHero.heroLevel].maxExp;

                boHeroes.Add(boHero);
            }
        }

        public BoHeroes(DtoRetainedHero dtoRetainedHero)
        {
            boHeroes ??= new List<BoHero>();

            if (dtoRetainedHero.heroID == string.Empty)
                return;

            var heroID = JsonMapper.ToObject<List<int>>(new JsonReader(dtoRetainedHero.heroID));
            var inParty = JsonMapper.ToObject<List<bool>>(new JsonReader(dtoRetainedHero.inParty));
            var heroLevel = JsonMapper.ToObject<List<int>>(new JsonReader(dtoRetainedHero.heroLevel));
            var heroCurrentExp = JsonMapper.ToObject<List<float>>(new JsonReader(dtoRetainedHero.heroCurrentExp));
            var heroListIndex = JsonMapper.ToObject<List<int>>(new JsonReader(dtoRetainedHero.heroListIndex));
            var heroPartyIndex = JsonMapper.ToObject<List<int>>(new JsonReader(dtoRetainedHero.heroPartyIndex));
            var heroNumber = JsonMapper.ToObject<List<int>>(new JsonReader(dtoRetainedHero.heroNumber));

            var heroTable = GameManager.SD.sdHeroInfos;

            for(int i = 0; i < heroID.Count; i++)
            {
                var sdHero = heroTable.Where(_ => _.num == heroNumber[i]).SingleOrDefault();

                BoHero boHero = new BoHero(heroNumber[i]);

                boHero.heroID = heroID[i];
                boHero.inParty = inParty[i];
                boHero.heroLevel = heroLevel[i];
                boHero.heroCurrentExp = heroCurrentExp[i];
                boHero.heroListIndex = heroListIndex[i];
                boHero.heroPartyIndex = heroPartyIndex[i];
                boHero.maxExp = GameManager.SD.sdMaxExpInfos[heroLevel[i]].maxExp;

                boHeroes.Add(boHero);
            }

            Define.IndexSetter.idIndex = heroID.Max() + 1;
            Define.IndexSetter.listIndex = heroListIndex.Max() + 1;
        }

        public void AddHero(BoHero boHero)
        {
            GameManager.User.boHeroes.boHeroes.Add(boHero);
        }

    }
}
