using RoR2;
using System;
using UnityEngine;

namespace ReleasedFromTheVoid.Scripts
{
    internal class RulebookExtras
    {
        public static bool runAllVoidChestCostVoidCoin
        {
            get
            {
                if (Run.instance != null)
                {
                    return (bool)Run.instance.ruleBook.GetRuleChoice(RuleCatalog.FindRuleDef("Misc.AllVoidChestCostVoidCoin")).extraData;
                }
                return false;
            }
        }
        public static int runStartingVoidCoins
        {
            get
            {
                if (Run.instance != null)
                {
                    return (int)Run.instance.ruleBook.GetRuleChoice(RuleCatalog.FindRuleDef("Misc.StartingVoidCoin")).extraData;
                }
                return 0;
            }
        }

        [SystemInitializer(new Type[]
        {
            typeof(RuleCatalog),
        })]
        private static void Init()
        {
            RuleDef allVoidChestCostVoidCoinRule = new RuleDef("Misc.AllVoidChestCostVoidCoin", "RULE_MISC_ALLVOIDCHESTCOSTVOIDCOIN");

            RuleChoiceDef voidChestCoinChoiceOn = allVoidChestCostVoidCoinRule.AddChoice("On", true, false);
            voidChestCoinChoiceOn.tooltipNameToken = "RULE_ALLVOIDCHESTCOSTVOIDCOIN_CHOICE_ON_NAME";
            voidChestCoinChoiceOn.tooltipBodyToken = "RULE_ALLVOIDCHESTCOSTVOIDCOIN_CHOICE_ON_DESC";
            voidChestCoinChoiceOn.tooltipNameColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.VoidCoin);
            //wormEliteHonorChoiceOn.tooltipBodyColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.BossItemDark); No rule does this
            voidChestCoinChoiceOn.onlyShowInGameBrowserIfNonDefault = true;
            voidChestCoinChoiceOn.requiredUnlockable = RoR2Content.Artifacts.EliteOnly.unlockableDef;
            voidChestCoinChoiceOn.excludeByDefault = false;
            voidChestCoinChoiceOn.sprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texVoidCoinRuleOn");

            RuleChoiceDef voidChestCoinChoiceOff = allVoidChestCostVoidCoinRule.AddChoice("Off", false, false);
            voidChestCoinChoiceOff.tooltipNameToken = "RULE_ALLVOIDCHESTCOSTVOIDCOIN_CHOICE_OFF_NAME";
            voidChestCoinChoiceOff.tooltipBodyToken = "RULE_ALLVOIDCHESTCOSTVOIDCOIN_CHOICE_OFF_DESC";
            voidChestCoinChoiceOff.tooltipNameColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.Unaffordable);
            //wormEliteHonorChoiceOff.tooltipBodyColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.BossItemDark); No rule does this
            voidChestCoinChoiceOff.onlyShowInGameBrowserIfNonDefault = true;
            voidChestCoinChoiceOff.excludeByDefault = false;
            voidChestCoinChoiceOff.sprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texVoidCoinRuleOff");

            allVoidChestCostVoidCoinRule.MakeNewestChoiceDefault();


            RuleDef StartingVoidCoin = new RuleDef("Misc.StartingVoidCoin", "RULE_MISC_STARTINGVOIDCOIN");

            RuleChoiceDef StartingVoidCoinZero = StartingVoidCoin.AddChoice("Zero", 0, false);
            StartingVoidCoinZero.tooltipNameToken = "RULE_STARTINGVOIDCOIN_CHOICE_ZERO_NAME";
            StartingVoidCoinZero.tooltipBodyToken = "RULE_STARTINGVOIDCOIN_CHOICE_ZERO_DESC";
            StartingVoidCoinZero.tooltipNameColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.Unaffordable);
            //wormEliteHonorChoiceOn.tooltipBodyColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.BossItemDark); No rule does this
            StartingVoidCoinZero.onlyShowInGameBrowserIfNonDefault = true;
            StartingVoidCoinZero.requiredUnlockable = RoR2Content.Artifacts.EliteOnly.unlockableDef;
            StartingVoidCoinZero.excludeByDefault = false;
            StartingVoidCoinZero.sprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texVoidCoinRuleZero");

            RuleChoiceDef StartingVoidCoinTen = StartingVoidCoin.AddChoice("Ten", 10, false);
            StartingVoidCoinTen.tooltipNameToken = "RULE_STARTINGVOIDCOIN_CHOICE_TEN_NAME";
            StartingVoidCoinTen.tooltipBodyToken = "RULE_STARTINGVOIDCOIN_CHOICE_TEN_DESC";
            StartingVoidCoinTen.tooltipNameColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.VoidCoin);
            //wormEliteHonorChoiceOff.tooltipBodyColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.BossItemDark); No rule does this
            StartingVoidCoinTen.onlyShowInGameBrowserIfNonDefault = true;
            StartingVoidCoinTen.excludeByDefault = false;
            StartingVoidCoinTen.sprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texVoidCoinRuleTen");

            StartingVoidCoin.defaultChoiceIndex = 0;


            RuleCatalog_PatchedAddRule(allVoidChestCostVoidCoinRule, 5);
            RuleCatalog_PatchedAddRule(StartingVoidCoin, 5);
        }

        //Categories as of survivors of the void
        //0 - Difficulty, 1 - Expansions, 2 - Artifacts, 3 - Items, 4 - Equipment, 5 - Misc
        private static void RuleCatalog_PatchedAddRule(RuleDef ruleDef, int RuleCategoryDefIndex) //Meant to be used post Init
        {
            ruleDef.category = RuleCatalog.GetCategoryDef(RuleCategoryDefIndex);
            ruleDef.globalIndex = RuleCatalog.allRuleDefs.Count;
            RuleCatalog.allCategoryDefs[RuleCategoryDefIndex].children.Add(ruleDef);
            RuleCatalog.allRuleDefs.Add(ruleDef);

            if (RuleCatalog.highestLocalChoiceCount < ruleDef.choices.Count)
                RuleCatalog.highestLocalChoiceCount = ruleDef.choices.Count;

            RuleCatalog.ruleDefsByGlobalName[ruleDef.globalName] = ruleDef;
            for (int i = 0; i < ruleDef.choices.Count; i++)
            {
                RuleChoiceDef ruleChoiceDef = ruleDef.choices[i];
                ruleChoiceDef.localIndex = i;
                ruleChoiceDef.globalIndex = RuleCatalog.allChoicesDefs.Count;
                RuleCatalog.allChoicesDefs.Add(ruleChoiceDef);

                RuleCatalog.ruleChoiceDefsByGlobalName[ruleChoiceDef.globalName] = ruleChoiceDef;

                if (ruleChoiceDef.requiredUnlockable)
                    HG.ArrayUtils.ArrayAppend(ref RuleCatalog._allChoiceDefsWithUnlocks, ruleChoiceDef);
            }
        }
    }
}