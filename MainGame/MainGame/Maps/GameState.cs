using Microsoft.Xna.Framework.Graphics;

namespace MainGame.Maps
{
    abstract class GameState
    {
        protected GameStateManager gsm;

        public GameState(GameStateManager gsm)
        {
            this.gsm = gsm;
        }

        public abstract void Init();
        public abstract void Update();
        public abstract void Draw(SpriteBatch g);
        public abstract void HandleInput();
    }
}
