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
            voidChestCoinChoiceOn.tooltipNameColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.Artifact);
            //wormEliteHonorChoiceOn.tooltipBodyColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.BossItemDark); No rule does this
            voidChestCoinChoiceOn.onlyShowInGameBrowserIfNonDefault = true;
            voidChestCoinChoiceOn.requiredUnlockable = RoR2Content.Artifacts.EliteOnly.unlockableDef;
            voidChestCoinChoiceOn.excludeByDefault = false;
            voidChestCoinChoiceOn.sprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texHonorWormRuleOn");

            RuleChoiceDef voidChestCoinChoiceOff = allVoidChestCostVoidCoinRule.AddChoice("Off", false, false);
            voidChestCoinChoiceOff.tooltipNameToken = "RULE_ALLVOIDCHESTCOSTVOIDCOIN_CHOICE_OFF_NAME";
            voidChestCoinChoiceOff.tooltipBodyToken = "RULE_ALLVOIDCHESTCOSTVOIDCOIN_CHOICE_OFF_DESC";
            voidChestCoinChoiceOff.tooltipNameColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.Unaffordable);
            //wormEliteHonorChoiceOff.tooltipBodyColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.BossItemDark); No rule does this
            voidChestCoinChoiceOff.onlyShowInGameBrowserIfNonDefault = true;
            voidChestCoinChoiceOff.excludeByDefault = false;
            voidChestCoinChoiceOff.sprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texHonorWormRuleOff");

            allVoidChestCostVoidCoinRule.MakeNewestChoiceDefault();
            RuleCatalog_PatchedAddRule(allVoidChestCostVoidCoinRule, 5);
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