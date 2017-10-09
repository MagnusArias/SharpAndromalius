using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Objects.Projectiles
{
    class P_Enemy
    {
        protected boolean hit;
        protected boolean remove;
        protected int damage;

        public EnemyProjectile(TileMap tm)
        {
            super(tm);
        }

        public int getDamage()
        {
            return damage;
        }

        public boolean shouldRemove()
        {
            return remove;
        }

        public abstract void setHit();

        public abstract void update();

        public void draw(Graphics2D g)
        {
            super.draw(g);
        }
    }
}
