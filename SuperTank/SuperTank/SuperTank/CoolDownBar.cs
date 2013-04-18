using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperTank
{
    public class CoolDownBar : DrawableGameComponent
    {
        public float cd;
        private static Texture2D bar;
        public float state;
        private SpriteFont CDText;

        public CoolDownBar(Game game)
            : base(game)
        {
            bar = Game.Content.Load<Texture2D>("CdMetre");
            CDText = Game.Content.Load<SpriteFont>("pericles10");
            cd = 100;
        }

        public void draw(SpriteBatch batch)
        {
            batch.Draw(bar, new Rectangle(((Game1)Game).graphics.PreferredBackBufferWidth / 8, ((((Game1)Game).graphics.PreferredBackBufferHeight * 15) / 16),
                (int)100, 25), new Rectangle(0, 45, (bar.Width / 4), 25), Color.Gray);

            // draw the current cd level based on the current cooldown of shots
            batch.Draw(bar, new Rectangle(((Game1)Game).graphics.PreferredBackBufferWidth / 8, ((((Game1)Game).graphics.PreferredBackBufferHeight * 15) / 16),
                (int)cd, 25), new Rectangle(0, 45, (bar.Width / 4), 25), Color.Yellow);

            batch.DrawString(CDText, "COOLDOWN", new Vector2(((Game1)Game).graphics.PreferredBackBufferWidth / 8, ((Game1)Game).graphics.PreferredBackBufferHeight * 17 / 18), Color.Black);
        }
    }
}
