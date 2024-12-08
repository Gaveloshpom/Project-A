using System;
using System.Collections.Generic;
using System.Text;

public class ElTree
{
    public string Inf { get; set; }
    public int R { get; set; } // Покажчик на правого брата 
    public bool PL { get; set; } // Ознака наявності лівого піддерева

    public ElTree(string inf)
    {
        Inf = inf;
        R = -1; // правого піддерева немає
        PL = false; // лівого піддерева немає
    }
}

public class FamilyTree
{
    public List<ElTree> nodes; // Масив вузлів

    public FamilyTree(string rootInf)
    {
        nodes = new List<ElTree>
        {
            new ElTree(rootInf)
        };
    }

    public void AddNode(int parentIndex, string childInf)
    {
        ElTree childNode = new ElTree(childInf);

        // Додаємо піддерево 
        if (!nodes[parentIndex].PL) 
        {
            nodes[parentIndex].PL = true; 
            nodes.Insert(parentIndex + 1, childNode);
            foreach (var node in nodes) 
            {
                if (node.R >= parentIndex + 1)
                {
                    node.R += 1;
                }
            }

        }
        else // Якщо ліве піддерево вже є - додаємо до правого
        {
            nodes.Add(childNode);
            nodes[parentIndex].R = nodes.IndexOf(childNode);
        }
    }

    public void PreorderTraversal(int index = 0)
    {
        if (index == -1 || index >= nodes.Count) return;

        // Вивести поточний вузол
        Console.Write(nodes[index].Inf + " ");

        // Обхід лівого піддерева, якщо воно існує
        if (nodes[index].PL)
        {
            PreorderTraversal(index + 1); 
        }

        // Обхід правого брата
        PreorderTraversal(nodes[index].R);
    }

    public void DisplayTree()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            Console.WriteLine($"Index: {i}, Info: {nodes[i].Inf}, R: {nodes[i].R}, PL: {nodes[i].PL}");
        }
    }

    public void DeleteNode(int index) 
    {
        bool leftChild = nodes[index].PL;
        int rightChild = nodes[index].R;

        if (rightChild != -1) 
        {
            rightChild--;
        }

        nodes.RemoveAt(index);

        if (nodes[index - 1].PL == true) 
        {
            nodes[index - 1].PL = false;
        }

        foreach (var node in nodes)
        {
            if (node.R == index)
            {
                node.R = -1;
            }

            if (node.R >= index + 1)
            {
                node.R -= 1;
            }
        }

        if (rightChild != -1) 
        {
            DeleteNode(rightChild);
        }

        if (leftChild) 
        {
            DeleteNode(index);
        }
    }

    public int FindNodeIndex(string inf)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].Inf == inf) return i;
        }
        return -1;
    }

    public bool IsEmpty() 
    {
        if (nodes[0].Inf == "Director" && nodes.Count == 1)
        {
            return true;
        }
        else { return false; }
    }
}

class Lab_4
{
    static void Main()
    {
        FamilyTree tree = new FamilyTree("Director");

        tree.AddNode(0, "HR");
        tree.AddNode(0, "IT");

        tree.AddNode(1, "Recruitment");
        tree.AddNode(1, "Training");

        tree.AddNode(3, "Infrastructure");
        tree.AddNode(3, "Development");

        tree.AddNode(2, "Recruitment_1");
        tree.AddNode(2, "Recruitment_2");

        tree.AddNode(6, "Training_1");
        tree.AddNode(6, "Training_2");

        Console.OutputEncoding = Encoding.UTF8;
           

            while (true)
            {
                try
                {
                    Console.WriteLine("\n--- Меню ---");
                    Console.WriteLine("1. Додати вузол");
                    Console.WriteLine("2. Видалити вузол");
                    Console.WriteLine("3. Здійснти обхід дерева");
                    Console.WriteLine("4. Дерево порожнє?");
                    Console.WriteLine("5. Вивести дерево на екран");
                    Console.WriteLine("6. Здійснити пошук вузла");
                    Console.Write("Введіть номер операції: ");

                    string choice = Console.ReadLine();
                    if (!IsValidChoice(choice, 6))
                    {
                        Console.WriteLine("Помилка: введіть коректний номер операції.");
                        continue;
                    }

                    switch (choice)
                    {
                        case "1":
                            int fatherRoot = GetValidIntInput("\nВведіть індекс батьківського вузла: ");


                            if (fatherRoot <= tree.nodes.Count - 1 && fatherRoot >= 0)
                            {
                                Console.Write("\nВведіть дочірній вузол: ");
                                string sonRoot = Console.ReadLine();

                                tree.AddNode(fatherRoot, sonRoot);

                                Console.WriteLine("\nВузол успіншно додано!");
                            }
                            else { throw new Exception("некоректний індекс"); }

                            break;// Додати елемент

                        case "2":
                            int rootDelete = GetValidIntInput("\nВведіть індекс вузла: ");

                            if (rootDelete <= tree.nodes.Count - 1 && rootDelete >= 0)
                            {
                                tree.DeleteNode(rootDelete);
                                Console.WriteLine("\nВузол успішно видалено!");
                            }
                            else { throw new Exception("некоректний індекс"); }

                            break;// Видалити елемент

                        case "3":
                            tree.PreorderTraversal();
                            break;// Здійснити обхід дерева

                        case "4":
                            if (tree.IsEmpty())
                            {
                                Console.WriteLine("\nДерево порожнє");
                            }
                            else { Console.WriteLine("\nДерево не порожнє"); }
                            break;// Перевірити на порожність

                        case "5":
                            tree.DisplayTree();
                            break;// Вивести структуру

                        case "6":
                            string root = GetValidStringInput("\nВведіть назву вузла: ");
                            int index = tree.FindNodeIndex(root);

                            if (index < 0) { throw new Exception("Невдалося нічого знайти"); }

                            Console.WriteLine($"Index: {index}, Info: {tree.nodes[index].Inf}, R: {tree.nodes[index].R}, PL: {tree.nodes[index].PL}");
                            break;// Здійснити пошук вузла

                        default:

                            break;
                    }
                }
                catch (Exception ex) { Console.WriteLine($"\n{ex.Message}"); }
            }

        }

        private static int GetValidIntInput(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out value))
                {
                    return value;
                }
                else
                {
                    Console.WriteLine("Помилка: введіть ціле число.");
                }
            }
        }

        private static string GetValidStringInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("Помилка: введіть непорожнє значення.");
                }
            }
        }

        private static bool IsValidChoice(string choice, int maxChoice)
        {
            if (int.TryParse(choice, out int numChoice))
            {
                return numChoice >= 1 && numChoice <= maxChoice;
            }
            return false;
        }
    
}
