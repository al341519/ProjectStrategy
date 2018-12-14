using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class InfluenceMapsLegacy : MonoBehaviour {

    Texture2D texture;
    GameObject[] influencers;
    BoxCollider boxCollider;
    MeshRenderer meshRenderer;
    GameObject currentInfluencer;
    Influencer currentInfluencerFeatures;
    HexGrid grid;
    List<GameObject> debugGO;

    bool updateStarted = false;

    readonly float SQRT2 = Mathf.Sqrt(2);
    
    public int width = 100, height = 100;
    public Color backgroundColor = new Color(0,0,0,0);
    public int numberOfPlayers = 1;
    public int updateTime = 1;
    public GameObject debugPrefab;

	void Start () {

        texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.anisoLevel = 0;
        meshRenderer = GetComponent<MeshRenderer>();

        grid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();

        boxCollider = GetComponent<BoxCollider>();
        debugGO = new List<GameObject>();

        for (int i = 0; i < grid.cells.Length; i++)
        {
            debugGO.Add(Instantiate(debugPrefab, grid.cells[i].transform));
        }

        meshRenderer.material.SetTexture("_MainTex", texture);
    }

	void Update () {
        UpdateMap();
        if (!updateStarted)
            StartCoroutine("GridUpdate");
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
            DrawTexture(pos.x, pos.y, currentInfluencerFeatures._InfluencePower);
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
        if((pos-influencerPos).magnitude > currentInfluencerFeatures._InfluenceRadius)
            return false;
        return true;
    }

    void DrawTexture(int x, int y, float influence)
    {
        DrawTexture(x, y, 0, influence);
    }

    void DrawTexture(int x, int y, int currentInfluence, float influence)
    {
        if (currentInfluence <= currentInfluencerFeatures._InfluenceRadius
            && (x >= 0 && x < width) && (y >= 0 && y < height))
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
                        PropagateHorizontal(i, x + i, y, currentInfluence + 1, influence * currentInfluencerFeatures._InfluencePropagationRatio);
                    }
                    else if(i==0 && j != 0) //vertical
                    {
                        PropagateVertical(j, x, y + j, currentInfluence + 1, influence * currentInfluencerFeatures._InfluencePropagationRatio);
                    }
                    else if(i!=0 && j!=0) //diagonal
                    {
                        PropagateDiagonal(i, j, x+i, y+j, currentInfluence+1, influence* currentInfluencerFeatures._InfluencePropagationRatio);
                    }
                }
            }
        }
    }

    void PropagateHorizontal(int dir, int x, int y, int currentInfluence, float influence)
    {
        if (currentInfluence == currentInfluencerFeatures._InfluenceRadius) return;

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
        PropagateHorizontal(dir, x + dir, y, currentInfluence + 1, influence * currentInfluencerFeatures._InfluencePropagationRatio);
    }

    void PropagateVertical(int dir, int x, int y, int currentInfluence, float influence)
    {
        if (currentInfluence == currentInfluencerFeatures._InfluenceRadius) return;

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
        PropagateVertical(dir, x, y+dir, currentInfluence + 1, influence * currentInfluencerFeatures._InfluencePropagationRatio);
    }
    
    void PropagateDiagonal(int dirX, int dirY, int x, int y, int currentInfluence, float influence)
    {
        if (currentInfluence == currentInfluencerFeatures._InfluenceRadius || !(y >= 0 && y < texture.height && x >= 0 && x < texture.width)) return;

        Color c = currentInfluencerFeatures.color;
        c.a = influence;

        if (IsInsideRadius(new Vector2Int(x, y)))
        {
            Color previousColor = texture.GetPixel(x, y);
            if (previousColor == backgroundColor) texture.SetPixel(x, y, c);
            else texture.SetPixel(x, y, CompareColors(previousColor, c));
        }
        PropagateDiagonal(dirX, dirY, x + dirX, y + dirY, currentInfluence + 1, influence * currentInfluencerFeatures._InfluencePropagationRatio);
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

    IEnumerator GridUpdate()
    {
        updateStarted = true;
        SetMapValues();
        yield return new WaitForSeconds(updateTime);
        print("Updated");
        updateStarted = false;
    }

    void SetMapValues()
    {
        List<Color>[] buffer = new List<Color>[grid.cells.Length];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                HexCell cell = grid.GetCell(PixelToWorldSpace(i, j));
                if (buffer[cell.Index] == null) buffer[cell.Index] = new List<Color>();
                buffer[cell.Index].Add(texture.GetPixel(i, j));
            }
        }
        for (int i = 0; i < buffer.Length; i++)
        {
            Vector4 finalColor = new Vector4();
            if(buffer[i] != null){
                foreach (Color c in buffer[i])
                {
                    finalColor.x += c.r;
                    finalColor.y += c.g;
                    finalColor.z += c.b;
                    finalColor.w += c.a;
                }
                Color col = new Color(finalColor.x / buffer[i].Count,
                                    finalColor.y / buffer[i].Count,
                                    finalColor.z / buffer[i].Count,
                                    finalColor.w / buffer[i].Count
                                    );
                grid.cells[i].influence = col;
                print(col);
                debugGO[i].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
            }
        }
        /*foreach(HexCell cell in grid.cells)
        {
            ShowInfluence(cell);
        }*/
    /*}

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

    void ShowInfluence(HexCell cell)
    {
        foreach (GameObject o in debugGO)
        {
            o.GetComponent<MeshRenderer>().material.color = cell.influence;
        }
    }
}*/
