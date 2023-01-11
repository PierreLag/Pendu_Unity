using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pendu
{
    public class ViesController : MonoBehaviour
    {
        [Tooltip("Liste de sprites affichant le nombre de vies et vies restantes du joueur.")]
        public SpriteRenderer[] listeVies;

        [Tooltip("L'objet PenduController contenant le mot � trouver et les lettres entr�es.")]
        public PenduController pendu;

        [Tooltip("Le sprite repr�sentant une vie intacte.")]
        public Sprite vieIntacte;

        [Tooltip("Le sprite repr�sentant une vie perdue.")]
        public Sprite viePerdue;

        private AudioSource m_audioSource;

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Met � jour l'affichage des vies en fonction du nombre de fausses lettres entr�es. Lorsqu'une nouvelle fausse lettre est entr�e, une nouvelle vie est perdue, et un effet sonore est jou�.
        /// </summary>
        public void UpdateVies()
        {
            int faussesLettres = pendu.GetFauxCharacteresUtilisesCount();

            if (faussesLettres > 0)
            {
                if (listeVies[faussesLettres - 1].sprite == vieIntacte)
                {
                    m_audioSource.Play();
                    listeVies[faussesLettres - 1].sprite = viePerdue;
                }
            }
        }

        /// <summary>
        /// R�initialise l'affichage des vies. � utiliser entre chaque partie.
        /// </summary>
        public void ResetVies()
        {
            foreach (SpriteRenderer vie in listeVies)
            {
                vie.sprite = vieIntacte;
            }
        }
    }
}