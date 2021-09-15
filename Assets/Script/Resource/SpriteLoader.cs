using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Eonix.RM
{
    using AtlasType = Define.Resource.AtlasType;

    public class SpriteLoader : MonoBehaviour
    {
        private static Dictionary<AtlasType, SpriteAtlas> atlasDic = new Dictionary<AtlasType, SpriteAtlas>();

        public static void SetAtlas(SpriteAtlas[] atlases)
        {
            for(int i =0;i< atlases.Length; ++i)
            {
                var key = (AtlasType)Enum.Parse(typeof(AtlasType), atlases[i].name);

                atlasDic.Add(key, atlases[i]);
            }
        }

        public static Sprite GetSprite(AtlasType atlasKey, string spriteKey)
        {
            if (!atlasDic.ContainsKey(atlasKey))
                return null;

            return atlasDic[atlasKey].GetSprite(spriteKey);
        }
    }
}
