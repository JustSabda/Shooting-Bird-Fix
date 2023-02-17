using System.Collections;

using UnityEngine;
using Photon.Pun;
using UnityEngine.VFX;


    public class CleanUpVFX : MonoBehaviour
    {
        #region Datamembers

        #region Private Fields

        private VisualEffect vfx;

        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks

        private void Start()
        {
            vfx = GetComponent<VisualEffect>();

            StartCoroutine(CleanUpRoutine());
        }


        #endregion

        private IEnumerator CleanUpRoutine()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            while (vfx.aliveParticleCount != 0)
            {
                yield return new WaitForFixedUpdate();
            }

            PhotonNetwork.Destroy(gameObject);
        }

        #endregion
    }
