using System.Collections.Generic;
using UnityEngine;

namespace RPG2D.Interfaces
{
    public interface IGameObjectHolder
    {
        GameObject ReturnMainGameObject();
        List<GameObject> ReturnAllGameObjects();
    }
}