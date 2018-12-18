using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadMapOnPlay : MonoBehaviour {

    HexGrid hexGrid;
    const int mapFileVersion = 5;

    public bool _LoadOnStart = true;
    public string _FileName = "CustomMap1";

	// Use this for initialization
	void Start () {
        hexGrid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();

        StartCoroutine(charge());
        if(_LoadOnStart)
            Load(Application.persistentDataPath + "/" + _FileName + ".map");
	}


    IEnumerator charge()
    {
        yield return new WaitForEndOfFrame();
        Load(Application.persistentDataPath +"/" + _FileName + ".map");

    }

    void Load(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("File does not exist " + path);
            return;
        }
        using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        {
            int header = reader.ReadInt32();
            if (header <= mapFileVersion)
            {
                hexGrid.Load(reader, header);
                HexMapCamera.ValidatePosition();
            }
            else
            {
                Debug.LogWarning("Unknown map format " + header);
            }
        }
    }
}
