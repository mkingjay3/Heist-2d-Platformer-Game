using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGFCore;

public abstract class GameBase
{
    public Point2 Resolution { get; protected set; } = new(640, 480); // TODO: Set this to the desired resolution!
    public string Title { get; protected set; } = "Minimalist Game Framework"; // TODO: Rename this!

    /// <summary>
    /// Initializes components of the game. Run only once.
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// Loads content required to begin the game. Run only once.
    /// </summary>
    public abstract void LoadContent();

    /// <summary>
    /// Updates the game state and renders the frame.
    /// </summary>
    public abstract void Update();
}
