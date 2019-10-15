using UnityEngine;
using System.Collections;

public interface IActionManager
{
    void SendUFO(Game game, Ruler ruler, int round);

    bool IsUFODead(UFO ufo);
}
