using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class HexUnit : MonoBehaviour
{

    [Header("Marcos")]//Cambio para dañar edif
    private UnitClass unidadDamage;


    [Header("Code origen")]
    const float rotationSpeed = 180f;
    const float travelSpeed = 1f;//4f original

    public static HexUnit unitPrefab;

    public HexGrid Grid { get; set; }

    public HexCell Location
    {
        get
        {
            return location;
        }
        set
        {
            if (location)
            {
                Grid.DecreaseVisibility(location, VisionRange);
                location.Unit = null;
            }
            location = value;
            value.Unit = this;
            Grid.IncreaseVisibility(value, VisionRange);
            transform.localPosition = value.Position;
            Grid.MakeChildOfColumn(transform, value.ColumnIndex);
        }
    }

    HexCell location, currentTravelLocation;

    public float Orientation
    {
        get
        {
            return orientation;
        }
        set
        {
            orientation = value;
            transform.localRotation = Quaternion.Euler(0f, value, 0f);
        }
    }

    public int Speed
    {
        get
        {
            return 6;//origen 24
        }
    }

    public int VisionRange
    {
        get
        {
            return 3;
        }
    }

    float orientation;

    List<HexCell> pathToTravel;

    public void ValidateLocation()
    {
        transform.localPosition = location.Position;
    }

    public bool IsValidDestination(HexCell cell)
    {
        //return cell.IsExplored && !cell.IsUnderwater && !cell.Unit; Mod Marcos

        if (cell.IsExplored && !cell.IsUnderwater && !cell.Unit)
        {
            return true;
        }
        else if (cell.Unit)
        {
            if (cell.Unit.tag != this.tag)
            {
                return true;
            }
        }
        else if (cell.SpecialIndex != 0)
        {
            if (this.tag == "AllyUnit" && cell.edificio.tag == "edificioEnemigo"/*cell.Owner!=1*/)
            {
                return true;
            }
            else if (this.tag == "EnemyUnit" && cell.Owner != 2)
            {
                return true;
            }
        }
        return false;
    }

    public void Travel(List<HexCell> path)
    {
        if (!location)
        {
            location.Unit = null;
            location = path[path.Count - 1];
            location.Unit = this;
        }
        /*location.Unit = null;
		location = path[path.Count - 1];
		location.Unit = this;*/
        pathToTravel = path;
        StopAllCoroutines();
        StartCoroutine(TravelPath());
    }

    IEnumerator TravelPath()
    {
        unidadDamage = this.GetComponent<UnitClass>();
        Vector3 a, b, c = pathToTravel[0].Position;
        yield return LookAt(pathToTravel[1].Position);

        if (!currentTravelLocation)
        {
            currentTravelLocation = pathToTravel[0];
        }
        Grid.DecreaseVisibility(currentTravelLocation, VisionRange);
        int currentColumn = currentTravelLocation.ColumnIndex;

        float t = Time.deltaTime * travelSpeed;
        for (int i = 1; i < pathToTravel.Count; i++)
        {
            currentTravelLocation = pathToTravel[i];
            a = c;
            b = pathToTravel[i - 1].Position;


            //Marcos
            if (pathToTravel[i].Unit)
            {//Mirar para rodear a unidades enemigas
                Debug.Log("HOLAAAAA hay una unidad");
				if (pathToTravel [i].Unit.tag != this.tag) {
					//Coger el rango de ataque y en base a eso calcular
					if (this.GetComponent<UnitClass> ().attackRange > 1/* && pathToTravel [i - 1] != location */&& i > this.GetComponent<UnitClass> ().attackRange) {
						Debug.Log ("Estoy en el if del rango");
						location.Unit = null;
						location = pathToTravel [i - (int)this.GetComponent<UnitClass> ().attackRange];
						location.Unit = this;
						this.GetComponent<UnitClass> ().targetPut (pathToTravel [i].Unit.GetComponent<HexUnit> ());
						break;
					} else {
						
						location.Unit = null;
						location = pathToTravel [i - 1];
						location.Unit = this;
						this.GetComponent<UnitClass> ().targetPut (pathToTravel [i].Unit.GetComponent<HexUnit> ());
						break;
					}
				} else if (pathToTravel [i].Unit.tag == this.tag) {
					location.Unit = null;
					location = pathToTravel [i - 1];
					location.Unit = this;
					break;
				}
            }//Cambio para dañar edif

            else if (pathToTravel[i].edificio)
            {
                //Debug.Log ("he visto que hay un edificio");
                if (/*pathToTravel [i].Owner!=1 &&*/ this.tag == "AllyUnit")
                {
                    location.Unit = null;
                    location = pathToTravel[i - 1];
                    location.Unit = this;
                    //Debug.Log ("El edificio seleccionado es: "+pathToTravel[i].edificio.name);
                    this.GetComponent<UnitClass>().targetPut(pathToTravel[i].edificio.GetComponent<Health>());
                    break;
                }
            }


            else {
                location.Unit = null;
                location = pathToTravel[i];
                location.Unit = this;

            }

            int nextColumn = currentTravelLocation.ColumnIndex;
            if (currentColumn != nextColumn)
            {
                if (nextColumn < currentColumn - 1)
                {
                    a.x -= HexMetrics.innerDiameter * HexMetrics.wrapSize;
                    b.x -= HexMetrics.innerDiameter * HexMetrics.wrapSize;
                }
                else if (nextColumn > currentColumn + 1)
                {
                    a.x += HexMetrics.innerDiameter * HexMetrics.wrapSize;
                    b.x += HexMetrics.innerDiameter * HexMetrics.wrapSize;
                }
                Grid.MakeChildOfColumn(transform, nextColumn);
                currentColumn = nextColumn;
            }

            c = (b + currentTravelLocation.Position) * 0.5f;
            Grid.IncreaseVisibility(pathToTravel[i], VisionRange);

            for (; t < 1f; t += Time.deltaTime * travelSpeed)
            {
                transform.localPosition = Bezier.GetPoint(a, b, c, t);
                Vector3 d = Bezier.GetDerivative(a, b, c, t);
                d.y = 0f;
                transform.localRotation = Quaternion.LookRotation(d);
                yield return null;
            }
            Grid.DecreaseVisibility(pathToTravel[i], VisionRange);
            t -= 1f;
        }
        currentTravelLocation = null;

        a = c;
        b = location.Position;
        c = b;
        Grid.IncreaseVisibility(location, VisionRange);
        for (; t < 1f; t += Time.deltaTime * travelSpeed)
        {
            transform.localPosition = Bezier.GetPoint(a, b, c, t);
            Vector3 d = Bezier.GetDerivative(a, b, c, t);
            d.y = 0f;
            transform.localRotation = Quaternion.LookRotation(d);
            yield return null;
        }

        transform.localPosition = location.Position;
        orientation = transform.localRotation.eulerAngles.y;
        ListPool<HexCell>.Add(pathToTravel);
        pathToTravel = null;
    }

    IEnumerator LookAt(Vector3 point)
    {
        if (HexMetrics.Wrapping)
        {
            float xDistance = point.x - transform.localPosition.x;
            if (xDistance < -HexMetrics.innerRadius * HexMetrics.wrapSize)
            {
                point.x += HexMetrics.innerDiameter * HexMetrics.wrapSize;
            }
            else if (xDistance > HexMetrics.innerRadius * HexMetrics.wrapSize)
            {
                point.x -= HexMetrics.innerDiameter * HexMetrics.wrapSize;
            }
        }

        point.y = transform.localPosition.y;
        Quaternion fromRotation = transform.localRotation;
        Quaternion toRotation =
            Quaternion.LookRotation(point - transform.localPosition);
        float angle = Quaternion.Angle(fromRotation, toRotation);

        if (angle > 0f)
        {
            float speed = rotationSpeed / angle;
            for (
                float t = Time.deltaTime * speed;
                t < 1f;
                t += Time.deltaTime * speed
            )
            {
                transform.localRotation =
                    Quaternion.Slerp(fromRotation, toRotation, t);
                yield return null;
            }
        }

        transform.LookAt(point);
        orientation = transform.localRotation.eulerAngles.y;
    }

    public int GetMoveCost(
        HexCell fromCell, HexCell toCell, HexDirection direction)
    {
        if (!IsValidDestination(toCell))
        {
            return -1;
        }
        HexEdgeType edgeType = fromCell.GetEdgeType(toCell);
        if (edgeType == HexEdgeType.Cliff)
        {
            return -1;
        }
        int moveCost;
        if (fromCell.HasRoadThroughEdge(direction))
        {
            moveCost = 1;
        }
		else if (fromCell.Walled != toCell.Walled )
        {
            return -1;
        }
        else {
            CellClass tCell = toCell.GetComponent<CellClass>();
            //Debug.Log ("El coste es: "+tCell.mobility);
            moveCost = edgeType == HexEdgeType.Flat ? 5 : 10;
            moveCost +=
                toCell.UrbanLevel + toCell.FarmLevel + toCell.PlantLevel + tCell.mobility;//Tocar el valor de coste de aquí para tener en cuenta el tipo de casilla
            if (toCell.Unit && toCell.Unit.tag != this.tag)
            {
                Debug.Log("Aumenta el coste");
                moveCost += 20000;
            }
        }
        return moveCost;
    }

    public void Die()
    {
        if (location)
        {
            Grid.DecreaseVisibility(location, VisionRange);
        }
        location.Unit = null;
        Destroy(gameObject);
    }

    public void Save(BinaryWriter writer)
    {
        location.coordinates.Save(writer);
        writer.Write(orientation);
    }

    public static void Load(BinaryReader reader, HexGrid grid)
    {
        HexCoordinates coordinates = HexCoordinates.Load(reader);
        float orientation = reader.ReadSingle();
        grid.AddUnit(
            Instantiate(unitPrefab), grid.GetCell(coordinates), orientation
        );
    }

    void OnEnable()
    {
        if (location)
        {
            transform.localPosition = location.Position;
            if (currentTravelLocation)
            {
                Grid.IncreaseVisibility(location, VisionRange);
                Grid.DecreaseVisibility(currentTravelLocation, VisionRange);
                currentTravelLocation = null;
            }
        }
    }
}