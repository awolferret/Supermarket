using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            Supermarket supermarket = new Supermarket();
            supermarket.Work();
        }
    }

    class Supermarket
    {
        private Queue<Client> _clients = new Queue<Client>();
        private List<Product> _products = new List<Product>();
        private Random _random = new Random();

        public void Work()
        {
            int revenue = 0;
            bool _isWorking = true;
            AddToAssorment();

            while (_isWorking)
            {
                Console.WriteLine("1. Создать очередь");
                Console.WriteLine("2. Обслужить очередь");
                Console.WriteLine("3. Выход");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        CreateClientQueue();
                        break;
                    case "2":
                        ServeClient(ref revenue);
                        break;
                    case "3":
                        _isWorking = false;
                        break;
                    default:
                        Console.WriteLine("Ошибка");
                        break;
                }
            }
        }

        private void AddToAssorment()
        {
            _products.Add(new Product("Молоко", GetProductCost()));
            _products.Add(new Product("Яблоки", GetProductCost()));
            _products.Add(new Product("Огурцы", GetProductCost()));
            _products.Add(new Product("Помидоры", GetProductCost()));
            _products.Add(new Product("Мясо", GetProductCost()));
            _products.Add(new Product("Сок Широкий Карамыш", GetProductCost()));
            _products.Add(new Product("Яйца", GetProductCost()));
            _products.Add(new Product("Хлеб", GetProductCost()));
        }

        private void CreateClientQueue()
        {
            int minCountClient = 2;
            int maxCountClient = 10;
            int countClient = _random.Next(minCountClient,maxCountClient);

            for (int i = 0; i < countClient; i++)
            {
                _clients.Enqueue(CreateClient());
            }
        }

        private void ServeClient(ref int revenue)
        {
            while (_clients.Count > 0)
            {
                _clients.Dequeue().PurchaseProducts(ref revenue);
            }
        }

        private Client CreateClient()
        {
            List<Product> products = new List<Product>();
            int minMoney = 25;
            int maxMoney = 100;
            int minProductCount = 1;
            int maxProductCount = 9;
            int money = _random.Next(minMoney, maxMoney);
            int productCount = _random.Next(minProductCount, maxProductCount);

            for (int i = 0; i < productCount; i++)
            {
                products.Add(_products[_random.Next(0, _products.Count)]);
            }

            return new Client(products, money);
        }

        private int GetProductCost()
        {
            int minCost = 5;
            int maxCost = 30;
            return _random.Next(minCost, maxCost);
        }
    }

    class Client
    {
        private List<Product> _products = new List<Product>();
        private int _money;

        public Client(List<Product> products, int money)
        {
            _money = money;
            _products = products;
        }

        public void PurchaseProducts(ref int revenue)
        {
            ShowProducts();
            Console.WriteLine($"Сумма товаров {GetPurchaseCost()}. У клиента {_money} денег");

            if (GetPurchaseCost() <= _money)
            {
                CalculateClient(ref revenue);
            }
            else 
            {
                RemoveProducts();
                CalculateClient(ref revenue);
            }
        }

        private void CalculateClient(ref int revenue) 
        {
            _money -= GetPurchaseCost();
            revenue += GetPurchaseCost();
            Console.WriteLine($"Клиент оплатил покупку и у него осталось {_money} денег");
            Console.WriteLine($"У магазина на счету {revenue}");
        }

        private void RemoveProducts()
        {
            while (GetPurchaseCost() > _money)
            {
                Random random = new Random();
                int index = random.Next(1, _products.Count);
                Product productToRemove = _products[index-1];
                Console.WriteLine($"Клиент отказался от товара {productToRemove.Name}");
                _products.RemoveAt(index-1);
            }
        }

        private int GetPurchaseCost()
        {
            int purchaseCoast = 0;

            foreach (var product in _products)
            {
                purchaseCoast += product.Cost;
            }

            return purchaseCoast;
        }

        private void ShowProducts()
        {
            Console.WriteLine("\nКорзина клиента");

            foreach (var product in _products)
            {
                Console.WriteLine(product.Name + " " + product.Cost);
            }
        }
    }

    class Product
    {
        public string Name { get; private set; }
        public int Cost { get; private set; }

        public Product(string name, int cost)
        {
            Name = name;
            Cost = cost;
        }
    }
}