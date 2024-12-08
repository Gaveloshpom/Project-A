using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASD_1_Lab 
{
    public class Elem
    {
        public double Data;  // Дійсне число
        public Elem Next;    // Посилання на наступний

        public Elem(double data)
        {
            Data = data;
            Next = null;
        }
    }

    public class QueueList
    {
        private Elem head;  // Початок черги
        private Elem tail;  // Кінець черги
        private int count;  // Лічильник елементів

        public QueueList()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public void Enqueue(double data)
        {
            Elem newElem = new Elem(data);

            if (tail != null)
            {
                tail.Next = newElem;
            }

            tail = newElem;

            if (head == null)
            {
                head = tail;
            }

            count++;
        }

        public double Dequeue()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Queue is empty");
            }

            double value = head.Data;
            head = head.Next;

            if (head == null)
            {
                tail = null; 
            }

            count--;  

            return value;
        }

        public double Peek()
        {
            if (IsEmpty())
            {
                throw new Exception("Queue is empty");
            }

            return head.Data;
        }

        public bool IsEmpty()
        {
            return head == null;
        }

        public void Print()
        {
            if (IsEmpty())
            {
                throw new Exception("Queue is empty");
            }

            Elem current = head;
            Console.WriteLine("Elements:");

            while (current != null)
            {
                Console.Write(current.Data + " ");
                current = current.Next;
            }

            Console.WriteLine();
        }

        public int Count()
        {
            return count;
        }

        public double CalculateSum()
        {
            double sum = 0;
            Elem current = head;

            while (current != null)
            {
                sum += current.Data;
                current = current.Next;
            }

            return sum;
        }
    }
}