using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace snakecomponents;

class PlayerSnake
{
    private int screenWidth;
    private int screenHeight;
    private int blockSize;
    public List<Rectangle> body { get; set; }
    public Rectangle head { get; set; }
    public List<char> moveQueue { get; set; }
    public PlayerSnake(int sw, int sh, int bs)
    {
        screenWidth = sw;
        screenHeight = sh;
        blockSize = bs;
        body = new List<Rectangle>();
        body.Add(new Rectangle(2 * blockSize, 8 * blockSize, blockSize, blockSize));
        body.Add(new Rectangle(3 * blockSize, 8 * blockSize, blockSize, blockSize));
        body.Add(new Rectangle(4 * blockSize, 8 * blockSize, blockSize, blockSize));
        head = body[body.Count - 1];
        moveQueue = ['r'];
    }

    public void AddMoveQueue(char direction)
    {
        if (moveQueue.Count < 2)
        {
            moveQueue.Add(direction);
        }
    }

    public void AddSegment()
    {
        body.Insert(0, body[0]);
    }

    public void Move()
    {
        if (moveQueue[0] == 'r')
        {
            if (head.X >= screenWidth - blockSize)
            {
                body.Add(new Rectangle(0, head.Y, blockSize, blockSize));
            }
            else
            {
                body.Add(new Rectangle(head.X + blockSize, head.Y, blockSize, blockSize));
            }
        }
        if (moveQueue[0] == 'l')
        {
            if (head.X <= 0)
            {
                body.Add(new Rectangle(screenWidth - blockSize, head.Y, blockSize, blockSize));
            }
            else
            {
                body.Add(new Rectangle(head.X - blockSize, head.Y, blockSize, blockSize));
            }
        }
        if (moveQueue[0] == 'u')
        {
            if (head.Y <= blockSize)
            {
                body.Add(new Rectangle(head.X, screenHeight - blockSize, blockSize, blockSize));
            }
            else
            {
                body.Add(new Rectangle(head.X, head.Y - blockSize, blockSize, blockSize));
            }
        }
        if (moveQueue[0] == 'd')
        {
            if (head.Y >= screenHeight - blockSize)
            {
                body.Add(new Rectangle(head.X, blockSize, blockSize, blockSize));
            }
            else
            {
                body.Add(new Rectangle(head.X, head.Y + blockSize, blockSize, blockSize));
            }
        }
        head = body[body.Count - 1];

        body.Remove(body[0]);
        if (moveQueue.Count > 1)
        {
            moveQueue.Remove(moveQueue[0]);
        }
    }

    public bool IsDead()
    {
        List<Rectangle> headlessBody = new List<Rectangle>();
        for (int i = 0; i < body.Count - 2; i++)
        {
            headlessBody.Add(body[i]);
        }
        if (headlessBody.Contains(head))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

class AppleGenerator
{
    private int blockSize;
    public Rectangle apple { get; set; }
    public AppleGenerator(int bs)
    {
        blockSize = bs;
        apple = new Rectangle(12 * blockSize, 8 * blockSize, blockSize, blockSize);
    }

    public void GenerateApple(List<Rectangle> body)
    {
        Random rnd = new Random();
        var run = true;
        while (run)
        {
            var rndX = rnd.Next(0, 16);
            var rndY = rnd.Next(1, 15);
            apple = new Rectangle(rndX * blockSize, rndY * blockSize, blockSize, blockSize);
            if (body.Contains(apple))
            {
                continue;
            }
            break;
        }
    }
}
