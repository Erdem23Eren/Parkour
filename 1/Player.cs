using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace _1;

public class Player
{
    private List<Texture2D> _walkTextures = new List<Texture2D>();
    private Texture2D _idleTexture;
    private Vector2 _position;
    private Vector2 _velocity;
    private bool _isJumping;

    private int _currentFrameIndex;
    private double _timer;
    private double _fps = 12;
    private float _characterScale = 4.0f;
    private bool _isMoving;

    public Vector2 Position => _position;

    public Player(Texture2D idle, List<Texture2D> walkFrames, Vector2 startPos)
    {
        _idleTexture = idle;
        _walkTextures = walkFrames;
        _position = startPos;
    }

    public void Update(GameTime gameTime, float groundY)
    {
        var kState = Keyboard.GetState();
        _isMoving = false;

        if (kState.IsKeyDown(Keys.D))
        {
            _velocity.X = 5f;
            _isMoving = true;
        }
        else if (kState.IsKeyDown(Keys.A))
        {
            _velocity.X = -5f;
            _isMoving = true;
        }
        else
        {
            _velocity.X = 0;
        }

        if (kState.IsKeyDown(Keys.W) && !_isJumping)
        {
            _velocity.Y = -12f;
            _isJumping = true;
        }

        _velocity.Y += 0.5f;
        _position += _velocity;

        if (_position.Y >= groundY)
        {
            _position.Y = groundY;
            _velocity.Y = 0;
            _isJumping = false;
        }

        if (_isMoving)
        {
            _timer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer > 1.0 / _fps)
            {
                _currentFrameIndex++;
                if (_currentFrameIndex >= _walkTextures.Count) _currentFrameIndex = 0;
                _timer = 0;
            }
        }
        else
        {
            _currentFrameIndex = 0;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Texture2D textureToDraw = _isMoving ? _walkTextures[_currentFrameIndex] : _idleTexture;
        
        Vector2 origin = new Vector2(textureToDraw.Width / 2f, textureToDraw.Height);

        SpriteEffects flipEffect = _velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        spriteBatch.Draw(
            textureToDraw, 
            _position, 
            null, 
            Color.White, 
            0f, 
            origin, 
            _characterScale, 
            flipEffect, 
            0f
        );
    }
}