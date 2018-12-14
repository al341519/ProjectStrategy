using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceMap{

    Texture2D currentTexture;
    GameObject[] influencers;
    BoxCollider boxCollider;
    MeshRenderer meshRenderer;
    Influencer currentInfluencer;
    HexGrid grid;
    Transform transform;

    List<Influencer> units, enemyUnits, buildings;

    bool updateStarted = false;
    Color currentColor;
    int textureLength;

    readonly float SQRT2 = Mathf.Sqrt(2);

    public int width, height;
    public Color backgroundColor = new Color(0, 0, 0, 0);
    public int numberOfPlayers;
    public int updateTime = 1;

    public Texture2D CurrentTexture
    {
        get
        {
            return currentTexture;
        }
    }

    public InfluenceMap(GameObject plane, Vector2Int size, List<Influencer> units, List<Influencer> enemyUnits, List<Influencer> buildings)
    {
        transform = plane.transform;

        width = size.x;
        height = size.y;

        currentTexture = new Texture2D(width, height);
        currentTexture.filterMode = FilterMode.Point;
        currentTexture.anisoLevel = 0;
        textureLength = currentTexture.width * currentTexture.height;

        meshRenderer = plane.GetComponent<MeshRenderer>();

        //grid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();
        this.units = units;
        this.enemyUnits = enemyUnits;
        this.buildings = buildings;

        boxCollider = plane.GetComponent<BoxCollider>();

        
    }

    public void Update()
    {
        DrawFullColor(backgroundColor, currentTexture);

        currentColor = new Color(0, 0, 1, 1);   //Building Color -> Blue
        UpdateMap(buildings);
        currentColor = new Color(1, 0, 0, 1);   //Enemy Color -> Red
        UpdateMap(enemyUnits);
        currentColor = new Color(0, 1, 0, 1);   //Units Color -> Green
        UpdateMap(units);
        currentTexture.Apply();
    }

    void UpdateMap(List<Influencer> influencers)
    {
        Vector2Int pos;
        Vector3 influencerPos;
        for (int i = 0; i < influencers.Count; i++)
        {
            currentInfluencer = influencers[i];
            influencerPos = currentInfluencer.transform.position;
            pos = WorldToPixelSpace(influencerPos.x, influencerPos.z);
            DrawTexture(pos.x, pos.y, currentInfluencer._InfluencePower);
        }
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

    Vector3 PixelToWorldSpace(int x, int y)
    {
        float minDistanceX = transform.position.x - boxCollider.bounds.extents.x;
        float maxDistanceX = transform.position.x + boxCollider.bounds.extents.x;

        float minDistanceZ = transform.position.z - boxCollider.bounds.extents.z;
        float maxDistanceZ = transform.position.z + boxCollider.bounds.extents.z;

        float worldX = Mathf.Lerp(maxDistanceX, minDistanceX, Mathf.InverseLerp(0, width, x));
        float worldY = Mathf.Lerp(maxDistanceZ, minDistanceZ, Mathf.InverseLerp(0, height, y));

        return new Vector3(worldX, transform.position.y+100, worldY);
    }

    bool IsInsideRadius(Vector2Int pos)
    {
        Vector2Int influencerPos = WorldToPixelSpace(currentInfluencer.transform.position.x, currentInfluencer.transform.position.z);
        if ((pos - influencerPos).magnitude > currentInfluencer._InfluenceRadius)
            return false;
        return true;
    }

    void DrawTexture(int x, int y, float influence)
    {
        DrawTexture(x, y, 0, influence);
    }

    void DrawTexture(int x, int y, int currentInfluence, float influence)
    {
        if (currentInfluence <= currentInfluencer._InfluenceRadius
            && (x >= 0 && x < width) && (y >= 0 && y < height))
        {
            Color c = currentColor;
            c.a = influence;
            currentTexture.SetPixel(x, y, c);
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i != 0 && j == 0) //horizontal
                    {
                        PropagateHorizontal(i, x + i, y, currentInfluence + 1, influence * currentInfluencer._InfluencePropagationRatio);
                    }
                    else if (i == 0 && j != 0) //vertical
                    {
                        PropagateVertical(j, x, y + j, currentInfluence + 1, influence * currentInfluencer._InfluencePropagationRatio);
                    }
                    else if (i != 0 && j != 0) //diagonal
                    {
                        PropagateDiagonal(i, j, x + i, y + j, currentInfluence + 1, influence * currentInfluencer._InfluencePropagationRatio);
                    }
                }
            }
        }
    }

    void PropagateHorizontal(int dir, int x, int y, int currentInfluence, float influence)
    {
        if (currentInfluence == currentInfluencer._InfluenceRadius) return;

        Color c = currentColor;
        c.a = influence;

        for (int i = -(currentInfluence - 1); i < currentInfluence; i++)
        {
            if (x >= 0 && x < currentTexture.width && y + i >= 0 && y + i < currentTexture.height)
                if (IsInsideRadius(new Vector2Int(x, y + i)))
                {
                    Color previousColor = currentTexture.GetPixel(x, y + i);
                    if (previousColor == backgroundColor) currentTexture.SetPixel(x, y + i, c);
                    else currentTexture.SetPixel(x, y + i, CompareColors(previousColor, c));
                }
        }
        PropagateHorizontal(dir, x + dir, y, currentInfluence + 1, influence * currentInfluencer._InfluencePropagationRatio);
    }

    void PropagateVertical(int dir, int x, int y, int currentInfluence, float influence)
    {
        if (currentInfluence == currentInfluencer._InfluenceRadius) return;

        Color c = currentColor;
        c.a = influence;

        for (int i = -(currentInfluence - 1); i < currentInfluence; i++)
        {
            if (x + i >= 0 && x + i < currentTexture.width && y >= 0 && y < currentTexture.height)
                if (IsInsideRadius(new Vector2Int(x + i, y)))
                {
                    Color previousColor = currentTexture.GetPixel(x + i, y);
                    if (previousColor == backgroundColor) currentTexture.SetPixel(x + i, y, c);
                    else currentTexture.SetPixel(x + i, y, CompareColors(previousColor, c));
                }
        }
        PropagateVertical(dir, x, y + dir, currentInfluence + 1, influence * currentInfluencer._InfluencePropagationRatio);
    }

    void PropagateDiagonal(int dirX, int dirY, int x, int y, int currentInfluence, float influence)
    {
        if (currentInfluence == currentInfluencer._InfluenceRadius || !(y >= 0 && y < currentTexture.height && x >= 0 && x < currentTexture.width)) return;

        Color c = currentColor;
        c.a = influence;

        if (IsInsideRadius(new Vector2Int(x, y)))
        {
            Color previousColor = currentTexture.GetPixel(x, y);
            if (previousColor == backgroundColor) currentTexture.SetPixel(x, y, c);
            else currentTexture.SetPixel(x, y, CompareColors(previousColor, c));
        }
        PropagateDiagonal(dirX, dirY, x + dirX, y + dirY, currentInfluence + 1, influence * currentInfluencer._InfluencePropagationRatio);
    }

    Color CompareColors(Color pc, Color c)
    {
        Color nc = new Color();

        if (pc.a > c.a) nc = pc;
        else nc = c;

        if (pc.r != 0 && c.r != 0) nc.a = pc.a + c.a;
        else if (pc.g != 0 && c.g != 0) nc.a = pc.a + c.a;

        return nc;
    }

    void DrawFullColor(Color c, Texture2D texture)
    {
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                texture.SetPixel(i, j, c);
            }
        }
    }

    public List<Color>[] GetColorBuffer(HexGrid grid)
    {
        int nullCount = 0, notNullCount = 0;
        List<Color>[] buffer = new List<Color>[grid.cells.Length];
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = new List<Color>();
        }
        for (int x = 0; x < currentTexture.width; x++)
        {
            for (int y = 0; y < currentTexture.height; y++)
            {
                Vector3 pos = PixelToWorldSpace(x, y);
                Ray ray = new Ray(pos, Vector3.down);
                HexCell cell = grid.GetCell(ray);
                if (cell != null)
                {
                    notNullCount++;
                    //Debug.Log("NOT NULL" + pos);
                    if (buffer[cell.Index] == null) Debug.Log("Polla");
                    buffer[cell.Index].Add(currentTexture.GetPixel(x, y));
                }
                else
                {
                    nullCount++;
                    //Debug.Log("NULL" + pos);
                }
            }
        }
        //Debug.Log("NULL: " + nullCount + " - NotNULL: " + notNullCount);
        return buffer;
    }
}
