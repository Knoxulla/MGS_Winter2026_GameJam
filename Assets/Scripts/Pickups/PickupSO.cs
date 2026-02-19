using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickupSO", menuName = "Mechanics/Drops/Pickup")]
public class PickupSO : ScriptableObject
{
    [field: Header("Essential")]
    [field: SerializeField, Tooltip("Will this pickup affect players or enemies")]
    public PickupTarget PickupTargetEffector { get; private set; }


    [field: Space(10)]

    [field: Header("Effects"), SerializeField]
    public bool AffectHealth { get; private set; }

    public bool AffectFaith { get; private set; }

    public bool AffectCurrency { get; private set; }

    [field: Space(10)]

    [field: Header("Lifetime"), SerializeField]
    public float TimeBeforeFadeout { get; private set; }
    public float TimeToFadeOut { get; private set; }

    [field: Space(10)]

    [field: Header("Visuals"), SerializeField]
    public List<Sprite> PickupSprites { get; private set; }

    public Color SpriteColor = Color.white;


    public Color LightColor = Color.white;


    public Vector2 PickupSize = new Vector2(1, 1);


    public float LightIntensity{ get; private set; }


    public float RotationSpeed { get; private set; }


    public Vector2 FloatAmp { get; private set; }

    public Vector2 FloatSpeed { get; private set; }

    [field: Space(10)]

    [field: Header("Movement"), SerializeField]
    public bool StaticPickup { get; private set; }

    public LayerMask MovementTargetLayers { get; private set; }

    [field: Space(10)]


    public float ScanRadius { get; private set; }


    public float PickupSpeed { get; private set; }

    public float Acceleration { get; private set; }


    public float AccelerationRate { get; private set; }

    [field: Space(10)]


    public bool Rotateability { get; private set; }

    [field: Space(10)]

    public float KickSpeed { get; private set; }

    public float KickRange { get; private set; }

    public float Mass { get; private set; }

    public float Damping { get; private set; }


    [field: Space(20)]

        
    [field: Header("Misc"), SerializeField]
    public bool CheckTargetHealth { get; private set; }

   
}

public enum PickupTarget
{
    Players,
    Enemies
}
