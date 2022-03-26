using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalSalmon.C360 {
    public class Menu : AnimatedBehaviour {

        [SerializeField]
        protected Transform buttonsParent;

        protected void DestroyExistingButtons() {
            foreach (Transform t in buttonsParent) {
                if (t == buttonsParent) continue;
                Destroy(t.gameObject);
            }
        }
        
    }
}
