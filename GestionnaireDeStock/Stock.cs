using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GestionnaireDeStock
{
    class Stock
    {
        readonly Action<string> consoleWriteLine = s => Console.WriteLine(s);
        readonly Action<string> consoleWrite = s => Console.Write(s);

        public string Path { get; private set; }

        public List<Article> Articles { get; private set; }

        /// <summary>
        /// Charge une seule fois la liste d'article contenue dans le fichier.
        /// </summary>
        /// <param name="path">Chemin à renseigner dans la méthode "Main" à l'instantiation du "stock"</param>
        public Stock(string path)
        {
            Path = path;
            Articles = Article.GetAllCharacteristics(Path);
        }

        public void Run()
        {
            AskAChoice();
        }

        /// <summary>
        /// Affiche le menu et demande à l'utilisateur de faire un choix.
        /// </summary>
        public void AskAChoice()
        {
            do
            {
                Menu.AskDataBaseMenu();

                try
                {
                    var choice = int.Parse(Console.ReadLine());

                    Console.Clear();

                    switch (choice)
                    {
                        case 1:
                            CreateNewArticle();
                            break;
                        case 2:
                            SearchForAnArticleByReference();
                            break;
                        case 3:
                            SearchForAnArticleByName();
                            break;
                        case 4:
                            SearchForAnArticleByPriceRange();
                            break;
                        case 5:
                            SearchInAllDataBase();
                            break;
                        case 6:
                            RemoveAnArticleByReference();
                            break;
                        case 7:
                            EditAnArticleByReference();
                            break;
                        case 8:
                            ShowAllItems();
                            break;
                        case 9:
                            Exit();
                            return; // Quitte le programme en sortant de la méthode.
                        default:
                            Console.Out.WriteLine("Choix invalide");
                            break;
                    }

                    Menu.ClicBeforeClear();
                }
                catch (Exception)
                {
                    Console.Clear();
                    consoleWriteLine("Saisie invalide.");
                    Menu.ClicBeforeClear();
                }
            } while (Menu.AskComebackToMenu());
        }

        /// <summary>
        /// Demande à l'utilisateur de créér article à partir de référence, nom, etc et l'ajoute au stock.
        /// </summary>
        public void CreateNewArticle()
        {
            try
            {
                var reference = AddAReference();

                bool duplicate = false;
                foreach (var article in Articles)
                {
                    if (article.Reference == reference)
                    {
                        duplicate = true;
                        consoleWriteLine("L'article existe déjà");
                        break;
                    }
                }

                if (!duplicate)
                {
                    string name = AddAName();
                    double price = AddAPrice();
                    int quantity = AddAQuantity();

                    var newArticle = new Article(reference, name, price, quantity);

                    Articles.Add(newArticle);

                    Write();

                    var art = ShowAnArticle(newArticle);

                    consoleWrite($"Le nouveau produit a été intégré au stock: \n{art}"); 
                }        
            }
            catch (Exception except)
            {
                throw new Exception($"L'erreur suivante est survenue: {except.Message}");
            }

        }

        public string ShowAnArticle(Article article)
        {
            return $"Numéro: {article.Reference}\t Nom: {article.Name}\nPrix: {article.Price}\t Quantité: {article.Quantity}\n\n";
        }

        /// <summary>
        /// Appel la fonction d'écriture de la classe "Article" et écrit dans le fichier.
        /// </summary>
        public void Write()
        {
            Article.WriteAFile(Articles, Path);
        }

        /// <summary>
        /// Ajoute une "référence" à un article en création.
        /// </summary>
        /// <returns></returns>
        public int AddAReference()
        {
            try
            {
                consoleWrite("Veuillez saisir le numéro de référence de l'article :");
                int reference;
                string newInput = Console.In.ReadLine();
                bool correctNum = int.TryParse(newInput, out reference);
                if (!correctNum)
                {
                    consoleWrite("Une erreur est survenue. Veuillez saisir une référence chiffrée.\n");
                    return AddAReference();
                }
                return reference;
            }
            catch (Exception except)
            {
                throw new Exception($"L'erreur suivante est survenue: {except.Message}");
            }

        }

        /// <summary>
        /// Ajoute une "nom" à un article en création.
        /// </summary>
        /// <returns></returns>
        public string AddAName()
        {
            try
            {
                consoleWrite("Veuillez saisir le nom de l'article:");
                string name = Console.In.ReadLine();
                if (!Regex.IsMatch(name, @"^[a-zA-Z ]+$"))
                {
                    throw new ArgumentException();
                }
                return name;
            }
            catch (ArgumentException argExcept)
            {
                throw new ArgumentException($"L'erreur suivante est survenue {argExcept.Message}. Veuillez saisir un un nom alphabétique.\n");
            }
        }

        /// <summary>
        /// Ajoute une "prix" à un article en création.
        /// </summary>
        /// <returns></returns>
        public double AddAPrice()
        {
            try
            {
                consoleWrite("Veuillez saisir le prix de l'article:");
                double price;
                string newInput = Console.In.ReadLine();
                bool correctNum = double.TryParse(newInput, out price);
                if (!correctNum)
                {
                    consoleWrite("Une erreur est survenue. Veuillez saisir une référence chiffrée.\n");
                    return AddAPrice();
                }
                return price;
            }
            catch (Exception except)
            {
                throw new Exception($"L'erreur suivante est survenue: {except.Message}");
            }
        }

        /// <summary>
        /// Ajoute une "quantité" à un article en création.
        /// </summary>
        /// <returns></returns>
        public int AddAQuantity()
        {
            try
            {
                consoleWrite("Veuillez saisir une quantité:");
                int quantity;
                string newInput = Console.In.ReadLine();
                bool correctNum = int.TryParse(newInput, out quantity);
                if (!correctNum)
                {
                    consoleWrite("Une erreur est survenue. Veuillez saisir une référence chiffrée.\n");
                    return AddAQuantity();
                }
                return quantity;
            }
            catch (Exception except)
            {
                throw new Exception($"L'erreur suivante est survenue: {except}");
            }
        }

        /// <summary>
        /// Recherche via la référence de l'article recherché.
        /// </summary>
        public void SearchForAnArticleByReference()
        {
            try
            {
                consoleWrite($"Veuillez entrez votre recherche par référence :");
                int searchReference;
                string newInput = Console.ReadLine();
                bool correctNUm = int.TryParse(newInput, out searchReference);

                if (!correctNUm)
                {
                    consoleWrite("Une erreur est survenue. Veuillez saisir une référence chiffrée.\n");
                    SearchForAnArticleByReference();
                }
                else
                {
                    bool duplicate = false;
                    foreach (var article in Articles)
                    {
                        if (article.Reference.ToString() == searchReference.ToString())
                        {
                            duplicate = true;
                            consoleWrite($"{article}");
                        }
                    }
                    if (!duplicate)
                    {
                        consoleWrite("Article introuvable\n");
                        consoleWrite("Voulez vous continuer votre recherche?\n(O/N):");
                        var response = Menu.AskContinue();
                        if (response == 'O')
                        {
                            SearchForAnArticleByReference();
                        }
                    }
                }
            }

            catch (Exception except)
            {
                throw new Exception($"L'erreur suivante est survenue: {except.Message}\n");
            }
        }

        /// <summary>
        /// Recherche via le nom de l'article recherché.
        /// </summary>
        public void SearchForAnArticleByName()
        {
            try
            {
                consoleWrite($"Veuillez entrez votre recherche par nom :");
                string input = Console.ReadLine();
                
                if (Regex.IsMatch(input, @"^[a-zA-Z ]+$"))
                {
                    bool duplicate = false;
                    foreach (var article in Articles)
                    {
                        if (article.Name.ToLowerInvariant().Contains(input.ToLowerInvariant()))
                        {
                            duplicate = true;
                            consoleWrite($"{article}");
                        }
                    }
                    if (!duplicate)
                    {
                        consoleWrite("Article introuvable\n");
                        consoleWrite("Voulez vous continuer votre recherche?\n(O/N):");
                        var response = Menu.AskContinue();
                        if (response == 'O')
                            SearchForAnArticleByName();
                    }
                }
                else
                throw new ArgumentException();
            }
            catch (ArgumentException argExcept)
            {
                consoleWrite($"L'erreur suivante est survenue: {argExcept.Message}. Veuillez saisir une entrée alphabétique.\n");
            }
        }

        /// <summary>
        /// Recherche via une intervalle de prix minimum et maximum, le ou les articles compris dedans.
        /// </summary>
        public void SearchForAnArticleByPriceRange()
        {
            consoleWriteLine("Veuillez saisir les valeurs minimale et maximale pour l'intervalle de recherche:");

            try
            {
                consoleWrite("Valeur minimale:");
                double priceMin;
                string newPriceMinInput = Console.ReadLine();
                bool correctMinNum = double.TryParse(newPriceMinInput, out priceMin);
                if(!correctMinNum)
                {
                    consoleWriteLine("La saisie ne correspond pas à une saisie chiffrée.");
                    SearchForAnArticleByPriceRange();
                }

                consoleWrite("Valeur maximale:");
                double priceMax;
                string newPriceMaxInput = Console.ReadLine();
                bool correctMaxNum = double.TryParse(newPriceMaxInput, out priceMax);
                if (!correctMaxNum)
                {
                    consoleWriteLine("La saisie ne correspond pas à une saisie chiffrée.");
                    SearchForAnArticleByPriceRange();
                }

                int itemCounter = 0;
                bool duplicate = false;
                foreach (var article in Articles)
                {
                    if (article.Price > priceMin && article.Price < priceMax)
                    {
                        duplicate = true;
                        consoleWrite($"{article}");
                        itemCounter++;
                    }
                }
                consoleWriteLine($"Nombre d'articles trouvés: {itemCounter}");
                if (!duplicate)
                {
                    consoleWriteLine("Aucun article trouvé");
                    consoleWrite("Voulez vous continuer votre recherche?\n(O/N):");
                    var response = Menu.AskContinue();
                    if (response == 'O')
                        SearchForAnArticleByPriceRange();
                }
            }
            catch (Exception except)
            {
                throw new Exception($"L'erreur suivante est survenue: {except.Message}");
            }
        }

        /// <summary>
        /// Recherche dans l'ensemble du fichier, une correspondance, quelque soit la saisie.
        /// </summary>
        public void SearchInAllDataBase()
        {
            try
            {
                consoleWrite("Veuillez entrer votre recherche:");
                string input = Console.ReadLine();

                if (Regex.IsMatch(input, @"^[a-zA-Z0-9,]+$"))
                {
                    int articleCounter = 0;
                    bool duplicate = false;
                    foreach (var article in Articles)
                    {
                        if (article.ToString().ToLower().Contains(input))
                        {
                            duplicate = true;
                            consoleWriteLine($"{article}");
                            articleCounter++;
                        }
                    }
                    consoleWriteLine($"Nombre d'articles trouvés: {articleCounter}");
                    if (!duplicate)
                    {
                        consoleWriteLine("Article introuvable");
                        consoleWrite("Voulez vous continuer votre recherche?\n(O/N):");
                        var response = Menu.AskContinue();
                        if (response == 'O')
                            SearchInAllDataBase();
                    }
                }
                else
                    throw new Exception();
            }
            catch (Exception except)
            {
                throw new Exception($"L'erreur suivante est survenue: {except.Message}.");
            }
        }

        /// <summary>
        /// Supprime un article via sa référence.
        /// </summary>
        public void RemoveAnArticleByReference()
        {
            try
            {
                consoleWrite($"Veuillez entrez la référence de l'article à supprimer:");
                int reference;
                string newInput = Console.ReadLine();
                bool correctNum = int.TryParse(newInput, out reference);
                if(!correctNum)
                {
                    consoleWrite("Une erreur est survenue. Veuillez saisir une référence chiffrée.\n");
                    RemoveAnArticleByReference();
                }

                int articleCounter = 0;
                bool duplicate = false;
                Article articleToDelete = null;

                foreach (var art in Articles)
                {
                    if (art.Reference == reference)
                    {
                        articleToDelete = art;
                        break;
                    }
                }
                if (articleToDelete != null)
                {
                    articleCounter++;
                    duplicate = true;
                    consoleWriteLine($"L'article suivant a été supprimé:\n{articleToDelete}");
                    consoleWriteLine($"Le nombre d'articles supprimé est: {articleCounter}");
                    Articles.Remove(articleToDelete);
                    Write();
                }
                if (!duplicate)
                {
                    consoleWrite("Article introuvable\n");
                    consoleWrite("Voulez vous continuer votre recherche?\n(O/N):");
                    var response = Menu.AskContinue();
                    if (response == 'O')
                        RemoveAnArticleByReference();
                }
            }
            catch (Exception except)
            {
                throw new Exception($"L'erreur suivante est survenue: {except.Message}.\n");
            }
        }

        /// <summary>
        /// Cible un article en fonction de la référence saisie, et propose un menu pour éditer une caractéristique, au choix, de l'article.
        /// </summary>
        /// <returns></returns>
        public Article EditAnArticleByReference()
        {
            try
            {
                consoleWrite("Veuillez saisir la référence de l'article à modifier:");
                int reference;
                string newInput = Console.ReadLine();
                bool correctNum = int.TryParse(newInput, out reference);
                if(!correctNum)
                {
                    consoleWrite("Une erreur est survenue. Veuillez saisir une référence chiffrée.\n");
                    return EditAnArticleByReference();
                }

                int articleCounter = 0;
                Article characToDelete = null;

                foreach (var item in Articles)
                {
                    if (item.Reference == reference)
                    {
                        characToDelete = item;
                        break;
                    }     
                }
                consoleWriteLine($"L'article séléctionné est:\n{characToDelete}");
                consoleWriteLine("Est-ce le bon article à modifier?\n(O/N)?");

                var choice = Menu.AskContinue();
                if (choice == 'O')
                {
                    Menu.AskCharacteriticsEditingMenu();

                    int characChoice = int.Parse(Console.ReadLine());
                    switch (characChoice)
                    {
                        case 1:
                            try
                            {
                                consoleWrite("Saisissez la nouvelle référence:");
                                int articleReference = characToDelete.Reference;
                                string newArtRefInput = Console.ReadLine();
                                bool correctArtRefNum = int.TryParse(newArtRefInput, out articleReference);
                                if(!correctArtRefNum)
                                {
                                    consoleWrite("Une erreur est survenue. Veuillez saisir une référence chiffrée.\n");
                                    EditAnArticleByReference();
                                }
                                Write();
                                articleCounter++;
                                consoleWriteLine($"La référence de l'article a été modifiée avec succès: \n{characToDelete}");
                                consoleWriteLine($"Le nombre d'articles modifié est: {articleCounter}");
                            }
                            catch (Exception except)
                            {
                                throw new Exception($"L'erreur suivante est survenue: {except.Message}.\n");
                            }
                            return characToDelete;
                        case 2:
                            try
                            {
                                consoleWrite("Saisissez le nouveau nom:");
                                characToDelete.Name = Console.ReadLine();
                                if (Regex.IsMatch(characToDelete.Name, @"^[a-zA-Z ]+$"))
                                {
                                    Write();
                                    articleCounter++;
                                    consoleWriteLine($"Le nom de l'article a été modifié avec succès: \n{characToDelete}");
                                    consoleWriteLine($"Le nombre d'articles modifié est: {articleCounter}");
                                }
                                else
                                    throw new ArgumentException();  
                            }
                            catch (ArgumentException argExcept)
                            {
                                consoleWrite($"L'erreur suivante est survenue: {argExcept.Message}. Veuillez saisir une entrée alphabétique.\n");
                                EditAnArticleByReference();
                            }
                            return characToDelete;
                        case 3:
                            try
                            {
                                consoleWrite("Saisissez le nouveau prix:");
                                double articlePrice = characToDelete.Price;
                                string newArtPriceInput = Console.ReadLine();
                                bool correctArtPriceNum = double.TryParse(newArtPriceInput, out articlePrice);
                                if(!correctArtPriceNum)
                                {
                                    consoleWrite("Une erreur est survenue. La saisie ne correspond pas à un prix.\n");
                                    EditAnArticleByReference();
                                }
                                Write();
                                articleCounter++;
                                consoleWriteLine($"Le prix de l'article a été modifié avec succès: \n{characToDelete}");
                                consoleWriteLine($"Le nombre d'articles modifié est: {articleCounter}");
                            }
                            catch (Exception except)
                            {
                                throw new Exception($"L'erreur suivante est survenue: {except.Message}.\n");
                            }
                            return characToDelete;
                        case 4:
                            try
                            {
                                consoleWrite("Saisissez la nouvelle quantité:");
                                int articleQuantity = characToDelete.Quantity;
                                string newArtQuantInput = Console.ReadLine();
                                bool correctArtQuantNum = int.TryParse(newArtQuantInput, out articleQuantity);
                                if(!correctArtQuantNum)
                                {
                                    consoleWrite("Une erreur est survenue. La saisie ne correspond pas à une quantité chiffrée.\n");
                                    EditAnArticleByReference();
                                }
                                Write();
                                articleCounter++;
                                consoleWriteLine($"La quantité de l'article a été modifiée avec succès: \n{characToDelete}");
                                consoleWriteLine($"Le nombre d'articles modifié est: {articleCounter}");
                            }
                            catch (Exception except)
                            {
                                throw new Exception($"L'erreur suivante est survenue: {except.Message}.\n");
                            }
                            return characToDelete;
                        default:
                            throw new NotSupportedException();
                    }
                }
                else
                    EditAnArticleByReference();

                return characToDelete;
            }
            catch (Exception except)
            {
                 throw new Exception($"L'erreur suivante est survenue: {except.Message}.\n");
            }
        }

        /// <summary>
        /// Affiche l'ensemble des articles compris dans le fichier texte.
        /// </summary>
        public void ShowAllItems()
        {  
            try
            {
                int articleCounter = 0;
                bool duplicate = false;
                var articles = Article.GetAllCharacteristics(Path);
                foreach (var article in articles)
                {
                    duplicate = true;
                    consoleWriteLine($"{article}");
                    articleCounter++;
                }
                consoleWriteLine($"Le nombre total d'articles trouvé est de {articleCounter}.");
                if (!duplicate)
                {
                    consoleWrite("Il n'y a pas d'article dans le stock");
                }
            }
            catch (Exception except)
            {
                throw new Exception($"L'erreur suivante est survenue: {except.Message}");
            }
        }

        public void Exit()
        {
            var choice = Menu.AskComebackToMenu();
            if (choice == false)
            {
                AskAChoice();
            }
        }
    }
}