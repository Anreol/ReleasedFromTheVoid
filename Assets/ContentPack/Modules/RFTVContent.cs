using EntityStates;
using RoR2;
using RoR2.ContentManagement;
using RoR2.ExpansionManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ReleasedFromTheVoid
{
    public class RFTVContent : IContentPackProvider
    {
        public delegate IEnumerator LoadStaticContentAsyncDelegate(LoadStaticContentAsyncArgs args);

        public delegate IEnumerator GenerateContentPackAsyncDelegate(GetContentPackAsyncArgs args);

        public delegate IEnumerator FinalizeAsyncDelegate(FinalizeAsyncArgs args);

        public static LoadStaticContentAsyncDelegate onLoadStaticContent { get; set; }
        public static GenerateContentPackAsyncDelegate onGenerateContentPack { get; set; }
        public static FinalizeAsyncDelegate onFinalizeAsync { get; set; }

        public string identifier => RFTVUnityPlugin.ModIdentifier;
        public SerializableContentPack serializableContentPack; //Registration
        public ContentPack tempPackFromSerializablePack = new ContentPack(); //One step away from finalization

        public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
        {
            //Assetbundle fuckery, unity stuff.
            List<AssetBundle> loadedBundles = new List<AssetBundle>();
            var bundlePaths = Assets.GetAssetBundlePaths();
            int num;
            for (int i = 0; i < bundlePaths.Length; i = num)
            {
                var bundleLoadRequest = AssetBundle.LoadFromFileAsync(bundlePaths[i]);
                while (!bundleLoadRequest.isDone)
                {
                    args.ReportProgress(Util.Remap(bundleLoadRequest.progress + i, 0f, bundlePaths.Length, 0f, 0.8f));
                    yield return null;
                }
                num = i + 1;
                loadedBundles.Add(bundleLoadRequest.assetBundle);
            }

            //Content pack things, RoR2 systems.
            Assets.loadedAssetBundles = new ReadOnlyCollection<AssetBundle>(loadedBundles);
            serializableContentPack = Assets.mainAssetBundle.LoadAsset<SerializableContentPack>("ContentPack");

            tempPackFromSerializablePack = serializableContentPack.CreateContentPack();
            tempPackFromSerializablePack.identifier = identifier;

            if (onLoadStaticContent != null)
                yield return onLoadStaticContent;

            args.ReportProgress(1f);
            yield break;
        }

        public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
        {
            //NOTE: For some reason any instructions that are put inside of here are done twice. This will cause serious errors if you do not watch out.
            ContentPack.Copy(tempPackFromSerializablePack, args.output);

            if (onGenerateContentPack != null)
                yield return onGenerateContentPack;

            args.ReportProgress(1f);
            yield break;
        }

        public IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
        {
            RoR2Application.isModded = true;
            args.ReportProgress(1f);
            yield break;
        }
    }
}