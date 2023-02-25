using UnityEngine;

namespace SteelHeart
{
    namespace JsonTest
    {
        public class JsonTest : MonoBehaviour
        {
            private void Awake()
            {
                // GameData.Settings.Initialize();
                // GameData.Player.Initialize();
                GameData.Upgrades.Initialize();
                var gun = new GameMeta.BasicGun
                {
                    id = 1,
                    damage = 100,
                };

                var gun2 = new GameMeta.ShrinkingGun
                {
                    actionTime = 1,
                    id = 2,
                };

                var upgrade = new GameMeta.Jetpack
                {
                    id = 100,
                    jumpsCount = 100,
                };
                
                MetaManager.Save<GameMeta.Upgrade>(gun, FilePath.PATH_UPGRADES);
                MetaManager.Save<GameMeta.Upgrade>(gun2, FilePath.PATH_UPGRADES);
                MetaManager.Save<GameMeta.Upgrade>(upgrade, FilePath.PATH_UPGRADES);
                GameData.Upgrades.Initialize();
            }
        }
    }
}