using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace _1;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    
    private Matrix _cameraTransform;
    private SpriteFont _font;
    private int _score = 0;
    private Vector2 _scorePosition = new Vector2(20, 20);
    private Vector2 _scoreOrigin;

   
    private Texture2D _blockTexture;
    private Texture2D _playerTexture;
    private Player _player;
    private Song _backgroundMusic;

    private const float GlobalScale = 0.1f;

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
        _playerTexture = Content.Load<Texture2D>("images/blok");
        _font = Content.Load<SpriteFont>("fonts/04B_30");
        _backgroundMusic = Content.Load<Song>("Audio/Ses");

       
        _player = new Player(_playerTexture, new Vector2(100, 200));

      
        MediaPlayer.Play(_backgroundMusic);
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.5f;

       
        _scoreOrigin = new Vector2(0, _font.MeasureString("Score").Y * 0.5f);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        
        float groundY = _graphics.PreferredBackBufferHeight - (_blockTexture.Height * GlobalScale);

        
        _player.Update(gameTime, groundY);

      
        float cameraX = -_player.Position.X + (GraphicsDevice.Viewport.Width / 2);
        _cameraTransform = Matrix.CreateTranslation(cameraX, 0, 0);

        
        if (Keyboard.GetState().IsKeyDown(Keys.D))
            _score += 1;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        
        _spriteBatch.Begin(transformMatrix: _cameraTransform);

        float scaledWidth = _blockTexture.Width * GlobalScale;
        float groundY = _graphics.PreferredBackBufferHeight - (_blockTexture.Height * GlobalScale);

        
        for (float x = 0; x < 10000; x += scaledWidth)
        {
            _spriteBatch.Draw(_blockTexture, new Vector2(x, groundY), null, Color.White, 0f, Vector2.Zero, GlobalScale, SpriteEffects.None, 0f);
        }

        _player.Draw(_spriteBatch);
        _spriteBatch.End();

      
        _spriteBatch.Begin(); 
        _spriteBatch.DrawString(
            _font,
            $"Score: {_score}",
            _scorePosition,
            Color.White,
            0f,
            _scoreOrigin,
            1.0f,
            SpriteEffects.None,
            0f
        );
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}