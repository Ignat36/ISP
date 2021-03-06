﻿using System;
using System.Text;
using System.Collections.Generic;

namespace Cars
{
    class Item
    {
        private string ItemName;
        private double ItemVolume;
        private bool NullItemFlag;

        public string Name
        {
            get
            {
                return ItemName;
            }
        }
        public double Volume
        {
            get
            {
                return ItemVolume;
            }
        }
        public Item(string name, double volume = 0.0001)
        {
            name.Clone();
            ItemName = name;
            ItemVolume = volume;
            NullItemFlag = false;
        }
        public Item(string name, double height, double length, double width)
        {
            name.Clone();
            ItemName = name;
            ItemVolume = height * length * width;
            NullItemFlag = false;
        }

        public Item() { NullItemFlag = true; }

        public bool IsNull
        {
            get { return NullItemFlag; }
        }
    }
    abstract class TransportFacility
    {
        protected int Weight, MaxWeight;
        protected int PassengerSeats;
        protected int MaxSpeed;
        protected double MaxDistance;
    }
    abstract class Car : TransportFacility
    {
        protected int WheelAxle, Wheels;

        protected Int64 IdentificationNumber;
        protected string Number;
        protected string Model, Brand;

        protected double Fuel;
        protected int TankCapacity;
        protected double FuelFlow;

        protected int height, length, width;
        protected int TrunkVolume;

        protected List<Item> Trunk;
        protected double OccupiedVolume;

        public Car() { }
        public Car(int Weight, int MaxWeight, int PassengerSeats, int TankCapacity, int TrunkVolume, double FuelFlow, int MaxSpeed)
        {
            this.Weight = Weight;
            this.MaxWeight = MaxWeight;
            this.PassengerSeats = PassengerSeats;
            this.TankCapacity = TankCapacity;
            this.TrunkVolume = TrunkVolume;
            this.FuelFlow = FuelFlow;
            this.MaxSpeed = MaxSpeed;

            Fuel = 0;
            OccupiedVolume = 0;
            MaxDistance = TankCapacity * FuelFlow;
            Trunk = new List<Item>();
            IdentificationNumber = Math.Abs(GetRand());
        }

        static Int64 GetRand()
        {
            Random rnd = new Random();
            Int64 tmp = (long)((rnd.Next() * rnd.Next()) % 1e18);
            return tmp;
        }

        public void AddItem(Item NewItem)
        {
            if (OccupiedVolume + NewItem.Volume > TrunkVolume)
                Console.WriteLine("В багажнике недостаточно места для данного предмета");
            else
            {
                Trunk.Add(NewItem);
                OccupiedVolume += NewItem.Volume;
            }
        }

        public void ShoWTrunkContent()
        {
            if (Trunk.Count <= 0) return;

            Console.WriteLine("*********************************\n" +
                              "Содержимое багажника : ");

            for(int i = 0; i < Trunk.Count; i++)
                Console.WriteLine(Trunk[i].Name);

            Console.WriteLine("*********************************");
        }

        public Item TakeItem(string name)
        {
            for (int i = 0; i < Trunk.Count; i++)
                if (Trunk[i].Name == name)
                {
                    Item tmp = Trunk[i];
                    
                    Trunk.RemoveAt(i);
                    OccupiedVolume -= tmp.Volume;

                    return tmp;
                }

            Console.WriteLine("В багажнике нат денного предмета");
            return new Item();
        }

        public virtual void FillUpTank(double Fuel)
        {
            if (Fuel <= 0)
                return;

            if (this.Fuel + Fuel >= TankCapacity)
            {
                this.Fuel = TankCapacity;
                if (TankCapacity - this.Fuel - Fuel < 0)
                    Console.WriteLine("Бак полон, остаток бензина : {0} л.", Fuel + this.Fuel - TankCapacity);
                else
                    Console.WriteLine("Бак полон");
                this.Fuel = TankCapacity;
            }
            else
            {
                this.Fuel += Fuel;
                Console.WriteLine("В баке {0} л. топлива", this.Fuel);
            }
        }

        public virtual void FillUpTank()
        {
            if(Fuel == TankCapacity)
                Console.WriteLine("Бак полон");
            else
            {
                Console.WriteLine("Долито {0} л. топлива, бак полон", TankCapacity - Fuel);
                Fuel = TankCapacity;
            }
        }

        public virtual double DrainFuel(double Fuel)
        {
            if(Fuel >= this.Fuel)
            {
                Console.WriteLine("Слито {0} л. топлива, бак пуст", this.Fuel);
                Fuel = this.Fuel;
                this.Fuel = 0;
                return Fuel;
            }
            else
            {
                Console.WriteLine("Слито {0} л. топлива, в баке осталось {1} л.", Fuel, this.Fuel - Fuel);
                this.Fuel -= Fuel;
                return Fuel;
            }
        }

        public void Move(double distance)
        {
            if(distance * FuelFlow > Fuel)
            {
                Console.WriteLine("Недостаточно топлива");
                return;
            }

            Fuel -= distance * FuelFlow;
        }

        public double CurrentFuelVolume
        {
            get
            {
                return Fuel;
            }
        }

        public void ShowInfo()
        {
            Console.WriteLine("Объем бака : {0}\n" +
                "Расход топлива : {1} л. на 100 км.\n" +
                "Объем багажника : {2} л.\n" +
                "Масса автомобиля : {3} т.\n" +
                "Грузоподъемность : {4} кг.\n" +
                "Максимальная скорость : {5} км/ч\n" +
                "Кол-во пассажирских мест : {6}\n" +
                "Идентификационный номер : {7}\n" +
                "Название автомобиля : {8}", TankCapacity, FuelFlow, TrunkVolume, Weight / 1000.0, MaxWeight - Weight, MaxSpeed, PassengerSeats, IdentificationNumber, Brand + Model);
        }
    }
    sealed class Lamborgini : Car
    {
        enum Mode { ON, OFF}
        private Mode SportiveMode;
        public void SportON()
        {
            if (SportiveMode == Mode.OFF)
            {
                MaxSpeed = (int)(MaxSpeed * 1.5);
                SportiveMode = Mode.ON;
            }
            else
                Console.WriteLine("Sportive mode is alrady ON!");
        }
        public void SportOFF()
        {
            if (SportiveMode == Mode.ON)
            {
                MaxSpeed = (int)(MaxSpeed / 1.5);
                SportiveMode = Mode.OFF;
            }
            else
                Console.WriteLine("Sportive mode is alrady ON!");
        }
        public Lamborgini(string CarModel, int Weight, int MaxWeight, int PassengerSeats, int TankCapacity, int TrunkVolume, double FuelFlow, int MaxSpeed) : 
            base(Weight, MaxWeight, PassengerSeats, TankCapacity, TrunkVolume, FuelFlow, MaxSpeed)
        {
            Brand = "Lamborgini";
            Model = CarModel;
            SportiveMode = Mode.OFF;
        }
    }
    sealed class Tesla : Car
    {
        public Tesla(string CarModel, int Weight, int MaxWeight, int PassengerSeats, int TankCapacity, int TrunkVolume, double FuelFlow, int MaxSpeed) :
            base(Weight, MaxWeight, PassengerSeats, TankCapacity, TrunkVolume, FuelFlow, MaxSpeed)
        {
            Brand = "Tesla";
            Model = CarModel;
        }

        public override void FillUpTank()
        {
            if (Fuel == TankCapacity)
                Console.WriteLine("Аккумулятор полностью заряжен");
            else
            {
                Console.WriteLine("Дозаряжено {0} mA/h., Аккумулятор полностью заряежн", TankCapacity - Fuel);
                Fuel = TankCapacity;
            }
        }

        public override void FillUpTank(double Fuel)
        {
            if (Fuel <= 0)
                return;

            if (this.Fuel + Fuel >= TankCapacity)
            {
                this.Fuel = TankCapacity;
                if (TankCapacity - this.Fuel - Fuel < 0)
                    Console.WriteLine("Аккумулятор полностью заряжен, остаток электричества : {0} mA/h.", Fuel + this.Fuel - TankCapacity);
                else
                    Console.WriteLine("Аккумулятор полностью заряжен");
                this.Fuel = TankCapacity;
            }
            else
            {
                this.Fuel += Fuel;
                Console.WriteLine("Аккумулятор заряжен до {0} mA/h.", this.Fuel);
            }
        }

        public override double DrainFuel(double Fuel)
        {
            if (Fuel >= this.Fuel)
            {
                Console.WriteLine("Передано {0} mA/h. заряда, аккумулятор разряжен", this.Fuel);
                Fuel = this.Fuel;
                this.Fuel = 0;
                return Fuel;
            }
            else
            {
                Console.WriteLine("Передано {0} mA/h. заряда, в аккумуляторе осталось {1} mA/h.", Fuel, this.Fuel - Fuel);
                this.Fuel -= Fuel;
                return Fuel;
            }
        }
    }
    sealed class McLaren : Car
    {
        public McLaren(string CarModel, int Weight, int MaxWeight, int PassengerSeats, int TankCapacity, int TrunkVolume, double FuelFlow, int MaxSpeed) :
            base(Weight, MaxWeight, PassengerSeats, TankCapacity, TrunkVolume, FuelFlow, MaxSpeed)
        {
            Brand = "McLaren";
            Model = CarModel;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int Weight, MaxWeight;
            int PassengerSeats;
            int TankCapacity;
            int TrunkVolume;
            double FuelFlow;
            int MaxSpeed;
            string tmp;
            do
            {
                Console.WriteLine("Введите вес автомобиля : ");
                tmp = Console.ReadLine();
            } while (!int.TryParse(tmp, out Weight));

            do
            {
                Console.WriteLine("Введите грузоподъемность автомобиля : ");
                tmp = Console.ReadLine();
            } while (!int.TryParse(tmp, out MaxWeight));

            MaxWeight += Weight;

            do
            {
                Console.WriteLine("Введите кол-во пассажирских мест в автомобиле : ");
                tmp = Console.ReadLine();
            } while (!int.TryParse(tmp, out PassengerSeats));

            do
            {
                Console.WriteLine("Введите объем бака автомобиля : ");
                tmp = Console.ReadLine();
            } while (!int.TryParse(tmp, out TankCapacity));

            do
            {
                Console.WriteLine("Введите объем багажника автомобиля : ");
                tmp = Console.ReadLine();
            } while (!int.TryParse(tmp, out TrunkVolume));

            do
            {
                Console.WriteLine("Введите расход топлива на 100 километров : ");
                tmp = Console.ReadLine();
            } while (!double.TryParse(tmp, out FuelFlow));

            do
            {
                Console.WriteLine("Введите максимальную скорость автомобиля : ");
                tmp = Console.ReadLine();
            } while (!int.TryParse(tmp, out MaxSpeed));

            Lamborgini MyCar = new Lamborgini("Aventodor", Weight, MaxWeight, PassengerSeats, TankCapacity, TrunkVolume, FuelFlow, MaxSpeed);

            int request = 1;

            while(request > 0)
            {
                tmp = Console.ReadLine();
                int.TryParse(tmp, out request);

                double x;
                switch (request)
                {
                    case 1:
                        MyCar.ShowInfo();
                        break;
                    case 2:
                        MyCar.ShoWTrunkContent();
                        break;
                    case 3:
                        Console.WriteLine("В баке {0} л. топлива", MyCar.CurrentFuelVolume);
                        break;
                    case 4:
                        tmp = Console.ReadLine();
                        double.TryParse(tmp, out x);
                        MyCar.Move(x);
                        break;
                    case 5:
                        tmp = Console.ReadLine();
                        double.TryParse(tmp, out x);
                        x = MyCar.DrainFuel(x);
                        break;
                    case 6:
                        MyCar.FillUpTank();
                        break;
                    case 7:
                        tmp = Console.ReadLine();
                        double.TryParse(tmp, out x);
                        MyCar.FillUpTank(x);
                        break;
                    case 8:
                        tmp = Console.ReadLine();
                        double.TryParse(tmp, out x);
                        tmp = Console.ReadLine();
                        MyCar.AddItem(new Item(tmp, x));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
