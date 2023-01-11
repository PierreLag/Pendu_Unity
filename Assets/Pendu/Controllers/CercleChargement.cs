using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pendu
{
    public class CercleChargement : MonoBehaviour
    {
        private Transform m_transform;

        private Vector3 vectorInitial;
        private float tempsEcoule;

        private void Awake()
        {
            m_transform = GetComponent<Transform>();
            tempsEcoule = 0;
            vectorInitial = m_transform.eulerAngles;
        }

        // Update is called once per frame
        void Update()
        {
            if (enabled)
            {
                tempsEcoule += Time.deltaTime;  // Accumule le temps écoulé entre chaque frame afin de contrôler la rotation du cercle de chargement.
                if (tempsEcoule >= 0.125)
                {
                    m_transform.Rotate(new Vector3(0, 0, -45)); // Tourne le cercle de 45 degrés sur l'axe Z.
                    tempsEcoule -= (float)0.125;
                }
            }
        }

        private void OnEnable()
        {
            m_transform.eulerAngles = vectorInitial;    // Réinitialise l'orientation du cercle lorsqu'il est affiché.
            tempsEcoule = 0;
        }
    }
}