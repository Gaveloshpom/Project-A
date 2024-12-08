using ASD_1_Lab;

void Main() 
{
    string input;

    QueueList queue = new QueueList();
//Enqueue — додавання елементу в чергу.
//Dequeue — видалення елементу з черги.
//Peek — перегляд першого елемента черги.
//IsEmpty — перевірка, чи порожня черга.
//Print — виведення елементів черги на екран.
//Count — розрахунок кількості елементів у черзі.
//CalculateSum — розрахунок суми елементів у черзі.
    while (true) 
    {
        try
        {
            Console.WriteLine("\n1-Add element" +
                "\n2-Delete element" +
                "\n3-First element" +
                "\n4-Is empty?" +
                "\n5-Show all elements" +
                "\n6-Number of elements" +
                "\n7-Calculate sum of all elements");
            input = Console.ReadLine();
            if (double.TryParse(input, out var value) && value >= 0 && value <= 7)
            {
                switch (int.Parse(input))
                {
                    case 0:
                        break;

                    case 1:
                        Console.Write("\nNumber to add:");
                        input = Console.ReadLine();
                        queue.Enqueue(Double.Parse(input));
                        Console.WriteLine("\nElement added");
                        break;

                    case 2:
                        if (!queue.IsEmpty())
                        {
                            Console.WriteLine("\nDeleted");
                        }
                        queue.Dequeue();
                        break;

                    case 3:
                        Console.WriteLine($"\nFirst element: {queue.Peek()}");
                        break;

                    case 4:
                        if (queue.IsEmpty() == true)
                        {
                            Console.WriteLine("\nQueue is empty");
                        }
                        else
                        {
                            Console.WriteLine("\nQueue is not empty");
                        }
                        break;

                    case 5:
                        queue.Print();
                        break;

                    case 6:
                        Console.WriteLine($"\nNumber of elements: {queue.Count()}");
                        break;

                    case 7:
                        Console.WriteLine($"\nSum of elements: {queue.CalculateSum()}");
                        break;
                }
            }
            else { throw new Exception("Input must be integer(0-7)"); }
        }
        catch (Exception e) { Console.WriteLine($"\n{e.Message}");}
    }
    
}
Main();

