using System;
using UnityEngine;

namespace Interfaces
{
    public abstract class ICharacter : MonoBehaviour
    {
        public String Name { get; private set; } = "Unnamed Character";
        public ITile CurrentTile { get; private set; }

        public Team Team { get; private set; } = new Team("test0", 0);
        public bool Shield { get; private set; }

        public GameObject ConfirmationCanvas { get; set; }
    }
}