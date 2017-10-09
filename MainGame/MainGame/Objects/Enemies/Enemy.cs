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

        public Boolean isDead()
        {
            return dead;
        }

        public int getLastBreath()
        {
            return lastBreath;
        }

        public Boolean shouldRemove()
        {
            return remove;
        }

        public int getDamage()
        {
            return damage;
        }

        public void hit(int damage)
        {
            health -= damage;
            if (health < 0) health = 0;
            if (health == 0)
            {
                dead = true;
            }
        }

        public void update() { }

        public void draw(java.awt.Graphics2D g)
        {
            base.draw(g);
        }
    }
}
