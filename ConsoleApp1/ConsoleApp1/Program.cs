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
        private bool _isWorking = true;

        public void Work()
        {
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
                        ServeClient();
                        break;
                    case "3":
                        Exit();
                        break;
                }
            }
        }

        public void AddToAssorment()
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

        public void CreateClientQueue()
        {
            int minCountClient = 2;
            int maxCountClient = 10;
            int countClient = _random.Next(minCountClient,maxCountClient);

            for (int i = 0; i < countClient; i++)
            {
                _clients.Enqueue(GetClient());
            }
        }

        public void ServeClient()
        {
            while (_clients.Count > 0)
            {
                _clients.Dequeue().PurchaseProducts();
            }
        }

        private Client GetClient()
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
            int costProduct = _random.Next(minCost, maxCost);
            return costProduct;
        }

        private void Exit()
        {
            _isWorking = false;
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

        public void PurchaseProducts()
        {
            ShowProducts();
            Console.WriteLine($"Сумма товаров {GetPurchaseCost()}. У клиента {_money} денег");

            if (GetPurchaseCost() <= _money)
            {
                Console.WriteLine("Клиент оплатил покупку");
            }
            else 
            {
                RemoveProduct();
            }
        }

        private void RemoveProduct()
        {
            while (GetPurchaseCost() > _money)
            {
                Random random = new Random();
                int index = random.Next(1, _products.Count);
                Product productToRemove = _products[index];
                Console.WriteLine($"Клиент отказался от товара {productToRemove.ProductName}");
                _products.RemoveAt(index);
            }
        }

        private int GetPurchaseCost()
        {
            int PurchaseCoast = 0;

            foreach (var item in _products)
            {
                PurchaseCoast += item.ProductCost;
            }

            return PurchaseCoast;
        }

        private void ShowProducts()
        {
            Console.WriteLine("Корзина клиента");

            foreach (var item in _products)
            {
                Console.WriteLine(item.ProductName + item.ProductCost);
            }
        }
    }

    class Product
    {
        public string ProductName { get; private set; }
        public int ProductCost { get; private set; }

        public Product(string productName, int productCost)
        {
            ProductName = productName;
            ProductCost = productCost;
        }
    }
}