using System.Collections.Generic;
using Domain;

namespace Services
{
    public class SimulationService
    {
        private readonly FeedingService feedingService;
        private readonly CareService careService;

        public SimulationService(FeedingService feedingService, CareService careService)
        {
            this.feedingService = feedingService;
            this.careService = careService;
        }

        // Симулювати один день (годування, дії, прибирання, перевірка смерті)
        public void SimulateDay(IEnumerable<Animal> animals)
        {
            // Симуляція 24 годин
            for (int hour = 0; hour < 24; hour++)
            {
                foreach (var animal in animals)
                {
                    if (!animal.IsAlive) continue;

                    //годування кожні 6 годин
                    if (hour % 6 == 0)
                    {
                        feedingService.Feed(animal);
                    }

                    // Тварина виконує дії
                    animal.Walk();
                    animal.Run();
                    animal.Fly();
                    animal.Sing();

                    // Проходить одна година
                    animal.AddHour();
                }
            }

            // Прибирання наприкінці дня
            careService.CleanAll(animals);

            // Завершення доби (перевірка мінімального годування + скидання лічильників)
            foreach (var animal in animals)
            {
                if (animal.IsAlive)
                    animal.NewDay();
            }
        }
        // Симулювати одну годину
        public void SimulateHour(IEnumerable<Animal> animals)
        {
            foreach (var animal in animals)
            {
                animal.AddHour();
            }
        }
    }
}