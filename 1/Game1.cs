using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace _1;

#region STATES
public enum GameState { MainMenu, Playing }
#endregion

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Matrix cam_matrix; 
    private SpriteFont game_font;
    private int score_counter = 0; 
    private Texture2D ground_img, plat_img, cactus_img; 
    private Player hero; 
    private Song music_loop;
    private SoundEffect jump_sfx, atk_sfx;
    
    private bool victory = false;
    private bool died = false;
    private const float SCALE_VAL = 0.1f;
    private Menu start_menu;
    private GameState current_state = GameState.MainMenu;

    private List<Vector2> plats = new List<Vector2>();
    private List<Vector2> traps = new List<Vector2>();
    private float p_scale = 4.0f;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        IsMouseVisible = true;
    }

    protected override void Initialize() { base.Initialize(); }

    #region CONTENT_LOAD
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        
        ground_img = Content.Load<Texture2D>("images/blok");
        plat_img = Content.Load<Texture2D>("plat");
        cactus_img = Content.Load<Texture2D>("images/Cactus");
        game_font = Content.Load<SpriteFont>("fonts/04B_30");
        music_loop = Content.Load<Song>("Audio/Ses");
        jump_sfx = Content.Load<SoundEffect>("Audio/zipla");
        atk_sfx = Content.Load<SoundEffect>("Audio/kilic");

        Texture2D t1 = Content.Load<Texture2D>("images/1");
       
        var w_list = new List<Texture2D> { Content.Load<Texture2D>("images/13"),
                    Content.Load<Texture2D> ("images/14"), Content.Load<Texture2D>("images/17"), 
         Content.Load<Texture2D>("images/18"), Content.Load<Texture2D>("images/19") };
        var j_list = new List<Texture2D> { Content.Load<Texture2D>("images/31"), 
        Content.Load<Texture2D>("images/32"), Content.Load<Texture2D>("images/33"),
         Content.Load<Texture2D>("images/34") };
        var a_list = new List<Texture2D> { Content.Load<Texture2D>("images/71"), 
        Content.Load<Texture2D>("images/65"),       Content.Load<Texture2D>("images/72"), 
        Content.Load<Texture2D>("images/73"),   Content.Load<Texture2D>("images/74"),
             Content.Load<Texture2D>("images/75") };

        hero = new Player(t1, w_list, j_list, a_list, jump_sfx, atk_sfx, 
        new Vector2(100, 630));
        start_menu = new Menu(game_font, GraphicsDevice.Viewport);

        for (int i = 1; i 
        < 100; i++)
        {
            plats.Add(new Vector2(i * 450, 420));
            if (i % 3 == 0) traps.Add(new Vector2(i * 450 + 200, 420));
        }
        
       
        traps.Add(new Vector2(800, 635 - (cactus_img.Height * 2)));
        traps.Add(new Vector2(1600, 635 - (cactus_img.Height * 2)));

        MediaPlayer.Play(music_loop);
        MediaPlayer.IsRepeating = true;
    }
    #endregion

    #region GAME_UPDATE
    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        if (current_state == GameState.Playing)
        {
            if (victory || died) return;

            float floor_y = 800; 

            foreach (var p in plats)
            {
                float p_w = plat_img.Width * p_scale;
               
                if (hero.Position.X > (p.X - 20) && hero.Position.X < (p.X + p_w + 20))
                {
                    if (hero.Velocity.Y >= 0 && hero.Position.Y <= p.Y + 200 && hero.Position.Y > p.Y - 50)
                    {
                        floor_y = p.Y + 165;

                        break;
                    }
                }
            }

            foreach (var t in traps)
            {
                
                Rectangle p_rect = new Rectangle((int)hero.Position.X - 10, (int)hero.Position.Y - 10, 20, 20);
                int off_val = (t.Y == 420) ? 165 : 165;
                Rectangle t_rect = new Rectangle((int)t.X + 5, (int)t.Y + off_val, (int)(cactus_img.Width * 2) - 10, (int)(cactus_img.Height * 2));

                if (p_rect.Intersects(t_rect))
                {
                    died = true;
                    MediaPlayer.Stop();
                }
            }

            hero.Update(gameTime, floor_y);

            if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.A)) 
                score_counter++;

            if (score_counter >= 1000)
            {
                victory = true;
                MediaPlayer.Stop();
            }

            cam_matrix = Matrix.CreateTranslation(-hero.Position.X + 640, 0, 0);
        }
        else 
        {
            if (start_menu.Update() == 0) current_state = GameState.Playing;
        }

        base.Update(gameTime);
    }
    #endregion


    #region  Ali için not(unutursan bak)

    //Draw kısmı burada Rendering zone olarak ayırdım. Update kısmı gameupdate olarak ayırdım.
    //oyunun kontrolleri zaten wasd ve mouse sol tık 
    //Menu de başlamak için enter bas , w ve s tuşuyla menüde hareket edebilirsin.
    //iyi oyunlar :)
    //kendim için de açıklama satırı açıyorum. Menu.cs içine oradan yazabilirsin.

    #endregion




    #region RENDERING_ZONE
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        if (current_state == GameState.Playing)
        {
            _spriteBatch.Begin(transformMatrix: cam_matrix);

            float tw = ground_img.Width * SCALE_VAL;
            float th = ground_img.Height * SCALE_VAL;

            
            for (float x = hero.Position.X - 1550; x < hero.Position.X + 2550; x += tw)
            {
                for (int r = 0; r < 5; r++)
                {
                    _spriteBatch.Draw(ground_img, new Vector2(x, 635 + (r * th)), 
                    null, Color.White, 0f, Vector2.Zero, SCALE_VAL, SpriteEffects.None, 0f);
                }
            }

            foreach (var p in plats) _spriteBatch.Draw(plat_img, p, null, Color.White,
             0f, Vector2.Zero, p_scale, SpriteEffects.None, 0f);
            foreach (var t in traps) _spriteBatch.Draw(cactus_img, t, null, Color.White, 
            0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0f);

            hero.Draw(_spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin();
            _spriteBatch.DrawString(game_font, "Score: " + 
            score_counter, new Vector2(25, 25), Color.White);

            if (victory) _spriteBatch.DrawString(game_font, "WIN!",
             new Vector2(550, 310), Color.Gold);
            if (died) _spriteBatch.DrawString(game_font, "GAME OVER!",
             new Vector2(510, 310), Color.Red);

            _spriteBatch.End();
        }
        else
        {
            _spriteBatch.Begin();
            start_menu.Draw(_spriteBatch);
            _spriteBatch.End();
        }
        base.Draw(gameTime);
    }
    #endregion
}

#region NOTE FOR THE PROFFESOR
//I had some problems with the platforms and traps (mostly with platforms)
//You can play the game with WASD, jump with W, attack with mouse left button
// The score increases when you move left or right, and the game ends when you reach 1000 points.
// You can die by touching the traps (cacti) or falling down from the platforms. The game starts with a menu, 
//just press Enter to start.
// The game and player file is so big and untidy because I had to make a lot of adjustments and 
//changes to the player and game mechanics, 
//so I didn't have time to refactor the code and make it cleaner. I hope you understand.

//When we reach the score 1000 we win

#endregion