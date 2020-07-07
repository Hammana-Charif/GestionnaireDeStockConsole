using System;

namespace GestionnaireDeStock
{
    class Menu
    {
        static Action<string> consoleWriteLine = s => Console.WriteLine(s);
        static Action<string> consoleWrite = s => Console.Write(s);

        /// <summary>
        /// Affiche le texte du sommaire.
        /// </summary>
        public static void AskDataBaseMenu()
        {
            consoleWriteLine("*********************************Menu*********************************");

            consoleWriteLine("1: Ajouter un article au stock");
            consoleWriteLine("2: Rechercher un article par référence");
            consoleWriteLine("3: Rechercher un article par nom");
            consoleWriteLine("4: Rechercher un article par intervalle de prix de vente");
            consoleWriteLine("5: Rechercher dans l'ensemble de la base de donnée");
            consoleWriteLine("6: Supprimer un article par référence");
            consoleWriteLine("7: Modifier un article par référence");
            consoleWriteLine("8: Afficher tous les articles");
            consoleWriteLine("9: Quitter");

            consoleWrite("\nSaisissez votre choix:");
        }
        
        /// <summary>
        /// Affiche le texte du sous-menu lié aux modifications des caractéristiques d'un objet.
        /// </summary>
        public static void AskCharacteriticsEditingMenu()
        {
            consoleWriteLine("Caractéristique à modifier:");
            consoleWriteLine("1: REFERENCE");
            consoleWriteLine("2: NOM");
            consoleWriteLine("3: PRIX");
            consoleWriteLine("4: QUANTITE");

            consoleWrite("\nSaisissez votre choix:");
        }

        /// <summary>
        /// Demande à l'utilisateur si il souhaite poursuivre ou non. Renvoie le caractère saisi (O/N).
        /// </summary>
        /// <returns></returns>
        public static char AskContinue()
        {
            char key;
            do
            {
                key = Console.ReadKey(true).KeyChar;
            } while (!"ON".Contains(char.ToUpper(key)));

            return char.ToUpper(key);
        }

        /// <summary>
        /// Demande à l'utilisateur si il souhaite poursuivre ou non. Renvoie un booléen true/false.
        /// </summary>
        /// <returns></returns>
        public static bool AskContinueBool()
        {
            char key;
            do
            {
                key = Console.ReadKey(true).KeyChar;
            } while (!"ON".Contains(char.ToUpper(key)));

            Console.Clear();

            return char.ToUpper(key) == 'O';
        }

        /// <summary>
        /// Affiche le texte pour l'utilisateur et appel la version booléenne de "AskContinue".
        /// </summary>
        /// <returns></returns>
        public static bool AskComebackToMenu()
        {
            consoleWriteLine("Voulez vous retournez au sommaire (O/N)?");
            return AskContinueBool();
        }

        /// <summary>
        /// Indique à l'utilisateur d'appuyer sur une touche pour continuer et reset l'affichage de la console.
        /// </summary>
        public static void ClicBeforeClear()
        {
            consoleWriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}