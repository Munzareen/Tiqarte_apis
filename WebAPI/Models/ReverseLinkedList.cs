using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class LinkedList
    {
        public string Text { get; set; } = null!;
        LinkedList Next { get; set; } = null!;

        public void Add(string text)
        {
            if (this.Next is not null)
            {
                this.Next.Add(text);
            }
            else
            {
                this.Next = new LinkedList { Text = text };
            }
        }

        public void Print()
        {
            if (this.Next is not null)
            {
                this.Next.Print();
            }

            Console.WriteLine(this.Text);
        }
    }

    public static void Main()
    {
        LinkedList lst = new LinkedList();
        lst.Add("One");
        lst.Add("Two");
        lst.Add("Three");

        lst.Print();
    }
}