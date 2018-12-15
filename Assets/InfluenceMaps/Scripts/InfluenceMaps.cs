using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceMaps : MonoBehaviour {

    Texture2D texture;
    GameObject[] influencers;
    BoxCollider boxCollider;
    MeshRenderer meshRenderer;
    GameObject currentInfluencer;
    Influencer currentInfluencerFeatures;

    readonly float SQRT2 = Mathf.Sqrt(2);
    
    public int width = 100, height = 100;
    public Color backgroundColor = new Color(0,0,0,0);
    public int numberOfPlayers = 1;

	void Start () {

        texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.anisoLevel = 0;
        meshRenderer = GetComponent<MeshRenderer>();

        boxCollider = GetComponent<BoxCollider>();

        meshRenderer.material.SetTexture("_MainTex", texture);
    }

	void Update () {
        UpdateMap();
    }

    void UpdateMap()
    {
        Vector2Int pos;
        Vector3 influencerPos;
        influencers = GameObject.FindGameObjectsWithTag("Influencer");
        DrawFullColor(backgroundColor);
        for (int i = 0; i < influencers.Length; i++)
        {
            currentInfluencer = influencers[i];
            currentInfluencerFeatures = currentInfluencer.GetComponent<Influencer>();
            influencerPos = currentInfluencer.transform.position;
            pos = WorldToPixelSpace(influencerPos.x, influencerPos.z);
            DrawTexture(pos.x, pos.y, currentInfluencerFeatures.influencePower);
        }
        texture.Apply();
    }

    Vector2Int WorldToPixelSpace(float x, float y)
    {
        float minDistanceX = transform.position.x - boxCollider.bounds.extents.x;
        float maxDistanceX = transform.position.x + boxCollider.bounds.extents.x;

        float minDistanceZ = transform.position.z - boxCollider.bounds.extents.z;
        float maxDistanceZ = transform.position.z + boxCollider.bounds.extents.z;

        int pixelX = (int)Mathf.Lerp(width, 0, Mathf.InverseLerp(minDistanceX, maxDistanceX, x));
        int pixelY = (int)Mathf.Lerp(height, 0, Mathf.InverseLerp(minDistanceZ, maxDistanceZ, y));

        return new Vector2Int(pixelX, pixelY);
    }

    Vector2 PixelToWorldSpace(int x, int y)
    {
        float minDistanceX = transform.position.x - boxCollider.bounds.extents.x;
        float maxDistanceX = transform.position.x + boxCollider.bounds.extents.x;

        float minDistanceZ = transform.position.z - boxCollider.bounds.extents.z;
        float maxDistanceZ = transform.position.z + boxCollider.bounds.extents.z;

        float worldX = Mathf.Lerp(maxDistanceX, minDistanceX, Mathf.InverseLerp(0, width, x));
        float worldY = Mathf.Lerp(maxDistanceZ, minDistanceZ, Mathf.InverseLerp(0, height, y));

        return new Vector2(worldX, worldY);
    }

    bool IsInsideRadius(Vector2Int pos)
    {
        Vector2Int influencerPos = WorldToPixelSpace(currentInfluencer.transform.position.x, currentInfluencer.transform.position.z);
        if((pos-influencerPos).magnitude > currentInfluencerFeatures.influenceRadius)
            return false;
        return true;
    }

    void DrawTexture(int x, int y, float influence)
    {
        DrawTexture(x, y, 0, influence);
    }

    void DrawTexture(int x, int y, int currentInfluence, float influence)
    {
        if (currentInfluence <= currentInfluencerFeatures.influenceRadius
            && (x >= 0 && x < width) && (y >= 0 && x < height))
        {
            Color c = currentInfluencerFeatures.color;
            c.a = influence;
            texture.SetPixel(x, y, c);
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if(i!=0 && j == 0) //horizontal
                    {
                        PropagateHorizontal(i, x + i, y, currentInfluence + 1, influence * currentInfluencerFeatures.influencePropagationRatio);
                    }
                    else if(i==0 && j != 0) //vertical
                    {
                        PropagateVertical(j, x, y + j, currentInfluence + 1, influence * currentInfluencerFeatures.influencePropagationRatio);
                    }
                    else if(i!=0 && j!=0) //diagonal
                    {
                        PropagateDiagonal(i, j, x+i, y+j, currentInfluence+1, influence* currentInfluencerFeatures.influencePropagationRatio);
                    }
                }
            }
        }
    }

    void PropagateHorizontal(int dir, int x, int y, int currentInfluence, float influence)
    {
        if (currentInfluence == currentInfluencerFeatures.influenceRadius) return;

        Color c = currentInfluencerFeatures.color;
        c.a = influence;

        for (int i = -(currentInfluence-1); i < currentInfluence; i++)
        {
            if (x >= 0 && x < texture.width && y + i >= 0 && y + i < texture.height)
                if (IsInsideRadius(new Vector2Int(x, y + i))) {
                    Color previousColor = texture.GetPixel(x, y + i);
                    if(previousColor == backgroundColor) texture.SetPixel(x, y + i, c);
                    else texture.SetPixel(x, y + i, CompareColors(previousColor, c));
                }
                    
        }
        PropagateHorizontal(dir, x + dir, y, currentInfluence + 1, influence * currentInfluencerFeatures.influencePropagationRatio);
    }

    void PropagateVertical(int dir, int x, int y, int currentInfluence, float influence)
    {
        if (currentInfluence == currentInfluencerFeatures.influenceRadius) return;

        Color c = currentInfluencerFeatures.color;
        c.a = influence;

        for (int i = -(currentInfluence - 1); i < currentInfluence; i++)
        {
            if (x+i >= 0 && x+i < texture.width && y >= 0 && y < texture.height)
                if (IsInsideRadius(new Vector2Int(x+i, y)))
                {
                    Color previousColor = texture.GetPixel(x+i, y);
                    if (previousColor == backgroundColor) texture.SetPixel(x+i, y, c);
                    else texture.SetPixel(x+i, y, CompareColors(previousColor, c));
                }
        }
        PropagateVertical(dir, x, y+dir, currentInfluence + 1, influence * currentInfluencerFeatures.influencePropagationRatio);
    }
    
    void PropagateDiagonal(int dirX, int dirY, int x, int y, int currentInfluence, float influence)
    {
        if (currentInfluence == currentInfluencerFeatures.influenceRadius || !(y >= 0 && y < texture.height && x >= 0 && x < texture.width)) return;

        Color c = currentInfluencerFeatures.color;
        c.a = influence;

        if (IsInsideRadius(new Vector2Int(x, y)))
        {
            Color previousColor = texture.GetPixel(x, y);
            if (previousColor == backgroundColor) texture.SetPixel(x, y, c);
            else texture.SetPixel(x, y, CompareColors(previousColor, c));
        }
        PropagateDiagonal(dirX, dirY, x + dirX, y + dirY, currentInfluence + 1, influence * currentInfluencerFeatures.influencePropagationRatio);
    }

    Color CompareColors(Color pc, Color c)
    {
        Color nc = new Color();

        if (pc.a > c.a) nc = pc;
        else nc = c;


        return nc;
    }

    void DrawFullColor(Color c)
    {
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                texture.SetPixel(i, j, c);
            }
        }
    }
}
