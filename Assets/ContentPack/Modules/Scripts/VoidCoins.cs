using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ReleasedFromTheVoid.Scripts
{
    public class VoidCoins
    {
        public static ExplicitPickupDropTable voidCoinTable;
        public static ExplicitPickupDropTable voidTierDropTable;
        public static InteractableSpawnCard voidBarrelSpawncard;
        public static InteractableSpawnCard voidChestSacrificeSpawncard;
        public static InteractableSpawnCard voidTripleChestSpawncard;

        [RoR2.SystemInitializer]
        public static void Init()
        {
            voidCoinTable = Addressables.LoadAssetAsync<ExplicitPickupDropTable>("RoR2/DLC1/Common/DropTables/dtVoidCoin.asset").WaitForCompletion();
            voidBarrelSpawncard = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/DLC1/VoidCoinBarrel/iscVoidCoinBarrel.asset").WaitForCompletion();

            voidChestSacrificeSpawncard = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/DLC1/VoidChest/iscVoidChestSacrificeOn.asset").WaitForCompletion();
            voidTripleChestSpawncard = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/DLC1/VoidTriple/iscVoidTriple.asset").WaitForCompletion();

            if (!ReleasedFromTheVoid.RFTVUnityPlugin.EnableVoidCoins.Value) return;

            voidBarrelSpawncard.prefab.GetComponent<ModelLocator>().gameObject.AddComponent<ChestBehavior>().dropTable = voidCoinTable;
            voidBarrelSpawncard.directorCreditCost = 7; //Decreased from 15

            GameObject runPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Common/DLC1RunBehavior.prefab").WaitForCompletion();
            RFTVUnityPlugin.DLC1.runBehaviorPrefab = runPrefab;
            //RFTVUnityPlugin.DLC1.runBehaviorPrefab.GetComponent<GlobalDeathRewards>().pickupRewards[0].chance += 0.5f; //Bump it to 10%

            Run.onPlayerFirstCreatedServer += GrantVoidCoins;
        }

        private static void GrantVoidCoins(Run arg1, PlayerCharacterMasterController arg2)
        {
            if (arg2.master)
            {
                arg2.master.GiveVoidCoins((uint)RulebookExtras.runStartingVoidCoins);
            }
        }
    }
}