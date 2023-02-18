using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using HG;

namespace ReleasedFromTheVoid.Scripts
{
    class AddUnusedBodies
    {
        static CharacterBody assassin2Body;
        static GameObject assassin2Master;
        static CharacterSpawnCard cscAssassin2;
        static SkillDef primaryAssassinSkillDashStrike;
        static EntityStateConfiguration primaryConfigCharge;
        static EntityStateConfiguration primaryConfigStrike;
        static SkillDef secondaryAssassinSkillShuriken;
        static EntityStateConfiguration secondaryConfig;
        static SkillDef utilityAssassinSkillHide;
        static EntityStateConfiguration utilityConfig;

        static CharacterBody majorConstructBody;
        static GameObject majorConstructMaster;
        static CharacterSpawnCard cscMajorConstruct;

        static DirectorCardCategorySelection mixCardSelection;
        static DirectorCardCategorySelection arenaCardSelection;
        static DirectorCardCategorySelection artifactWorldCardSelection;
        static DirectorCardCategorySelection golemplains1Monsters;
        static DirectorCardCategorySelection rootJungleCardSelection;
        static DirectorCardCategorySelection shipGraveyardCardSelection;
        static DirectorCardCategorySelection skyMeadowCardSelection;
        static DirectorCardCategorySelection wispGraveyardCardSelection;
        static DirectorCardCategorySelection sulfurPoolsCardSelection;

        static DirectorCardCategorySelection itAncientLoftCardSelection;
        static DirectorCardCategorySelection itDampCaveCardSelection;
        static DirectorCardCategorySelection itFrozenWallCardSelection;
        static DirectorCardCategorySelection itGolemPlainsCardSelection;
        static DirectorCardCategorySelection itGooLakeCardSelection;
        static DirectorCardCategorySelection itMoonSelection;
        static DirectorCardCategorySelection itSkyMeadowSelection;

        [RoR2.SystemInitializer(new Type[]
        {
            typeof(RoR2.BodyCatalog),
        })]
        public static void Init()
        {
            
            assassin2Body = BodyCatalog.GetBodyPrefabBodyComponent(BodyCatalog.FindBodyIndex("Assassin2Body"));
            assassin2Master = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Assassin2/Assassin2Master.prefab").WaitForCompletion();
            cscAssassin2 = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/DLC1/Assassin2/cscAssassin2.asset").WaitForCompletion();
            primaryAssassinSkillDashStrike = Addressables.LoadAssetAsync<SkillDef>("RoR2/DLC1/Assassin2/Assassin2Strike.asset").WaitForCompletion();
            secondaryAssassinSkillShuriken = Addressables.LoadAssetAsync<SkillDef>("RoR2/DLC1/Assassin2/Assassin2Shuriken.asset").WaitForCompletion();
            utilityAssassinSkillHide = Addressables.LoadAssetAsync<SkillDef>("RoR2/DLC1/Assassin2/Assassin2Hide.asset").WaitForCompletion();

            majorConstructBody = BodyCatalog.GetBodyPrefabBodyComponent(BodyCatalog.FindBodyIndex("MajorConstructBody"));
            majorConstructMaster = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/MajorAndMinorConstruct/MajorConstructMaster.prefab").WaitForCompletion();
            cscMajorConstruct = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/DLC1/MajorAndMinorConstruct/cscMajorConstruct.asset").WaitForCompletion();

            UnityEngine.Object.Destroy(majorConstructMaster.GetComponent<SetDontDestroyOnLoad>());

            LoadDirectorCards();
            BalanceAssAssIn2();
            if (RFTVUnityPlugin.EnableAssAssin.Value)
                AddAssAssInToStages();
            BalanceMajorConstruct();
            if (RFTVUnityPlugin.EnableMajorConstruct.Value)
                AddMajorConstructToStages();
        }
        public static void LoadDirectorCards()
        {
            mixCardSelection = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/Base/MixEnemy/dccsMixEnemy.asset").WaitForCompletion();
            arenaCardSelection = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/Base/arena/dccsArenaMonstersDLC1.asset").WaitForCompletion();
            artifactWorldCardSelection = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/Base/artifactworld/dccsArtifactWorldMonstersDLC1.asset").WaitForCompletion();
            golemplains1Monsters = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/Base/golemplains/dccsGolemplainsMonstersDLC1.asset").WaitForCompletion();
            rootJungleCardSelection = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/Base/rootjungle/dccsRootJungleMonstersDLC1.asset").WaitForCompletion();
            shipGraveyardCardSelection = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/Base/shipgraveyard/dccsShipgraveyardMonstersDLC1.asset").WaitForCompletion();
            skyMeadowCardSelection = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/Base/skymeadow/dccsSkyMeadowMonstersDLC1.asset").WaitForCompletion();
            wispGraveyardCardSelection = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/Base/wispgraveyard/dccsWispGraveyardMonstersDLC1.asset").WaitForCompletion();
            sulfurPoolsCardSelection = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/DLC1/sulfurpools/dccsSulfurPoolsMonstersDLC1.asset").WaitForCompletion();

            itAncientLoftCardSelection = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/DLC1/itancientloft/dccsITAncientLoftMonsters.asset").WaitForCompletion();
            itDampCaveCardSelection = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/DLC1/itdampcave/dccsITDampCaveMonsters.asset").WaitForCompletion();
            itFrozenWallCardSelection = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/DLC1/itfrozenwall/dccsITFrozenWallMonsters.asset").WaitForCompletion();
            itGolemPlainsCardSelection = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/DLC1/itgolemplains/dccsITGolemplainsMonsters.asset").WaitForCompletion();
            itGooLakeCardSelection = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/DLC1/itgoolake/dccsITGooLakeMonsters.asset").WaitForCompletion();
            itMoonSelection = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/DLC1/itmoon/dccsITMoonMonsters.asset").WaitForCompletion();
            itSkyMeadowSelection = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/DLC1/itskymeadow/dccsITSkyMeadowMonsters.asset").WaitForCompletion();

        }
        public static void BalanceAssAssIn2()
        {
            assassin2Body.gameObject.AddComponent<DeathRewards>().logUnlockableDef = Addressables.LoadAssetAsync<UnlockableDef>("RoR2/DLC1/Assassin2/Logs.Assassin2Body.asset").WaitForCompletion();
            primaryConfigCharge = Addressables.LoadAssetAsync<EntityStateConfiguration>("RoR2/DLC1/Assassin2/EntityStates.Assassin2.ChargeDash.asset").WaitForCompletion();
            primaryConfigStrike = Addressables.LoadAssetAsync<EntityStateConfiguration>("RoR2/DLC1/Assassin2/EntityStates.Assassin2.DashStrike.asset").WaitForCompletion();
            secondaryConfig = Addressables.LoadAssetAsync<EntityStateConfiguration>("RoR2/DLC1/Assassin2/EntityStates.Assassin2.ThrowShuriken.asset").WaitForCompletion();
            utilityConfig = Addressables.LoadAssetAsync<EntityStateConfiguration>("RoR2/DLC1/Assassin2/EntityStates.Assassin2.Hide.asset").WaitForCompletion();

            //Primary
            primaryConfigCharge.serializedFieldsCollection.serializedFields[1].fieldValue.stringValue = "0.5"; //BaseDuration: 1 => 0.5
            primaryConfigStrike.serializedFieldsCollection.serializedFields[3].fieldValue.stringValue = "0.75"; //SlashDuration: 1 => 0.75
            primaryConfigStrike.serializedFieldsCollection.serializedFields[4].fieldValue.stringValue = "6"; //SelfForce: 4 => 6
            primaryConfigStrike.serializedFieldsCollection.serializedFields[6].fieldValue.stringValue = "80"; //MaxSlashDistance: 20 => 40
            primaryConfigStrike.serializedFieldsCollection.serializedFields[7].fieldValue.stringValue = "12"; //MaxSpeedCoefficent: 4 => 12
            primaryConfigStrike.serializedFieldsCollection.serializedFields[9].fieldValue.stringValue = "32"; //ForceMagnitude: 4 => 32
            primaryConfigStrike.serializedFieldsCollection.serializedFields[11].fieldValue.stringValue = "0.10"; //DashDuration: 0.5 => 0.10

            //Secondary
            secondaryAssassinSkillShuriken.baseRechargeInterval = 4; //Double
            secondaryAssassinSkillShuriken.baseMaxStock = 3; //Triple

            //Utility
            utilityAssassinSkillHide.beginSkillCooldownOnSkillEnd = true; //Tweak Hide skill so they aren't permanently cloaked
            utilityAssassinSkillHide.baseMaxStock = 2; //Makes it a 10 seconds wait
            utilityAssassinSkillHide.stockToConsume = 2;

            AISkillDriver[] aISkillDrivers = new AISkillDriver[5];
            aISkillDrivers = assassin2Master.GetComponents<AISkillDriver>();
            aISkillDrivers[1].maxDistance = 35;
            aISkillDrivers[1].movementType = AISkillDriver.MovementType.Stop; //Stop so there's no movement vector.
            aISkillDrivers[1].moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            aISkillDrivers[1].aimType = AISkillDriver.AimType.AtCurrentEnemy;
            aISkillDrivers[1].activationRequiresTargetLoS = true; //If there's no LOS, the dash entity state will just dash to whatever the fuck is the movement type.
            aISkillDrivers[1].activationRequiresAimTargetLoS = true;
            aISkillDrivers[1].selectionRequiresAimTarget = true;
            aISkillDrivers[1].activationRequiresAimConfirmation = true;
            aISkillDrivers[1].selectionRequiresOnGround = false;

            aISkillDrivers[2].maxDistance = 45;
            aISkillDrivers[2].aimType = AISkillDriver.AimType.AtCurrentEnemy;
            aISkillDrivers[2].movementType = AISkillDriver.MovementType.StrafeMovetarget;

            aISkillDrivers[3].shouldSprint = true;
        }
        public static void AddAssAssInToStages()
        {
            DirectorCard assassin2DC = new DirectorCard()
            {
                spawnCard = cscAssassin2,
                selectionWeight = 1,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Standard,
                minimumStageCompletions = 0,
                preventOverhead = false
            };
            mixCardSelection.AddCard(2, assassin2DC); //Add to dissonance
            rootJungleCardSelection.AddCard(2, assassin2DC); //Add to RootJungle
            wispGraveyardCardSelection.AddCard(2, assassin2DC); //Add to WispGraveyard
        }
        public static void BalanceMajorConstruct()
        {
            majorConstructBody.isChampion = true;
            majorConstructBody.GetComponent<DeathRewards>().bossDropTable = Addressables.LoadAssetAsync<ExplicitPickupDropTable>("RoR2/DLC1/MajorAndMinorConstruct/dtBossMegaConstruct.asset").WaitForCompletion();
            //majorConstructBody.GetComponent<DeathRewards>().logUnlockableDef = Addressables.LoadAssetAsync<UnlockableDef>("RoR2/DLC1/MajorAndMinorConstruct/Logs.MegaConstructBody.asset").WaitForCompletion();
            majorConstructBody.GetComponent<DeathRewards>().logUnlockableDef = Assets.mainAssetBundle.LoadAsset<UnlockableDef>("Logs.MajorConstruct");
        }
        public static void AddMajorConstructToStages()
        {
            DirectorCard majorConstructDC = new DirectorCard()
            {
                spawnCard = cscMajorConstruct,
                selectionWeight = 1,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Standard,
                minimumStageCompletions = 3,
                preventOverhead = true
            };
            On.RoR2.ClassicStageInfo.RebuildCards += (orig, self) => // add it wherever Xi Construct is loaded
            {
                SceneDef mostRecentSceneDef = SceneCatalog.mostRecentSceneDef;
                if (self.monsterDccsPool) HandleDccsPool(self.monsterDccsPool);
                if (self.monsterCategories) HandleDccs(self.monsterCategories);
                orig(self);
            };
            On.EntityStates.GenericCharacterDeath.FixedUpdate += (orig, self) =>
            {
                orig(self);
                if (self is EntityStates.MajorConstruct.Death)
                {
                    if (self.fixedAge <= 5f || !NetworkServer.active) return;
                    self.DestroyBodyAsapServer();
                }
            };
            void HandleDccsPool(DccsPool dccsPool)
            {
                for (int i = 0; i < dccsPool.poolCategories.Length; i++)
                {
                    DccsPool.Category category = dccsPool.poolCategories[i];
                    HandleDccsPoolEntries(category.alwaysIncluded);
                    HandleDccsPoolEntries(category.includedIfConditionsMet);
                    HandleDccsPoolEntries(category.includedIfNoConditionsMet);
                }
            }
            void HandleDccsPoolEntries(DccsPool.PoolEntry[] entries) { foreach (DccsPool.PoolEntry poolEntry in entries) { HandleDccs(poolEntry.dccs); } }
            void HandleDccs(DirectorCardCategorySelection dccs)
            {
                for (int i = 0; i < dccs.categories.Length; i++)
                {
                    ref DirectorCardCategorySelection.Category cat = ref dccs.categories[i];
                    if (Array.Exists(cat.cards, x => x.spawnCard.prefab.name.Contains("MegaConstruct"))) ArrayUtils.ArrayAppend(ref cat.cards, majorConstructDC);
                }
            }
        }
    }
}
