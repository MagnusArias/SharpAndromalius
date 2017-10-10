using MainGame.Control;
using MainGame.Maps.Tiles;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MainGame.Objects.Enemies
{
    class Enemy : ParentObject
    {
        protected int health;
        protected int maxHealth;
        protected int lastBreath;
        protected int damage;

        protected Boolean dead;
        protected Boolean remove;
        
        protected Player player;
        protected Texture2D sprites;

        public Enemy(TileMap tm) : base(tm)
        {
            remove = false;
            animation = new Animation();
        }

        public Boolean IsDead()
        {
            return dead;
        }

        public int GetLastBreath()
        {
            return lastBreath;
        }

        public Boolean ShouldRemove()
        {
            return remove;
        }

        public int GetDamage()
        {
            return damage;
        }

        public void Hit(int damage)
        {
            health -= damage;
            if (health < 0) health = 0;
            if (health == 0)
            {
                dead = true;
            }
        }

        public void Update() { }
        
        public void Draw(SpriteBatch g)
        {
            base.Draw(g);
        }
    }
}
