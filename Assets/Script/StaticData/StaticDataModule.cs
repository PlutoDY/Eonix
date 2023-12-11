using BackEnd;
using Eonix.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eonix.SD
{
    using SDType = Define.StaticData.SDType;

    [Serializable]
    public class StaticDataModule
    {
        public List<SDHeroInfo> sdHeroInfos;
        public List<SDHeroStatInfo> sdHeroStatInfos;
        public List<SDHeroSkillInfo> sdHeroSkillInfos;
        public List<SDMonsterInfo> sdMonsterInfos;
        public List<SDMonsterStatInfo> sdMonsterStatInfos;
        public List<SDMaxExpInfo> sdMaxExpInfos;
        public List<SDStage> sdStages;

        public void Initialize()
        {
            var loader = new StaticDataLoader(this);

            GameManager.Instance.StartCoroutine(WaitForLoadComplete());
            loader.LoadAllData();

            IEnumerator WaitForLoadComplete()
            {
                yield return new WaitUntil(() => loader.allLoaded);

                GameManager.Instance.titleController.LoadComplete = true;
            }
        }

        private class StaticDataLoader
        {
            public bool allLoaded;
            public int currentLoadedCount;
            public const int maxLoadedCount = (int)SDType.End;

            private StaticDataModule module;

            public StaticDataLoader(StaticDataModule module)
            {
                this.module = module;
            }

            public void LoadAllData()
            {
                LoadDataIds();
            }

            private void LoadDataIds()
            {
                Backend.Chart.GetChartListV2(callback =>
                {
                    if (callback.IsSuccess())
                    {
                        var rows = callback.Rows();
                        for (int i = 0; i < rows.Count; ++i)
                        {
                            if (!rows[i]["selectedChartFileId"].ContainsKey("N"))
                                continue;

                            var dataId = Convert.ToString(rows[i]["selectedChartFileId"]["N"]);
                            var dataName = Convert.ToString(rows[i]["chartName"]["S"]);

                            MatchAndLoad(dataId, dataName);
                        }
                    }
                    else
                    {
                        GameManager.Log($" ### Load All Table Id Failed ###\n{callback}");
                    }
                });
            }

            private void MatchAndLoad(string dataId, string dataName)
            {
                SDType type = (SDType)Enum.Parse(typeof(SDType), dataName);

                switch (type)
                {
                    case SDType.HeroInfo:
                        LoadData(dataId, module.sdHeroInfos);
                        break;
                    case SDType.HeroStatInfo:
                        LoadData(dataId, module.sdHeroStatInfos);
                        break;
                    case SDType.HeroSkillInfo:
                        LoadData(dataId, module.sdHeroSkillInfos);
                        break;
                    case SDType.MonsterInfo:
                        LoadData(dataId, module.sdMonsterInfos);
                        break;
                    case SDType.MonsterStatInfo:
                        LoadData(dataId, module.sdMonsterStatInfos);
                        break;
                    case SDType.MaxExpInfo:
                        LoadData(dataId, module.sdMaxExpInfos);
                        break;
                    case SDType.Stage:
                        LoadData(dataId, module.sdStages);
                        break;
                    case SDType.End:
                        break;
                    default:
                        break;
                }

            }

            private void LoadData<T>(string charId, List<T> data) where T : StaticData
            {
                Backend.Chart.GetChartContents(charId, callback =>
                {
                    if (callback.IsSuccess())
                    {
                        var rows = callback.Rows();

                        for (int i = 0; i < rows.Count; ++i)
                        {
                            data.Add(SerializationUtil.JsonToObject<T>(rows[i], Define.DeserializeType.SD));
                        }

                        CheckLoadedCount();
                    }
                    else
                    {
                        GameManager.Log($"### Load {typeof(T).Name} Table Failed ###\n{callback}");
                    }
                });
            }

            private void CheckLoadedCount()
            {
                ++currentLoadedCount;

                if(currentLoadedCount >= maxLoadedCount)
                {
                    allLoaded = true;
                }
            }
        }

    }
}