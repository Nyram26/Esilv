using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace _2Projets_Mots_Meles_SION_Martin_SUHIT_Benjamin_TDO
{
    public class Plateau
    {
        private char[,] board;
        private Dictionnaire dico;
        private int difficulte;
        private string[] mots_à_trouver;
        private Random r = new Random();
        public Plateau(int type_plateau, int difficulte, Dictionnaire dico)
        {
            this.difficulte = difficulte;
            this.dico = dico;

            if (type_plateau == 2) // si le plateau doit être généré de façon aléatoire
            {
                int nb_mots = 3 + 5*difficulte;
                string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                init_plateau(); // initialisation du plateau selon la difficulté
                mots_à_trouver = new string[nb_mots];
                for (int i = 1; i <= nb_mots; ++i)
                {
                    bool b = false;
                    string mot_int = "";
                    while (!b)
                    {
                        mot_int = choix_random_mot();
                        b = Remplir_Plateau(mot_int);
                    }
                    mots_à_trouver[i - 1] = mot_int;
                }
                for (int i = 0; i < board.GetLength(0); ++i) // remplissage aléatoire des dernières cases vides
                {
                    for (int j = 0; j < board.GetLength(1); ++j)
                    {
                        if (board[i, j] == '\0')
                        {
                            board[i, j] = alphabet[r.Next(0, 26)];
                        }
                    }
                }
            }
            else // si le plateau sera initialisé par un fichier .csv
            {
                string file  = dico.Langue + "_Difficulte_" + difficulte + ".csv";
                ToRead(file);
            }
        }

        // propriétés du plateau

        public char[,] Board
        {
            get { return board; }
        }
        public string[] Mots_à_trouver
        {
            get { return mots_à_trouver; }
        }

        // fonctions disponibles pour la classe Plateau

        /// <summary>
        /// construit et renvoie un string contenant toutes les informations du plateau à savoir la grille de mots, ainsi que les mots à trouver dans la grille
        /// </summary>
        /// <returns>retourne une chaine de caractère avec toutes les informations du plateau</returns>
        public override string ToString()
        {
            string ligne = "+---";
            string espace = "|   ";
            string numeros_colonnes = "   ";
            string plateau = "";
            for (int i = 0; i < board.GetLength(1) - 1; i++) //création du haut et du bas du cadre ainsi que de l'espace entre chaque ligne
            {
                ligne += "---";
                espace += "   ";
                if (i < 10)
                {
                    numeros_colonnes += i + "  ";
                }
                else
                {
                    numeros_colonnes += i + " ";
                }

            }
            ligne += "--+";
            espace += "  |";
            numeros_colonnes += (board.GetLength(1) - 1);
            plateau += numeros_colonnes + "\n" + ligne + "\n" + espace + "\n";
            for (int j = 0; j < board.GetLength(0); j++)
            {
                plateau += "|";
                for (int k = 0; k < board.GetLength(1); k++)
                {
                    plateau += "  " + board[j, k];
                }
                plateau += "  |  " + j + "\n" + espace + "\n";
            }
            plateau += ligne + "\nMots à trouver :";//on affiche les mots à trouver dans le plateau
            for (int j = 0; j < mots_à_trouver.Length; j++)
            {
                if (mots_à_trouver[j] != " ")
                {
                    plateau += " " + mots_à_trouver[j];
                }
            }
            return plateau;
        }

        //FONCTIONS POUR LA SAUVEGARDE

        /// <summary>
        /// Permet d'écrire les informations d'un plateau dans un fichier .csv à savoir difficulté, taille, nombre de mots, mots à trouver ainsi que la grille en elle-même
        /// </summary>
        /// <param name="file_name">nom du dossier dans lequel toFile rédige les information du plateau</param>
        /// <returns>void donc pas de return, mais modifie le fichier passé en paramètre</returns>
        public void ToFile(string file_name) // construit un fichier csv à partir d'un plateau
        {
            string[] fichier = new string[board.GetLength(0)  +  2];
            string ligne1 = "";
            ligne1 += "" + difficulte + ";" + board.GetLength(0)   + ";"  +   board.GetLength(1)   +   ";"   +   mots_à_trouver.Length + ";;;;;;;;;;;;;;;;;;;;;;;;";
            fichier[0] = ligne1;
            string ligne2 = "";
            
            for (int  i = 0; i < mots_à_trouver.Length-1; ++i)
            {
                ligne2 += mots_à_trouver[i]  +  ';';
            }
            ligne2 += mots_à_trouver[mots_à_trouver.Length  -  1];
            fichier[1] = ligne2;
            string ligneX = "";
            for (int i = 0; i < board.GetLength(0); ++i)
            {
                for (int j = 0; j < board.GetLength(1); ++j)
                {
                    ligneX += "" + board[i, j] + ';';
                }
                ligneX += ";;;;;;;;;;;;;;";
                fichier[i  +  2] = ligneX;
                ligneX = "";
            }
            File.WriteAllLines(file_name,  fichier);
        }

        // FONCTIONS POUR LA CONSTRUCTION DU PLATEAU

        //PLATEAU ISSU D'UN FICHIER .CSV

        /// <summary>
        /// permet de lire dans un fichier .csv et de créer d'initialiser un objet Plateau à partir de toutes les informations contenues dans le fichier (fonction inverse de ToFile())
        /// </summary>
        /// <param name="file_name">nom du fichier dans lequel ToRead() va chercher les informations pour initialiser le plateau</param>
        /// <returns>void donc pas de return mais modifie les attributs board, difficulte, mots_à_trouver de la classe Plateau</returns>
        public void ToRead(string file_name)
        {
            string[] file_content = File.ReadAllLines(file_name);
            difficulte = 0;
            int x = 0;
            int y = 0;
            int k;
            int nb_mots = 0;
            string save = "";
            int count = 0;
            for (int n = 0; n < file_content.Length; n++)
            {
                k = 0;
                count = 0;
                switch (n)
                {
                    case 0:
                        for (int j = 0; j < file_content[n].Length && count < 2; j++)// analyse de la 1ere ligne et assignation des attributs
                        {
                            if (file_content[n][j] != ';')
                            {
                                save += file_content[n][j];
                                count = 0;
                            }
                            else
                            {
                                if (difficulte == 0)
                                {
                                    difficulte = Convert.ToInt32(save);
                                    save = "";
                                }
                                else
                                {
                                    if (x == 0)
                                    {
                                        x = Convert.ToInt32(save);
                                        save = "";
                                    }
                                    else
                                    {
                                        if (y == 0)
                                        {
                                            y = Convert.ToInt32(save);
                                            save = "";
                                        }
                                        else
                                        {
                                            if (nb_mots == 0)
                                            {
                                                nb_mots = Convert.ToInt32(save);
                                                save = "";
                                            }
                                        }                               
                                    }
                                }
                                count++;
                            }
                        }
                        board = new char[x, y];
                        mots_à_trouver = new string[nb_mots];
                        break;
                    case 1:
                        for (int i = 0; i < file_content[n].Length && count < 2; i++)//remplissage du tableau de mots à trouver
                        {
                            if (file_content[n][i] != ';')
                            {
                                save += file_content[n][i];
                                count = 0;
                            }
                            else
                            {
                                mots_à_trouver[k] = save;
                                save = "";
                                k++;
                                count++;
                            }
                        }
                        break;
                    default:
                        for (int i = 0; i < file_content[n].Length && count < 2; i++)//remplissage de chaque ligne du plateau
                        {
                            if (file_content[n][i] != ';')
                            {
                                board[n - 2, k] = file_content[n][i];
                                k++;
                                count = 0;
                            }
                            else
                            {
                                count++;
                            }
                        }
                        break;
                }
            }
        }

        //PLATEAU GENERE ALEATOIREMENT

        /// <summary>
        /// Permet de choisir de façon aléatoire un mot dans le dictionnaire associé au plateau
        /// </summary>
        /// <returns>le mot choisi aléatoirement par la fonction dans le dictionnaire</returns>
        public string choix_random_mot() // fixe un nombre de mots a placer dans la grille pour chaque difficulté
        {
            int i = r.Next(2, 16);
            int j = r.Next(0, dico.Dico[i].Count);
            return dico.Dico[i][j];
        }

        /// <summary>
        /// permet d'initialiser avec la bonne taille de plateau en fonction de la difficulté attribuée au plateau
        /// </summary>
        /// <returns>void donc pas de return, modifie l'attribut board</returns>
        public void init_plateau()
        {
            switch (difficulte)
            {
                case 1:
                    board = new char[6, 7];
                    break;
                case 2:
                    board = new char[8, 9];
                    break;
                case 3:
                    board = new char[10, 11];
                    break;
                case 4:
                    board = new char[12, 13];
                    break;
                case 5:
                    board = new char[13, 13];
                    break;
            }
        }

        /// <summary>
        /// permet de placer un mot sur la grille à partir de coordonnées précises et suivant une direction donnée
        /// </summary>
        /// <param name="mot">mot que l'on souhaite placer dans la grille</param>
        /// <param name="ligne">ligne de la première lettre du mot</param>
        /// <param name="colonne">colonne de la première lettre du mot</param>
        /// <param name="direction">direction que doit suivre Placer_mot pour écrire la suite du mot en partant des coordonnées (ligne, colonne)</param>
        /// <return>void donc pas de return, modifie l'attribut board</return>
        public void Placer_mot(string mot, int ligne, int colonne, string direction) // cette fonction place le mot dans une suite de cases donnée
        {
            int k = 0;
            int j = colonne;

            switch (direction)
            {
                case "N":
                    for (int i = ligne; i > ligne - mot.Length; i--)
                    {
                        board[i, colonne] = mot[k];
                        k++;
                    }
                    break;
                case "S":
                    for (int i = ligne; i < mot.Length + ligne; i++)
                    {
                        board[i, colonne] = mot[k];
                        k++;
                    }
                    break;
                case "E":
                    for (int i = colonne; i < mot.Length + colonne; i++)
                    {
                        board[ligne, i] = mot[k];
                        k++;
                    }
                    break;
                case "O":
                    for (int i = colonne; i > colonne - mot.Length; i--)
                    {
                        board[ligne, i] = mot[k];
                        k++;
                    }
                    break;
                case "NE":
                    for (int i = ligne; i > ligne - mot.Length; i--)
                    {
                        board[i, j] = mot[k];
                        k++;
                        j++;
                    }
                    break;
                case "NO":
                    for (int i = ligne; i > ligne - mot.Length; i--)
                    {
                        board[i, j] = mot[k];
                        k++;
                        j--;
                    }
                    break;
                case "SE":
                    for (int i = ligne; i < mot.Length + ligne; i++)
                    {
                        board[i, j] = mot[k];
                        k++;
                        j++;
                    }
                    break;
                case "SO":
                    for (int i = ligne; i < mot.Length + ligne; i++)
                    {
                        board[i, j] = mot[k];
                        k++;
                        j--;
                    }
                    break;
            }
        }
        
        /// <summary>
        /// permet de remplir le plateau avec un mot en tirant au hasard sa postion de départ, sa direction et son inversion
        /// </summary>
        /// <param name="mot"></param>
        /// <returns>retourne un booléen qui indique si le remplissage du plateau a été un succès ou un échec</returns>
        public bool Remplir_Plateau(string mot) //cette fonction prend un mot en paramètre et le place aléatoirement dans board
        {

            string[] directions = new string[8] { "N", "S", "E", "O", "SO", "SE", "NE", "NO" };
            string direction = "";
            bool succes = true;
            int ln = 0;
            int col = 0;

            switch (difficulte)
            {
                case 1:
                    ln = r.Next(0, board.GetLength(0)); // position aléatoire du mot
                    col = r.Next(0, board.GetLength(1));
                    direction = directions[r.Next(1, 3)]; // on pioche une direction aléatoire dans le tableau directions
                    if (!deja_choisi(mot))
                    {
                        if (verif_positionnement(mot, ln, col, direction))
                        {
                            Placer_mot(mot, ln, col, direction);
                        }
                        else
                        {
                            succes = false;
                        }
                    }
                    else
                    {
                        succes = false;
                    }
                    break;
                case 2:
                    ln = r.Next(0, board.GetLength(0));
                    col = r.Next(0, board.GetLength(1));
                    direction = directions[r.Next(0, 4)];
                    if (!deja_choisi(mot))
                    {
                        if (verif_positionnement(mot, ln, col, direction))
                        {
                            Placer_mot(mot, ln, col, direction);
                        }
                        else
                        {
                            succes = false;
                        }
                    }
                    else
                    {
                        succes = false;
                    }
                    break;
                case 3:
                    ln = r.Next(0, board.GetLength(0));
                    col = r.Next(0, board.GetLength(1));
                    direction = directions[r.Next(0, 5)];
                    if (!deja_choisi(mot))
                    {
                        if (verif_positionnement(mot, ln, col, direction))
                        {
                            Placer_mot(mot, ln, col, direction);
                        }
                        else
                        {
                            succes = false;
                        }
                    }
                    else
                    {
                        succes = false;
                    }
                    break;
                case 4:
                    ln = r.Next(0, board.GetLength(0));
                    col = r.Next(0, board.GetLength(1));
                    direction = directions[r.Next(0, 6)];
                    if (!deja_choisi(mot))
                    {
                        if (verif_positionnement(mot, ln, col, direction))
                        {
                            Placer_mot(mot, ln, col, direction);
                        }
                        else
                        {
                            succes = false;
                        }
                    }
                    else
                    {
                        succes = false;
                    }
                    break;
                case 5:
                    ln = r.Next(0, board.GetLength(0));
                    col = r.Next(0, board.GetLength(1));
                    direction = directions[r.Next(0, 8)];
                    if (!deja_choisi(mot))
                    {
                        if (verif_positionnement(mot, ln, col, direction))
                        {
                            Placer_mot(mot, ln, col, direction);
                        }
                        else
                        {
                            succes = false;
                        }
                    }
                    else
                    {
                        succes = false;
                    }
                    break;
            }
            return succes;
        }
        
        /// <summary>
        /// permet de vérifier si le mot en entrée n'est pas déjà présent dans le tableau des mots à trouver
        /// </summary>
        /// <param name="mot">mot dont on souhaite vérifier la présence dans le tableau des mots à trouver</param>
        /// <returns></returns>
        public bool deja_choisi(string mot) //lors de la génération du plateau, permet d'éviter de placer plusieurs fois le même mot dans la grille
        {
            bool dejala = false;
            for (int i = 0; i < mots_à_trouver.Length; i++)
            {
                if (mots_à_trouver[i] == mot)
                {
                    dejala = true;
                }
            }
            return dejala;
        }
        
        /// <summary>
        /// permet de vérifier si un mot peut être placé/présent dans la grille à partir de coordonnées et d'une direction saisies (si suffisamment de place, si coordonnées valides...)
        /// dissociée de Placer_mot car égalemnt utilisée dans Test_plateau
        /// </summary>
        /// <param name="mot">mot que l'on souhaiterait placer à partir des coordonnées saisies et dans la direction saisie</param>
        /// <param name="ligne">ligne où le mot commence</param>
        /// <param name="colonne">colonne où le mot commence</param>
        /// <param name="direction">direction que suit le mot</param>
        /// <returns>retourne un booléen qui indique si le possitionnement est possible ou non</returns>
        public bool verif_positionnement(string mot, int ligne, int colonne, string direction) //vérifie si suffisamment de place pour le mot et si aucun obstacle dans la trajectoire (lettres différentes)
        {                                                                                      //utilisée pour la génération aléatoire de plateau ainsi que Test_plateau
            int j = colonne;
            bool valided = true;
            if(ligne < 0 || colonne < 0 || ligne == board.GetLength(0) || colonne == board.GetLength(1))
            {
                valided = false;
            }
            else
            {
                switch (direction)
                {
                    case "N":
                        if (ligne + 1 - mot.Length < 0)
                        {
                            valided = false;
                        }
                        for (int i = ligne; i > ligne - mot.Length && valided; i--)
                        {
                            if (board[i, colonne] != mot[ligne - i] && board[i, colonne] != '\0')
                            {

                                valided = false;
                            }
                        }
                        break;
                    case "S":
                        if (ligne + mot.Length > board.GetLength(0))
                        {
                            valided = false;
                        }
                        for (int i = ligne; i < mot.Length + ligne && valided; i++)
                        {
                            if (board[i, colonne] != mot[i - ligne] && board[i, colonne] != '\0')
                            {
                                valided = false;
                            }
                        }
                        break;
                    case "E":
                        if (colonne + mot.Length > board.GetLength(1))
                        {
                            valided = false;
                        }
                        for (int i = colonne; i < mot.Length + colonne && valided; i++)
                        {
                            if (board[ligne, i] != mot[i - colonne] && board[ligne, i] != '\0')
                            {
                                valided = false;
                            }
                        }
                        break;
                    case "O":
                        if (colonne + 1 - mot.Length < 0)
                        {
                            valided = false;
                        }
                        for (int i = colonne; i > colonne - mot.Length && valided; i--)
                        {
                            if (board[ligne, i] != mot[colonne - i] && board[ligne, i] != '\0')
                            {
                                valided = false;
                            }
                        }
                        break;
                    case "NE":
                        if (ligne + 1 - mot.Length < 0 || colonne + mot.Length > board.GetLength(1))
                        {
                            valided = false;
                        }
                        for (int i = ligne; i > ligne - mot.Length && valided; i--)
                        {
                            if (board[i, j] != mot[ligne - i] && board[i, j] != '\0')
                            {
                                valided = false;
                            }
                            j++;
                        }
                        break;
                    case "NO":
                        if (ligne + 1 - mot.Length < 0 || colonne + 1 - mot.Length < 0)
                        {
                            valided = false;
                        }
                        for (int i = ligne; i > ligne - mot.Length && valided; i--)
                        {
                            if (board[i, j] != mot[ligne - i] && board[i, j] != '\0')
                            {
                                valided = false;
                            }
                            j--;
                        }
                        break;
                    case "SE":
                        if (ligne + mot.Length > board.GetLength(0) || colonne + mot.Length > board.GetLength(1))
                        {
                            valided = false;
                        }
                        for (int i = ligne; i < mot.Length + ligne && valided; i++)
                        {
                            if (board[i, j] != mot[i - ligne] && board[i, j] != '\0')
                            {
                                valided = false;
                            }
                            j++;
                        }
                        break;
                    case "SO":
                        if (ligne + mot.Length > board.GetLength(0) || colonne + 1 - mot.Length < 0)
                        {
                            valided = false;
                        }
                        for (int i = ligne; i < mot.Length + ligne && valided; i++)
                        {
                            if (board[i, j] != mot[i - ligne] && board[i, j] != '\0')
                            {
                                valided = false;
                            }
                            j--;
                        }
                        break;
                }
            }
            return valided;
        }
        
        
        // FONCTIONS ANNEXES 
        
        /// <summary>
        /// permet de remplacer le mot saisi par un espace dans le tableau des mots à trouver afin de simuler sa suppression (la fonction toString ignore les mots transformés en espace)
        /// </summary>
        /// <param name="mot">mots que l'on souhaite supprimer du tableau des mots à trouver</param>
        /// <return></return>
        public void Remove_mot_from_mots_à_trouver(string mot)
        {
            for (int i = 0; i < mots_à_trouver.Length; i++)
            {
                if (mots_à_trouver[i] == mot)
                {
                    mots_à_trouver[i] = " ";
                }
            }
        }

        /// <summary>
        /// permet de vérifier si le mot saisi par le joueur se trouve bien dans la grille avec les coordonnées et la direction saisies et si ce mot existe bien dans le dictionnaire
        /// </summary>
        /// <param name="mot">mot saisi par le joueur dont on souhaite vérifier la présence dans les spécifications saisies ainsi que l'existence dans le dictionnaire</param>
        /// <param name="ligne">ligne où se trouve la première lettre du mot saisi (selon le joueur)</param>
        /// <param name="colonne">colonne où se trouve la première lettre du mot saisi (selon le joueur)</param>
        /// <param name="direction">direction que suit le mot dans la grille à partir des coordonnées saisies (selon le joueur)</param>
        /// <returns></returns>
        public bool Test_plateau(string mot, int ligne, int colonne, string direction)
        {
            bool valide = verif_positionnement(mot, ligne, colonne, direction);
            if (valide && !dico.RechDichoRecursif(mot))
            {
                valide = false;
                Console.WriteLine("Ce mot n'existe pas dans le dictionnaire !");
            }
            return valide;
        }
    }
}
