using System;
using Residence;

namespace Domain
{
    public abstract class Animal
    {
        public string Name { get; private set; }
        public int Eyes { get; protected set; }
        public int Legs { get; protected set; }
        public int Wings { get; protected set; }

        // Стан та лічильники
        public int HoursSinceFed { get; private set; } = 0;
        public int TimesFedToday { get; private set; } = 0;
        public int TimesCleanedToday { get; private set; } = 0;

        public bool IsAlive { get; private set; } = true;
        public bool IsHappy { get; private set; } = false;

        public IResidence? Residence { get; set; }

        // Подія: повідомляє про спробу виконати дію і результат (успіх / причина невдачі)
        public event EventHandler<ActionEventArgs>? ActionOccurred;

        // Подія смерті
        public event EventHandler? Died;

        protected Animal(string name, IResidence? residence)
        {
            Name = name;
            Residence = residence;
        }

        // ================= ЕATING =================
        public void Eat()
        {
            if (!IsAlive)
            {
                RaiseAction("Eat", false, "Тварина мертва.");
                return;
            }

            if (TimesFedToday >= 5)
            {
                RaiseAction("Eat", false, "Перевищено максимум (5) годувань за добу.");
                return;
            }

            HoursSinceFed = 0;
            TimesFedToday++;
            RaiseAction("Eat", true, "Тварина поїла.");
            UpdateHappiness();
        }

        // ================= MOVEMENT / ACTIONS =================
        // Walk — завжди дозволено для живої тварини
        public void Walk()
        {
            if (!IsAlive)
            {
                RaiseAction("Walk", false, "Тварина мертва.");
                return;
            }

            RaiseAction("Walk", true, "Тварина йде.");
        }

        public void Run()
        {
            if (!IsAlive)
            {
                RaiseAction("Run", false, "Тварина мертва.");
                return;
            }

            if (HoursSinceFed > 8)
            {
                RaiseAction("Run", false, "Тварина голодна більше 8 годин і не може бігати.");
                return;
            }

            RaiseAction("Run", true, "Тварина біжить.");
        }

        public void Fly()
        {
            if (!IsAlive)
            {
                RaiseAction("Fly", false, "Тварина мертва.");
                return;
            }

            if (Wings == 0)
            {
                RaiseAction("Fly", false, "Ця тварина не має крил.");
                return;
            }

            if (HoursSinceFed > 8)
            {
                RaiseAction("Fly", false, "Тварина голодна більше 8 годин і не може літати.");
                return;
            }

            RaiseAction("Fly", true, "Тварина летить.");
        }

        public void Sing()
        {
            if (!IsAlive)
            {
                RaiseAction("Sing", false, "Тварина мертва.");
                return;
            }

            if (HoursSinceFed > 8)
            {
                RaiseAction("Sing", false, "Тварина голодна більше 8 годин і не може співати.");
                return;
            }

            RaiseAction("Sing", true, "Тварина співає.");
        }

        // ================= CARE =================
        public void Clean()
        {
            if (!IsAlive)
            {
                RaiseAction("Clean", false, "Тварина мертва.");
                return;
            }

            if (Residence is Wild)
            {
                TimesCleanedToday = 0;
                UpdateHappiness();
                RaiseAction("Clean", true, "Тварина на волі — догляд не потрібен, але вона щаслива.");
                return;
            }

            TimesCleanedToday++;
            UpdateHappiness();
            RaiseAction("Clean", true, "Тварина доглянута.");
        }

        // ================= TIME / DAY =================
        public void AddHour()
        {
            if (!IsAlive) return;

            HoursSinceFed++;

            // помре якщо не кормити 24+ годин
            if (HoursSinceFed >= 24)
            {
                IsAlive = false;
                Died?.Invoke(this, EventArgs.Empty);
            }
        }
        public void NewDay()
        {
            if (!IsAlive) return;

            if (TimesFedToday < 1)
            {
                IsAlive = false;
                Died?.Invoke(this, EventArgs.Empty);
                return;
            }

            // ресет прибирання та годування 
            TimesFedToday = 0;
            TimesCleanedToday = 0;
            UpdateHappiness();
        }

        // =============== Helpers ===============
        protected void RaiseAction(string actionName, bool success, string message)
        {
            ActionOccurred?.Invoke(this, new ActionEventArgs(actionName, success, message));
        }

        private void UpdateHappiness()
        {
            if (Residence is Wild)
                IsHappy = true;
            else
                IsHappy = TimesCleanedToday >= 1;
        }
    }
}