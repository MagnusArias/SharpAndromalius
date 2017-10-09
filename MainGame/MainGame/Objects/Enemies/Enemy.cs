using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Objects.Enemies
{
    class Enemy
    {
        protected int health;
        protected int maxHealth;
        protected boolean dead;
        protected int damage;
        protected boolean remove;
        protected int lastBreath;
        protected Player player;

        protected BufferedImage[] sprites;


        public Enemy(TileMap tm)
        {
            super(tm);
            remove = false;
            animation = new Animation();
        }

        public boolean isDead()
        {
            return dead;
        }

        public int getLastBreath()
        {
            return lastBreath;
        }

        public boolean shouldRemove()
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
            super.draw(g);
        }
    }
}
