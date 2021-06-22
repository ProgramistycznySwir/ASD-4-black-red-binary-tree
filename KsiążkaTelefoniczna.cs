using System;
using System.Collections.Generic;
using System.Text;

namespace ASD___4
{
    // To klasa bardziej do prezentacji zadania i jest tu raczej brzydko po to by aplikacja w konsoli był responsywna.
    //  Tak prawidłowo to większość case'ów w switchach powinna być porozdzielana po wyodrębnionych funkcjach, chcecie
    //  to się zabawiajcie, mi się nie chce tego teraz robić.
    static class KsiążkaTelefoniczna
    {
        static BRTree.Tree tree;

        public static void MainMenu()
        {
            char input;
            while(true)
            {
                Draw.MainMenu();
                input = Console.ReadKey(true).KeyChar;
                if(tree == null)
                {
                    switch (input)
                    {
                        case '1':
                            Console.SetCursorPosition(22, 3);
                            tree = new BRTree.Tree();
                            tree.LoadFromFile(Console.ReadLine() + ".3");
                            break;
                        case '2':
                            tree = new BRTree.Tree();
                            break;
                        case '3':
                            return;
                        default:
                            break;
                    }
                    Console.Clear();
                    continue;
                }

                switch (input)
                {
                    case '1':
                    {
                        Console.SetCursorPosition(0, 11);
                        Data searchedData = SearchForm();
                        ClearLines(9, 5);
                        Console.SetCursorPosition(0, 11);
                        Console.WriteLine("Search result:");
                        searchedData.WriteToConsole();
                        Console.WriteLine("[Press any key to continue.]");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    }
                    case '2':
                    {
                        var newData = new Data_FreeAccess();
                        Console.SetCursorPosition(0, 11);
                        Console.WriteLine("New abonent data:");
                        Console.Write("Surname:"); newData.surname = Console.ReadLine();
                        Console.Write("Name:"); newData.name = Console.ReadLine();
                        Console.Write("Address:"); newData.address = Console.ReadLine();
                        Console.WriteLine("Phone numbers (divide with space and assert with enter):");
                        {
                            string[] stringNumbers = Console.ReadLine().Split(" ");
                            var newNumbers = new List<int>();
                            int newNumber;
                            foreach (string stringNumber in stringNumbers)
                            {
                                try
                                    { newNumber = Convert.ToInt32(stringNumber); }
                                catch(Exception e)
                                    { continue; }
                                newNumbers.Add(newNumber);
                            }
                            newData.phoneNumbers = newNumbers.ToArray();
                        }
                        tree.Insert(new Data(newData));
                        Console.Clear();
                        break;
                    }
                    case '3':
                    {
                        Console.SetCursorPosition(0, 11);
                        Data searchedData = StrictSearchForm();
                        ClearLines(9, 10);
                        Console.SetCursorPosition(0, 11);
                        Console.WriteLine("Search result:");
                        searchedData.WriteToConsole();
                        tree.Remove(searchedData);
                        Console.WriteLine("[Press any key to continue.]");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    }
                        // Nie jest wymagane więc i nie jest zaimplementowane, simple as that...
                    //case '4':
                    // {

                    //     break;
                    // }
                    case '5':
                    {
                        Console.SetCursorPosition(29, 7);
                        tree = new BRTree.Tree();
                        tree.SaveToFile_ByLevel(Console.ReadLine() + ".3");
                        break;
                    }
                    case '6':
                    {
                        Console.SetCursorPosition(25, 8);
                        Console.Write("Confirm with Y:");
                        if(Console.ReadKey(true).KeyChar == 'y')
                            tree = null;
                        Console.Clear();
                        break;
                    }
                    case '7':
                        return;
                    default:
                    {
                        Console.Clear();
                        break;
                    }
                }
            }
        }

        const string needMoreDataText = "There are multiple abonents with this surname, I need more data.";
        static Data SearchForm()
        {
            // Funkcja po to jest tak skomplikowana by zapewnić użytkownikowi jak najlepsze UI.
            Console.Write("Surname: ");
            var searchData = new Data_FreeAccess();
            searchData.surname = Console.ReadLine();
            BRTree.Node node = tree.SearchAtLeastSimilar(new Data(searchData));
            if (node == null)
                return Data.Clear;
            if (node.left != null && node.left.data.surname == searchData.surname
                || node.right != null && node.right.data.surname == searchData.surname)
            {
                Console.WriteLine(needMoreDataText);
                Console.Write("Name: ");
                searchData.name = Console.ReadLine();
                node = tree.SearchAtLeastSimilar(new Data(searchData));
                if (node == null)
                    return Data.Clear;
                if (node.left != null && node.left.data.name == searchData.name
                    || node.right != null && node.right.data.name == searchData.name)
                {
                    Console.WriteLine(needMoreDataText);
                    Console.Write("Address: ");
                    searchData.address = Console.ReadLine();
                    node = tree.Search(new Data(searchData));
                    if (node == null)
                        return Data.Clear;
                }
            }
            return node.data;
        }
        static Data StrictSearchForm()
        {
            // Funkcja po to jest tak skomplikowana by zapewnić użytkownikowi jak najlepsze UI.
            var searchData = new Data_FreeAccess();

            Console.Write("Surname: ");
            searchData.surname = Console.ReadLine();
            Console.Write("Name: ");
            searchData.name = Console.ReadLine();
            Console.Write("Address: ");
            searchData.address = Console.ReadLine();

            return tree.Search(new Data(searchData)).data;
        }

        static void ClearLines(int line, int count = 1)
        {
            for(int i = 0; i < count; i++)
            {
                Console.SetCursorPosition(0, line + count);
                Console.Write(new String(' ', Console.BufferWidth));
            }
        }

        static class Draw
        {
            static public void MainMenu()
            {
                // Żeby nie zaśmiecać MainMenu() (i tak już jest wystarczająco zaśmiecone, to nie jest najśliczniejszy kod).
                Console.WriteLine("Welcome to the phone book: ");
                if(tree == null)
                {
                    Console.WriteLine("  Currently there is no phone book:");
                    Console.WriteLine(" (Choose option by pressing proper key)");
                    Console.WriteLine("   1. Load from file:");
                    Console.WriteLine("   2. Create new phone book.");
                    Console.WriteLine("   3. Close programm.");
                }
                else
                {
                    Console.WriteLine($"  Phone book has {(tree.count > 0 ? tree.count.ToString() : "no")} abonent{(tree.count == 1 ? "" : "s")}, you can:");
                    Console.WriteLine(" (Choose option by pressing proper key)");
                    Console.WriteLine("   1. Read numbers of abonent.");
                    Console.WriteLine("   2. Add new abonent.");
                    Console.WriteLine("   3. Remove abonent.");
                    Console.WriteLine("   4. Add abonents from file: <NOT IMPLEMENTED>");
                    Console.WriteLine("   5. Save abonents to file:");
                    Console.WriteLine("   6. Clear phone book: ");
                    Console.WriteLine("   7. Close programm. ");
                }
            }
        }
    }
}
