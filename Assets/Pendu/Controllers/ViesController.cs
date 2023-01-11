using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pendu
{
    public class ViesController : MonoBehaviour
    {
        [Tooltip("Liste de sprites affichant le nombre de vies et vies restantes du joueur.")]
        public SpriteRenderer[] listeVies;

        [Tooltip("L'objet PenduController contenant le mot à trouver et les lettres entrées.")]
        public PenduController pendu;

        [Tooltip("Le sprite représentant une vie intacte.")]
        public Sprite vieIntacte;

        [Tooltip("Le sprite représentant une vie perdue.")]
        public Sprite viePerdue;

        private AudioSource m_audioSource;

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Met à jour l'affichage des vies en fonction du nombre de fausses lettres entrées. Lorsqu'une nouvelle fausse lettre est entrée, une nouvelle vie est perdue, et un effet sonore est joué.
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
        /// Réinitialise l'affichage des vies. À utiliser entre chaque partie.
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