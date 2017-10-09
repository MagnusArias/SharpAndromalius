using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

using MainGame.Control;
using MainGame.Objects;
using MainGame.Maps.TileMap;

namespace MainGame.Objects.Enemies
{
    class Enemy : ParentObject
    {
        protected int health;
        protected int maxHealth;
        protected Boolean dead;
        protected int damage;
        protected Boolean remove;
        protected int lastBreath;
        protected Player player;

        protected Texture2D[] sprites;


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

        public void Draw(java.awt.Graphics2D g)
        {
            base.Draw(g);
        }
    }
}
