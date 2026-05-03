using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace _1;

public class Player
{
    private List<Texture2D> _walkTextures = new List<Texture2D>();
    private List<Texture2D> _jumpTextures = new List<Texture2D>();
    private Texture2D _idleTexture;
    private Vector2 _position;
    private Vector2 _velocity;
    private bool _isJumping;

    private int _currentFrameIndex;
    private double _timer;
    private float _characterScale = 4.0f;
    private bool _isMoving;

    public Vector2 Position => _position;

    public Player(Texture2D idle, List<Texture2D> walkFrames, List<Texture2D> jumpFrames, Vector2 startPos)
    {
        _idleTexture = idle;
        _walkTextures = walkFrames;
        _jumpTextures = jumpFrames;
        _position = startPos;
    }

    public void Update(GameTime gameTime, float groundY)
    {
        var kState = Keyboard.GetState();
        _isMoving = false;

        if (kState.IsKeyDown(Keys.D)) { _velocity.X = 5f; _isMoving = true; }
        else if (kState.IsKeyDown(Keys.A)) { _velocity.X = -5f; _isMoving = true; }
        else { _velocity.X = 0; }

        if (kState.IsKeyDown(Keys.W) && !_isJumping) { _velocity.Y = -12f; _isJumping = true; }

        _velocity.Y += 0.5f;
        _position += _velocity;

        if (_position.Y >= groundY)
        {
            _position.Y = groundY;
            _velocity.Y = 0;
            _isJumping = false;
        }

        _timer += gameTime.ElapsedGameTime.TotalSeconds;
        if (_timer > 0.1)
        {
            _currentFrameIndex++;
            _timer = 0;
            
            if (_isJumping)
            {
                if (_currentFrameIndex >= _jumpTextures.Count) _currentFrameIndex = 0;
            }
            else if (_isMoving)
            {
                if (_currentFrameIndex >= _walkTextures.Count) _currentFrameIndex = 0;
            }
            else
            {
                _currentFrameIndex = 0;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Texture2D textureToDraw;

        if (_isJumping) textureToDraw = _jumpTextures[_currentFrameIndex];
        else if (_isMoving) textureToDraw = _walkTextures[_currentFrameIndex];
        else textureToDraw = _idleTexture;

        Vector2 origin = new Vector2(textureToDraw.Width / 2f, textureToDraw.Height);
        SpriteEffects flip = _velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        spriteBatch.Draw(textureToDraw, _position, null, Color.White, 0f, origin, _characterScale, flip, 0f);
    }
}