using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            Hashtable ht = new Hashtable();
            ht.Add("URKA", "Сергей Чемезов");
            //ht.Add("URKA", "Дмитрий Осипов");
            ht.Add("GAZP", "Алексей Миллер");
            //ht.Add("GAZP", "Виктор Зубков");
            //ht.Add("GAZP", "Александр Новак");

            foreach(var key in ht.Keys)
                Console.WriteLine($"{key} : {ht[key]}");

            Console.ReadLine();
        }
    }
}
