using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SuperTank
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D tankTex;
        Texture2D reticle;
        Tank t1;
        float timer = 0f;
        float interval = 100f;
        float step = 1f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            reticle = Content.Load<Texture2D>("reticle");
            tankTex = Content.Load<Texture2D>("tanksheet4");
            t1 = new Tank(tankTex, new Vector2(graphics.PreferredBackBufferWidth/2, graphics.PreferredBackBufferHeight/2), 0);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            KeyboardState ks = Keyboard.GetState();
            Keys[] keys = ks.GetPressedKeys();

            foreach (Keys key in keys)
            {
                switch (key)
                {
                    case Keys.W:
                        timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (timer > interval)
                        {
                            //Show the next frame
                            t1.CurrentFrame++;
                            t1.updateRectangles();
                            //Reset the timer
                            timer = 0f;
                        }

                        if (t1.CurrentFrame == 2) 
                        {
                            t1.CurrentFrame = 0;
                            t1.updateRectangles();
                        }

                        t1.Position += new Vector2((float)Math.Cos((t1.ChassisAngle)) * step, (float)Math.Sin((t1.ChassisAngle)) * step);
                        break;

                    case Keys.S:
                        timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (timer > interval)
                        {
                            //Show the next frame
                            t1.CurrentFrame++;
                            t1.updateRectangles();
                            //Reset the timer
                            timer = 0f;
                        }

                        if (t1.CurrentFrame == 2)
                        {
                            t1.CurrentFrame = 0;
                            t1.updateRectangles();
                        }
                       
                        break;

                    case Keys.A:
                        t1.ChassisAngle -= 0.05f;
                        break;

                    case Keys.D:
                        t1.ChassisAngle += 0.05f;
                        break;
                }
            }

            Vector2 aim = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - new Vector2(t1.Position.X, t1.Position.Y);
            if (Mouse.GetState().X > t1.Position.X && Mouse.GetState().Y > t1.Position.Y)
            {
                t1.CannonAngle = (float)Math.Atan2(aim.Y , aim.X);
            }
            else
            {
                t1.CannonAngle = (float)Math.Atan2(aim.Y , aim.X);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            t1.Draw(spriteBatch);
            spriteBatch.Draw(reticle, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), new Rectangle(9*0, 9*0, 9, 9), Color.White, 0f, new Vector2(9/2, 9/2), 1.0f, SpriteEffects.None, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}