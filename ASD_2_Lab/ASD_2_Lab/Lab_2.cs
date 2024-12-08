using System;
using System.Xml.Linq;

class Elem
{
    public double Value;
    public Elem Next;

    public Elem(double value)
    {
        Value = value;
        Next = null;
    }
}

class CyclicList
{
    private Elem top;

    public CyclicList()
    {
        top = null;
    }

    public void Push(double value)
    {
        Elem newNode = new Elem(value);

        if (top == null)
        {
            top = newNode;
            top.Next = top; 
        }
        else
        {
            Elem temp = top;
            while (temp.Next != top)
            {
                temp = temp.Next;
            }
            temp.Next = newNode;
            newNode.Next = top;
            top = newNode; 
        }
    }

    public double Pop()
    {
        if (top == null)
        {
            throw new Exception("Stack is empty");
        }

        double value = top.Value;

        if (top.Next == top)
        {
            top = null;
        }
        else
        {
            Elem temp = top;
            while (temp.Next != top)
            {
                temp = temp.Next;
            }
            temp.Next = top.Next;
            top = top.Next;
        }

        return value;
    }

    public double Peek()
    {
        if (top == null)
        {
            throw new Exception("Stack is empty");
        }

        return top.Value;
    }

    public bool IsEmpty()
    {
        return top == null;
    }

    public int Count()
    {
        if (top == null)
        {
            return 0;
        }

        int count = 0;
        Elem current = top;

        do
        {
            count++;
            current = current.Next;
        }
        while (current != top);

        return count;
    }

    public void Print()
    {
        if (top == null)
        {
            throw new Exception("Stack is empty");
        }

        Elem current = top;
        int index = 0;

        Console.WriteLine("Stack elements:");

        do
        {
            Console.WriteLine($"Elem {current.Value} -> {current.Next.Value}");
            current = current.Next;
            index++;
        }
        while (current != top);
    }

    public double CalculatePositiveProduct()
    {
        if (top == null)
        {
            throw new Exception("Stack is empty");
        }

        double product = 1;
        bool hasPositive = false;
        Elem current = top;

        do
        {
            if (current.Value > 0)
            {
                product *= current.Value;
                hasPositive = true;
            }
            current = current.Next;
        }
        while (current != top);

        return hasPositive ? product : 0;
    }
}

class Lab_2
{
    static void Main()
    {
        string input;

        CyclicList Clist = new CyclicList();
        //Enqueue — додавання елементу в список.
        //Dequeue — видалення елементу зі списку.
        //Peek — перегляд першого елемента списку.
        //IsEmpty — перевірка, чи порожня список.
        //Print — виведення елементів списку на екран.
        //Count — розрахунок кількості елементів у списку.
        //Calculate — розрахунок добутку додатних елементів у списку.
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
                    "\n7-Calculate");
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
                            Clist.Push(Double.Parse(input));
                            Console.WriteLine("\nSuccess");
                            break;

                        case 2:
                            Clist.Pop();
                            Console.WriteLine("\nDeleted");
                            break;

                        case 3:
                            Console.WriteLine($"\nFirst element: {Clist.Peek()}");
                            break;

                        case 4:
                            if (Clist.IsEmpty() == true)
                            {
                                Console.WriteLine("\nStack is empty");
                            }
                            else
                            {
                                Console.WriteLine("\nStack is not empty");
                            }
                            break;

                        case 5:
                            Clist.Print();
                            break;

                        case 6:
                            Console.WriteLine($"\nNumber of elements: {Clist.Count()}");
                            break;

                        case 7:
                            Console.WriteLine($"\nProduct of elements: {Clist.CalculatePositiveProduct()}");
                            break;
                    }
                }
                else { throw new Exception("Input must be integer(0-7)"); }
            }
            catch (Exception e) { Console.WriteLine($"\n{e.Message}"); }
        }

    }
}
