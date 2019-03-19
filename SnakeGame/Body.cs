using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SnakeGame
{
    class Body
    {
        public int x, y, size;
        
        public Body(int _x, int _y, int _size)
        {
            x = _x;
            y = _y;
            size = _size;
        }

        public void MoveHead(string direction, int snakeSpeed)
        {
            if (direction == "right")
            {
                x += snakeSpeed;
            }
            else if (direction == "left")
            {
                x -= snakeSpeed;
            }
            else if (direction == "up")
            {
               y -= snakeSpeed;
            }
            else if (direction == "down")
            {
                y += snakeSpeed;
            }
        }

        public void Move(Point p)
        {
            x = p.X;
            y = p.Y;
        }

        public bool Collision(Rectangle r)
        {
            Rectangle thisRec = new Rectangle(x, y, size, size);

            if (thisRec.IntersectsWith(r))
            {
                return true;
            }

            return false;                
        }

        public bool Collision(Wall w)
        {
            Rectangle thisRec = new Rectangle(x, y, size, size);
            Rectangle wallRec = new Rectangle(w.x, w.y, w.width, w.height);

            if (thisRec.IntersectsWith(wallRec))
            {
                return true;
            }

            return false;
        }

        public bool Collision(Body b)
        {
            Rectangle thisRec = new Rectangle(x, y, size, size);
            Rectangle bodyRec = new Rectangle(b.x, b.y, b.size, b.size);

            if (thisRec.IntersectsWith(bodyRec))
            {
                return true;
            }

            return false;
        }
    }
}
