using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;

public class Tests
{
    [Test]
    public void ShapeRotation()
    {
        var shape = new Vector2Int[] {new Vector2Int(-1,1), new Vector2Int(0,1), 
            new Vector2Int(0,0), new Vector2Int(1,0)};
        
        var rotated = Figure.Rotate(shape);
        var expected = new Vector2Int[] {new Vector2Int(1,1), new Vector2Int(1,0), 
            new Vector2Int(0,0), new Vector2Int(0,-1)};
        CollectionAssert.AreEqual(expected, rotated);
    }
    
    [Test]
    public void ShapeR()
    {
        var shape = new Vector2Int[] {new Vector2Int(-1,0), new Vector2Int(0,0), 
            new Vector2Int(1,0), new Vector2Int(2,0)};
        
        var rotated = Figure.Rotate(shape);
        var expected = new Vector2Int[] {new Vector2Int(0,1), new Vector2Int(0,0), 
            new Vector2Int(0,-1), new Vector2Int(0,-2)};
        CollectionAssert.AreEqual(expected, rotated);
    }

    [Test]
    public void MoveDownTest()
    {
        var field = new GameObject().AddComponent<Field>();
        field.grid = new int[4,4];
        field.StartPosition = new Vector3(1,3);
        var figure = new GameObject().AddComponent<Figure>();
        field.figures = new[] {figure};
        figure.blocks = new[] {new Vector2Int(0, 0), new Vector2Int(1, 0)};
        field.SpawnFigure();
        field.MoveDown();
        var expected = new int[,] {{0, 0, 0, 0}, {0, 0, 1, 0}, {0, 0, 1, 0}, {0, 0, 0, 0}};
        CollectionAssert.AreEqual(expected, field.grid);
    }
    
    [Test]
    public void CheckOutOfBorder()
    {
        var field = new GameObject().AddComponent<Field>();
        field.grid = new int[4, 4] {{1, 0, 0, 0}, {1, 0, 0, 0}, {1, 0, 0, 0}, {1, 0, 0, 0} };
        field.StartPosition = new Vector3(1,1);
        var figure = new GameObject().AddComponent<Figure>();
        field.figures = new[] {figure};
        figure.blocks = new[] {new Vector2Int(0, 0), new Vector2Int(1, 0)};
        field.SpawnFigure();
        field.MoveDown();
        var expected = new int[,] {{1, 0, 0, 0}, {1, 1, 0, 0}, {1, 1, 0, 0}, {1, 0, 0, 0}};
        CollectionAssert.AreEqual(expected, field.grid);
    }

    [Test]
    public void RemoveLineTest1()
    {
        var field = new GameObject().AddComponent<Field>();
        field.grid = new int[3, 3] {{0, 1, 0}, {0, 1, 0}, {0, 1, 0}};
        var actual = field.FindFullLines();
        var expected = new int[1] {1};
        CollectionAssert.AreEqual(expected, actual);
    }
    
    [Test]
    public void RemoveLineTest2()
    {
        var field = new GameObject().AddComponent<Field>();
        field.grid = new int[3, 3] {{1, 1, 0}, {1, 1, 0}, {1, 1, 0}};
        var actual = field.FindFullLines();
        var expected = new int[2] {0, 1};
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void RemoveLineAndSlide()
    {
        var field = new GameObject().AddComponent<Field>();
        field.grid = new int[3, 3] {{1, 0, 0}, {1, 1, 0}, {1, 0, 0}};
        field.RemoveLineAndSlideDown(new List<int>(){0});
        var expected = new int[3,3] {{0, 0, 0}, {1, 0, 0}, {0, 0, 0}};
        CollectionAssert.AreEqual(expected, field.grid);
    }
}
