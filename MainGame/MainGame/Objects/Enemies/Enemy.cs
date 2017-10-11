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
        
        protected Player player;
        protected Texture2D sprites;

        public Enemy(TileMap tm) : base(tm) { }

        public Boolean IsDead() => dead;

        public int GetLastBreath() => lastBreath;

        public int GetDamage() => damage;

        public void Hit(int damage)
        {
            health -= damage;
            if (health < 0) health = 0;
            if (health == 0)
            {
                dead = true;
            }
        }
    }
}
