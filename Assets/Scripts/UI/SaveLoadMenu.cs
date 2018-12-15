using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class SaveLoadMenu : MonoBehaviour {

	const int mapFileVersion = 5;

	public Text menuLabel, actionButtonLabel;

	public InputField nameInput;

	public RectTransform listContent;

	public SaveLoadItem itemPrefab;

	public HexGrid hexGrid;

	bool saveMode;

	public void Open (bool saveMode) {
		this.saveMode = saveMode;
		if (saveMode) {
			menuLabel.text = "Save Map";
			actionButtonLabel.text = "Save";
		}
		else {
			menuLabel.text = "Load Map";
			actionButtonLabel.text = "Load";
		}
		FillList();
		gameObject.SetActive(true);
		HexMapCamera.Locked = true;
	}

	public void Close () {
		gameObject.SetActive(false);
		HexMapCamera.Locked = false;
	}

	public void Action () {
		string path = GetSelectedPath();
		if (path == null) {
			return;
		}
		if (saveMode) {
			Save(path);
		}
		else {
			Load(path);
		}
		Close();
	}

	public void SelectItem (string path) {
		nameInput.text = path;
	}

	public void Delete () {
		string path = GetSelectedPath();
		if (path == null) {
			return;
		}
		if (File.Exists(path)) {
			File.Delete(path);
		}
		nameInput.text = "";
		FillList();
	}

	void FillList () {
		for (int i = 0; i < listContent.childCount; i++) {
			Destroy(listContent.GetChild(i).gameObject);
		}
		string[] paths =
			Directory.GetFiles(Application.persistentDataPath, "*.map");
        print(Application.persistentDataPath);
        string[] anotherPaths =
            Directory.GetFiles(Application.dataPath + "/CustomMaps", "*.map");
        Array.Resize(ref paths, paths.Length + anotherPaths.Length);
        Array.ConstrainedCopy(anotherPaths, 0, paths, paths.Length-1, anotherPaths.Length);
		Array.Sort(paths);
		for (int i = 0; i < paths.Length; i++) {
			SaveLoadItem item = Instantiate(itemPrefab);
			item.menu = this;
            item.MapName = Path.GetFileNameWithoutExtension(paths[i]);//Path.GetFileName(paths[i]);
            item.Path = Path.GetFullPath(paths[i]);
			item.transform.SetParent(listContent, false);
		}
	}

	string GetSelectedPath () {
		string mapPath = nameInput.text;
		if (mapPath.Length == 0) {
			return null;
		}
		return mapPath;
	}

	void Save (string path) {
		using (
			BinaryWriter writer =
			new BinaryWriter(File.Open(path, FileMode.Create))
		) {
			writer.Write(mapFileVersion);
			hexGrid.Save(writer);
		}
	}

	void Load (string path) {
		if (!File.Exists(path)) {
			Debug.LogError("File does not exist " + path);
			return;
		}
		using (BinaryReader reader = new BinaryReader(File.OpenRead(path))) {
			int header = reader.ReadInt32();
			if (header <= mapFileVersion) {
				hexGrid.Load(reader, header);
				HexMapCamera.ValidatePosition();
			}
			else {
				Debug.LogWarning("Unknown map format " + header);
			}
		}
	}
}