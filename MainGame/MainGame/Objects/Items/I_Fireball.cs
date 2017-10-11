using MainGame.Maps.Tiles;
using Microsoft.Xna.Framework.Graphics;

namespace MainGame.Objects.Items
{
    class I_Fireball : Item
    {
        public I_Fireball(TileMap tm, Player pl, Texture2D[] spr) : base(tm, pl, spr) { }

        public override void Draw(SpriteBatch g)
        {
            base.Draw(g);
            if (player.GetSkill(3)) base.DrawInHUD(g, 30, 15);
        }
    }
}
