using MainGame.Control;
using MainGame.Maps.Tiles;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MainGame.Objects.Enemies
{
    class Enemy : ParentObject
    {
        protected int lastBreath;

        protected Player player;
        protected Texture2D sprites;

        public Enemy(TileMap tm) : base(tm) { }

        public Boolean IsDead() => dead;

        public int GetLastBreath() => lastBreath;
    }
}
