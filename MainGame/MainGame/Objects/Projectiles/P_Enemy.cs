using MainGame.Maps.TileMap;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MainGame.Objects.Projectiles
{
    abstract class P_Enemy : ParentObject
    {
        protected Boolean hit;
        protected Boolean remove;
        protected int damage;

        public P_Enemy(TileMap tm) : base(tm)
        {
            
        }

        public int GetDamage()
        {
            return damage;
        }

        public Boolean ShouldRemove()
        {
            return remove;
        }

        public abstract void SetHit();

        public abstract void Update();

        public void Draw(SpriteBatch g)
        {
            base.Draw(g);
        }
    }
}
