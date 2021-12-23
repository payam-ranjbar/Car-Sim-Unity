using System.Collections;
using DG.Tweening;
using Traffic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Navigation
{
    public class Pedestrian : MonoBehaviour
    {
        public Animator animator;
        public PedestrianProfile profile;
        public int startPos;
        private bool isIdle;
        private int currentNode = 0;
        private bool block;
        private bool _iRunning;

        private string MoveAnimTag => !_iRunning ? profile.walkTrigger : profile.runTrigger;
        private float MaxSpeed => !_iRunning ? profile.maxSpeed : profile.maxRunSpeed;
        private Vector3 _spawnPosition;

        private Vector3 ChosenPoint => _path[currentNode];
        [SerializeField] private Vector3 _choosenPoint;
        public Path _path ;
        [SerializeField]private float _intrepolationTime;

        private void Update()
        {
            CheckWayPointDistance();
        }
        
        private void Start()
        {
            
            _spawnPosition = transform.position;
            currentNode = startPos;

            Walk();
        }


        [ContextMenu("SetFirstNode")]
        
        private void FindStartNode()
        {
            var nodes = new Transform[]{};
            var position = transform.position;
            var minDistance = 10000f;
            for (int i = 0; i < nodes.Length; i++)
            {
                var node = nodes[i];
                var direction =  position - node.position;
                
                var isBehind = direction.z < 0f;
                
                if(isBehind) continue;
                
                var distance = Vector3.Distance(position, node.position);
                if (distance <= minDistance)
                {
                    minDistance = distance;
                    startPos = i;
                }
            }
        }

        private void CheckWayPointDistance() {
            if (Vector3.Distance(transform.position, ChosenPoint) < profile.decisionMakingRadius) 
            {
                if (currentNode == _path.NodeCount- 1)
                {
                    Respawn();
                } 
                else
                {
                    currentNode++;
                }
                if (block)
                {
                    Idle();
                }
                else
                {
                    transform.DOKill();
                    Walk();
                }
                LookAtTheWay();

            }

        }

        public void LookAtTheWay()
        {
            var point = ChosenPoint;

            point = new Vector3(point.x, transform.position.y, point.z);
            transform.DOLookAt(point, profile.lookAtDuration);
        }
        public void Walk()
        {
            LookAtTheWay();
            isIdle = false;
            var point = ChosenPoint;
            PlayWalkAnimation();

            _choosenPoint = point;
            transform.DOMove(point, CalculateTime(MaxSpeed)).SetEase(Ease.Linear);
        }

        private void PlayWalkAnimation()
        {
            animator.SetTrigger(MoveAnimTag);
        }

        public void Idle()
        {
            isIdle = true;
            animator.SetTrigger(profile.idleTrigger);
        }

        public float CalculateTime(float sp)
        {
            var vec = Vector3.Distance(transform.position, ChosenPoint);
            _intrepolationTime = vec / sp;
            return _intrepolationTime;
        }
        
        public void StartWalkingAfterCollisionWith(bool human)
        {
            if (human)
            {
                StopCoroutine(StartWalkingEnumerator());
                StartCoroutine(StartWalkingEnumerator());
            }
            else
            {
                block = false;
                Walk();
            }
        }

        public void StopWalking()
        {
            transform.DOKill();
            Idle();
            block = true;
        }

        public IEnumerator StartWalkingEnumerator()
        {
            yield return new WaitForSeconds(Random.Range(0.4f, 1f));
            block = false;
            Walk(); 
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("TrafficBlockHuman"))
            {
                StopWalking();
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("TrafficBlockHuman"))
            {
                StartWalkingAfterCollisionWith(false);
            }
        }

        public void EnableRun()
        {
            _iRunning = true;
        }

        private void Respawn()
        {
            _iRunning = false;
            currentNode = startPos;
            transform.position = _spawnPosition;
            StartWalkingAfterCollisionWith(false);
        }
    }
}