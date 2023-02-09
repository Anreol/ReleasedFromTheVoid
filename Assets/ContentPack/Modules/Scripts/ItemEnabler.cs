using RoR2;
using System;

namespace ReleasedFromTheVoid.Scripts
{
    internal class ItemEnabler
    {
        [RoR2.SystemInitializer(new Type[]
        {
            typeof(RoR2.ItemCatalog),
            typeof(RoR2.EquipmentCatalog),
        })]
        public static void Init()
        {
            DLC1Content.Equipment.LunarPortalOnUse.canDrop = true;
            //Everything below is for making sure it appears, it's already assigned in the item def.
            DLC1Content.Equipment.LunarPortalOnUse.appearsInMultiPlayer = true;
            DLC1Content.Equipment.LunarPortalOnUse.appearsInSinglePlayer = true;
            DLC1Content.Equipment.LunarPortalOnUse.enigmaCompatible = false;
            DLC1Content.Equipment.LunarPortalOnUse.canBeRandomlyTriggered = false;
            DLC1Content.Equipment.LunarPortalOnUse.isLunar = true;
            DLC1Content.Equipment.LunarPortalOnUse.requiredExpansion = RFTVUnityPlugin.DLC1;
        }
    }
}