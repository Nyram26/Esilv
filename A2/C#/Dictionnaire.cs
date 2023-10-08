using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _2Projets_Mots_Meles_SION_Martin_SUHIT_Benjamin_TDO
{
    public class Dictionnaire
    {
        private Dictionary<int, List<string>> dico;
        private string langue;
        public Dictionnaire(string langue)//Dictionnaire est initialisé depuis la classe Jeu    
        {
            this.langue = langue;
            string dico_file = "MotsPossibles" + langue + ".txt";
            Readfile(dico_file);
        }
        public Dictionary<int, List<string>> Dico
        {
            get { return dico; }
        }
        public string Langue
        {
            get { return langue; }
        }

        /// <summary>
        /// fonction Readfile permettant de remplir l'attribut dico de type Dictionary<int, List<string>> à partir d'un fichier .txt
        /// </summary>
        /// <param name="file_name">le nom du fichier dans lequel la fonction récupère toutes les informations pour le remplissage</param>
        /// <return>void donc pas de return , modifie simplement le dictionnaire</return>
        public void Readfile(string file_name)
        {
            if (File.Exists(file_name))//Par sécurité on vérifie si le fichier utilisé existe bien avant de lancer le remplissage du dictionnaire
            {
                dico = new Dictionary<int, List<string>>();
                string[] lines = File.ReadAllLines(file_name);
                string save = "";
                int k = 0;
                foreach (string line in lines)
                {
                    if (int.TryParse(line, out int value)) //si l'élément de la ligne est un entier
                    {
                        k = value;
                        dico[k] = new List<string>();
                    }
                    else
                    {
                        for (int i = 0; i < line.Length; i++)
                        {
                            if (line[i] != ' ')
                            {
                                save += line[i];
                            }
                            else
                            {
                                dico[k].Add(save);
                                save = "";
                            }
                        }
                        dico[k].Add(save);//étant donné qu'il n'y a pas d'espace après le dernier mot il faut ajouter le mot contenu dans "save' à la fin
                    }
                }
            }
        }
        
        /// <summary>
        /// Renvoie sous forme de string toutes les informations du dictionnaire à savoir la langue ainsi que le nombre de mots par longueur
        /// </summary>
        /// <returns>renvoie une chaine de caractère avec les infos du dictionnaire</returns>
        public override string ToString() //nombre de mots par longueur + langue
        {
            string info_dico = "Langue sélectionnée : " + langue + "\n";
            for (int i = 2; i < 16; i++)
            {
                info_dico += "Nombre de mots à " + i + " lettres : " + dico[i].Count + "\n";
            }
            return info_dico;

        }
        /// <summary>
        /// permet de trouver un mot spécifié dans le dictionnaire en utilisant la recherche par dichotomie (cette fonction est récursive)
        /// </summary>
        /// <param name="mot">mot dont on souhaite vérifier l'existence dans le dictionnaire</param>
        /// <param name="debut">l'indice de début de la liste de mots de même longueur que le mot en entrée</param>
        /// <param name="fin">l'indice de fin de la liste de mots de même longueur que le mot en entrée</param>
        /// <param name="start">un booléen servant qu'une seule fois pour initialiser la variable fin avec l'indice de fin de la liste de mots de même longueur que le mot en entrée</param>
        /// <returns></returns>
        public bool RechDichoRecursif(string mot, int debut = 0, int fin = 0, bool start = true)//int debut, int fin, int[] t, int Elt
        {
            if (start)//permet d'initialiser la variable 'fin' avec la longueur du string 'mot' et donc de prendre seulement le string mot en paramètre (et non en plus sa longueur)
            {
                mot = mot.ToUpper().Trim(); //on met le mot en majuscules et on supprime les potentiels espaces
                fin = dico[mot.Length].Count - 1;
            }
            int milieu = (debut + fin) / 2;
            if (debut > fin)
            {
                return false;
            }
            else
            {
                if (mot == dico[mot.Length][milieu])
                {
                    return true;
                }
                else
                {
                    if (mot.CompareTo(dico[mot.Length][milieu]) > 0)
                    {
                        return RechDichoRecursif(mot, milieu + 1, fin, false);
                    }
                    else
                    {
                        return RechDichoRecursif(mot, debut, milieu - 1, false);
                    }
                }
            }
        }
    }
}
