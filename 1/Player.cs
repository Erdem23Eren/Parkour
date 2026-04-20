using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _1;

public class Player
{
    private Texture2D _texture;
    private Vector2 _position;
    private Vector2 _velocity;
    private bool _isJumping;
    public Vector2 Position => _position;
    
    public Vector2 GetPosition()
    {
        return _position;
    }
    
  
    private const float Gravity = 0.5f;
    private const float JumpStrength = -10f;
    private const float MoveSpeed = 5f;
    private const float Scale = 0.1f; 

    public Player(Texture2D texture, Vector2 startPos)
    {
        _texture = texture;
        _position = startPos;
        _isJumping = false;
    }

    public void Update(GameTime gameTime, float groundY)
    {
        var kState = Keyboard.GetState();

      
        if (kState.IsKeyDown(Keys.A)) _velocity.X = -MoveSpeed;
        else if (kState.IsKeyDown(Keys.D)) _velocity.X = MoveSpeed;
        else _velocity.X = 0;

       
        if (kState.IsKeyDown(Keys.W) && !_isJumping)
        {
            _velocity.Y = JumpStrength;
            _isJumping = true;
        }

        
        _velocity.Y += Gravity;
        _position += _velocity;

     
        float playerBottom = _position.Y + (_texture.Height * Scale);
        if (playerBottom >= groundY)
        {
            _position.Y = groundY - (_texture.Height * Scale);
            _velocity.Y = 0;
            _isJumping = false;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
    }
}