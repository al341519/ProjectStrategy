using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTester : MonoBehaviour {

    public GameObject map;
    public InfluenceMapSystem sys;

    BoxCollider boxCollider;
    Transform planeTransform;
    int width, height;

    // Use this for initialization
    void Start () {

        width = sys._Size.x;
        height = sys._Size.y;

        boxCollider = map.GetComponent<BoxCollider>();
        planeTransform = map.transform;

	}
	
	// Update is called once per frame
	void Update () {
        Vector2Int pixelPos = WorldToPixelSpace(transform.position.x, transform.position.z);
        print("PixelSpace: " + pixelPos + " - WorldSpaceConverted: " + PixelToWorldSpace(pixelPos.x, pixelPos.y));
	}

    Vector2Int WorldToPixelSpace(float x, float y)
    {
        float minDistanceX = planeTransform.position.x - boxCollider.bounds.extents.x;
        float maxDistanceX = planeTransform.position.x + boxCollider.bounds.extents.x;

        float minDistanceZ = planeTransform.position.z - boxCollider.bounds.extents.z;
        float maxDistanceZ = planeTransform.position.z + boxCollider.bounds.extents.z;

        int pixelX = (int)Mathf.Lerp(width, 0, Mathf.InverseLerp(minDistanceX, maxDistanceX, x));
        int pixelY = (int)Mathf.Lerp(height, 0, Mathf.InverseLerp(minDistanceZ, maxDistanceZ, y));

        return new Vector2Int(pixelX, pixelY);
    }

    Vector3 PixelToWorldSpace(int x, int y)
    {
        float minDistanceX = planeTransform.position.x - boxCollider.bounds.extents.x;
        float maxDistanceX = planeTransform.position.x + boxCollider.bounds.extents.x;

        float minDistanceZ = planeTransform.position.z - boxCollider.bounds.extents.z;
        float maxDistanceZ = planeTransform.position.z + boxCollider.bounds.extents.z;

        float worldX = Mathf.Lerp(maxDistanceX, minDistanceX, Mathf.InverseLerp(0, width, x));
        float worldY = Mathf.Lerp(maxDistanceZ, minDistanceZ, Mathf.InverseLerp(0, height, y));

        return new Vector3(worldX, transform.position.y + 100, worldY);
    }

}
