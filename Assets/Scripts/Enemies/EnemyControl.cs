using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{

    public enum PlayerState
    {
        INIT, PATROL, DETECTSOUND, TALK
    }
    public PlayerState state;
    private NavMeshAgent _agent;
    public List<GameObject> wayPoints;
    private int _currentWayPoints;
    public float remainingDistance, stairsSpeed = 0.5f;
    private GameObject playerDetect = null;
    private RaycastHit HitInfo;
    private Vector3 soundDetect;
    private float _timer = 0f, _timerSound = 0f;
    private Animator anim;
    private float initSpeed, stairsUp = 0, stairsDw = 0;
    private int idleCount = 0;
    private bool firstCol = false, mixAnim = false, wasDetect = false;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        initSpeed = _agent.speed;
        ChangeState(PlayerState.INIT);
        anim = transform.GetChild(1).GetComponent<Animator>();
    }
    void Update()
    {

        switch (state)
        {
            case PlayerState.INIT:
                anim.SetBool("walk", true);
                anim.SetBool("detect", false);
                _agent.speed = initSpeed;
                _agent.SetDestination(wayPoints[_currentWayPoints].transform.position);
                ChangeState(PlayerState.PATROL);
                break;
            case PlayerState.PATROL:
                if (mixAnim == true)
                {
                    stairsUp += Time.deltaTime;
                    stairsDw += Time.deltaTime;
                }
                else
                {
                    stairsUp -= Time.deltaTime;
                    stairsDw -= Time.deltaTime;
                }
                anim.SetFloat("BlendUp", stairsUp);
                anim.SetFloat("BlendDw", stairsDw);
                if (_agent.remainingDistance < remainingDistance)
                {
                    anim.SetBool("walk", true);
                    _currentWayPoints++;
                    StartCoroutine(IdleWayPoints());
                    if (_currentWayPoints >= wayPoints.Count) _currentWayPoints = 0;
                    _agent.SetDestination(wayPoints[_currentWayPoints].transform.position);
                }
                if (playerDetect != null) StartCoroutine(DetectPlayerEnum());        
                if(wasDetect == true)
                {
                    if(playerDetect == null)
                    {                        
                        wasDetect = false;
                        ChangeState(PlayerState.INIT);
                    }
                }
                break;
            case PlayerState.DETECTSOUND:
                anim.SetBool("noise", true);
                _agent.SetDestination(soundDetect);
                if (_agent.remainingDistance < 0.5)
                {
                    _timerSound += Time.deltaTime;
                    if (_timerSound >= 3f)
                    {
                        _timerSound = 0f;
                        ChangeState(PlayerState.PATROL);
                    }
                }
                if (playerDetect != null)
                {
                    StartCoroutine(DetectPlayerEnum());
                }
                break;
            case PlayerState.TALK:
                _agent.speed = 0f;
                //condicio parlar amb el 1 o amb el 2
                //anim.SetInteger("talk", 1);
                //anim.SetInteger("talk", 2);
                //al acabar tornar a init
                break;
             
        }
    }
    //Corrutina parar para hacer idle al llegar a un punto de la patrulla
    IEnumerator IdleWayPoints()
    {
        anim.SetBool("walk", false);
        _agent.speed = 0f;
        if (idleCount == 0) anim.SetInteger("idle", 1);
        else anim.SetInteger("idle", 2);
        idleCount++;
        if (idleCount > 2) idleCount = 0;
        yield return new WaitForSeconds(6.5f);
        anim.SetBool("walk", true);
        _agent.speed = initSpeed;
    }
    IEnumerator DetectPlayerEnum()
    {
        //instanciar sonido detectado
        anim.SetBool("walk", false);
        _agent.speed = 0f;
        anim.SetBool("detect", true);
        wasDetect = true;
        yield return new WaitForSeconds(1);
        //Reiniciar a spawn al temps que decidim
    }
    private void AlertPlayer()
    {
        _timer += Time.deltaTime;
        if (_timer > 4)
        {
            _timer = 0;
            ChangeState(PlayerState.INIT);
        }
    }
    public void OnChildTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerDetect = other.gameObject;
        }
    }
    public void OnChildTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerDetect = null;
        }
    }
    private void ChangeState(PlayerState _newState)
    {
        print(_newState);
        state = _newState;
    }
    public void GetFinalPos(Vector3 _pos)
    {
        ChangeState(PlayerState.DETECTSOUND);
        soundDetect = _pos;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "escaleraUp")
        {
            anim.SetInteger("idle", 0);
            anim.SetBool("walk", false);
            if (!firstCol) UpStairs();
        }
        if (other.tag == "escaleraDw")
        {
            anim.SetInteger("idle", 0);
            anim.SetBool("walk", false);
            if (!firstCol) DownStairs();
        }
        if (other.tag == "escalera")
        {
            mixAnim = true;
            stairsDw = 0;
            stairsUp = 0;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "escalera")
        {
            anim.SetBool("walk", true);
            anim.SetInteger("stairs", 0);
            firstCol = false;
            _agent.speed = initSpeed;
            mixAnim = false;
            stairsDw = 1;
            stairsUp = 1;
        }
    }
    private void UpStairs()
    {
        firstCol = true;
        _agent.speed = stairsSpeed;
        anim.SetInteger("stairs", 1);
    }
    private void DownStairs()
    {
        firstCol = true;
        _agent.speed = stairsSpeed;
        anim.SetInteger("stairs", 2);
    }
}
