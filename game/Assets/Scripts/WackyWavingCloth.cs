using UnityEngine;

[RequireComponent(typeof(Cloth))]
public class WackyWavingCloth : MonoBehaviour
{
  private Cloth cloth;
  public float pulseAccel = 2;
  public float randomForce = 12;

  void Start()
  {
    cloth = GetComponent<Cloth>();
  }

  void FixedUpdate()
  {
    cloth.externalAcceleration = -Physics.gravity * pulseAccel + Random.insideUnitSphere * randomForce;
  }
}
