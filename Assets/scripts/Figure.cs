using UnityEngine;

public class Figure : MonoBehaviour
{
    public Vector2Int[] blocks;
    public bool isBlockRotation;
    
    public static Vector2Int[] Rotate(Vector2Int[] input)
    {
        var output = new Vector2Int[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            var x = input[i].x;
            var y = input[i].y;
            output[i] = new Vector2Int(y,-x);
        }

        return output;
    }

    public Vector2Int Position;

    public Transform[] BlockTransforms;
}