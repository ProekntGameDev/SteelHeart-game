using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteelHeart
{
    public class GrapplingHook : MonoBehaviour
    {

        public float velLauncher;
        public float sizeRope;
        public float gallowsRope;
        public float weight;


        public KeyCode KeyGrappelr;


        private GameObject _player;
        private Rigidbody _rigidbody;
        private SpringJoint _springjoint;

        private float _DistanceToPlayer;

        private bool _shootrope;
        private bool _collidedrope;


        // Start is called before the first frame update
        void Start()
        {

            _player = GameObject.FindGameObjectWithTag("Player");

            _rigidbody = GetComponent<Rigidbody>();
            _springjoint = _player.GetComponent<SpringJoint>();

            _shootrope = true;
            _collidedrope = false;
        }

        // Update is called once per frame
        void Update()
        {


            _DistanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

            if (Input.GetMouseButtonDown(0))
            {


                _shootrope = false;
            }

            if (_shootrope)
                ShootGrapplingHook();
            else
                RecallGrapplingHook();

            GetComponent<LineRenderer>().SetPosition(0, _player.transform.position);
            GetComponent<LineRenderer>().SetPosition(1, transform.position);
        }

        void OnTriggerEnter(Collider coll)
        {
            if (coll.tag != "Player")
            {
                _collidedrope = true;
            }


        }


        public void ShootGrapplingHook()
        {


            if (_DistanceToPlayer <= sizeRope)
            {
                if (!_collidedrope)
                {
                    transform.Translate(0, 0, velLauncher * Time.deltaTime);
                }
                else
                {
                    _springjoint.connectedBody = _rigidbody;
                    _springjoint.spring = gallowsRope;
                    _springjoint.damper = weight;
                }
            }

            if (_DistanceToPlayer > sizeRope)
            {

                _shootrope = false;
            }

        }

        public void RecallGrapplingHook()
        {

            transform.position =
                Vector3.MoveTowards(transform.position, _player.transform.position, 25 * Time.deltaTime);

            if (_DistanceToPlayer <= 2)
            {
                Destroy(gameObject);
            }

        }
    }
}
