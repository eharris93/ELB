using UnityEngine;

public class Player : MonoBehaviour
{
    public TileData CommanderPosition;
    public PlayerType Type;
    public PointsSystem currency;
    public PlayerCards hand;

    public void SetPosition()
    {

    }
}
public enum PlayerType
{
    None,
    Battlebeard,
    Stormshaper
}