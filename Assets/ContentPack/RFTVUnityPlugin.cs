using BepInEx;
using RoR2;
using RoR2.ContentManagement;
using RoR2.ExpansionManagement;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

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
        /*private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                Addressables.LoadSceneAsync("RoR2/Dev/renderitem/renderitem.unity",UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                Addressables.LoadSceneAsync("RoR2/Dev/ItemLogBookPositionalOffsets/ItemLogBookPositionalOffsets.unity", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                Addressables.LoadSceneAsync("RoR2/Junk/dampcave/dampcave.unity", UnityEngine.SceneManagement.LoadSceneMode.Additive);
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                Addressables.LoadSceneAsync("RoR2/Junk/slice1/slice1.unity", UnityEngine.SceneManagement.LoadSceneMode.Additive);
            }
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                Addressables.LoadSceneAsync("RoR2/Junk/slice2/slice2.unity", UnityEngine.SceneManagement.LoadSceneMode.Additive);
            }
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                Addressables.LoadSceneAsync("RoR2/Junk/space/space.unity", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                Addressables.LoadSceneAsync("RoR2/Junk/stage1/stage1.unity", UnityEngine.SceneManagement.LoadSceneMode.Additive);
            }
            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                Addressables.LoadSceneAsync("RoR2/Junk/testscene/testscene.unity", UnityEngine.SceneManagement.LoadSceneMode.Additive);
            }
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                ScanSceneForMissingShaders();
            }
        }
        private static void ScanSceneForMissingShaders()
        {
            Shader standardHopoo = Addressables.LoadAssetAsync<Shader>("RoR2/Base/Shaders/HGStandard.shader").WaitForCompletion();
            Shader triplanar = Addressables.LoadAssetAsync<Shader>("RoR2/Base/Shaders/HGTriplanarTerrainBlend.shader").WaitForCompletion();
            foreach (Renderer render in Resources.FindObjectsOfTypeAll(typeof(Renderer)) as Renderer[])
            {
                if (render.material.shader.name.Contains("InternalErrorShader"))
                {
                    render.material.shader = standardHopoo;
                }
            }
            foreach (Terrain terrain in Resources.FindObjectsOfTypeAll(typeof(Terrain)) as Terrain[])
            {
                if (terrain.materialTemplate.shader.name.Contains("InternalErrorShader"))
                {
                    terrain.materialTemplate.shader = triplanar;
                }
            }
        }*/
    }
}