namespace GestionnaireDeStock
{
    static class Program
    {
        static void Main(string[] args)
        {
            Stock stock = new Stock(@"C:\Users\raikh\OneDrive\Bureau\Numérique\Développement Informatique\CSHARP Development\GestionnaireDeStock\GestionnaireDeStock\Data\Liste des articles.txt");
            stock.Run();
        }
    }
}