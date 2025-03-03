using UnityEngine;

[CreateAssetMenu(fileName = "ClubStats", menuName = "Clubs")]
public class ClubStats : ScriptableObject
{
    public string clubName;
    public float forceMultiplier;
    public float ballMass;
    public float linearDamping;
    public float angularDamping;
}
