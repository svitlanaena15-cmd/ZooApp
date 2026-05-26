using System;
using System.Collections.Generic;
using Domain;
using Residence;
using Services;

namespace Application
{
    public class ZooApp
    {
        private List<Animal> animals;
        private Owner owner;
        private PetShop shop;
        private Wild wild;

        private FeedingService feedingService;
        private CareService careService;
        private SimulationService simulationService;

        public ZooApp()
        {
            owner = new Owner("Світлана");
            shop = new PetShop("Зоомагазин");
            wild = new Wild();

            feedingService = new FeedingService();
            careService = new CareService();
            simulationService = new SimulationService(feedingService, careService);

            animals = new List<Animal>();

//Використання Factory Method Pattern
//Дозволяє легко додавати нові типи тварин без зміни цього класу
            var dogFactory = new DogFactory();
            var canaryFactory = new CanaryFactory();
            var lizardFactory = new LizardFactory();

            var dog = dogFactory.CreateAnimal("Бобік", owner);
            var canary = canaryFactory.CreateAnimal("Кеша", shop);
            var lizard = lizardFactory.CreateAnimal("Рексі", owner);

            owner.AddAnimal(dog);
            shop.AddAnimal(canary);
            owner.AddAnimal(lizard);

            animals.Add(dog);
            animals.Add(canary);
            animals.Add(lizard);

            //Підписка на події домену — UI буде виводити повідомлення
            foreach (var a in animals)
            {
                a.ActionOccurred += OnAnimalAction;
                a.Died += OnAnimalDied;
            }
        }

        private void OnAnimalAction(object? sender, ActionEventArgs e)
        {
            //Вивод повідомлень
            var animal = sender as Animal;
            string name = animal?.Name ?? "Тварина";
            string status = e.Success ? "Успіх" : "Невдача";
            Console.WriteLine($"[{name}] {e.ActionName}: {(e.Success ? e.Message : ("Не вдалося — " + e.Message))}");
        }

        private void OnAnimalDied(object? sender, EventArgs e)
        {
            var animal = sender as Animal;
            Console.WriteLine($"[Смерть] {animal?.Name ?? "Тварина"} померла через голод.");
        }

        // інтерактив
        public void Run()
        {
            bool exit = false;

            while (!exit)
            {
                ShowAnimals();
                Console.WriteLine("Оберіть дію:");
                Console.WriteLine("1. Покормити всіх тварин");
                Console.WriteLine("2. Симуляція 1 години");
                Console.WriteLine("3. Прибрати за тваринами");
                Console.WriteLine("4. Випустити на волю");
                Console.WriteLine("5. Пограти з твариною");
                Console.WriteLine("6. Прогулянка");
                Console.WriteLine("7. Симуляція одного дня");
                Console.WriteLine("8. Додати нову тварину");
                Console.WriteLine("9. Вихід");

                string input = Console.ReadLine() ?? "";

                switch (input)
                {
                    case "1":
                        foreach (var a in animals)
                            feedingService.Feed(a);
                        break;

                    case "2":
                        simulationService.SimulateHour(animals);
                        Console.WriteLine("Одна година симуляції пройшла.");
                        break;

                    case "3":
                        foreach (var a in animals) 
                            careService.Clean(a);
                        break;

                    case "4":
                        ReleaseAnimal();
                        break;

                    case "5":
                        PlayMenu();
                        break;

                    case "6":
                        WalkMenu();
                        break;

                    case "7":
                        simulationService.SimulateDay(animals);
                        Console.WriteLine("День симуляції пройшов.");
                        break;

                    case "8":
                        AddNewAnimal();
                        break;

                    case "9":
                        exit = true;
                        break;
                }

                Console.WriteLine();
            }
        }

        private void ShowAnimals()
        {
            Console.WriteLine("=== Ваші тварини ===");

            foreach (var a in animals)
            {
                Console.WriteLine($"{a.Name} | {(a.IsAlive ? "Живий" : "Мертвий")} | Годин без їжі: {a.HoursSinceFed} | Щасливий: {(a.IsHappy ? "Так" : "Ні")} | Місце: {a.Residence?.Name ?? "Невідомо"}");
            }

            Console.WriteLine();
        }

        private void ReleaseAnimal()
        {
            Console.Write("Введіть ім'я тварини: ");
            string name = Console.ReadLine() ?? "";

            var animal = animals.Find(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (animal == null || !animal.IsAlive)
            {
                Console.WriteLine("Тварина не знайдена або мертва.");
                return;
            }

            if (animal.Residence is Owner o) o.RemoveAnimal(animal);
            if (animal.Residence is PetShop s) s.RemoveAnimal(animal);

            wild.AddAnimal(animal);

            Console.WriteLine($"{animal.Name} випущений на волю.");
        }

        private void PlayMenu()
        {
            Console.Write("Оберіть тварину: ");
            string name = Console.ReadLine() ?? "";

            var animal = animals.Find(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (animal == null || !animal.IsAlive)
            {
                Console.WriteLine("Тварина не знайдена або мертва.");
                return;
            }

            bool back = false;

            while (!back)
            {
                Console.WriteLine("1. Кинути м'ячик");
                Console.WriteLine("2. Команда 'Голос'");
                Console.WriteLine("3. Назад");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Ви кинули м'ячик.");
                        if (animal is Canary)
                            animal.Fly();
                        else
                            animal.Run();
                        break;

                    case "2":
                        animal.Sing();
                        break;

                    case "3":
                        back = true;
                        break;
                }
            }
        }

        private void AddNewAnimal()
        {
            Console.WriteLine("Оберіть тип тварини:");
            Console.WriteLine("1. Собака");
            Console.WriteLine("2. Канарка");
            Console.WriteLine("3. Ящірка");
            string typeChoice = Console.ReadLine() ?? "";

            string type = typeChoice switch
            {
                "1" => "dog",
                "2" => "canary",
                "3" => "lizard",
                _ => ""
            };

            if (string.IsNullOrEmpty(type))
            {
                Console.WriteLine("Невірний вибір.");
                return;
            }

            Console.Write("Введіть ім'я тварини: ");
            string name = Console.ReadLine() ?? "Невідома";

            Console.WriteLine("Де буде жити тварина?");
            Console.WriteLine("1. У власника");
            Console.WriteLine("2. У зоомагазині");
            string residenceChoice = Console.ReadLine() ?? "1";

            IResidence residence = residenceChoice == "2" ? shop : owner;

            // === Використання SimpleAnimalFactory ===
            Animal newAnimal = SimpleAnimalFactory.Create(type, name, residence);

            animals.Add(newAnimal);
            newAnimal.ActionOccurred += OnAnimalAction;
            newAnimal.Died += OnAnimalDied;

            if (residence is Owner o) o.AddAnimal(newAnimal);
            else if (residence is PetShop s) s.AddAnimal(newAnimal);

            Console.WriteLine($"Тварина {name} успішно додана!");
        }

        private void WalkMenu()
        {
            Console.Write("Оберіть тварину для прогулянки: ");
            string name = Console.ReadLine() ?? "";

            var animal = animals.Find(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (animal == null || !animal.IsAlive)
            {
                Console.WriteLine("Тварина не знайдена або мертва.");
                return;
            }

            animal.Walk();
        }
    }
}