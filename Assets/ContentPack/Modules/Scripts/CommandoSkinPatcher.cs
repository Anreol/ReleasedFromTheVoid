using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ReleasedFromTheVoid.Scripts
{
    class CommandoSkinPatcher
    {
        public static SkinDef commandoMarineSkin;

        //TODO: Figure out how to do it at bootup, so everything is automatic:
        //1. Load body prefab directly from disk
        //2. Hold reference to prefab til game is done loading 
        //3. Pray it works
        //4. Profit
        public static void Init()
        {
            if (!ReleasedFromTheVoid.RFTVUnityPlugin.EnableCommandoSkin.Value) return;
            commandoMarineSkin = Addressables.LoadAssetAsync<SkinDef>("RoR2/DLC1/skinCommandoMarine.asset").WaitForCompletion();
            commandoMarineSkin.unlockableDef = Addressables.LoadAssetAsync<UnlockableDef>("RoR2/DLC1/VoidSurvivor/Characters.VoidSurvivor.asset").WaitForCompletion();
            SurvivorDef mando = SurvivorCatalog.FindSurvivorDef("Commando");
            BodyIndex bodyIndex = BodyCatalog.FindBodyIndex("CommandoBody");
            if (!mando || !commandoMarineSkin)
            {
                Debug.LogError("Failed to find either mando's survivor def or the marine skin");
            }
            HG.ArrayUtils.ArrayAppend(ref mando.bodyPrefab.GetComponentInChildren<ModelSkinController>().skins, commandoMarineSkin);
            SkinDef[] skinDefs = new SkinDef[SkinCatalog.skinsByBody[(int)bodyIndex].Length + 1];
            SkinCatalog.skinsByBody[(int)bodyIndex].CopyTo(skinDefs, 0);
            SkinCatalog.skinsByBody[(int)bodyIndex] = skinDefs;

            SkinDef[] death = new SkinDef[BodyCatalog.skins[(int)bodyIndex].Length + 1];
            BodyCatalog.skins[(int)bodyIndex].CopyTo(death, 0);
            death[death.Length - 1] = commandoMarineSkin; //Already set +1
            BodyCatalog.skins[(int)bodyIndex] = death;

            //Add to viewable catalog, entirely unnecesary. This is what makes it show as [NEW!]
            ViewablesCatalog.Node parent = ViewablesCatalog.FindNode("/Loadout/Bodies/CommandoBody/Skins/");
            ViewablesCatalog.Node child = new ViewablesCatalog.Node("skinCommandoMarine", false, parent);
            child.shouldShowUnviewed = ((UserProfile userProfile) => !userProfile.HasViewedViewable(child.fullName));
            ViewablesCatalog.fullNameToNodeMap.Add(child.fullName, child);
        }
    }
}
