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
    private Vector2 _scorePosition = new Vector2(20, 20);
    private Texture2D _blockTexture;
    private Player _player;
    private Song _backgroundMusic;
    private SoundEffect _jumpSound;
    private SoundEffect _attackSound;
    private bool _hasWon = false;
    private const float GlobalScale = 0.1f;

   
    private Menu _mainMenu;
    private GameState _currentState = GameState.MainMenu;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _blockTexture = Content.Load<Texture2D>("images/blok");
        _font = Content.Load<SpriteFont>("fonts/04B_30");
        _backgroundMusic = Content.Load<Song>("Audio/Ses");

        _jumpSound = Content.Load<SoundEffect>("Audio/zipla");
        _attackSound = Content.Load<SoundEffect>("Audio/kilic");
        Texture2D idleTexture = Content.Load<Texture2D>("images/1");

        List<Texture2D> walkFrames = new List<Texture2D> 
        {
            Content.Load<Texture2D>("images/13"),
            Content.Load<Texture2D>("images/14"),
            Content.Load<Texture2D>("images/17"),
            Content.Load<Texture2D>("images/18"),
            Content.Load<Texture2D>("images/19")
        };

        List<Texture2D> jumpFrames = new List<Texture2D> 
        {
            Content.Load<Texture2D>("images/31"),
            Content.Load<Texture2D>("images/32"),
            Content.Load<Texture2D>("images/33"),
            Content.Load<Texture2D>("images/34")
        };

        List<Texture2D> attack = new List<Texture2D> 
        {
            Content.Load<Texture2D>("images/71"), Content.Load<Texture2D>("images/65"),
            Content.Load<Texture2D>("images/72"), Content.Load<Texture2D>("images/73"),
            Content.Load<Texture2D>("images/74"), Content.Load<Texture2D>("images/75")
        };

        _player = new Player(idleTexture, walkFrames, jumpFrames, attack, _jumpSound, _attackSound, new Vector2(100, 100));

       
        _mainMenu = new Menu(_font, GraphicsDevice.Viewport);

        MediaPlayer.Play(_backgroundMusic);
        MediaPlayer.IsRepeating = true;
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

      
        if (_currentState == GameState.MainMenu)
        {
            int menuResult = _mainMenu.Update();
            if (menuResult == 0) _currentState = GameState.Playing;
            else if (menuResult == 1) Exit();
        }
        else
        {
            if (_hasWon) return;

            float groundY = 600;
            _player.Update(gameTime, groundY);

            if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _score += 1;
            }

            if (_score >= 2500)
            {
                _hasWon = true;
            }

            float cameraX = -_player.Position.X + (GraphicsDevice.Viewport.Width / 2f);
            _cameraTransform = Matrix.CreateTranslation(cameraX, 0, 0);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        if (_currentState == GameState.MainMenu)
        {
            _spriteBatch.Begin();
            _mainMenu.Draw(_spriteBatch);
            _spriteBatch.End();
        }
        else
        {
            _spriteBatch.Begin(transformMatrix: _cameraTransform);
            float scaledWidth = _blockTexture.Width * GlobalScale;
            float groundY = _graphics.PreferredBackBufferHeight - (_blockTexture.Height * GlobalScale);

            for (float x = -2000; x < 10000; x += scaledWidth)
            {
                _spriteBatch.Draw(_blockTexture, new Vector2(x, groundY), null, Color.White, 0f, Vector2.Zero, GlobalScale, SpriteEffects.None, 0f);
            }

            _player.Draw(_spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin(); 
            _spriteBatch.DrawString(_font, $"Score: {_score}", _scorePosition, Color.White);

            if (_hasWon)
            {
                string winMessage = "WIN!";
                Vector2 messageSize = _font.MeasureString(winMessage);
                Vector2 screenCenter = new Vector2(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height / 2f);
                _spriteBatch.DrawString(_font, winMessage, screenCenter - (messageSize / 2f), Color.Yellow);
            }

            _spriteBatch.End();
        }

        base.Draw(gameTime);
    }
}