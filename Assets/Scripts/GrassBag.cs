using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBag : MonoBehaviour
{
    [SerializeField]
    public GameObject grassBag;
    [SerializeField]
    private CountText countText;
    [SerializeField]
    private Vector3Int size;
    [SerializeField]
    private float grassBlockSize;

    public List<GameObject> grassBlocks;

    public int count;
    private int maxCount;
    private Vector3Int currentPoint;
    private Vector3Int previousPoint;
    private void Awake()
    {
        maxCount = size.x * size.y * size.z;
    }

    public Vector3 AddGrass()
    {
        if (count + 1 <= maxCount)
        {
            previousPoint = currentPoint;
            count++;
            countText.count = count;
            countText.UpdateText();

            if (currentPoint.x + 1 < size.x)
            {
                currentPoint.x++;
            }
            else if (currentPoint.z + 1 < size.z)
            {
                currentPoint.x = 0;
                currentPoint.z++;
            }
            else if (currentPoint.y + 1 < size.y)
            {
                currentPoint.x = 0;
                currentPoint.z = 0;
                currentPoint.y++;
            }
            Vector3 _temp = previousPoint;
            return _temp * grassBlockSize;
        }
        else return Vector3.down;
    }

    public bool TakeGrass()
    {
        if(count > 0)
        {
            count = 0;
            countText.count = count;
            countText.UpdateText();
            return true;
        }
        else return false;
    }

    public void ClearBag()
    {
        grassBlocks.Clear();
        currentPoint = Vector3Int.zero;
        previousPoint = Vector3Int.zero;
    }
}
