using RoR2;
using RoR2.ExpansionManagement;
using RoR2.Hologram;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace ReleasedFromTheVoid.Scripts
{
    internal class VoidSupressorSpawner
    {
        public static InteractableSpawnCard voidSupressorSpawnCard;
        public static GameObject lunarInfectionLarge;
        public static Material brotherInfectionWhite;

        public static Vector3 hologramLocalPosOriginal = new Vector3(0.2f, 0.80f, 1f);
        public static Vector3 hologramLocalPosOpen = new Vector3(0.2f, 0.80f, 1.4f);

        [RoR2.SystemInitializer(new Type[]
        {
            typeof(RoR2.SceneCatalog),
        })]
        public static void Init()
        {
            if (!ReleasedFromTheVoid.RFTVUnityPlugin.EnableVoidSuppressors.Value) return;
            FixStrangeScrap();
            SetupVoidSuppressorPrefab();
            Stage.onServerStageBegin += onServerStageBegin;
        }

        private static void FixStrangeScrap()
        {
            DLC1Content.Items.ScrapWhiteSuppressed.tier = ItemTier.Tier1;
            if (DLC1Content.Items.ScrapWhiteSuppressed.DoesNotContainTag(ItemTag.Scrap))
                HG.ArrayUtils.ArrayAppend(ref DLC1Content.Items.ScrapWhiteSuppressed.tags, ItemTag.Scrap);

            DLC1Content.Items.ScrapGreenSuppressed.tier = ItemTier.Tier2;
            if (DLC1Content.Items.ScrapGreenSuppressed.DoesNotContainTag(ItemTag.Scrap))
                HG.ArrayUtils.ArrayAppend(ref DLC1Content.Items.ScrapGreenSuppressed.tags, ItemTag.Scrap);

            DLC1Content.Items.ScrapRedSuppressed.tier = ItemTier.Tier3;
            if (DLC1Content.Items.ScrapRedSuppressed.DoesNotContainTag(ItemTag.Scrap))
                HG.ArrayUtils.ArrayAppend(ref DLC1Content.Items.ScrapRedSuppressed.tags, ItemTag.Scrap);
        }

        private static void SetupVoidSuppressorPrefab()
        {
            lunarInfectionLarge = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/bazaar/LunarInfectionLargeMesh.prefab").WaitForCompletion();
            brotherInfectionWhite = Addressables.LoadAssetAsync<Material>("RoR2/Base/Brother/matBrotherInfectionWhite.mat").WaitForCompletion();
            voidSupressorSpawnCard = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/DLC1/VoidSuppressor/iscVoidSuppressor.asset").WaitForCompletion();

            //Edit transforms because they are fucked up
            voidSupressorSpawnCard.prefab.transform.position = new Vector3(0, 0, 0);
            Vector3 childLocalTransform = voidSupressorSpawnCard.prefab.gameObject.transform.GetChild(0).gameObject.transform.localPosition;
            voidSupressorSpawnCard.prefab.gameObject.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(childLocalTransform.x, childLocalTransform.y - 1.45f, childLocalTransform.z);

            //Make them cost void coins
            voidSupressorSpawnCard.prefab.GetComponent<PurchaseInteraction>().cost = 1;
            voidSupressorSpawnCard.prefab.GetComponent<PurchaseInteraction>().costType = CostTypeIndex.VoidCoin;
            voidSupressorSpawnCard.prefab.GetComponent<VoidSuppressorBehavior>().costMultiplierPerPurchase = 2f;

            //Fix the highlight
            voidSupressorSpawnCard.prefab.GetComponent<Highlight>().targetRenderer = voidSupressorSpawnCard.prefab.gameObject.transform.GetChild(0).gameObject.transform.GetChild(5).GetComponent<SkinnedMeshRenderer>();

            //Add extra components thing
            voidSupressorSpawnCard.prefab.AddComponent<VoidSuppressorBehaviorFixer>();
            voidSupressorSpawnCard.prefab.AddComponent<HologramProjector>().disableHologramRotation = true;
        }

        private static void onServerStageBegin(Stage obj)
        {
            if (Run.instance.IsExpansionEnabled(RFTVUnityPlugin.DLC1))
            {
                float num = UnityEngine.Random.Range(0f, 100f);
                if (num <= 100f) //Keeping the roll here in case I want to undo the guarantee spawn again.
                {
                    //Debug.LogWarning("Spawned a supressor!");
                    FindAltarsInScene();
                }
            }
        }

        public static void FindAltarsInScene()
        {
            foreach (PortalStatueBehavior psb in Resources.FindObjectsOfTypeAll(typeof(PortalStatueBehavior)) as PortalStatueBehavior[])
            {
                if (psb == null || psb.gameObject == null) //wtf
                    continue;
                if (psb.portalType == PortalStatueBehavior.PortalType.Shop && !psb.gameObject.activeSelf)
                {
                    //oh nononon ahahaha what THE FUCK are we doing
                    InteractableSpawnCard tempSpawnCard = UnityEngine.Object.Instantiate(voidSupressorSpawnCard);

                    tempSpawnCard.prefab.transform.rotation = psb.gameObject.transform.rotation; //Assign rotation
                    tempSpawnCard.prefab.GetComponent<Highlight>().targetRenderer.gameObject.transform.localPosition = psb.GetComponent<Highlight>().targetRenderer.gameObject.transform.localPosition; //Set the model's transform to be the same
                    tempSpawnCard.prefab.GetComponent<Highlight>().targetRenderer.gameObject.transform.localRotation = psb.GetComponent<Highlight>().targetRenderer.gameObject.transform.localRotation;

                    DirectorPlacementRule placementRule = new DirectorPlacementRule
                    {
                        placementMode = DirectorPlacementRule.PlacementMode.Direct,
                        spawnOnTarget = psb.gameObject.transform,
                        position = psb.gameObject.transform.position,
                    };

                    DirectorSpawnRequest directorSpawnRequest = new DirectorSpawnRequest(tempSpawnCard, placementRule, Run.instance.stageRng);
                    GameObject suppressor = DirectorCore.instance.TrySpawnObject(directorSpawnRequest);
                    
                    //Randomize supress amount
                    VoidSuppressorBehavior voidSuppressorBehavior = suppressor.GetComponent<VoidSuppressorBehavior>();
                    voidSuppressorBehavior.numItemsToReveal = 0;
                    voidSuppressorBehavior.itemsSuppressedPerPurchase = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        float rolledChance = UnityEngine.Random.Range(0f, 100f);
                        if (rolledChance <= 75 / (i + 1)) //75 base chance
                        {
                            voidSuppressorBehavior.numItemsToReveal++;
                        }
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        float rolledChance = UnityEngine.Random.Range(0f, 100f);
                        if (rolledChance <= 100 / (i + 1)) //100 base chance, to guarantee at least one
                        {
                            voidSuppressorBehavior.itemsSuppressedPerPurchase++;
                        }
                    }

                    Renderer[] renderers = (from thing in psb.GetComponentsInChildren<Renderer>() where thing.material.name == "matLunarInfection (Instance)" select thing).ToArray(); //Get lunar infections.
                    foreach (Renderer item in renderers)
                    {
                        item.gameObject.SetActive(true);
                        item.material = UnityEngine.Object.Instantiate(brotherInfectionWhite);
                        item.material.SetColor("_EmColor", ColorCatalog.GetColor(ItemTierCatalog.GetItemTierDef(ItemTier.VoidTier1).colorIndex));
                        GameObject gameObject = UnityEngine.Object.Instantiate(item.gameObject, suppressor.transform); //then we transfer it to the suppressor, we do not care if we edited the original, as the original has the parent disabled.
                    }
                    return;
                }
            }
        }

        public class VoidSuppressorBehaviorFixer : MonoBehaviour
        {
            private VoidSuppressorBehavior voidSuppressor;
            private PurchaseInteraction purchaseInteraction;
            private HologramProjector hologramProjector;
            private int stage;

            public void Awake()
            {
                voidSuppressor = base.gameObject.GetComponent<VoidSuppressorBehavior>();
                purchaseInteraction = base.gameObject.GetComponent<PurchaseInteraction>();
                hologramProjector = base.gameObject.GetComponent<HologramProjector>();
            }

            public void Start()
            {
                //Add hologram.
                int c = gameObject.transform.GetChild(0).transform.childCount;
                GameObject holoThing = new GameObject();
                holoThing.transform.localPosition = hologramLocalPosOriginal;
                holoThing.transform.localRotation = new Quaternion(0f, 180f, 0f, 0f);
                UnityEngine.Object.Instantiate(holoThing, gameObject.transform.GetChild(0).gameObject.transform);
                UnityEngine.Object.Destroy(holoThing);
                hologramProjector.hologramPivot = gameObject.transform.GetChild(0).gameObject.transform.GetChild(c);

                if (!NetworkServer.active) //fucking lmao
                {
                    GameObject troll = UnityEngine.Object.Instantiate(lunarInfectionLarge, gameObject.transform);
                    Renderer[] renderers = (from thing in troll.GetComponentsInChildren<Renderer>() where thing.material.name == "matLunarInfection (Instance)" select thing).ToArray(); //Get lunar infections.
                    foreach (Renderer item in renderers)
                    {
                        item.gameObject.SetActive(true);
                        item.gameObject.transform.localScale = new Vector3(0.3f, 0.4f, 0.4f);
                        item.gameObject.transform.localPosition = new Vector3(0.15f, -0.65f, 0.4f);
                        item.material = UnityEngine.Object.Instantiate(brotherInfectionWhite);
                        item.material.SetColor("_EmColor", ColorCatalog.GetColor(ItemTierCatalog.GetItemTierDef(ItemTier.VoidTier1).colorIndex));
                    }
                }
                //runs on the server only, it seems.
                if (!purchaseInteraction)
                    return;
                purchaseInteraction.onPurchase.AddListener((Interactor interactor) =>
                {
                    if (voidSuppressor.purchaseCount <= 0) //Player has revealed it, make the first activation free.
                    {
                        purchaseInteraction.cost = 0;
                    }
                    else if (voidSuppressor.purchaseCount == 1) //After first purchase. Green.
                    {
                        purchaseInteraction.cost = (int)(1 * voidSuppressor.costMultiplierPerPurchase);
                    }
                });
            }

            public void Update()
            {
                if (voidSuppressor)
                {
                    if (voidSuppressor.hasRevealed)
                    {
                        if (voidSuppressor.purchaseCount <= 0) //Player has revealed it, make the first activation free.
                        {
                            hologramProjector.hologramPivot.gameObject.SetActive(false);
                        }
                        else if (voidSuppressor.purchaseCount == 1) //After first purchase. Green.
                        {
                            hologramProjector.hologramPivot.gameObject.SetActive(true);
                        }
                        hologramProjector.hologramPivot.gameObject.transform.localPosition = Vector3.MoveTowards(hologramProjector.hologramPivot.gameObject.transform.localPosition, hologramLocalPosOpen, Time.fixedDeltaTime);
                    }
                    else
                    {
                        hologramProjector.hologramPivot.gameObject.transform.localPosition = Vector3.MoveTowards(hologramProjector.hologramPivot.gameObject.transform.localPosition, hologramLocalPosOriginal, Time.fixedDeltaTime);
                    }
                }
            }
        }
    }
}