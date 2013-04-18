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
        public int color;
        int offset = 300;

        public CoolDownBar(Game game)
            : base(game)
        {
            bar = Game.Content.Load<Texture2D>("CdMetre");
            CDText = Game.Content.Load<SpriteFont>("pericles10");
            cd = 100;
        }

        public CoolDownBar(Game game, int col)
            : base(game)
        {
            bar = Game.Content.Load<Texture2D>("CdMetre");
            CDText = Game.Content.Load<SpriteFont>("pericles10");
            cd = 100;
            color = col;
        }

        public void draw(SpriteBatch batch)
        {
            if (color == 2)
            {
                batch.Draw(bar, new Rectangle(((Game1)Game).graphics.PreferredBackBufferWidth / 8, ((((Game1)Game).graphics.PreferredBackBufferHeight * 15) / 16),
                (int)100, 25), new Rectangle(0, 45, (bar.Width / 4), 25), Color.Gray);
                // draw the current cd level based on the current cooldown of shots
                batch.Draw(bar, new Rectangle((((Game1)Game).graphics.PreferredBackBufferWidth / 8), ((((Game1)Game).graphics.PreferredBackBufferHeight * 15) / 16),
                    (int)cd, 25), new Rectangle(0, 45, (bar.Width / 4), 25), Color.Yellow);

                batch.DrawString(CDText, "COOLDOWN", new Vector2(((Game1)Game).graphics.PreferredBackBufferWidth / 8, ((Game1)Game).graphics.PreferredBackBufferHeight * 17 / 18), Color.Black);
            }

            if (color == 1)
            {
                batch.Draw(bar, new Rectangle((((Game1)Game).graphics.PreferredBackBufferWidth / 8) + offset, ((((Game1)Game).graphics.PreferredBackBufferHeight * 15) / 16),
                (int)100, 25), new Rectangle(0, 45, (bar.Width / 4), 25), Color.Gray);
                // draw the current cd level based on the current cooldown of shots
                batch.Draw(bar, new Rectangle((((Game1)Game).graphics.PreferredBackBufferWidth / 8) + offset, ((((Game1)Game).graphics.PreferredBackBufferHeight * 15) / 16),
                    (int)cd, 25), new Rectangle(0, 45, (bar.Width / 4), 25), Color.Red);

                batch.DrawString(CDText, "COOLDOWN", new Vector2((((Game1)Game).graphics.PreferredBackBufferWidth / 8) + offset, ((Game1)Game).graphics.PreferredBackBufferHeight * 17 / 18), Color.Black);
            }

            if (color == 3)
            {
                batch.Draw(bar, new Rectangle((((Game1)Game).graphics.PreferredBackBufferWidth / 8) + offset * 2, ((((Game1)Game).graphics.PreferredBackBufferHeight * 15) / 16),
                (int)100, 25), new Rectangle(0, 45, (bar.Width / 4), 25), Color.Gray);
                // draw the current cd level based on the current cooldown of shots
                batch.Draw(bar, new Rectangle((((Game1)Game).graphics.PreferredBackBufferWidth / 8) + offset * 2, ((((Game1)Game).graphics.PreferredBackBufferHeight * 15) / 16),
                    (int)cd, 25), new Rectangle(0, 45, (bar.Width / 4), 25), Color.Green);

                batch.DrawString(CDText, "COOLDOWN", new Vector2((((Game1)Game).graphics.PreferredBackBufferWidth / 8) + offset * 2, ((Game1)Game).graphics.PreferredBackBufferHeight * 17 / 18), Color.Black);
            }

            if (color == 0)
            {
                batch.Draw(bar, new Rectangle((((Game1)Game).graphics.PreferredBackBufferWidth / 8) + offset * 3, ((((Game1)Game).graphics.PreferredBackBufferHeight * 15) / 16),
                (int)100, 25), new Rectangle(0, 45, (bar.Width / 4), 25), Color.Gray);
                // draw the current cd level based on the current cooldown of shots
                batch.Draw(bar, new Rectangle((((Game1)Game).graphics.PreferredBackBufferWidth / 8) + offset * 3, ((((Game1)Game).graphics.PreferredBackBufferHeight * 15) / 16),
                    (int)cd, 25), new Rectangle(0, 45, (bar.Width / 4), 25), Color.Blue);

                batch.DrawString(CDText, "COOLDOWN", new Vector2((((Game1)Game).graphics.PreferredBackBufferWidth / 8) + offset * 3, ((Game1)Game).graphics.PreferredBackBufferHeight * 17 / 18), Color.Black);
            }
        }
    }
}
