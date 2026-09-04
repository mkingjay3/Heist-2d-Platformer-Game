using System.IO;

namespace MGFCore.Engine;

public static partial class Engine
{
    static string rootAssetPath = "Assets";
    static bool rootPathInitialized = false;

    static HashSet<string> loadedAssets = new HashSet<string>();

    private static void VerifyAssetNotLoaded(string path)
    {
        if (loadedAssets.Contains(path))
        {
            throw new InvalidOperationException($"The asset '{path}' has already been loaded. Assets should only be loaded once and then reused.");
        }
        loadedAssets.Add(path);
    }

    private static string GetAssetPath(string path)
    {
        while (!rootPathInitialized)
        {
            if (!Directory.Exists(rootAssetPath))
            {
                rootAssetPath = Path.Combine(@"..\", rootAssetPath);
            }
            else
            {
                rootPathInitialized = true;
            }
        }
        return Path.Combine(rootAssetPath, path);
    }

    /// <summary>
    /// Loads a texture from the Assets directory. Supports the following formats: BMP, GIF, JPEG, PNG, SVG, TGA, TIFF, WEBP.
    /// </summary>
    /// <param name="path">The path to the texture file, relative to the Assets directory.</param>
    public static Texture LoadTexture(string path)
    {
        VerifyAssetNotLoaded(path);

        path = GetAssetPath(path);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("The specified texture file was not found.", path);
        }

        IntPtr handle = SDL_image.IMG_LoadTexture(Renderer, path);

        if (handle == IntPtr.Zero)
        {
            throw new EngineException("Failed to load texture.", new SDLException());
        }

        uint format;
        int access, width, height;
        SDL.SDL_QueryTexture(handle, out format, out access, out width, out height);

        return new Texture(handle, width, height);
    }

    /// <summary>
    /// Loads a resizable texture from the Assets directory. Supports the following formats: BMP, GIF, JPEG, PNG, SVG, TGA, TIFF, WEBP.
    /// See the documentation for an explanation of what these parameters _actually_ mean.
    /// </summary>
    /// <param name="path">The path to the texture file, relative to the Assets directory.</param>
    /// <param name="leftOffset">The resize offset from the left of the texture (in pixels).</param>
    /// <param name="rightOffset">The resize offset from the right of the texture (in pixels).</param>
    /// <param name="topOffset">The resize offset from the top of the texture (in pixels).</param>
    /// <param name="bottomOffset">The resize offset from the bottom of the texture (in pixels).</param>
    public static ResizableTexture LoadResizableTexture(string path, int leftOffset, int rightOffset, int topOffset, int bottomOffset)
    {
        VerifyAssetNotLoaded(path);

        path = GetAssetPath(path);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("The specified texture file was not found.", path);
        }

        IntPtr handle = SDL_image.IMG_LoadTexture(Renderer, path);

        if (handle == IntPtr.Zero)
        {
            throw new EngineException("Failed to load texture.", new SDLException());
        }

        uint format;
        int access, width, height;
        SDL.SDL_QueryTexture(handle, out format, out access, out width, out height);

        // Convert the relative offsets (from the edges) into absolute offsets (from the origin):
        rightOffset = width - rightOffset - 1;
        bottomOffset = height - bottomOffset - 1;

        if (leftOffset < 0 || rightOffset >= width || topOffset < 0 || bottomOffset >= height || leftOffset > rightOffset || topOffset > bottomOffset)
        {
            throw new ArgumentException("Invalid offset parameter.");
        }

        return new ResizableTexture(handle, width, height, leftOffset, rightOffset, topOffset, bottomOffset);
    }

    /// <summary>
    /// Loads a font from the Assets directory for a single text size. Supports the following formats: TTF, FON.
    /// </summary>
    /// <param name="path">The path to the font file, relative to the Assets directory.</param>
    /// <param name="pointSize">The size of the text that will be rendered by this font (in points).</param>
    public static Font LoadFont(string path, int pointSize)
    {
        VerifyAssetNotLoaded(path + pointSize);

        if (pointSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pointSize), "Point size must be greater than zero.");
        }

        path = GetAssetPath(path);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("The specified font file was not found.", path);
        }

        IntPtr handle = SDL_ttf.TTF_OpenFont(path, pointSize);

        if (handle == IntPtr.Zero)
        {
            throw new EngineException("Failed to load font.", new SDLException());
        }

        return new Font(handle);
    }

    /// <summary>
    /// Loads a sound file from the Assets directory. Supports the following formats: WAV, OGG.
    /// </summary>
    /// <param name="path">The path to the sound file, relative to the Assets directory.</param>
    public static Sound LoadSound(string path)
    {
        VerifyAssetNotLoaded(path);

        path = GetAssetPath(path);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("The specified sound file was not found.", path);
        }

        IntPtr handle = SDL_mixer.Mix_LoadWAV(path);

        if (handle == IntPtr.Zero)
        {
            throw new EngineException("Failed to load sound.", new SDLException());
        }

        return new Sound(handle);
    }

    /// <summary>
    /// Loads a music file from the Assets directory. Supports the following formats: WAV, OGG, MP3, FLAC.
    /// </summary>
    /// <param name="path">The path to the music file, relative to the Assets directory.</param>
    public static Music LoadMusic(string path)
    {
        VerifyAssetNotLoaded(path);

        path = GetAssetPath(path);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("The specified music file was not found.", path);
        }

        IntPtr handle = SDL_mixer.Mix_LoadMUS(path);

        if (handle == IntPtr.Zero)
        {
            throw new EngineException("Failed to load music.", new SDLException());
        }

        return new Music(handle);
    }
}

/// <summary>
/// A handle to a texture. These should only be created by calling LoadTexture().
/// </summary>
public class Texture
{
    public readonly IntPtr Handle;
    public readonly int Width;
    public readonly int Height;
    public readonly Vector2 Size;

    public Texture(IntPtr handle, int width, int height)
    {
        Handle = handle;
        Width = width;
        Height = height;
        Size = new Vector2(width, height);
    }
}

/// <summary>
/// A handle to a resizable texture. These should only be created by calling LoadResizableTexture().
/// </summary>
public class ResizableTexture
{
    public readonly IntPtr Handle;
    public readonly int Width;
    public readonly int Height;
    public readonly int LeftOffset;
    public readonly int RightOffset;
    public readonly int TopOffset;
    public readonly int BottomOffset;

    public ResizableTexture(IntPtr handle, int width, int height, int leftOffset, int rightOffset, int topOffset, int bottomOffset)
    {
        Handle = handle;
        Width = width;
        Height = height;
        LeftOffset = leftOffset;
        RightOffset = rightOffset;
        TopOffset = topOffset;
        BottomOffset = bottomOffset;
    }
}

/// <summary>
/// A handle to a font. These should only be created by calling LoadFont().
/// </summary>
public class Font
{
    public readonly IntPtr Handle;

    public Font(IntPtr handle)
    {
        Handle = handle;
    }
}

/// <summary>
/// A handle to a sound file. These should only be created by calling LoadSound().
/// </summary>
public class Sound
{
    public readonly IntPtr Handle;

    public Sound(IntPtr handle)
    {
        Handle = handle;
    }
}

/// <summary>
/// A handle to a music file. These should only be created by calling LoadMusic().
/// </summary>
public class Music
{
    public readonly IntPtr Handle;

    public Music(IntPtr handle)
    {
        Handle = handle;
    }
}
