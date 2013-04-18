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

        // win screen background
        private Texture2D winFool;

        // tutorial textures
        private Texture2D wizard;
        private Texture2D speechBubble;
        private Texture2D keyboardKeys;

        // title
        private Texture2D title;

        SpriteFont pericles20;
        SpriteFont pericles16;

        private Texture2D hat1;
        private Texture2D hat2;
        private Texture2D hat3;

        private CoolDownBar cdBar;
        private CoolDownBar cdBar1;
        private CoolDownBar cdBar2;
        private CoolDownBar cdBar3;

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
        Texture2D frame;

        Map m1;
        Tank t1;

        Tank t1Life;

        List<EnemyTanks> ets = new List<EnemyTanks>();
        List<EnemyTanks> etLives = new List<EnemyTanks>();

        int livesOffset;

        float playerTimer = 0f;
        float timer0 = 0f;
        float timer1 = 0f;
        float timer2 = 0f;
        float interval = 100f;
        float playerFireCount = 100f;
        float fireCount0 = 100f;
        float fireCount1 = 100f;
        float fireCount2 = 100f;
        bool canPlayerFire = true;
        bool canFire0 = true;
        bool canFire1 = true;
        bool canFire2 = true;
        float step = 1f;
        BulletManager bm;
        bool resetTanks = false;

        List<ParticleSystem> explosions;
        List<Texture2D> images;

        KeyboardState ks;
        KeyboardState lastKs;
        MouseState ms;
        MouseState lastMs;

        private Song titleMusic;
        private Song gameMusic;

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
            frame = Content.Load<Texture2D>("stwhudframe");

            wizard = Content.Load<Texture2D>("wizard");
            speechBubble = Content.Load<Texture2D>("bubble");
            keyboardKeys = Content.Load<Texture2D>("keyboardcontrols");

            pericles20 = Content.Load<SpriteFont>("Pericles20");
            pericles16 = Content.Load<SpriteFont>("Pericles16");

            GameOver = Content.Load<Texture2D>("gameOver");

            winFool = Content.Load<Texture2D>("win");

            Tank.texture2 = bullet;

            livesOffset = 300;

            m1 = new Map(tiles);
            t1 = new Tank(tankTex, new Vector2(80, 60), 3);
            t1Life = new Tank(tankTex, new Vector2(graphics.PreferredBackBufferWidth / 20, (21 * graphics.PreferredBackBufferHeight / 22)), 3);

            ets.Add(new EnemyTanks(tankTex, new Vector2(80, 600), 1));
            ets.Add(new EnemyTanks(tankTex, new Vector2(1180, 600), 2));
            ets[1].ChassisAngle = -90;
            ets[1].CannonAngle = -90;
            ets.Add(new EnemyTanks(tankTex, new Vector2(1180, 60), 0));
            ets[2].ChassisAngle = -180;
            ets[2].CannonAngle = -180;

            etLives.Add(new EnemyTanks(tankTex, new Vector2((graphics.PreferredBackBufferWidth / 20) + livesOffset, (21 * graphics.PreferredBackBufferHeight / 22)), 1));
            etLives.Add(new EnemyTanks(tankTex, new Vector2((graphics.PreferredBackBufferWidth / 20) + livesOffset * 2, (21 * graphics.PreferredBackBufferHeight / 22)), 2));
            etLives.Add(new EnemyTanks(tankTex, new Vector2((graphics.PreferredBackBufferWidth / 20) + livesOffset * 3, (21 * graphics.PreferredBackBufferHeight / 22)), 0));

            bm = new BulletManager(bullet);

            cdBar = new CoolDownBar(this, 2);
            cdBar1 = new CoolDownBar(this, 1);
            cdBar2 = new CoolDownBar(this, 3);
            cdBar3 = new CoolDownBar(this, 0);

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

            images = new List<Texture2D>();
            images.Add(Content.Load<Texture2D>("circle"));
            images.Add(Content.Load<Texture2D>("star"));
            images.Add(Content.Load<Texture2D>("diamond"));

            explosions = new List<ParticleSystem>();
            explosions.Add(new ParticleSystem(images, Vector2.Zero, 1, 0));
            explosions.Add(new ParticleSystem(images, Vector2.Zero, 3, 0));
            explosions.Add(new ParticleSystem(images, Vector2.Zero, 2, 0));

            titleMusic = Content.Load<Song>("STW title");
            gameMusic = Content.Load<Song>("STW Battle");
            MediaPlayer.Play(titleMusic);
            MediaPlayer.IsRepeating = true;
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
                    {
                        currentGameState = GameState.Tutorial;
                        MediaPlayer.Stop();
                        MediaPlayer.Play(gameMusic);
                        MediaPlayer.IsRepeating = true;
                    }
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
                    if (resetTanks)
                    {
                        m1 = new Map(tiles);
                        t1 = new Tank(tankTex, new Vector2(80, 60), 3);
                        ets.Clear();
                        ets.Add(new EnemyTanks(tankTex, new Vector2(80, 600), 1));
                        ets.Add(new EnemyTanks(tankTex, new Vector2(1180, 600), 2));
                        ets[1].ChassisAngle = -90;
                        ets[1].CannonAngle = -90;
                        ets.Add(new EnemyTanks(tankTex, new Vector2(1180, 60), 0));
                        ets[2].ChassisAngle = -180;
                        ets[2].CannonAngle = -180;

                        resetTanks = false;
                    }


                    for (int i = 0; i < 3; i++)
                    {
                        explosions[i].Update();
                        explosions[i].EmitterLocation = ets[i].Position;
                    }

                    if (t1.livesRemaining <= 0)
                    {
                        currentGameState = GameState.Lose;
                    }

                    timer0 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    timer1 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    timer2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    for (int i = 0; i < 3; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                if (timer0 > interval && ets[i].LivesRemaining > 0)
                                {
                                    if (!ets[i].Still)
                                    {
                                        ets[i].CurrentFrame++;
                                        ets[i].updateRectangles();
                                        timer0 = 0f;
                                    }
                                }
                                if (ets[i].CurrentFrame == 2 && ets[i].LivesRemaining > 0)
                                {
                                    if (!ets[i].Still)
                                    {
                                        ets[i].CurrentFrame = 0;
                                        ets[i].updateRectangles();
                                    }
                                }
                                break;
                            case 1:
                                if (timer1 > interval && ets[i].LivesRemaining > 0)
                                {
                                    if (!ets[i].Still)
                                    {
                                        ets[i].CurrentFrame++;
                                        ets[i].updateRectangles();
                                        timer1 = 0f;
                                    }
                                }
                                if (ets[i].CurrentFrame == 2 && ets[i].LivesRemaining > 0)
                                {
                                    if (!ets[i].Still)
                                    {
                                        ets[i].CurrentFrame = 0;
                                        ets[i].updateRectangles();
                                    }
                                }
                                break;
                            case 2:
                                if (timer2 > interval && ets[i].LivesRemaining > 0)
                                {
                                    if (!ets[i].Still)
                                    {
                                        ets[i].CurrentFrame++;
                                        ets[i].updateRectangles();
                                        timer2 = 0f;
                                    }
                                }
                                if (ets[i].CurrentFrame == 2 && ets[i].LivesRemaining > 0)
                                {
                                    if (!ets[i].Still)
                                    {
                                        ets[i].CurrentFrame = 0;
                                        ets[i].updateRectangles();
                                    }
                                }
                                break;

                            default:
                                break;

                        }

                        bool willShoot = ets[i].scan(t1, ets);
                        if (willShoot)
                        {
                            

                            switch (i)
                            {
                                case 0:
                                    if (canFire0)
                                    {
                                        bm.createBullet(ets[i].TankColor, ets[i].Position, new Vector2(ets[i].Target.Position.X, ets[i].Target.Position.Y));
                                        canFire0 = false;
                                        cdBar1.cd = 0;
                                    }

                                    ets[i].FoundTarget = false;
                                    break;
                                case 1:
                                    if (canFire1)
                                    {
                                        bm.createBullet(ets[i].TankColor, ets[i].Position, new Vector2(ets[i].Target.Position.X, ets[i].Target.Position.Y));
                                        canFire1 = false;
                                        cdBar2.cd = 0;
                                    }
                                    

                                    ets[i].FoundTarget = false;
                                    break;
                                case 2:
                                    if (canFire2)
                                    {
                                        bm.createBullet(ets[i].TankColor, ets[i].Position, new Vector2(ets[i].Target.Position.X, ets[i].Target.Position.Y));
                                        canFire2 = false;
                                        cdBar3.cd = 0;
                                    }

                                    ets[i].FoundTarget = false;
                                    break;

                                default:
                                    break;
                            }

                        }
                        if (ets[i].Target != null && ets[i].Target == t1)
                        {
                            ets[i].Seek(m1, t1, ets);
                        }
                        else
                        {
                            ets[i].Wander(m1, t1, ets);
                            }
                    }

                    IsMouseVisible = false;
                        if (lastMs.LeftButton == ButtonState.Released && ms.LeftButton == ButtonState.Pressed)
                        {
                            if (canPlayerFire)
                            {
                                bm.createBullet(t1.TankColor, t1.Position, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
                                canPlayerFire = false;
                                cdBar.cd = 0;
                            }
                        }

                        if (cdBar.cd == 0)
                        {
                            while (cdBar.cd < playerFireCount)
                                cdBar.cd++;
                        }
                        else if (cdBar.cd >= playerFireCount)
                        {
                            cdBar.cd = playerFireCount;
                        }

                        if (cdBar1.cd == 0)
                        {
                            while (cdBar1.cd < fireCount0)
                                cdBar1.cd++;
                        }
                        else if (cdBar1.cd >= fireCount0)
                        {
                            cdBar1.cd = fireCount0;
                        }

                        if (cdBar2.cd == 0)
                        {
                            while (cdBar2.cd < fireCount1)
                                cdBar2.cd++;
                        }
                        else if (cdBar2.cd >= fireCount1)
                        {
                            cdBar2.cd = fireCount1;
                        }

                        if (cdBar3.cd == 0)
                        {
                            while (cdBar3.cd < fireCount2)
                                cdBar3.cd++;
                        }
                        else if (cdBar3.cd >= fireCount2)
                        {
                            cdBar3.cd = fireCount2;
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
                                        if ((t1.Position - et.Position).Length() <= 65)
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
                                        if ((t1.Position - et.Position).Length() <= 65)
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
                                        Tank.DrawBSPheres = !Tank.DrawBSPheres;
                                    }
                                    break;

                                case Keys.U:
                                    if (lastKs.IsKeyUp(Keys.U))
                                    {
                                        resetTanks = true;
                                    }
                                    break;
                            }

                        }

                        Vector2 playerAim = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - new Vector2(t1.Position.X, t1.Position.Y);
                        if (Mouse.GetState().X > t1.Position.X && Mouse.GetState().Y > t1.Position.Y)
                        {
                            t1.CannonAngle = (float)Math.Atan2(playerAim.Y, playerAim.X);
                        }
                        else
                        {
                            t1.CannonAngle = (float)Math.Atan2(playerAim.Y, playerAim.X);
                        }

                        if (playerFireCount > 0 && !canPlayerFire)
                        {
                            playerFireCount--;
                        }
                        else
                        {
                            canPlayerFire = true;
                            playerFireCount = 100;
                        }

                        if (fireCount0 > 0 && !canFire0)
                        {
                            fireCount0--;
                            ets[0].Target = null;
                        }
                        else
                        {
                            canFire0 = true;
                            fireCount0 = 100;
                        }

                        if (fireCount1 > 0 && !canFire1)
                        {
                            fireCount1--;
                            ets[1].Target = null;
                        }
                        else
                        {
                            canFire1 = true;
                            fireCount1 = 100;
                        }

                        if (fireCount2 > 0 && !canFire2)
                        {
                            fireCount2--;
                            ets[2].Target = null;
                        }
                        else
                        {
                            canFire2 = true;
                            fireCount2 = 100;
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
                case GameState.Win:
                    m1.Draw(spriteBatch);
                    t1.Draw(spriteBatch);
                    for (int i = 0; i < 3; i++)
                    {
                        if (ets[i].LivesRemaining > 0)
                        {
                            ets[i].Draw(spriteBatch);
                        }

                        else
                        {
                            ets[i].DrawDead(spriteBatch);
                            explosions[i].Draw(spriteBatch);
                        }
                    }
                    spriteBatch.Draw(winFool, new Rectangle((graphics.PreferredBackBufferWidth / 2) - winFool.Width / 2, (graphics.PreferredBackBufferHeight / 2) - winFool.Height / 2, winFool.Width, winFool.Height), Color.White);
                    spriteBatch.DrawString(
                        pericles20,
                        " You destroyed these nincompoops!!! ",
                        new Vector2((graphics.PreferredBackBufferWidth / 2) - graphics.PreferredBackBufferWidth / 4, (graphics.PreferredBackBufferHeight / 2) + graphics.PreferredBackBufferHeight / 4),
                        Color.White);
                    break;
                case GameState.Play:
                    m1.Draw(spriteBatch);
                    t1.Draw(spriteBatch);
                    for (int i = 0; i <3; i++)
                    {
                        if (ets[i].LivesRemaining > 0)
                        {
                            ets[i].Draw(spriteBatch);
                        }

                        else
                        {
                            ets[i].DrawDead(spriteBatch);
                            explosions[i].Draw(spriteBatch);
                        }
                    }

                    spriteBatch.Draw(reticle, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), new Rectangle(0, 0, 9, 9), Color.White, 0f, new Vector2(9 / 2, 9 / 2), 1.0f, SpriteEffects.None, 0);

                    spriteBatch.Draw(frame, new Vector2(0,720), new Rectangle(0, 0, frame.Width, frame.Height), Color.White, 0f, new Vector2(0,0), 1.0f, SpriteEffects.None, 0);

                    int k = 1;

                    for (int i = 0; i < 3; i++)
                    {
                        etLives[i].Draw(spriteBatch);
                            spriteBatch.DrawString(
                                pericles20,
                                " X " + ets[i].LivesRemaining.ToString(),
                                new Vector2((graphics.PreferredBackBufferWidth / 30) + 50 + 300 * k, (14 * graphics.PreferredBackBufferHeight / 15)),
                                Color.White);
                            k++;
                    }

                    if (ets[0].LivesRemaining == 0 && ets[1].LivesRemaining == 0 && ets[2].LivesRemaining == 0)
                    {
                        currentGameState = GameState.Win;
                    }

                    foreach (Bullet b in bm.Bullets)
                    {
                        if (b.LifeTime <91/*< 521*/)
                        {
                            b.Draw(spriteBatch);
                        }
                    }

                    
                    spriteBatch.DrawString(
                        pericles20,
                        " X " + t1.livesRemaining.ToString(),
                        new Vector2((graphics.PreferredBackBufferWidth / 30) + 50, (14 * graphics.PreferredBackBufferHeight / 15)),
                        Color.White);
                    t1Life.Draw(spriteBatch);
                    cdBar.draw(spriteBatch);
                    if(ets[0].LivesRemaining > 0)
                        cdBar1.draw(spriteBatch);
                    if (ets[1].LivesRemaining > 0)
                        cdBar2.draw(spriteBatch);
                    if (ets[2].LivesRemaining > 0)
                        cdBar3.draw(spriteBatch);
                    break;
            }
            
            spriteBatch.End();                   

            base.Draw(gameTime);
        }
    }
}