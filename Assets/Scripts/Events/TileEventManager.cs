using Interfaces;
using UnityEngine;

namespace Events
{
    // workaround because unity does not support generic monobehaviour
    public class TileEventManager : EventManager<TileEvent>
    {
    }
}