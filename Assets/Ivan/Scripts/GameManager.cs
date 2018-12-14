using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    int[] recursos;

    HexCell cell;

    int madera;
    int trigo;
    int metal;
    int aldeanos;
    int vida;

    int x;
    int y;

    public Texture2D IconContainer;
    public Texture2D iconMine;
    public Texture2D iconHoverMine;
    public Texture2D iconCastle;
    public Texture2D iconHoverCastle;


    float timer;
    
    private HexCell abc;
    Ray ray;
    RaycastHit hit;
    Vector3 abcd;

    public HexGrid hexGrid;
    bool casillaElegida;

    Player player1;
    Player player2;

    private void Awake()
    {
        player1 = new Player(1);
        player2 = new Player(2);

    }

    public Player getPlayer(int i) {
        if(i == 1)
        {
            return player1;
        }
        else
        {
            return player2;
        }
    }

    private void Update()
    {
        Debug.Log("madera: " + player1.Madera + " hierro: " + player1.Metal + " comida: " + player1.Trigo);

        timer += Time.deltaTime;

        if(timer >= 1)
        {
            foreach (HexCell hexCell in hexGrid.GetCells()) {
                hexCell.Do();
                timer = 0;
            }
        }
    }


}
