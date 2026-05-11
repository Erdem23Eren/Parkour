using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1;

public class Platform
{
public Vector2 Position;
public Texture2D Texture;
public float Scale;

    public Platform(Texture2D texture, Vector2 position, 
    float scale)
    {
        Texture = texture;
        Position = position;
        Scale = scale;
    }

    public Rectangle Bounds => new Rectangle(
        (int)Position.X, 
        (int)Position.Y, 
        (int)(Texture.Width * Scale), 
        (int)(Texture.Height * Scale)
    );

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, null, Color.White, 0f, 
        Vector2.Zero, Scale, SpriteEffects.None, 0f);
    }
}
#region  ekstra notlar
// bu kısım platform için 
// Menu cs menü için 
// Player cs oyuncu için
// Game1 cs ise oyun mekanikleri ve genel oyun döngüsü için
// bu şekilde ayırdım dosyaları, kodlar biraz karışık .



#endregion