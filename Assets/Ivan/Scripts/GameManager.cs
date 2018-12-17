using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    int[] recursos;

    HexCell cell;

    int madera;
    int trigo;
    int piedra;
    int aldeanos;
    int vida;

    int x;
    int y;

    public Texture2D IconContainer;
    public Texture2D iconMine;
    public Texture2D iconHoverMine;
    public Texture2D iconCastle;
    public Texture2D iconHoverCastle;

    public Text text;


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
        text.text = "Md: " + player1.Madera + " Pd: " + player1.Piedra + " Cm: " + player1.Trigo + " Ald: " + player1.Aldeano;

        timer += Time.deltaTime;

        if(timer >= HexMetrics.tiempo)
        {
            foreach (HexCell hexCell in hexGrid.GetCells()) {
                hexCell.Do();
                timer = 0;
            }
        }
    }


}
