using EntityStates;
using EntityStates.Barrel;
using RoR2;
using RoR2.ExpansionManagement;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

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
            voidBarrelSpawncard.prefab.GetComponent<ModelLocator>().gameObject.AddComponent<ChestBehavior>().dropTable = voidCoinTable;
            voidBarrelSpawncard.directorCreditCost = 7; //Decreased from 15

            voidChestSacrificeSpawncard = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/DLC1/VoidChest/iscVoidChestSacrificeOn.asset").WaitForCompletion();
            voidTripleChestSpawncard = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/DLC1/VoidTriple/iscVoidTriple.asset").WaitForCompletion();

            ExpansionDef dlc1 = Addressables.LoadAssetAsync<ExpansionDef>("RoR2/DLC1/Common/DLC1.asset").WaitForCompletion();
            GameObject runPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Common/DLC1RunBehavior.prefab").WaitForCompletion();
            dlc1.runBehaviorPrefab = runPrefab;
            dlc1.runBehaviorPrefab.GetComponent<GlobalDeathRewards>().pickupRewards[0].chance += 0.5f; //Bump it to 10%

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
