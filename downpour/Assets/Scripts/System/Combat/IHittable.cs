using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Entity.Player;

namespace Downpour.Combat
{
    public interface IHittable
    {
        void OnHit(Player player, int damage, int direction);

        Vector2 GetSlashEffectPosition();
    }
}
