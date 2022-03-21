using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace ReleasedFromTheVoid.Scripts
{
    internal class VoidLocusTweaker
    {
        public const int extraVoidBarrelPerPlayer = 7;
        public const int extraVoidChestPerPlayer = 2;
        public const int extraVoidTripleChestPerPlayer = 1;

        public static GameObject arenaPortal;
        public static int stageCountInLastVoidStageVisit;

        [RoR2.SystemInitializer(new Type[]
        {
            typeof(RoR2.SceneCatalog),
        })]
        public static void Init()
        {
            LoadPrefabs();
            Stage.onServerStageBegin += onServerStageBegin;
            Stage.onServerStageComplete += onServerStageComplete;
            Stage.onStageStartGlobal += onStageStartGlobal;
            SceneDirector.onPrePopulateSceneServer += onPrePopulateSceneServer;
            SceneDirector.onPostPopulateSceneServer += onPostPopulateScene;
        }

        public static void LoadPrefabs()
        {
            arenaPortal = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/PortalArena/PortalArena.prefab").WaitForCompletion();
        }

        private static void onServerStageBegin(Stage obj)
        {
            if (Run.instance.IsExpansionEnabled(RFTVUnityPlugin.DLC1))
            {
                if (obj.sceneDef == SceneCatalog.GetSceneDefFromSceneName("bazaar") && (Run.instance.stageClearCount + 1 - stageCountInLastVoidStageVisit) >= 4 && !Run.instance.GetEventFlag("NoVoidStage"))
                {
                    //BazaarController.instance.seerSceneOverrides[1].overrideChance += 0.23f; //This doesn't work
                    float num = UnityEngine.Random.Range(0f, 100f);
                    if (num <= 40f) //40% chance
                    {
                        BazaarController.instance.seerStations[1].SetTargetScene(SceneCatalog.GetSceneDefFromSceneName("voidstage"));
                    }
                }
                if (obj.sceneDef == SceneCatalog.GetSceneDefFromSceneName("voidstage"))
                {
                    stageCountInLastVoidStageVisit = Run.instance.stageClearCount + 1;
                    if (RFTVUnityPlugin.DLC1.runBehaviorPrefab)
                    {
                        RFTVUnityPlugin.DLC1.runBehaviorPrefab.GetComponent<GlobalDeathRewards>().pickupRewards[0].chance += 0.15f;
                    }
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(arenaPortal, new Vector3(-144f, 30f, 246.15f), new Quaternion(0, 70, 0, 0));
                    NetworkServer.Spawn(gameObject);
                }
            }
        }

        private static void onServerStageComplete(Stage obj)
        {
            if (Run.instance.IsExpansionEnabled(RFTVUnityPlugin.DLC1))
            {
                if (obj.sceneDef == SceneCatalog.GetSceneDefFromSceneName("voidstage"))
                {
                    stageCountInLastVoidStageVisit = Run.instance.stageClearCount + 1;
                    if (RFTVUnityPlugin.DLC1.runBehaviorPrefab)
                    {
                        RFTVUnityPlugin.DLC1.runBehaviorPrefab.GetComponent<GlobalDeathRewards>().pickupRewards[0].chance -= 0.15f;
                    }
                }
            }
        }

        private static void onStageStartGlobal(Stage obj)
        {
            if (SceneCatalog.mostRecentSceneDef == SceneCatalog.GetSceneDefFromSceneName("voidstage") || RulebookExtras.runAllVoidChestCostVoidCoin)
            {
                foreach (PurchaseInteraction item in InstanceTracker.GetInstancesList<PurchaseInteraction>())
                {
                    ChestBehavior chestBehavior = item.gameObject.GetComponent<ChestBehavior>();
                    if (chestBehavior)
                    {
                        BasicPickupDropTable dropTable = (BasicPickupDropTable)chestBehavior.dropTable;
                        if (item.costType == CostTypeIndex.PercentHealth && (dropTable.voidBossWeight + dropTable.voidTier1Weight + dropTable.voidTier2Weight + dropTable.voidTier3Weight) > 0)
                        {
                            ScriptedCombatEncounter scriptedCombatEncounter = item.GetComponent<ScriptedCombatEncounter>();
                            if (scriptedCombatEncounter)
                                UnityEngine.Object.Destroy(scriptedCombatEncounter);

                            item.costType = CostTypeIndex.VoidCoin;
                            item.cost = 1;
                            if (chestBehavior.dropCount > 1)
                                item.cost++;
                        }
                        continue;
                    }
                    OptionChestBehavior optionChestBehavior = item.gameObject.GetComponent<OptionChestBehavior>();
                    if (optionChestBehavior)
                    {
                        BasicPickupDropTable dropTable = (BasicPickupDropTable)optionChestBehavior.dropTable;
                        if (item.costType == CostTypeIndex.PercentHealth && dropTable.name == "dtVoidTriple") //Name comparisons are bad, but I don't know any better
                        {
                            item.costType = CostTypeIndex.VoidCoin;
                            item.cost = 2;
                        }
                        continue;
                    }
                }
            }
        }

        private static void onPrePopulateSceneServer(SceneDirector obj)
        {
            if (SceneCatalog.mostRecentSceneDef == SceneCatalog.GetSceneDefFromSceneName("voidstage"))
            {
                obj.interactableCredit += Mathf.Min(20 * ((Run.instance.stageClearCount + 1) / 2), 520); //Increase amount of credits overtime up to Sky Meadows count
            }
        }

        private static void onPostPopulateScene(SceneDirector obj)
        {
            if (SceneCatalog.mostRecentSceneDef != SceneCatalog.GetSceneDefFromSceneName("voidstage"))
                return;
            for (int i = 0; i < PlayerCharacterMasterController.instances.Count * extraVoidBarrelPerPlayer; i++)
            {
                DirectorCore.instance.TrySpawnObject(new DirectorSpawnRequest(VoidCoins.voidBarrelSpawncard, new DirectorPlacementRule
                {
                    placementMode = DirectorPlacementRule.PlacementMode.Random
                }, Run.instance.stageRng));
            }
            for (int i = 0; i < PlayerCharacterMasterController.instances.Count * extraVoidChestPerPlayer; i++)
            {
                DirectorCore.instance.TrySpawnObject(new DirectorSpawnRequest(VoidCoins.voidChestSacrificeSpawncard, new DirectorPlacementRule
                {
                    placementMode = DirectorPlacementRule.PlacementMode.Random
                }, Run.instance.stageRng));
            }
            for (int i = 0; i < PlayerCharacterMasterController.instances.Count * extraVoidTripleChestPerPlayer; i++)
            {
                DirectorCore.instance.TrySpawnObject(new DirectorSpawnRequest(VoidCoins.voidTripleChestSpawncard, new DirectorPlacementRule
                {
                    placementMode = DirectorPlacementRule.PlacementMode.Random
                }, Run.instance.stageRng));
            }
        }
    }
}