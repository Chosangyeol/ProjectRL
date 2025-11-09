using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Telpo
{
    public class Telepo : MonoBehaviour
    {
        public Transform destination;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (destination != null)
                {
                    other.transform.position = destination.position;

                }

            }
        }
    }
}
