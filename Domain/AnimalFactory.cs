using Residence;

namespace Domain
{
          // Абстрактний Factory Method (Creational Pattern)
    /// Використовується для створення різних типів тварин
    public abstract class AnimalFactory
    {
        public abstract Animal CreateAnimal(string name, IResidence residence);
    }

    public class DogFactory : AnimalFactory
    {
        public override Animal CreateAnimal(string name, IResidence residence)
        {
            return new Dog(name, residence);
        }
    }

    public class CanaryFactory : AnimalFactory
    {
        public override Animal CreateAnimal(string name, IResidence residence)
        {
            return new Canary(name, residence);
        }
    }

    public class LizardFactory : AnimalFactory
    {
        public override Animal CreateAnimal(string name, IResidence residence)
        {
            return new Lizard(name, residence);
        }
    }

 ///Simple Factory - зручний варіант для динамічного створення наприклад черещ рядок
    public static class SimpleAnimalFactory
    {
        public static Animal Create(string animalType, string name, IResidence residence)
        {
            return animalType.ToLower() switch
            {
                "dog" or "собака" => new Dog(name, residence),
                "canary" or "канарка" => new Canary(name, residence),
                "lizard" or "ящірка" => new Lizard(name, residence),
                _ => throw new ArgumentException($"Невідомий тип тварини: {animalType}")
            };
        }
    }
}