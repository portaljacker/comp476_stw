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
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Tutorial ctrlsTutorial;

        // Buttons
        private Button[] btnArray = new Button[3];

        private Button btnPlay;
        private Button btnControls;
        private Button btnBack;
        private Button btnExit;

        // main menu background
        private Texture2D menuBackground;

        // controls screen background
        private Texture2D controlsBackground;

        // lose screen background
        private Texture2D GameOver;

        // tutorial textures
        private Texture2D wizard;
        private Texture2D speechBubble;
        private Texture2D keyboardKeys;

        // title
        private Texture2D title;

        private Texture2D life;

        SpriteFont pericles20;
        SpriteFont pericles16;

        private Vector2 livesLocation;

        private Texture2D hat1;
        private Texture2D hat2;
        private Texture2D hat3;

        // enum for the different states of the game
        enum GameState
        {
            MainMenu,
            Tutorial,
            Play,
            Controls,
            Win,
            Lose,
            Exit,
        }

        // enum for the different parts of the tutorial
        enum TutorialState
        {
            Tutorial1,
            Tutorial2,
            Tutorial3,
            Tutorial4,
            Tutorial5,
            Tutorial6,
        }

        GameState currentGameState = GameState.MainMenu;
        TutorialState currentTutorialState = TutorialState.Tutorial1;

        Texture2D tankTex;
        Texture2D reticle;
        Texture2D bullet;
        Texture2D tiles;

        Map m1;
        Tank t1;
        //EnemyTanks et1;
        List<EnemyTanks> ets = new List<EnemyTanks>();

        float playerTimer = 0f;
        float timer = 0f;
        float interval = 100f;
        float fireCount = 100f;
        bool canFire = true;
        float step = 1f;
        BulletManager bm;

        KeyboardState ks;
        KeyboardState lastKs;
        MouseState ms;
        MouseState lastMs;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 780;
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

            spriteBatch = new SpriteBatch(this.graphics.GraphicsDevice);

            btnArray[0] = btnPlay;
            btnArray[1] = btnControls;
            btnArray[2] = btnExit;
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
            bullet = Content.Load<Texture2D>("bullet");
            tiles = Content.Load<Texture2D>("tiles");

            wizard = Content.Load<Texture2D>("wizard");
            speechBubble = Content.Load<Texture2D>("bubble");
            keyboardKeys = Content.Load<Texture2D>("keyboardcontrols");

            pericles20 = Content.Load<SpriteFont>("Pericles20");
            pericles16 = Content.Load<SpriteFont>("Pericles16");

            GameOver = Content.Load<Texture2D>("gameOver");

            int offset = 50;

            livesLocation = new Vector2((graphics.PreferredBackBufferWidth / 30) + offset, (14 * graphics.PreferredBackBufferHeight / 15));

            life = Content.Load<Texture2D>("life");

            Tank.texture2 = bullet;

            m1 = new Map(tiles);
            t1 = new Tank(tankTex, new Vector2(graphics.PreferredBackBufferWidth/2, graphics.PreferredBackBufferHeight/2), 0);
            ets.Add(new EnemyTanks(tankTex, new Vector2((graphics.PreferredBackBufferWidth / 5), (graphics.PreferredBackBufferHeight - graphics.PreferredBackBufferHeight / 5)), 1));



            bm = new BulletManager(bullet);

            //display the mouse
            IsMouseVisible = true;

            Viewport viewPort = new Viewport();
            viewPort.X = 0;
            viewPort.Y = 0;
            viewPort.Width = graphics.PreferredBackBufferWidth;
            viewPort.Height = graphics.PreferredBackBufferHeight + graphics.PreferredBackBufferHeight / 20;

            ctrlsTutorial = new Tutorial(this, Content, viewPort, this.graphics.GraphicsDevice);

            ctrlsTutorial.AKeyText = "Move Left";
            ctrlsTutorial.WKeyText = "Move Up";
            ctrlsTutorial.SKeyText = "Move Down";
            ctrlsTutorial.DKeyText = "Move Right";

            ctrlsTutorial.LeftMouseClickText = "FIRE!";
            ctrlsTutorial.LeftMouseClickText = "FIRE!";

            // title
            title = Content.Load<Texture2D>("title");

            // menu items
            btnPlay = new Button(Content.Load<Texture2D>("btnPlay"), graphics.GraphicsDevice);
            btnPlay.SetPosition(new Vector2((graphics.PreferredBackBufferWidth / 2) + graphics.PreferredBackBufferWidth / 12, (graphics.PreferredBackBufferHeight / 3)));
            btnControls = new Button(Content.Load<Texture2D>("btnControls"), graphics.GraphicsDevice);
            btnControls.SetPosition(new Vector2((int)(graphics.PreferredBackBufferWidth / 2) + graphics.PreferredBackBufferWidth / 12, (int)(graphics.PreferredBackBufferHeight / 3) + (int)(graphics.PreferredBackBufferHeight / 6)));
            btnExit = new Button(Content.Load<Texture2D>("btnExit"), graphics.GraphicsDevice);
            btnExit.SetPosition(new Vector2((int)(graphics.PreferredBackBufferWidth / 2) + graphics.PreferredBackBufferWidth / 12, (int)(graphics.PreferredBackBufferHeight / 3) + (int)(graphics.PreferredBackBufferHeight / 3)));

            // controls screen button
            btnBack = new Button(Content.Load<Texture2D>("btnBack"), graphics.GraphicsDevice);
            btnBack.SetPosition(new Vector2(graphics.PreferredBackBufferWidth / 24, graphics.PreferredBackBufferHeight - ((btnBack.size.Y) + graphics.PreferredBackBufferHeight / 24)));

            menuBackground = Content.Load<Texture2D>("castleMenuBack");
            controlsBackground = Content.Load<Texture2D>("tankSchem");

            hat1 = Content.Load<Texture2D>("hat1");
            hat2 = Content.Load<Texture2D>("hat2");
            hat3 = Content.Load<Texture2D>("hat3");
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

            lastKs = ks;
            ks = Keyboard.GetState();
            Keys[] keys = ks.GetPressedKeys();

            lastMs = ms;
            ms = Mouse.GetState();

            switch (currentGameState)
            {
                case GameState.Tutorial:
                    if (ks.IsKeyDown(Keys.Space) && lastKs.IsKeyUp(Keys.Space))
                    {
                        switch (currentTutorialState)
                        {
                            case TutorialState.Tutorial1:
                                currentTutorialState = TutorialState.Tutorial2;
                                break;
                            case TutorialState.Tutorial2:
                                currentTutorialState = TutorialState.Tutorial3;
                                break;
                            case TutorialState.Tutorial3:
                                currentTutorialState = TutorialState.Tutorial4;
                                break;
                            case TutorialState.Tutorial4:
                                currentTutorialState = TutorialState.Tutorial5;
                                break;
                            case TutorialState.Tutorial5:
                                currentTutorialState = TutorialState.Tutorial6;
                                break;
                            case TutorialState.Tutorial6:
                                currentGameState = GameState.Play;
                                break;
                        }
                    }
                    break;
                case GameState.MainMenu:
                    if (btnArray[0].isClicked == true)
                        currentGameState = GameState.Tutorial;
                    else if (btnArray[1].isClicked == true)
                    {
                        currentGameState = GameState.Controls;
                        if (btnBack.isClicked == true)
                            btnBack.isClicked = false;
                    }
                    else if (btnArray[2].isClicked == true)
                        this.Exit();
                    btnArray[0].Update(ms);
                    btnArray[1].Update(ms);
                    btnArray[2].Update(ms);
                    break;
                case GameState.Controls:
                    if (btnBack.isClicked == true)
                    {
                        currentGameState = GameState.MainMenu;
                        btnArray[1].isClicked = false;
                    }
                    btnBack.Update(ms);
                    break;
                case GameState.Play:
                    timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    foreach (EnemyTanks et in ets)
                    {
                        if (timer > interval)
                        {
                            et.CurrentFrame++;
                            et.updateRectangles();
                            timer = 0f;
                        }
                        if (et.CurrentFrame == 2)
                        {
                            et.CurrentFrame = 0;
                            et.updateRectangles();
                        }

                        et.Wander(m1, t1, ets);
                    }

                    KeyboardState mKeys = Keyboard.GetState();
                    if (mKeys.IsKeyDown(Keys.Up) == true)
                    {
                        if (t1.LivesRemaining > 0)
                            t1.LivesRemaining -= 1;
                        else
                            currentGameState = GameState.Lose;
                    }

                    IsMouseVisible = false;
                        if (lastMs.LeftButton == ButtonState.Released && ms.LeftButton == ButtonState.Pressed)
                        {
                            if (canFire)
                            {
                                bm.createBullet(t1.TankColor, t1.Position, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
                                canFire = false;
                            }
                        }

                        foreach (Keys key in keys)
                        {
                            switch (key)
                            {
                                case Keys.W:
                                    playerTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                                    if (playerTimer > interval)
                                    {
                                        //Show the next frame
                                        t1.CurrentFrame++;
                                        t1.updateRectangles();
                                        //Reset the timer
                                        playerTimer = 0f;
                                    }

                                    if (t1.CurrentFrame == 2)
                                    {
                                        t1.CurrentFrame = 0;
                                        t1.updateRectangles();
                                    }

                                    t1.Position += new Vector2((float)Math.Cos((t1.ChassisAngle)) * step, (float)Math.Sin((t1.ChassisAngle)) * step);
                                    t1.updateSpheres();
                                    foreach(BoundingSphere b in m1.Walls)
                                    {
                                        for (int i = 0; i < t1.Spheres.Count; i++)
                                        {
                                            int x = (int)Math.Floor(t1.Spheres[i].Center.X / 20);
                                            int y = (int)Math.Floor(t1.Spheres[i].Center.Y / 20);

                                            if (t1.Spheres[i].Intersects(b))
                                            {
                                                t1.Position -= new Vector2((float)Math.Cos((t1.ChassisAngle)) * step, (float)Math.Sin((t1.ChassisAngle)) * step);
                                                t1.updateSpheres();
                                                break;
                                            }
                                        }
                                    }

                                    foreach (EnemyTanks et in ets)
                                    {
                                        if ((t1.Position - et.Position).Length() <= 60)
                                        {
                                            for (int i = 0; i < t1.Spheres.Count; i++)
                                            {
                                                for (int j = 0; j < et.Spheres.Count; j++)
                                                {
                                                    if (t1.Spheres[i].Intersects(et.Spheres[j]))
                                                    {
                                                        t1.Position -= new Vector2((float)Math.Cos((t1.ChassisAngle)) * step, (float)Math.Sin((t1.ChassisAngle)) * step);
                                                        t1.updateSpheres();
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case Keys.S:
                                    playerTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                                    if (playerTimer > interval)
                                    {
                                        //Show the next frame
                                        t1.CurrentFrame++;
                                        t1.updateRectangles();
                                        //Reset the timer
                                        playerTimer = 0f;
                                    }

                                    if (t1.CurrentFrame == 2)
                                    {
                                        t1.CurrentFrame = 0;
                                        t1.updateRectangles();
                                    }

                                    t1.Position -= new Vector2((float)Math.Cos((t1.ChassisAngle)) * step, (float)Math.Sin((t1.ChassisAngle)) * step);
                                    t1.updateSpheres();
                                    foreach (BoundingSphere b in m1.Walls)
                                    {
                                        for (int i = 0; i < t1.Spheres.Count; i++)
                                        {

                                            if (t1.Spheres[i].Intersects(b))
                                            {

                                                t1.Position += new Vector2((float)Math.Cos((t1.ChassisAngle)) * step, (float)Math.Sin((t1.ChassisAngle)) * step);
                                                t1.updateSpheres();
                                                break;
                                            }
                                        }
                                    }
                                    foreach (EnemyTanks et in ets)
                                    {
                                        if ((t1.Position - et.Position).Length() <= 60)
                                        {
                                            for (int i = 0; i < t1.Spheres.Count; i++)
                                            {
                                                for (int j = 0; j < et.Spheres.Count; j++)
                                                {
                                                    if (t1.Spheres[i].Intersects(et.Spheres[j]))
                                                    {
                                                        t1.Position += new Vector2((float)Math.Cos((t1.ChassisAngle)) * step, (float)Math.Sin((t1.ChassisAngle)) * step);
                                                        t1.updateSpheres();
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    break;

                                case Keys.A:
                                    t1.ChassisAngle -= 0.03f;
                                    t1.updateRectangles();
                                    t1.updateSpheres();
                                    foreach (BoundingSphere b in m1.Walls)
                                    {
                                        for (int i = 0; i < t1.Spheres.Count; i++)
                                        {
                                            if (t1.Spheres[i].Intersects(b))
                                            {
                                                t1.ChassisAngle += 0.03f;
                                                t1.updateRectangles();
                                                t1.updateSpheres();
                                                break;
                                            }
                                        }
                                    }
                                    break;

                                case Keys.D:
                                    t1.ChassisAngle += 0.03f;
                                    t1.updateRectangles();
                                    t1.updateSpheres();
                                    foreach (BoundingSphere b in m1.Walls)
                                    {
                                        for (int i = 0; i < t1.Spheres.Count; i++)
                                        {
                                            if (t1.Spheres[i].Intersects(b))
                                            {
                                                t1.ChassisAngle -= 0.03f;
                                                t1.updateRectangles();
                                                t1.updateSpheres();
                                                break;
                                            }
                                        }
                                    }
                                    break;

                                case Keys.Space:
                                    if (lastKs.IsKeyUp(Keys.Space))
                                    {
                                        if (canFire)
                                        {
                                            bm.createBullet(t1.TankColor, t1.Position, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
                                            canFire = false;
                                        }
                                    }
                                    break;
                            }

                        }

                        Vector2 aim = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - new Vector2(t1.Position.X, t1.Position.Y);
                        if (Mouse.GetState().X > t1.Position.X && Mouse.GetState().Y > t1.Position.Y)
                        {
                            t1.CannonAngle = (float)Math.Atan2(aim.Y, aim.X);
                        }
                        else
                        {
                            t1.CannonAngle = (float)Math.Atan2(aim.Y, aim.X);
                        }

                        if (fireCount > 0)
                        {
                            fireCount--;
                        }
                        else
                        {
                            canFire = true;
                            fireCount = 100;
                        }

                        bm.update(m1, t1, ets);
                        break;
                    }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(menuBackground, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.Draw(title, new Rectangle((graphics.PreferredBackBufferWidth / 10) - (graphics.PreferredBackBufferWidth / 15), (graphics.PreferredBackBufferHeight / 12) - graphics.PreferredBackBufferHeight / 15, (5 * graphics.PreferredBackBufferWidth / 9), (5 * graphics.PreferredBackBufferHeight / 9)), Color.White);
                    spriteBatch.Draw(hat1, new Rectangle((2 * graphics.PreferredBackBufferWidth / 3), graphics.PreferredBackBufferHeight / 15, graphics.PreferredBackBufferWidth / 12, graphics.PreferredBackBufferHeight / 10), null, Color.White, 1.2f, new Vector2(0, 0), SpriteEffects.None, 0);
                    spriteBatch.Draw(hat2, new Rectangle((graphics.PreferredBackBufferWidth / 10) - (graphics.PreferredBackBufferWidth / 18), (2 * graphics.PreferredBackBufferHeight / 3), graphics.PreferredBackBufferWidth / 12, graphics.PreferredBackBufferHeight / 10), null, Color.White, 4.0f, new Vector2(0, 0), SpriteEffects.None, 0);
                    spriteBatch.Draw(hat3, new Rectangle(graphics.PreferredBackBufferWidth / 2, (2 * graphics.PreferredBackBufferHeight / 3), graphics.PreferredBackBufferWidth / 12, graphics.PreferredBackBufferHeight / 10), null, Color.White, 2.8f, new Vector2(0, 0), SpriteEffects.None, 0);
                    btnArray[0].Draw(spriteBatch);
                    btnArray[1].Draw(spriteBatch);
                    btnArray[2].Draw(spriteBatch);
                    break;
                case GameState.Lose:
                    spriteBatch.Draw(GameOver, new Rectangle((graphics.PreferredBackBufferWidth / 2) - GameOver.Width / 2, (graphics.PreferredBackBufferHeight / 2) - GameOver.Height / 2, GameOver.Width, GameOver.Height), Color.White);
                    spriteBatch.DrawString(
                        pericles20,
                        " You got stomped by wizards... in tanks ",
                        new Vector2((graphics.PreferredBackBufferWidth / 2) - graphics.PreferredBackBufferWidth / 4, (graphics.PreferredBackBufferHeight / 2) + graphics.PreferredBackBufferHeight / 4),
                        Color.DarkRed);
                    break;
                case GameState.Controls:
                    spriteBatch.Draw(controlsBackground, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    ctrlsTutorial.Draw(spriteBatch);
                    btnBack.Draw(spriteBatch);
                    break;
                case GameState.Tutorial:
                    m1.Draw(spriteBatch);
                    //t1.Draw(spriteBatch);
                    //et1.Draw(spriteBatch);

                    spriteBatch.Draw(wizard, new Rectangle(graphics.PreferredBackBufferWidth / 8, graphics.PreferredBackBufferHeight / 4, (2 * wizard.Width / 3), (2 * wizard.Height / 3)), Color.White);
                    spriteBatch.Draw(speechBubble, new Rectangle(graphics.PreferredBackBufferWidth / 4, graphics.PreferredBackBufferHeight / 5, (3 * speechBubble.Width / 4), (3 * speechBubble.Height / 4)), Color.White);

                    if (currentTutorialState == TutorialState.Tutorial1)
                    {
                        spriteBatch.DrawString(pericles16, "Welcome to our arena, \nbattle wizard! In order to \nsucceed and join our ranks, \nyou will have to defeat other \npotential suitors.",
                           new Vector2((graphics.PreferredBackBufferWidth / 3) + (graphics.PreferredBackBufferWidth / 20), (graphics.PreferredBackBufferHeight / 3) + graphics.PreferredBackBufferHeight / 30), Color.Black);
                        spriteBatch.DrawString(pericles20, "Press SPACE to continue.",
                            new Vector2((graphics.PreferredBackBufferWidth / 3) + (graphics.PreferredBackBufferWidth / 15), (3 * graphics.PreferredBackBufferHeight / 4)), Color.White);
                    }

                    if (currentTutorialState == TutorialState.Tutorial2)
                    {
                        spriteBatch.DrawString(pericles16, "In order to defeat them, \nyou will need to use your \nskills, for only the most \nruthless tank wizard will \nemerge victorious.",
                           new Vector2((graphics.PreferredBackBufferWidth / 3) + (graphics.PreferredBackBufferWidth / 20), (graphics.PreferredBackBufferHeight / 3) + graphics.PreferredBackBufferHeight / 30), Color.Black);
                        spriteBatch.DrawString(pericles20, "Press SPACE to continue.",
                            new Vector2((graphics.PreferredBackBufferWidth / 3) + (graphics.PreferredBackBufferWidth / 15), (3 * graphics.PreferredBackBufferHeight / 4)), Color.White);
                    }

                    if (currentTutorialState == TutorialState.Tutorial3)
                    {
                        spriteBatch.DrawString(pericles16, "Use the mouse to rotate the \ntank turret by moving it in \nthe direction you desire.",
                           new Vector2((graphics.PreferredBackBufferWidth / 3) + (graphics.PreferredBackBufferWidth / 20), (graphics.PreferredBackBufferHeight / 3) + graphics.PreferredBackBufferHeight / 30), Color.Black);
                        spriteBatch.DrawString(pericles20, "Press SPACE to continue.",
                            new Vector2((graphics.PreferredBackBufferWidth / 3) + (graphics.PreferredBackBufferWidth / 15), (3 * graphics.PreferredBackBufferHeight / 4)), Color.White);
                    }

                    if (currentTutorialState == TutorialState.Tutorial4)
                    {
                        spriteBatch.DrawString(pericles16, "Click on the left mouse \nbutton to fire the cannon \nand blow your enemies to \nsmithereens!",
                           new Vector2((graphics.PreferredBackBufferWidth / 3) + (graphics.PreferredBackBufferWidth / 20), (graphics.PreferredBackBufferHeight / 3) + graphics.PreferredBackBufferHeight / 30), Color.Black);
                        spriteBatch.DrawString(pericles20, "Press SPACE to continue.",
                            new Vector2((graphics.PreferredBackBufferWidth / 3) + (graphics.PreferredBackBufferWidth / 15), (3 * graphics.PreferredBackBufferHeight / 4)), Color.White);
                    }

                    if (currentTutorialState == TutorialState.Tutorial5)
                    {
                        spriteBatch.DrawString(pericles16, "Finally, move around the \narena with ",
                           new Vector2((graphics.PreferredBackBufferWidth / 3) + (graphics.PreferredBackBufferWidth / 20), (graphics.PreferredBackBufferHeight / 3) + graphics.PreferredBackBufferHeight / 30), Color.Black);
                        spriteBatch.Draw(keyboardKeys, new Rectangle((graphics.PreferredBackBufferWidth / 2), (graphics.PreferredBackBufferHeight / 3) + graphics.PreferredBackBufferHeight / 20, keyboardKeys.Width / 4, keyboardKeys.Height / 4), Color.White);
                        spriteBatch.DrawString(pericles20, "Press SPACE to continue.",
                            new Vector2((graphics.PreferredBackBufferWidth / 3) + (graphics.PreferredBackBufferWidth / 15), (3 * graphics.PreferredBackBufferHeight / 4)), Color.White);
                    }

                    if (currentTutorialState == TutorialState.Tutorial6)
                    {
                        spriteBatch.DrawString(pericles16, "Good luck! And we implore \nyou; try to survive! Too many \nof our students have \nperished already!",
                           new Vector2((graphics.PreferredBackBufferWidth / 3) + (graphics.PreferredBackBufferWidth / 20), (graphics.PreferredBackBufferHeight / 3) + graphics.PreferredBackBufferHeight / 30), Color.Black);
                        spriteBatch.DrawString(pericles20, "Press SPACE to start.",
                            new Vector2((graphics.PreferredBackBufferWidth / 3) + (graphics.PreferredBackBufferWidth / 15), (3 * graphics.PreferredBackBufferHeight / 4)), Color.White);
                    }
                    break;
                case GameState.Play:
                    m1.Draw(spriteBatch);
                    t1.Draw(spriteBatch);
                    foreach (EnemyTanks et in ets)
                    {
                        et.Draw(spriteBatch);
                    }

                    foreach (Bullet b in bm.Bullets)
                    {
                        if (b.LifeTime < 521)
                        {
                            b.Draw(spriteBatch);
                        }
                    }

                    spriteBatch.Draw(reticle, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), new Rectangle(0, 0, 9, 9), Color.White, 0f, new Vector2(9 / 2, 9 / 2), 1.0f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(
                        pericles20,
                        " X " + t1.LivesRemaining.ToString(),
                        livesLocation,
                        Color.White);
                    spriteBatch.Draw(life, new Rectangle(graphics.PreferredBackBufferWidth / 30, (12 * graphics.PreferredBackBufferHeight / 13), life.Width, life.Height), Color.White);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}