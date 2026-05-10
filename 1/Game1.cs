using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace _1;

public enum GameState { MainMenu, Playing }

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Matrix _cameraTransform;
    private SpriteFont _font;
    private int _score = 0;
    private Texture2D _blockTexture;
    private Texture2D _platTexture; 
    private Player _player;
    private Song _backgroundMusic;
    private SoundEffect _jumpSound;
    private SoundEffect _attackSound;
    private bool _hasWon = false;
    private const float GlobalScale = 0.1f;
    private Menu _mainMenu;
    private GameState _currentState = GameState.MainMenu;

    private List<Vector2> _platforms = new List<Vector2>();
    private float _platScale = 4.0f; 

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        IsMouseVisible = true;
    }

    protected override void Initialize() { base.Initialize(); }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _blockTexture = Content.Load<Texture2D>("images/blok");
        _platTexture = Content.Load<Texture2D>("plat"); 
        _font = Content.Load<SpriteFont>("fonts/04B_30");
        _backgroundMusic = Content.Load<Song>("Audio/Ses");
        _jumpSound = Content.Load<SoundEffect>("Audio/zipla");
        _attackSound = Content.Load<SoundEffect>("Audio/kilic");

        Texture2D idle = Content.Load<Texture2D>("images/1");
        List<Texture2D> walk = new List<Texture2D> { Content.Load<Texture2D>("images/13"), Content.Load<Texture2D>("images/14"), Content.Load<Texture2D>("images/17"), Content.Load<Texture2D>("images/18"), Content.Load<Texture2D>("images/19") };
        List<Texture2D> jump = new List<Texture2D> { Content.Load<Texture2D>("images/31"), Content.Load<Texture2D>("images/32"), Content.Load<Texture2D>("images/33"), Content.Load<Texture2D>("images/34") };
        List<Texture2D> atk = new List<Texture2D> { Content.Load<Texture2D>("images/71"), Content.Load<Texture2D>("images/65"), Content.Load<Texture2D>("images/72"), Content.Load<Texture2D>("images/73"), Content.Load<Texture2D>("images/74"), Content.Load<Texture2D>("images/75") };

        
        _player = new Player(idle, walk, jump, atk, _jumpSound, _attackSound, new Vector2(100, 630));
        _mainMenu = new Menu(_font, GraphicsDevice.Viewport);

        for (int i = 1; i < 100; i++) 
        { 
            _platforms.Add(new Vector2(i * 450, 420)); 
        }

        MediaPlayer.Play(_backgroundMusic);
        MediaPlayer.IsRepeating = true;
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        if (_currentState == GameState.Playing)
        {
            if (_hasWon) return;

            
            float targetY = 800; 

            foreach (var plat in _platforms)
            {
                float pw = _platTexture.Width * _platScale;
                
                if (_player.Position.X > plat.X - 20 && _player.Position.X < plat.X + pw + 20)
                {
                    
                    if (_player.Velocity.Y >= 0 && _player.Position.Y <= plat.Y + 200 && _player.Position.Y > plat.Y - 50)
                    {
                        System.Diagnostics.Debug.WriteLine($"Platform Y: {plat.Y} - Karakter Y: {_player.Position.Y}");
                        targetY = plat.Y + (800 - 635); 
                        break;
                    }
                }
            }

            _player.Update(gameTime, targetY);
            
            if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.A)) _score++;
            _cameraTransform = Matrix.CreateTranslation(-_player.Position.X + 640, 0, 0);
        }
        else if (_mainMenu.Update() == 0) _currentState = GameState.Playing;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        if (_currentState == GameState.Playing)
        {
            _spriteBatch.Begin(transformMatrix: _cameraTransform);
            
            float bw = _blockTexture.Width * GlobalScale;
            float bh = _blockTexture.Height * GlobalScale;
            
          
            for (float x = _player.Position.X - 1500; x < _player.Position.X + 2500; x += bw)
            {
                for (int row = 0; row < 5; row++)
                {
                    _spriteBatch.Draw(_blockTexture, new Vector2(x, 635 + (row * bh)), null, Color.White, 0f, Vector2.Zero, GlobalScale, SpriteEffects.None, 0f);
                }
            }

            foreach (var plat in _platforms)
            {
                _spriteBatch.Draw(_platTexture, plat, null, Color.White, 0f, Vector2.Zero, _platScale, SpriteEffects.None, 0f);
            }

            _player.Draw(_spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, $"Score: {_score}", new Vector2(20, 20), Color.White);
            _spriteBatch.End();
        }
        else
        {
            _spriteBatch.Begin();
            _mainMenu.Draw(_spriteBatch);
            _spriteBatch.End();
        }
        base.Draw(gameTime);
    }
}