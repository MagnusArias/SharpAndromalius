using MainGame.Maps.Tiles;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MainGame.Objects.Projectiles
{
    abstract class P_Enemy : ParentObject
    {
        protected Boolean hit;
        protected int damage;

        public P_Enemy(TileMap tm) : base(tm) { }

        public int GetDamage() => damage;

        public abstract void SetHit();
    }
}
