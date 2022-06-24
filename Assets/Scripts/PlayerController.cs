using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform deathCube;
    public static bool isSafty;
    private Transform finish;
    private NavMeshAgent agent;
    private Vector3 destination;
    private Color saftyColor = new Color(0.6784314f, 1f, 0.1843137f, 1f);
    private Color baseColor;
    private Material mat;
    private Rigidbody rb;

    public static event onStateChange onGameOverEvent;
    public delegate void onStateChange();

    // Start is called before the first frame update
    void Start()
    {
        TouchController.onStartTouch += SaftyMode;
        TouchController.onFinishTouch += BaseMode;
        UIController.onPauseEvent += Pause;
        UIController.onRestartEvent += Restart;

        mat = transform.GetComponent<MeshRenderer>().material;
        rb = transform.GetComponent<Rigidbody>();
        baseColor = mat.color;
        agent = GetComponent<NavMeshAgent>();        
        finish = GameObject.Find("FinishSpace").transform;
        destination = new Vector3(finish.position.x, 0f, finish.position.z);

        if (!UIController.isPaused) {
            agent.isStopped = false;
            StartCoroutine(StartMove());
        }      
        
    }

    private void Pause()
    {        
        agent.isStopped = true;
        rb.isKinematic = true;        
    }

    private void Restart()
    {
        agent.isStopped = false;
        StartCoroutine(StartMove());
        rb.isKinematic = false;
    }

    private void SaftyMode()
    {
        isSafty = true;
        mat.DOColor(saftyColor, 0.2f);
        StartCoroutine(EndShild());
    }

    private void BaseMode()
    {
        isSafty = false;
        StopAllCoroutines();
        mat.DOColor(baseColor, 0.2f);
    }

    IEnumerator StartMove()
    {
        yield return new WaitForSeconds(2f);
        agent.SetDestination(destination);
    }

    IEnumerator EndShild()
    {
        yield return new WaitForSeconds(2f);
        BaseMode();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Finish") return;
        if (!isSafty) Respawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Finish") return;

        EndGame();
    }

    private void Respawn()
    {
        agent.isStopped = true;
        Instantiate(deathCube, new Vector3(transform.position.x, 0.3f, transform.position.z), Quaternion.identity);
        TouchController.onStartTouch -= SaftyMode;
        TouchController.onFinishTouch -= BaseMode;
        UIController.onPauseEvent -= Pause;
        UIController.onRestartEvent -= Restart;
        Destroy(this.gameObject);
    }

    private void EndGame()
    {
        agent.isStopped = true;
        TouchController.onStartTouch -= SaftyMode;
        TouchController.onFinishTouch -= BaseMode;
        UIController.onPauseEvent -= Pause;
        UIController.onRestartEvent -= Restart;
        transform.GetChild(0).gameObject.SetActive(true);

        StartCoroutine(EndConfetti());
    }

    IEnumerator EndConfetti()
    {
        yield return new WaitForSeconds(3f);
        onGameOverEvent?.Invoke();          
    }
}
