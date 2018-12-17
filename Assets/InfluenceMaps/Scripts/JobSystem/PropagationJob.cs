using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct PropagationJob : IJob {

    public int x, y;
    public int currentInfluence;
    public float influence;
    public bool isResources;
    //public Influencer currentInfluencer;
    public int width, height;
    public int influenceRadius;
    public float influencePropagationRatio;
    public Vector3 influencerPosition;

    public Color currentColor, backgroundColor;
    //public Color[] currentTexture;
    public NativeArray<Color> nativeTexture;
    public Vector3 transformPosition;
    public Bounds bounds;

    public void Execute()
    {
        DrawTexture(x, y, 0, influence, isResources);
        //throw new System.NotImplementedException();
    }

    void DrawTexture(int x, int y, int currentInfluence, float influence, bool isResources)
    {
        if (currentInfluence <= influenceRadius
            && (x >= 0 && x < width) && (y >= 0 && y < height))
        {
            Color c = currentColor;
            if (isResources) c.b = influence;
            else c.a = influence;
            //currentTexture.SetPixel(x, y, c);
            nativeTexture[y * height+x] = c;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i != 0 && j == 0) //horizontal
                    {
                        PropagateHorizontal(i, x + i, y, currentInfluence + 1, influence * influencePropagationRatio, isResources);
                    }
                    else if (i == 0 && j != 0) //vertical
                    {
                        PropagateVertical(j, x, y + j, currentInfluence + 1, influence * influencePropagationRatio, isResources);
                    }
                    else if (i != 0 && j != 0) //diagonal
                    {
                        PropagateDiagonal(i, j, x + i, y + j, currentInfluence + 1, influence * influencePropagationRatio, isResources);
                    }
                }
            }
        }
    }

    void PropagateHorizontal(int dir, int x, int y, int currentInfluence, float influence, bool isResources)
    {
        if (currentInfluence == influenceRadius) return;

        Color c = currentColor;
        if (isResources) c.b = influence;
        else c.a = influence;

        for (int i = -(currentInfluence - 1); i < currentInfluence; i++)
        {
            if (x >= 0 && x < width && y + i >= 0 && y + i < height)
                if (IsInsideRadius(new Vector2Int(x, y + i)))
                {
                    Color previousColor = nativeTexture[(y + i) * height + x];
                    if (previousColor == backgroundColor) nativeTexture[(y + i) * height + x] = c;
                    //else currentTexture.SetPixel(x, y + i, CompareColors(previousColor, c));
                    else nativeTexture[(y+i) * height + x] = CompareColors(previousColor, c);
                }
        }
        PropagateHorizontal(dir, x + dir, y, currentInfluence + 1, influence * influencePropagationRatio, isResources);
    }

    void PropagateVertical(int dir, int x, int y, int currentInfluence, float influence, bool isResources)
    {
        if (currentInfluence == influenceRadius) return;

        Color c = currentColor;
        if (isResources) c.b = influence;
        else c.a = influence;

        for (int i = -(currentInfluence - 1); i < currentInfluence; i++)
        {
            if (x + i >= 0 && x + i < width && y >= 0 && y < height)
                if (IsInsideRadius(new Vector2Int(x + i, y)))
                {
                    Color previousColor = nativeTexture[y * height + x+1];
                    if (previousColor == backgroundColor) nativeTexture[y * height + x + 1]=c;
                    else nativeTexture[y * height + x + 1]=CompareColors(previousColor, c);
                }
        }
        PropagateVertical(dir, x, y + dir, currentInfluence + 1, influence * influencePropagationRatio, isResources);
    }

    void PropagateDiagonal(int dirX, int dirY, int x, int y, int currentInfluence, float influence, bool isResources)
    {
        if (currentInfluence == influenceRadius || !(y >= 0 && y < height && x >= 0 && x < width)) return;

        Color c = currentColor;
        if (isResources) c.b = influence;
        else c.a = influence;

        if (IsInsideRadius(new Vector2Int(x, y)))
        {
            Color previousColor = nativeTexture[y * height + x];
            if (previousColor == backgroundColor) nativeTexture[y * height + x]=c;
            else nativeTexture[y * height + x]=CompareColors(previousColor, c);
        }
        PropagateDiagonal(dirX, dirY, x + dirX, y + dirY, currentInfluence + 1, influence * influencePropagationRatio, isResources);
    }

    Color CompareColors(Color pc, Color c)
    {
        Color nc = new Color();

        if (pc.b > 0) c.b += pc.b;

        if (pc.a > c.a) nc = pc;
        else nc = c;

        if (pc.r != 0 && c.r != 0) nc.a = pc.a + c.a;
        else if (pc.g != 0 && c.g != 0) nc.a = pc.a + c.a;

        return nc;
    }

    bool IsInsideRadius(Vector2Int pos)
    {
        Vector2Int influencerPos = WorldToPixelSpace(transformPosition.x, transformPosition.z);
        if ((pos - influencerPos).magnitude > influenceRadius)
            return false;
        return true;
    }

    Vector2Int WorldToPixelSpace(float x, float y)
    {
        float minDistanceX = transformPosition.x - bounds.extents.x;
        float maxDistanceX = transformPosition.x + bounds.extents.x;

        float minDistanceZ = transformPosition.z - bounds.extents.z;
        float maxDistanceZ = transformPosition.z + bounds.extents.z;

        int pixelX = (int)Mathf.Lerp(width, 0, Mathf.InverseLerp(minDistanceX, maxDistanceX, x));
        int pixelY = (int)Mathf.Lerp(height, 0, Mathf.InverseLerp(minDistanceZ, maxDistanceZ, y));

        return new Vector2Int(pixelX, pixelY);
    }
}
