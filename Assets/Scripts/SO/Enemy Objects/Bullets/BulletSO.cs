using UnityEngine;

[CreateAssetMenu(fileName = "BulletSO_", menuName = "Bullet Hell Mechanics/Create Bullet...")]
public class BulletSO : ScriptableObject
{
    [Header("Visuals Bullet Settings")]
    [Tooltip("Sprite of the bullet")]
    public Sprite bulletSprite;

    [Header("General Bullet Settings"), Tooltip("When toggles on, allows you to modify the size of the bullet sprite")]
    public bool modifySize;

    [Tooltip("This will set the Scale attribute on x and y axis for the entire bullet (changes size of collider)")]
    public float bulletSizeModifier;

    [Tooltip("This will set the Scale attribute on x and y axis for only the Sprite (no collider changes)")]
    public float spriteSizeModifier;

    [Header("Mechanics Bullet Settings")]
    [Tooltip("The bullet type to determine behavior")]
    public BulletType bulletType;


    public int damageAmount;

    // Optionally store references to Bullet Patterns

    [Tooltip("What pattern should be emitted? It will inherit all settings from this pattern")]
    public BulletPatternSO bulletPattern;

    
    [Header("Emitter Settings")]
    [Tooltip("How long should it be between each pattern instance?")]
    public float delayBetween; // Delay between bursts for emitter bullets

    //[field: BoxGroup("Mechanics Bullet Settings"), Tooltip("How many freeze frames happen when this bullet dies"), SerializeField]
    //public float FreezeFramesOnDeath { get; private set; }

    // AUDIO
    //[Header("Audio Settings"), SerializeField]
    //[Tooltip("Plays on fire")]
    //public EventReference FireSound { get; private set; }

    //[Header("Audio Settings"), SerializeField]
    //[Tooltip("Plays on destruction or hitting something")]
    //public EventReference BreakSound { get; private set; }

    // Lighting
    [Header("Lighting Settings"), SerializeField]
    [Tooltip("Plays on fire")]
    public Color LightingColor { get; private set; }

    [Tooltip("Plays on destruction or hitting something")]
    public float LightIntensity { get; private set; }

    // VFX
    //[Header("VFX Settings"), SerializeField]
    //[field: Tooltip("Summons on fire")]
    //public ParticleSO SummonParticle { get; private set; }

    //[field: BoxGroup("VFX Settings"), SerializeField]
    //[field: Tooltip("Summons on destruction")]
    //public ParticleSO BreakParticle { get; private set; }



    public enum BulletType
    {
        Standard,  // A regular bullet
        Emitter,    // A bullet that emits a pattern over time
        EmitOnDeath // Emits a pattern on Death
    }

    public static class BulletHandlerFactory
    {
        public static IBulletHandler GetHandler(BulletType bulletType)
        {
            switch (bulletType)
            {
                case BulletType.Standard:
                    return new StandardBulletHandler();
                case BulletType.Emitter:
                    return new EmitterBulletHandler();
                case BulletType.EmitOnDeath:
                    return new DeathBasedEmitterBulletHandler();
                default:
                    Debug.LogError("No handler found for bullet type: " + bulletType + ", using " + BulletType.Standard);
                    return new StandardBulletHandler();
            }
        }
    }
}
