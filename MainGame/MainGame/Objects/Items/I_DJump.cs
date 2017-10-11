using MainGame.Maps.Tiles;
using Microsoft.Xna.Framework.Graphics;

namespace MainGame.Objects.Items
{
    class I_DJump : Item
    {
        public I_DJump(TileMap tm, Player pl, Texture2D[] spr) : base(tm, pl, spr) { }

        public override void Draw(SpriteBatch g)
        {
            base.Draw(g);
            if (player.GetSkill(0)) base.DrawInHUD(g, 10, 35);
        }
    }
}
