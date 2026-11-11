using UnityEngine;

public class PlayerParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem shootParticleSystem;


    private void Start()
    {
        var main = shootParticleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }
    private void OnParticleSystemStopped()
    {
        Debug.Log("Il ParticleSystem ha terminato.");
        if (shootParticleSystem.gameObject.activeSelf)
        {
            shootParticleSystem.gameObject.SetActive(false);
        }
        else Debug.Log("Il ParticleSystem non era attivo quando ha terminato.");
    }

    private void OnEnable()
    {
        shootParticleSystem.Play();
    }
}
