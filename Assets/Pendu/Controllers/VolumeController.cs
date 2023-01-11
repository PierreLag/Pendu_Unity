using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pendu
{
    [RequireComponent(typeof(Slider))]
    public class VolumeController : MonoBehaviour
    {
        [Tooltip("La liste d'objets contenants les sons contrôllés par cet objet.")]
        public AudioSource[] sources;

        private Slider m_slider;

        private void Awake()
        {
            m_slider = GetComponent<Slider>();
        }

        /// <summary>
        /// Lorsqu'elle est appelée, cette méthode met à jour le volume de chaque source audio dans la liste, avec la valeur du Slider lié au controlleur.
        /// </summary>
        public void UpdateVolumes()
        {
            foreach (AudioSource audio in sources)
            {
                audio.volume = m_slider.value;
            }
        }
    }
}