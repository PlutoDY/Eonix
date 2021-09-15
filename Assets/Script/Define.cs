using System;
using System.Collections.Generic;
using System.Globalization;

namespace Eonix.Define
{
    public enum IntroPhase
    {
        Start,
        ApplicationSetting,
        ServerInit,
        VersionCheck,
        Login,
        StaticData,
        UserData,
        Resource,
        UI,
        Complete
    }

    public enum PoolType { Character, Monster, Projectile, UI, EmptyCell }
    public enum DeserializeType { SD,   DTO,DefinedDtoByBackend}

    public enum CardType { Party, List}

    public enum SceneType { Title, InGame, Loading,Battle}

    public class MoveDistance
    {
        public static int width = 0;

        public static int[] moveDistance = new int[8] { width, width+1, 1, -width+1, -width, -width-1, -1, width-1 };

        public static void SetMoveDistance(int w)
        {
            width = w;

            moveDistance = new int[8] { width, width + 1, 1, -width + 1, -width, -width - 1, -1, width - 1 };
        }
    }

    public enum Layer 
    {
        Player = 3,
        Checker = 6
    }

    public class Define
    {
        public enum MouseEvent { Press, Click, MouseUp}
    }

    public class Resource
    {
        public enum AtlasType { SkillAtlas }

        public const string AtlasPath = "Art/Sprite";
        public const string UIPath = "Prefabs/UI";
        public const string ActorPath = "Prefabs/Actor";
        public const string ProjectilePath = "Prefabs/Projectile";
    }

    public class StaticData
    {
        public enum SDType { HeroInfo, HeroStatInfo, HeroSkillInfo, MonsterInfo, MonsterStatInfo, MaxExpInfo ,Stage,End}
    }

    public class Actor
    {
        public enum SkillType { Nomal, Upgrade, Consecutive }
    }

    public class PrefabsPath
    {
        private const string basicUIPath = "Prefabs/UI/";

        private const string basicPanelPath = "Prefabs/Panel/";

        private const string basicSkillPath = "Prefabs/Skill/";

        private const string basicContorllerPath = "Prefabs/Controller/";

        public static string extendsPath;

        public static string UIPath(PrefabsUIs prefabsUis)
        {
            extendsPath = prefabsUis.ToString();

            var rePath = basicUIPath + extendsPath;


            return rePath;
        }

        public static string ControllerPath(Controller controller)
        {
            extendsPath = controller.ToString();

            var rePath = basicContorllerPath + controller;

            return rePath;
        }

        public static string PanelPath(PrefabsPanels prefabsPanels)
        {
            extendsPath = prefabsPanels.ToString();

            var rePath = basicPanelPath + extendsPath;

            return rePath;
        }

        public static string SkillPath(string skillPath)
        {
            return basicSkillPath + $"MainObject/{skillPath}";
        }

        public enum PrefabsUIs {UILoading , TownCanvas, InventoryCanvas, PvECanvas, BattleUI
                , ShopCanvas, LoginCanvas, BattleTurn, EndBattleUI}

        public enum Controller { ActorController, BattleController }

        public enum SkillObjectType { MainObject, Objects }

        public enum PrefabsManager { ActorController, Battle}

        public enum PrefabsPanels { VoidPanel, DreamyForestPanel, EmptyCell, Floor3X3}
    }

    public class Audio
    {
        private const string basePath = "Art/Audio/";

        public enum SoundType { BackGroundSound, EffectSound}

        public static readonly string[] backGroudnSoundAudioClipName
            = new string[6] { "DreamyForest01", "DreamyForest02", "DreamyForest03", "Void01", "Void02", "Void03" };

        public static readonly string[] EffectSoundAudioClip
            = new string[4] { "Attack", "BattleStart", "Guard", "Start" };

        public static UnityEngine.AudioClip audioClip(SoundType soundType, string name)
        {
            var path = basePath + soundType.ToString() + "/" + name;

            return UnityEngine.Resources.Load<UnityEngine.AudioClip>(path);
        }
    }

    public enum AttackerType { Hero, Monster}

    public struct Distance
    {
        public int rowDistance;
        public int columnDistance;

        public Distance(int startIndex, int endIndex, int width)
        {
            rowDistance = Math.Abs((startIndex % width) - (endIndex % width));
            columnDistance = Math.Abs((startIndex / width) - (endIndex / width));
        }

        public static int CalculationDistance(Distance distance)
        {
            return CalculationDistanceShortVer(distance) * 10;
        }

        public static int CalculationDistanceShortVer(Distance distance)
        {
            return distance.rowDistance + distance.columnDistance;
        }

        public static int CalculationShortIndex(int currentShortValue, ref int currentShortHeroIndex, Distance calculatedMonsterDistance, int currentHeroIndex)
        {
            if (currentShortValue > CalculationDistance(calculatedMonsterDistance))
            {
                currentShortHeroIndex = currentHeroIndex;
                return CalculationDistance(calculatedMonsterDistance);
            }

            return currentShortValue;
        }
    }

    public class IndexSetter
    {
        public static int idIndex = 0;

        public static int listIndex = 0;

        public static int cardIndex = 0;
    }

    public class Battle
    {
        public static List<string> attackHeroToMonsterText = new List<string>();
        public static List<string> attackMonsterToHeroText = new List<string>();

        public enum BattlePhase { Power_Resistance, Power_Defense, SkillSelect ,SkillAttack, HpAdjustment, End}

        public static void InitText()
        {
            attackHeroToMonsterText.Add("저지력");
            attackHeroToMonsterText.Add("저항력");

            attackHeroToMonsterText.Add("공격력");
            attackHeroToMonsterText.Add("방어력");
        }
    }

    public class SpriteArtPath
    {
        private const string basePath = "Art/Sprite/";

        public enum ArtType {Battle, Card, Inventory, Nomal, Party, PvE, Shop, Skill };

        public static UnityEngine.Sprite sprite(ArtType artType, string name)
        {
            var path = basePath + artType.ToString() + "/" + name;

            return UnityEngine.Resources.Load<UnityEngine.Sprite>(path);
        }
    }

    public class DicePath
    {
        public static string nomalPath = "Dice";
        public static string breakPath = "BreakDice";
    }
}
