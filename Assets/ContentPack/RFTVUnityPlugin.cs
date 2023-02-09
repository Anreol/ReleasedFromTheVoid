using BepInEx;
using BepInEx.Configuration;
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
            "0.0.4";

        internal const string ModIdentifier = "ReleasedFromTheVoid";
        internal const string ModGuid = "com.Anreol." + ModIdentifier;

        public static RFTVUnityPlugin instance;
        public static PluginInfo pluginInfo;

        public static ExpansionDef DLC1;

        public static ConfigEntry<bool> EnableAssAssin;
        public static ConfigEntry<bool> EnableMajorConstruct;
        public static ConfigEntry<bool> EnableVoidCoins;
        public static ConfigEntry<bool> EnableVoidSuppressors;
        public static ConfigEntry<bool> EnableCommandoSkin;
        public static ConfigEntry<bool> EnableVoidLocusChanges;

        public void Awake()
        {
            Debug.Log("Running " + ModGuid + "!");
            InitConfigFileValues();
#if DEBUG
            RFTVLog.logger = Logger;
            RFTVLog.LogW("Running ReleasedFromTheVoid DEBUG build. PANIC!");
#endif
            pluginInfo = Info;
            instance = this;

            ContentManager.collectContentPackProviders += (addContentPackProvider) => addContentPackProvider(new RFTVContent());
            RoR2Application.onLoad += (delegate ()
            {
                DLC1 = Addressables.LoadAssetAsync<ExpansionDef>("RoR2/DLC1/Common/DLC1.asset").WaitForCompletion();
                Scripts.CommandoSkinPatcher.Init();
            });

        }

        /// <summary>
        /// HAS TO BE CALLED THE VERY FIRST, AS ANY OTHER SYSTEM HAS DEPENDENCIES THAT LOAD BEFORE APPLICATION IS FINISHED LOADING
        /// </summary>
        public void InitConfigFileValues()
        {
            EnableAssAssin = Config.Bind(
                "Enemy Settings",
                "EnableAssAssIn",
                true,
                "Should Assassin be added to card decks."
                );
            EnableMajorConstruct = Config.Bind(
                "Enemy Settings",
                "EnableMajorConstruct",
                true,
                "Should Major / Iota Construct be added to card decks."
                );
            EnableVoidCoins = Config.Bind(
                "Module Settings",
                "Enable Void Coins",
                true,
                "Creates rules related to the void coins, void barrels dropping coins, or void enemies dropping coins."
                );
            EnableVoidSuppressors = Config.Bind(
                "Module Settings",
                "Enable Void Suppressors",
                true,
                "Should Void Suppressors spawn and should strange scrap be changed to have a proper tier and item tags."
                );
            EnableCommandoSkin = Config.Bind(
                "Module Settings",
                "Enable Commando Skin",
                true,
                "Should the Commando Helot skin be enabled. This does not bypass the requirement of having Void Fiend unlocked."
                );
            EnableVoidLocusChanges = Config.Bind(
                "Module Settings",
                "Enable Void Locus",
                true,
                "Should Void Locus changes be enabled, such as spawning a exit portal and spawning additional interactables at no additional cost."
                );
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