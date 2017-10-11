using MainGame.Maps.Tiles;
using Microsoft.Xna.Framework.Graphics;

namespace MainGame.Objects.Items
{
    class I_Sword : Item
    {
        public I_Sword(TileMap tm, Player pl, Texture2D[] spr) : base(tm, pl, spr) { }

        public override void Draw(SpriteBatch g)
        {
            base.Draw(g);
            if (player.GetSkill(2)) base.DrawInHUD(g, 10, 15);
        }
    }
}
