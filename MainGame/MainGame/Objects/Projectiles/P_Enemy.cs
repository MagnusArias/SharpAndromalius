using MainGame.Maps.Tiles;
using System;

namespace MainGame.Objects.Projectiles
{
    abstract class P_Enemy : ParentObject
    {
        protected Boolean hit;

        public P_Enemy(TileMap tm) : base(tm) { }

        public abstract void SetHit();
    }
}
