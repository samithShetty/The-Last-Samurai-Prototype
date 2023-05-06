using UnityEngine;
using PathCreation;

    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class GrindController : MonoBehaviour
    {

        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed;
        private float grindVel;
        private Transform playerTransform;
        private Rigidbody playerRb;
        private float distanceTravelled;

        private void Awake() {
            playerTransform = GetComponent<Transform>();
            playerRb = GetComponent<Rigidbody>();
        }

        private void OnEnable() {
            // We may need to reverse direction of path traversal based on velocity
            float startPointTime = pathCreator.path.GetClosestTimeOnPath (playerTransform.position);
            Vector3 defaultDir = pathCreator.path.GetDirection(startPointTime, endOfPathInstruction); 
            
            grindVel = Vector3.Angle(defaultDir, playerRb.velocity) < 90 ? speed : -speed;
            
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(playerTransform.position);
        }

        void Update()
        {
            distanceTravelled += grindVel * Time.fixedDeltaTime;
            playerTransform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            //playerTransform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(playerTransform.position);
        }
    }

    // units/sec * sec = units
