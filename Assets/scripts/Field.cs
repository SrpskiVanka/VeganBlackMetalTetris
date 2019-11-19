using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour
{
    public Figure[] figures;
    [SerializeField] public Vector3 StartPosition;
    public int [,] grid = new int[10,20];
    public Figure current;
    public Figure preview;
    
    Dictionary<Vector2Int, Transform> Blocks = new Dictionary<Vector2Int, Transform>();
    public GameObject GameOverScreen;
    public GameObject Pause;
    public bool isPaused;
    public int score;
    public TextMeshProUGUI scoreLabel;
    [SerializeField] private Transform previewPosition;
    public float speed = 1;


    private void OnDrawGizmos()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i,j] != 0)
                {
                    Gizmos.DrawCube(new Vector3(i,j), Vector3.one);
                }
                else
                {
                    Gizmos.DrawWireCube(new Vector3(i,j), Vector3.one);
                }
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SpawnFigure()
    {
        if (current != null)
        {
            DissolveFigure();
        }
        
        var fulledLines = FindFullLines();
        AddScore(fulledLines.Count);
        RemoveLineAndSlideDown(fulledLines);
        

        if (!CanSpawn(preview))
        {
            GameOverScreen.SetActive(true);
            return;
        }

        preview.transform.SetParent(null);
        var figureInstance = preview;
        figureInstance.transform.position = StartPosition;

        foreach (var position in figureInstance.blocks)
        {
            var x = (int)StartPosition.x + position.x;
            var y = (int)StartPosition.y + position.y;
            grid[x, y] = 1;
        }
        figureInstance.Position = new Vector2Int((int)StartPosition.x, (int)StartPosition.y);
        current = figureInstance;
        CreatePreview();
    }

    private void CreatePreview()
    {
        var index = Random.Range(0, figures.Length);
        var figure = figures[index];
        preview = Instantiate(figure, previewPosition, false);
    }

    private bool CanSpawn(Figure figure)
    {
        foreach (var position in figure.blocks)
        {
            var x = (int)StartPosition.x + position.x;
            var y = (int)StartPosition.y + position.y;
            if (grid[x, y] == 1)
            {
                return false;
            }
        }

        return true;
    }

    private void DissolveFigure()
    {
        for (var index = 0; index < current.BlockTransforms.Length; index++)
        {
            var currentBlock = current.BlockTransforms[index];
            var offset = current.blocks[index];
            var gridPosition = current.Position + offset;
            Blocks[gridPosition] = currentBlock;
            currentBlock.SetParent(null);
        }
    }

    public void MoveDown()
    {
        foreach (var position in current.blocks)
        {
            var y = current.Position.y + position.y;
            if (y == 0)
            {
                SpawnFigure();
                return;
            }
        }
        
        
        foreach (var position in current.blocks)
        {
            var x = current.Position.x + position.x;
            var y = current.Position.y + position.y;
            grid[x, y] = 0;
        }

        var canMove = true;
        
        foreach (var position in current.blocks)
        {
            var x = current.Position.x + position.x;
            var y = current.Position.y + position.y;
            
            if (grid[x,y -1] != 0)
            {
                canMove = false;
                 break;
            }
        }

        if (canMove)
        {
            current.Position = new Vector2Int(current.Position.x, current.Position.y - 1);
        }

        current.transform.position = new Vector3(current.Position.x, current.Position.y);
        
        foreach (var position in current.blocks)
        {
            var x = current.Position.x + position.x;
            var y = current.Position.y + position.y;
            grid[x, y] = 1;
        }

        if (canMove == false)
        {
            SpawnFigure();
        }
    }

    public void MoveLeft()
    {
        foreach (var position in current.blocks)
        {
            var x = current.Position.x + position.x;
            if (x == 0)
            {
                return;
            }
        }

        
        foreach (var position in current.blocks)
        {
            var x = current.Position.x + position.x;
            var y = current.Position.y + position.y;
            grid[x, y] = 0;
        }
        
        var canMove = true;
        
        foreach (var position in current.blocks)
        {
            var x = current.Position.x + position.x;
            var y = current.Position.y + position.y;
            
            if (grid[x-1,y] != 0)
            {
                canMove = false;
                break;
            }
        }

        if (canMove)
        {
            current.Position = new Vector2Int(current.Position.x - 1, current.Position.y);
        }

        current.transform.position = new Vector3(current.Position.x, current.Position.y);
        
        foreach (var position in current.blocks)
        {
            var x = current.Position.x + position.x;
            var y = current.Position.y + position.y;
            grid[x, y] = 1;
        }
    }
    
    
    public void MoveRight()
    {
        foreach (var position in current.blocks)
        {
            var x = current.Position.x + position.x;
            if (x == grid.GetLength(0) - 1)
            {
                return;
            }
        }

        foreach (var position in current.blocks)
        {
            var x = current.Position.x + position.x;
            var y = current.Position.y + position.y;
            grid[x, y] = 0;
        }
        
        var canMove = true;
        
        foreach (var position in current.blocks)
        {
            var x = current.Position.x + position.x;
            var y = current.Position.y + position.y;
            
            if (grid[x+1,y] != 0)
            {
                canMove = false;
                break;
            }
        }

        if (canMove)
        {
            current.Position = new Vector2Int(current.Position.x + 1, current.Position.y);
        }

        current.transform.position = new Vector3(current.Position.x, current.Position.y);
        
        foreach (var position in current.blocks)
        {
            var x = current.Position.x + position.x;
            var y = current.Position.y + position.y;
            grid[x, y] = 1;
        }
    }
    public void SpeedUp()
    {
        foreach (var position in current.blocks)
        {
            var y = current.Position.y + position.y;
            if (y == 0)
            {
                SpawnFigure();
                return;
            }
        }
        
        
        foreach (var position in current.blocks)
        {
            var x = current.Position.x + position.x;
            var y = current.Position.y + position.y;
            grid[x, y] = 0;
        }

        var canMove = true;
        
        foreach (var position in current.blocks)
        {
            var x = current.Position.x + position.x;
            var y = current.Position.y + position.y;
            
            if (grid[x,y -1] != 0)
            {
                canMove = false;
                break;
            }
        }

        if (canMove)
        {
            current.Position = new Vector2Int(current.Position.x, current.Position.y - 1);
        }

        current.transform.position = new Vector3(current.Position.x, current.Position.y);
        
        foreach (var position in current.blocks)
        {
            var x = current.Position.x + position.x;
            var y = current.Position.y + position.y;
            grid[x, y] = 1;
        }

        if (canMove == false)
        {
            SpawnFigure();
        }
    }

    void Rotate()
    {
        if (current.isBlockRotation)
        {
            return;
        }
        
        foreach (var position in current.blocks)
        {
            var x = current.Position.x + position.x;
            var y = current.Position.y + position.y;
            grid[x, y] = 0;
        }

        var rotatedBlock = Figure.Rotate(current.blocks);

        var canRotate = true;
        
        foreach (var position in rotatedBlock)
        {
            var x = current.Position.x + position.x;
            var y = current.Position.y + position.y;
            
            if (x<0 || x>= grid.GetLength(0) || y<0 || y>=grid.GetLength(1) || grid[x,y] != 0)
            {
                canRotate = false;
                break;
            }
        }

        if (canRotate)
        {
            current.blocks = rotatedBlock;
            current.transform.Rotate(new Vector3(0,0,-90));
        }
        
        
        foreach (var position in current.blocks)
        {
            var x = current.Position.x + position.x;
            var y = current.Position.y + position.y;
            grid[x, y] = 1;
        }
    }

    public List<int> FindFullLines()
    {
        List<int> fullLines = new List<int>(4);
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            bool full = true;
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (grid[x, y] != 1)
                {
                    full = false;
                    break;
                }
            }

            if (full)
            {
                fullLines.Add(y);
            }
        }
        return fullLines;
    }


    public void RemoveLineAndSlideDown(List<int> fullLines)
    {
        for (var i = fullLines.Count - 1; i >= 0; i--)
        {
            var index = fullLines[i];
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                var toDel = new Vector2Int(x, index);
                if (Blocks.ContainsKey(toDel))
                {
                        Destroy(Blocks[toDel].gameObject);
                        Blocks.Remove(toDel);
                }
                for (int y = index; y < grid.GetLength(1); y++)
                {
                    
                    var pos = new Vector2Int(x, y);
                    
                    if (y == grid.GetLength(1) - 1)
                    {
                        grid[x, y] = 0;
                        if (Blocks.ContainsKey(pos))
                        {
                            Blocks.Remove(pos);
                        }
                    }
                    else {
                        var posNext = new Vector2Int(x, y+1);
                        if (Blocks.ContainsKey(posNext))
                        {
                            Blocks[pos] = Blocks[posNext];
                            Blocks[pos].position = new Vector3(pos.x, pos.y);
                        }
                        else
                        {
                            if (Blocks.ContainsKey(pos))
                            {
                                Blocks.Remove(pos);
                            }
                        }
                        grid[x, y] = grid[x, y + 1];
                    }
                }
            }
        }
    }
    void Start()
    {
        var difficulty = (Difficulty)PlayerPrefs.GetInt("difficulty");
        switch (difficulty)
        {
            case Difficulty.Omnivore:
                speed = 0.4f;
                break;
            case Difficulty.Vegetarian:
                speed = 0.2f;
                break;
            case Difficulty.Vegan:
                speed = 0.1f;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        CreatePreview();
        SpawnFigure();
        StartCoroutine(MoveCoroutine());

    }

    void Update()
    {
        if (!isPaused)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveLeft();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveRight();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SpeedUp();
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Rotate();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (Pause.activeInHierarchy)
            {
                Pause.SetActive(false);
            }
            else
            {
                Pause.SetActive(true);
            }
            
        }
    }

    IEnumerator MoveCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(speed);
            if (!isPaused)
            {
                MoveDown();   
            }
        }
    }

    public void AddScore(int fulledLines)
    {
        switch (fulledLines)
        {
            case 1:
                score += 40;
                break;
                
            case 2:
                score += 100;
                break;
            case 3:
                score += 300;
                break;
            case 4:
                score += 1200;
                break;
        }

        scoreLabel.text = score.ToString();
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
