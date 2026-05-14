using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class ResourcesLoader
{
    private static readonly HashSet<Sprite> _loadedSprites = new HashSet<Sprite>();
    private static readonly HashSet<Texture> _loadedTextures = new HashSet<Texture>();
    private static readonly HashSet<Material> _loadedMaterials = new HashSet<Material>();
    private static readonly HashSet<AudioClip> _loadedAudioClips = new HashSet<AudioClip>();
    private static readonly HashSet<GameObject> _loadedGameObjects = new HashSet<GameObject>();

    private static Sprite AddSprite(Sprite sprite)
    {
        if (sprite != null && _loadedSprites.Contains(sprite) == false)
        {
            _loadedSprites.Add(sprite);
        }
        return sprite;
    }
    private static Sprite[] AddSprites(Sprite[] sprites)
    {
        if (sprites != null)
        {
            foreach (var sprite in sprites)
            {
                _ = AddSprite(sprite);
            }
        }
        return sprites;
    }

    private static Texture AddTexture(Texture texture)
    {
        if (texture != null && _loadedTextures.Contains(texture) == false)
        {
            _loadedTextures.Add(texture);
        }
        return texture;
    }

    private static Material AddMaterial(Material material)
    {
        if (material != null && _loadedMaterials.Contains(material) == false)
        {
            _loadedMaterials.Add(material);
        }
        return material;
    }

    private static AudioClip AddAudioClip(AudioClip audioClip)
    {
        if (audioClip != null && _loadedAudioClips.Contains(audioClip) == false)
        {
            _loadedAudioClips.Add(audioClip);
        }
        return audioClip;
    }

    private static GameObject AddGameObject(GameObject gameObject)
    {
        if (gameObject != null && _loadedGameObjects.Contains(gameObject) == false)
        {
            _loadedGameObjects.Add(gameObject);
        }
        return gameObject;
    }

    public static void UnloadAssets()
    {
        foreach (var sprite in _loadedSprites)
        {
            Resources.UnloadAsset(sprite);
        }
        foreach (var texture in _loadedTextures)
        {
            Resources.UnloadAsset(texture);
        }
        foreach (var material in _loadedMaterials)
        {
            Resources.UnloadAsset(material);
        }
        foreach (var audioClip in _loadedAudioClips)
        {
            Resources.UnloadAsset(audioClip);
        }
        _loadedSprites.Clear();
        _loadedTextures.Clear();
        _loadedMaterials.Clear();
        _loadedAudioClips.Clear();
        _loadedGameObjects.Clear();
    }
}
public static partial class ResourcesLoader
{
    public static Sprite Sprite(string path) => AddSprite(Resources.Load<Sprite>(path));
    public static Sprite[] Sprites(string path) => AddSprites(Resources.LoadAll<Sprite>(path));
    public static Texture Texture(string path) => AddTexture(Resources.Load<Texture>(path));
    public static Material Material(string path) => AddMaterial(Resources.Load<Material>(path));
    public static AudioClip AudioClip(string path) => AddAudioClip(Resources.Load<AudioClip>(path));
    public static GameObject GameObject(string path) => AddGameObject(Resources.Load<GameObject>(path));
}
public static partial class ResourcesLoader
{
    public static void SpriteAsync(string path, Action<Sprite> loaded)
    {
        Resources.LoadAsync<Sprite>(path).completed += req =>
        {
            Sprite sprite = (req as ResourceRequest).asset as Sprite;
            AddSprite(sprite);
            loaded?.Invoke(sprite);
        };
    }
    public static void TextureAsync(string path, Action<Texture> loaded)
    {
        Resources.LoadAsync<Texture>(path).completed += req =>
        {
            Texture texture = (req as ResourceRequest).asset as Texture;
            AddTexture(texture);
            loaded?.Invoke(texture);
        };
    }
    public static void MaterialAsync(string path, Action<Material> loaded)
    {
        Resources.LoadAsync<Material>(path).completed += req =>
        {
            Material material = (req as ResourceRequest).asset as Material;
            AddMaterial(material);
            loaded?.Invoke(material);
        };
    }
    public static void AudioClipAsync(string path, Action<AudioClip> loaded)
    {
        Resources.LoadAsync<AudioClip>(path).completed += req =>
        {
            AudioClip audioClip = (req as ResourceRequest).asset as AudioClip;
            AddAudioClip(audioClip);
            loaded?.Invoke(audioClip);
        };
    }
    public static void GameObjectAsync(string path, Action<GameObject> loaded)
    {
        Resources.LoadAsync<GameObject>(path).completed += req =>
        {
            GameObject gameObject = (req as ResourceRequest).asset as GameObject;
            AddGameObject(gameObject);
            loaded?.Invoke(gameObject);
        };
    }
}