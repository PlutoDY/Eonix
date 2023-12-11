using Eonix.Network;
using System;
using BackEnd;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonix.DB
{
    public partial class DataBaseManager
    {
        #region User Data 초기 데이터 삽입/유저 데이터 읽기
        private int currentCompleteCount;
        private int maxCompleteCount;
        private object syncObject = new object();

        public void LoaduserData(Action complete = null)
        {
            if(ServerManager.Instance.isFirstLogin)
            {
                WriteDefaultUserData(complete);
                return;
            }

            var user = GameManager.User;
            InitCompleteCount(3);

            ReadMyData<DtoAccount>(dtoAccount =>
            {
                user.boAccount = new BoAccount(dtoAccount);
                CheckCompleteCount(complete);
            });

            ReadMyData<DtoRetainedHero>(dtoHeroList =>
            {
                user.boHeroes = new BoHeroes(dtoHeroList);
                CheckCompleteCount(complete);
            });

            ReadMyData<DtoStage>(dtoheroStage =>
            {
                user.boStage = new BoStage(dtoheroStage);
                CheckCompleteCount(complete);
            }); 
        }

        private void WriteDefaultUserData(Action complete = null)
        {
            var user = GameManager.User;
            InitCompleteCount(3);

            var dtoAccount = new DtoAccount { nickname = ServerManager.Instance.userInfo.nickname, gold = 10000, diamond = 10000};
            WriteMyData(dtoAccount, new Where(), () =>
            {
                user.boAccount = new BoAccount(dtoAccount);
                CheckCompleteCount(complete);
            });

            var dtoRetainedHero = new DtoRetainedHero
            {
                heroID = string.Empty , inParty = string.Empty , heroLevel = string.Empty, heroCurrentExp = string.Empty, 
                heroListIndex = string.Empty, heroPartyIndex = string.Empty, heroNumber = string.Empty
            };
            WriteMyData(dtoRetainedHero, new Where(), () =>
            {
                user.boHeroes = new BoHeroes(dtoRetainedHero);
                CheckCompleteCount(complete);
            });

            var dtoStage = new DtoStage { index = 1 };
            WriteMyData(dtoStage, new Where(), () =>
             {
                 user.boStage = new BoStage(dtoStage);
                 CheckCompleteCount(complete);
             });
        }

        private void InitCompleteCount(int maxCompleteCount)
        {
            currentCompleteCount = 0;
            this.maxCompleteCount = maxCompleteCount;
        }

        private void CheckCompleteCount(Action complete = null)
        {
            lock(syncObject)
            {
                ++currentCompleteCount;
                if(currentCompleteCount >= maxCompleteCount)
                {
                    complete?.Invoke();
                }
            }
        }

        #endregion
    }
}
