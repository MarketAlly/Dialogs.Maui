using Microsoft.Maui.Controls;
using System.Collections.Concurrent;

namespace MarketAlly.Dialogs.Maui.Core
{
    /// <summary>
    /// Cache for dialog images to improve performance
    /// </summary>
    internal static class ImageCache
    {
        private static readonly ConcurrentDictionary<string, ImageSource> _cache = new();
        private static readonly ConcurrentDictionary<string, byte[]> _resourceCache = new();
        private static HashSet<string>? _availableResources;
        private static readonly object _resourceLock = new();

        /// <summary>
        /// Gets or creates a cached image source
        /// </summary>
        public static ImageSource? GetOrCreate(string key, Func<ImageSource?> factory)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            return _cache.GetOrAdd(key, _ => factory() ?? ImageSource.FromFile(""));
        }

        /// <summary>
        /// Gets cached resource bytes or loads them
        /// </summary>
        public static byte[]? GetResourceBytes(string resourceName)
        {
            if (_resourceCache.TryGetValue(resourceName, out var cached))
                return cached;

            var assembly = typeof(ImageCache).Assembly;
            var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                return null;

            using (stream)
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                _resourceCache[resourceName] = buffer;
                return buffer;
            }
        }

        /// <summary>
        /// Checks if an embedded resource exists (cached)
        /// </summary>
        public static bool ResourceExists(string resourceName)
        {
            if (_availableResources == null)
            {
                lock (_resourceLock)
                {
                    if (_availableResources == null)
                    {
                        var assembly = typeof(ImageCache).Assembly;
                        _availableResources = new HashSet<string>(assembly.GetManifestResourceNames());
                    }
                }
            }

            return _availableResources.Contains(resourceName);
        }

        /// <summary>
        /// Clears the cache (useful for theme changes)
        /// </summary>
        public static void Clear()
        {
            _cache.Clear();
            // Keep resource cache as those don't change
        }
    }
}