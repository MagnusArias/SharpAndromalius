using MainGame.Maps.Tiles;
using Microsoft.Xna.Framework.Graphics;

namespace MainGame.Objects.Items
{
    class I_Dash : Item
    {
        public I_Dash(TileMap tm, Player pl, Texture2D[] spr) : base(tm, pl, spr) { }

        public override void Draw(SpriteBatch g)
        {
            base.Draw(g);
            if (player.GetSkill(1)) base.DrawInHUD(g, 30, 35);
        }
    }
}
