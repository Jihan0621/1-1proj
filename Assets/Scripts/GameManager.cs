using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int mazeX;
    public int mazeY;

    public int interval;

    int[,] maze;

    public GameObject wall;
    public GameObject Door;

    Transform wallHolder;

    static GameManager instance = null;
    // Start is called before the first frame update
    void Awake()
    {
        wallHolder = new GameObject().transform;
        wallHolder.name = "wallHolder";
        maze = new int[mazeY, mazeX];

        wall.transform.localScale = new Vector3(interval, interval, interval);
        Door.transform.localScale = new Vector3(interval, interval, interval);

        for (int y = 0; y<mazeY; y++)
        {
            for(int x= 0; x<mazeX; x++)
            {
                 if(x % 2 == 0 || y % 2 == 0)
                {
                    maze[y, x] = 1;
                }
                else
                {
                    maze[y, x] = 0;
                }

            }
        }
        Generate();
        for (int y = 0; y < mazeY; y++)
        {
            for (int x = 0; x < mazeX; x++)
            {
                if (maze[y,x] == 1)
                {
                    GameObject temp = Instantiate(wall, new Vector3(x * interval, 0.5f * interval, y * interval), Quaternion.identity);
                    temp.transform.SetParent(wallHolder);
                }
            }
        }
    }

    void Generate()
    {
        for (int y = 0; y < mazeY; y++)
        {
            for (int x = 0; x < mazeX; x++)
            {
                if (x % 2 == 0 || y % 2 == 0)
                    continue;

                if (x == mazeX - 2 && y == mazeY - 2)
                {
                    Instantiate(Door, new Vector3(x * interval, 0.5f * interval, (y + 1) * interval), Quaternion.identity);
                    maze[y + 1, x] = 0;
                    continue;
                }

                if (x == mazeX - 2)
                {
                    maze[y + 1, x] = 0;
                    continue;
                }
                if (y == mazeY - 2)
                {
                    maze[y, x + 1] = 0;
                    continue;
                }
                else
                {
                    int dir = Random.Range(0, 2);
                    if(dir == 0)
                    {
                        maze[y + 1, x] = 0;
                    }
                    else
                    {
                        maze[y, x + 1] = 0;
                    }
                }
            }
        }
    }

    void Singleton()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
