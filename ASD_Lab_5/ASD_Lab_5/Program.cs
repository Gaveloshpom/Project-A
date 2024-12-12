using System;
using System.Collections.Generic;

// Клас для вузла двозв'язного списку (зв'язки)
class EdgeNode
{
    public int Vertex { get; set; } // Вершина, до якої веде зв'язок
    public EdgeNode Next { get; set; } // Наступний зв'язок
    public EdgeNode Prev { get; set; } // Попередній зв'язок

    public EdgeNode(int vertex)
    {
        Vertex = vertex;
        Next = null;
        Prev = null;
    }
}

// Клас для вузла однозв'язного циклічного списку (вершини)
class VertexNode
{
    public int Vertex { get; set; } // Номер вершини
    public EdgeNode EdgeList { get; set; } // Початок двозв'язного списку зв'язків
    public VertexNode Next { get; set; } // Наступна вершина

    public VertexNode(int vertex)
    {
        Vertex = vertex;
        EdgeList = null;
        Next = null;
    }

    // Додавання зв'язку до двозв'язного списку
    public void AddEdge(int toVertex)
    {
        EdgeNode newEdge = new EdgeNode(toVertex);
        if (EdgeList == null)
        {
            EdgeList = newEdge;
            EdgeList.Next = newEdge; // Циклічність
            EdgeList.Prev = newEdge; // Циклічність
        }
        else
        {
            EdgeNode last = EdgeList.Prev;
            last.Next = newEdge;
            newEdge.Prev = last;
            newEdge.Next = EdgeList;
            EdgeList.Prev = newEdge;
        }
    }
}

// Клас для графа
class Graph
{
    private VertexNode head; // Головна вершина графа

    public Graph()
    {
        head = null;
    }

    // Додавання вершини до графа
    public VertexNode AddVertex(int vertex)
    {
        VertexNode newVertex = new VertexNode(vertex);
        if (head == null)
        {
            head = newVertex;
            head.Next = head; // Циклічність
        }
        else
        {
            VertexNode temp = head;
            while (temp.Next != head)
                temp = temp.Next;

            temp.Next = newVertex;
            newVertex.Next = head; // Циклічність
        }
        return newVertex;
    }

    // Знайти вершину за номером
    public VertexNode FindVertex(int vertex)
    {
        if (head == null) return null;

        VertexNode temp = head;
        do
        {
            if (temp.Vertex == vertex)
                return temp;
            temp = temp.Next;
        } while (temp != head);

        return null;
    }

    // Додавання зв'язку між вершинами
    public void AddEdge(int fromVertex, int toVertex)
    {
        VertexNode fromNode = FindVertex(fromVertex);
        if (fromNode == null)
            fromNode = AddVertex(fromVertex);

        fromNode.AddEdge(toVertex);
    }

    // Виведення графа
    public void Display()
    {
        if (head == null)
        {
            Console.WriteLine("Graph is empty.");
            return;
        }

        VertexNode temp = head;
        do
        {
            Console.Write($"Vertex {temp.Vertex}: ");
            EdgeNode edgeTemp = temp.EdgeList;
            if (edgeTemp != null)
            {
                do
                {
                    Console.Write($"{edgeTemp.Vertex} -> ");
                    edgeTemp = edgeTemp.Next;
                } while (edgeTemp != temp.EdgeList);
            }
            Console.WriteLine("Cyclic");
            temp = temp.Next;
        } while (temp != head);
    }
}

// Основна програма
class Program
{
    static void Main(string[] args)
    {
        Graph graph = new Graph();

        // Зчитування з файлу
        Console.WriteLine("Enter the file path:");
        string filePath = Console.ReadLine();

        try
        {
            string[] lines = System.IO.File.ReadAllLines(filePath);
            int m = int.Parse(lines[0]); // Кількість зв'язків

            for (int i = 1; i <= m; i++)
            {
                string[] edge = lines[i].Split(' ');
                int from = int.Parse(edge[0]);
                int to = int.Parse(edge[1]);
                graph.AddEdge(from, to);
            }

            // Виведення графа
            Console.WriteLine("Graph Representation:");
            graph.Display();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
