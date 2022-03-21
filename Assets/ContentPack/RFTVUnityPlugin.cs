using BepInEx;
using RoR2;
using RoR2.ContentManagement;
using RoR2.ExpansionManagement;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ReleasedFromTheVoid
{
    [BepInPlugin(ModGuid, ModIdentifier, ModVer)]
    public class RFTVUnityPlugin : BaseUnityPlugin
    {
        internal const string ModVer =
#if DEBUG
            "9999." +
#endif
            "0.0.2";

        internal const string ModIdentifier = "ReleasedFromTheVoid";
        internal const string ModGuid = "com.Anreol." + ModIdentifier;

        public static RFTVUnityPlugin instance;
        public static PluginInfo pluginInfo;

        public static ExpansionDef DLC1;
        public void Awake()
        {
            Debug.Log("Running ReleasedFromTheVoid!");
#if DEBUG
            RFTVLog.logger = Logger;
            RFTVLog.LogW("Running ReleasedFromTheVoid DEBUG build. PANIC!");
#endif
            pluginInfo = Info;
            instance = this;

            On.RoR2.Language.SetFolders += Death;
            ContentManager.collectContentPackProviders += (addContentPackProvider) => addContentPackProvider(new RFTVContent());
            RoR2Application.onLoad += (delegate ()
            {
                DLC1Content.Equipment.LunarPortalOnUse.canDrop = true;
                DLC1 = Addressables.LoadAssetAsync<ExpansionDef>("RoR2/DLC1/Common/DLC1.asset").WaitForCompletion();
                Scripts.CommandoSkinPatcher.Init();
            });

        }
        private void Death(On.RoR2.Language.orig_SetFolders orig, Language self, System.Collections.Generic.IEnumerable<string> newFolders)
        {
            if (System.IO.Directory.Exists(Assets.languageRoot))
            {
                var dirs = System.IO.Directory.EnumerateDirectories(System.IO.Path.Combine(Assets.languageRoot), self.name);
                orig(self, newFolders.Union(dirs));
                return;
            }
            orig(self, newFolders);
        }
    }
}