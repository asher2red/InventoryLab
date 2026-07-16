using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AddressableSpriteLoader
{
    private static readonly Dictionary<AssetReferenceSprite, Sprite> cache = new();

    private static readonly Dictionary<AssetReferenceSprite, AsyncOperationHandle<Sprite>> handles = new();

    public static async Awaitable Initialize(IEnumerable<ItemData> items)
    {
        foreach (var item in items)
        {
            if (item == null)
                continue;

            if (item.iconReference == null)
                continue;

            if (cache.ContainsKey(item.iconReference))
                continue;

            var handle = item.iconReference.LoadAssetAsync<Sprite>();

            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to load : {item.itemName}");
                continue;
            }

            Debug.Log($"Preload : {item.itemName}");

            cache.Add(item.iconReference, handle.Result);
            handles.Add(item.iconReference, handle);
        }
    }

    public static Sprite GetSprite(AssetReferenceSprite reference)
    {
        if (reference == null) {
            return null;
        }

        if (cache.TryGetValue(reference, out var sprite)) {
            return sprite;
        }

        Debug.LogWarning($"Sprite is not preloaded : {reference.RuntimeKey}");

        return null;
    }

    public static void ReleaseAll()
    {
        Debug.Log($"Release Addressables : {handles.Count}");
        
        foreach (var handle in handles.Values)
        {
            Addressables.Release(handle);
        }

        handles.Clear();
        cache.Clear();
    }
}