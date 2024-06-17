﻿using Ex03.GarageLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ex03.GarageLogic.VehicleBuilder;

namespace Ex03.ConsoleUI
{
    public class GarageManager
    {
        private readonly Garage r_Garage = new Garage();
        private readonly VehicleBuilder r_VehicleBuilder = new VehicleBuilder();

        public void StartGarageWork()
        {
            bool stayInTheGarage = true; 
            Console.WriteLine("Welcome to our garage!");
            while(stayInTheGarage)
            {
                PresentGarageMenu();
                eMenuOptions userInput = (eMenuOptions)int.Parse(Console.ReadLine());
                MenuChoice(userInput,stayInTheGarage);
            }

        }
        public void PresentGarageMenu()
        {
            Console.WriteLine("Garage Menu:");
            Console.WriteLine("==============================================");
            Console.WriteLine("Choose your option:");

            Console.WriteLine(@"(1) Add new vehicle to the garage
(2) Present all the license numbers in the garage
(3) Change vehicle status
(4) Infalte your vehicle's tires
(5) Add gas to your vehicle - if you have a vehicle on fuel
(6) Charge your vehicle - if you have an electric vehicle
(7) Present full details about a vehicle
(8) Exit");
        }

        public void MenuChoice(eMenuOptions i_UserInput, bool io_Exit)
        {
            try
            {
                Console.WriteLine();
                switch (i_UserInput)
                {
                    case eMenuOptions.AddNewVehicle:
                        GetVehicleAndAddToTheGarage();
                        break;
                    case eMenuOptions.PresentAllLicense:
                        PresentLicenseNumbersInTheGarage();
                        break;
                    case eMenuOptions.ChangeVehicleStatus:
                        ChangeVehicleStatus();
                        break;
                    case eMenuOptions.InfalteTires:
                        InfalteTiresToMax();
                        break;
                    case eMenuOptions.AddGas:
                        AddFuelToVehicle();
                        break;
                    case eMenuOptions.ChargeBattery:
                        ChargeYourVehicle();
                        break;
                    case eMenuOptions.PresentFullDetails:
                        PresentAllVehicleDetails();
                        break;
                    case eMenuOptions.Exit:
                        io_Exit = false;
                        break;
                    default:
                        throw new ArgumentException("Your choice is not from the menu");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}. Please try again.", ex.Message);
            }

        }
        private void printEnumOptions<T>()
        {
            for (int i = 0; i < Enum.GetValues(typeof(T)).Length; i++)
            {
                Console.WriteLine("({0}) {1}", i + 1, Enum.GetName(typeof(T), i + 1));
            }
        }

        private void checkIfNumber(string i_UserInput, out float o_UserInput)
        {
            if (!float.TryParse(i_UserInput, out o_UserInput))
            {
                throw new FormatException("should be a number");
            }
        }
        private void isValidChoice(string i_UserInput, int i_MinOption, int i_MaxOption)
        {
            int userInput;
            if (int.TryParse(i_UserInput, out userInput))
            {
                if (userInput < i_MinOption || userInput > i_MaxOption)
                {
                    throw new ValueOutOfRangeException(i_MaxOption, i_MinOption);
                }
            }
            else
            {
                throw new FormatException("Value should be a number.");
            }
        }

        ///1
        public void GetVehicleAndAddToTheGarage()
        {
            string carLicense = GetLicenseNumberFromUser();

            if (r_Garage.CheckIfTheVehicleIsInGarage(carLicense))
            {
                Console.Write("Vehicle license {0} is already exist in the garage!", carLicense);
                r_Garage.ChangeVehicleStatusToInRepair(carLicense);
            }
            else
            {
                getNewVehicle(carLicense);
                Console.WriteLine("Vehicle {0} added to the grage! ", carLicense);
                Console.WriteLine();
            }
        }

        private void printOptionalVehicleTypes()
        {
            Console.WriteLine("Choose a vehicle type option:");

            for (int i = 0; i < r_VehicleBuilder.VehicleOptionalTypes.Length; i++)
            {
                Console.WriteLine("({0}) {1}", i + 1, r_VehicleBuilder.VehicleOptionalTypes[i]);
            }
            Console.Write("Your choice is: ");

        }

        private void getNewVehicle(string i_CarLicense)
        {
            bool notValid = true;
            printOptionalVehicleTypes();

            while (notValid)
            {
                try
                {
                    string ownerVehicleType = Console.ReadLine();
                    isValidChoice(ownerVehicleType, 1, r_VehicleBuilder.VehicleOptionalTypes.Length);
                    VehicleBuilder.eVehicleType vehicleType = (VehicleBuilder.eVehicleType)int.Parse(ownerVehicleType);
                    Vehicle newVehicle = r_VehicleBuilder.CreateVehicleObject(vehicleType, i_CarLicense);

                    setPersonalInformation(newVehicle);
                    setModel(newVehicle);
                    setSpesicifVehicleData(newVehicle);
                    setReaminEnergy(newVehicle);
                    setTires(newVehicle);
                    notValid = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}. Please try again.", ex.Message);
                }
            }
        }
        private void setTires(Vehicle i_NewVehicle)
        {
            bool notValid = true;
            string manufacturerName = getManufacturerNameTires();

            while (notValid)
            {
                try
                {

                    Console.Write(@"Do you want to add tire air pressure of all the tires together?
(1) yes
(2) no
your choice: ");
                    string userTirePressureChoice = Console.ReadLine();

                    isValidChoice(userTirePressureChoice, 1, 2);
                    setTiresPressure(i_NewVehicle, isGetAllTirePressureTogether(userTirePressureChoice), manufacturerName);
                    notValid = false;
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}. Please try again.", ex.Message);
                    Console.WriteLine();
                }
            }
        }
        private string getManufacturerNameTires()
        {
            Console.Write("Please enter tire manufactor name: ");
            string tireManufacturerName = Console.ReadLine();
            return tireManufacturerName;
        }
        private bool isGetAllTirePressureTogether(string i_UserTirePressureChoice)
        {
            return (i_UserTirePressureChoice == "1");
        }
        private void setReaminEnergy(Vehicle i_NewVehicle)
        {
            bool notValid = true;

            while (notValid)
            {
                try
                {
                    Console.Write("Please enter the current energy situation in your car: ");
                    string remainingEnergy = Console.ReadLine();
                    float energyRemaining;

                    checkIfNumber(remainingEnergy, out energyRemaining);
                    i_NewVehicle.Engine.EnergyRemaining = energyRemaining;
                    i_NewVehicle.calculatePrecentRemainingEnergy();
                    notValid = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}. Please try again.", ex.Message);
                }
            }
        }
        private void setSpesicifVehicleData(Vehicle i_NewVehicle)
        {
            bool notValid = true;

            while (notValid)
            {
                try
                {
                    List<string> specificData = new List<string>();
                    for (int i = 0; i < i_NewVehicle.SpecificData().Length; i++)
                    {
                        Console.Write("please enter {0}: ", i_NewVehicle.SpecificData()[i]);
                        specificData.Add(Console.ReadLine());
                    }
                    i_NewVehicle.SpecieficDetailsForEachKind = specificData;
                    notValid = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}. Please try again.", ex.Message);
                }
            }
        }
        private void setPersonalInformation(Vehicle i_NewVehicle)
        {
            Console.Write("Please enter the owner name: ");
            string vehicleOwnerName = Console.ReadLine();

            Console.Write("Please enter the owner phone: ");
            string vehicleOwnerPhone = Console.ReadLine();
            r_Garage.AddVehicle(i_NewVehicle, vehicleOwnerName, vehicleOwnerPhone);
        }
        private void setModel(Vehicle i_NewVehicle)
        {
            Console.Write("Please enter the vehicle model: ");
            string vehicleModel = Console.ReadLine();
            r_Garage.Vehicles[i_NewVehicle.LicenseNumber].Vehicle.ModelName = vehicleModel;
        }
        private void setTiresPressure(Vehicle i_Vehicle,
                                  bool i_IsGetAllTirePressureTogether, string i_ManufacturerName)
        {
            bool notValid = true;

            while (notValid)
            {
                try
                {
                    float tireAirPressuerNumber;

                    Console.Write("Please enter tires air pressure: ");
                    if (i_IsGetAllTirePressureTogether)
                    {
                        checkIfNumber(Console.ReadLine(), out tireAirPressuerNumber);

                        for (int i = 0; i < i_Vehicle.NumberOfTires; i++)
                        {
                            i_Vehicle.AddNewTireToTireList(i_ManufacturerName, tireAirPressuerNumber);
                        }
                    }
                    else
                    {
                        for (int i = 1; i < i_Vehicle.NumberOfTires + 1; i++)
                        {
                            Console.Write("Please enter tire number {0} pressure: ", i);
                            checkIfNumber(Console.ReadLine(), out tireAirPressuerNumber);
                            i_Vehicle.AddNewTireToTireList(i_ManufacturerName, tireAirPressuerNumber);
                        }
                    }
                    i_Vehicle.checkIfTireNumberMatchVehicle();
                    notValid = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}. Please try again.", ex.Message);
                }
            }
        }

        ///2
        public void PresentLicenseNumbersInTheGarage()
        {
            bool notValid = true;

            while (notValid)
            {
                try
                {
                    if (r_Garage.Vehicles.Count == 0)
                    {
                        notValid = false;
                        throw new ArgumentException("There are no vehicles in the garage");
                    }
                    Console.WriteLine("Here are all the cars we have in the garage right now:");
                    foreach (var vehicle in r_Garage.Vehicles)
                    {
                        Console.WriteLine(vehicle.Key);
                    }
                    Console.Write(@"Do you want to filter them by status?
(1) yes
(2) no");
                    Console.Write("Your choice: ");
                    string filterBYChoice = Console.ReadLine();

                    isValidChoice(filterBYChoice, 1, 2);
                    int toFilterBy = int.Parse(filterBYChoice);

                    if (toFilterBy == 1)
                    {
                        eVehicleStatus chosenStatus;
                        chosenStatus = ChooseFilter();
                        PresentLicenseNumbersInTheGarageFiltered(chosenStatus);
                    }
                    notValid = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}. Please try again.", ex.Message);
                    Console.WriteLine();
                }

            }
        }
        public eVehicleStatus ChooseFilter()
        {
            Console.Write("By which status you would like to filter by? ");
            return getVehicleStatus();
        }
        private void PresentLicenseNumbersInTheGarageFiltered(eVehicleStatus i_CarStatus)
        {
            Console.WriteLine("Here are all the cars we have in the garage right now in the status you chose:");
            foreach (var vehicle in r_Garage.Vehicles)
            {
                if (vehicle.Value.VehicleStatus == i_CarStatus)
                {
                    Console.WriteLine(vehicle.Key);
                }
            }
        }

        private eVehicleStatus getVehicleStatus()
        {
            printEnumOptions<eVehicleStatus>();
            string whichStatus = Console.ReadLine();

            isValidChoice(whichStatus, 1, Enum.GetValues(typeof(eVehicleStatus)).Length);
            return (eVehicleStatus)int.Parse(whichStatus);
        }

        /// 3
        public void ChangeVehicleStatus()
        {
            bool notValid = true;

            bool emptyGrage = IsGrageEmpty();

            while (notValid && !emptyGrage)
            {
                try
                {
                    string vehicleLicense = GetLicenseNumberFromUser();

                    r_Garage.CheckIfTheVehicleIsInGarage(vehicleLicense);
                    eVehicleStatus vehicleStatus = getVehicleStatus();

                    r_Garage.ChangeVehicleStatus(vehicleLicense, vehicleStatus);
                    Console.WriteLine("This vehicle status as cahnged to {0}", vehicleStatus);
                    Console.WriteLine();
                    notValid = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}. Please try again.", ex.Message);
                    Console.WriteLine();
                }

            }
        }
        public bool IsGrageEmpty()
        {
            bool isEmpty = false;

            if (r_Garage.Vehicles.Count == 0)
            {
                Console.WriteLine("The garage have no cars inside yet");
                Console.WriteLine();
                isEmpty = true;
            }

            return isEmpty;
        }
        public string GetLicenseNumberFromUser()
        {
            Console.Write("Please Enter your vehicle license number: ");
            string vehicleLicense = Console.ReadLine();

            return vehicleLicense;
        }

        public eVehicleStatus GetVehicleStatusFromTheUser()
        {

            Console.Write("Please enter the new status for the car: ");
            printEnumOptions<eVehicleStatus>();
            string whichStatus = Console.ReadLine();

            isValidChoice(whichStatus, 1, Enum.GetValues(typeof(eVehicleStatus)).Length);
            return (eVehicleStatus)int.Parse(whichStatus);
        }

        /// 4
        public void InfalteTiresToMax()
        {
            bool notValid = true;

            while (notValid)
            {
                try
                {
                    string vehicleLicense = GetLicenseNumberFromUser();

                    r_Garage.InfaltionToMax(vehicleLicense);
                    notValid = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}. Please try again.", ex.Message);
                    Console.WriteLine();
                }
            }
        }

        /// 5
        public void AddFuelToVehicle()
        {
            bool notValid = true;

            while (notValid)
            {
                try
                {
                    string vehicleLicense = GetLicenseNumberFromUser();

                    Console.Write("Please enter your fuel type: ");
                    printEnumOptions<eFuelType>();
                    string fuelType = Console.ReadLine();
                    isValidChoice(fuelType, 1, Enum.GetValues(typeof(eFuelType)).Length - 1);

                    Console.Write("Please enter how much fuel you want to add: ");
                    string fuelAmount = Console.ReadLine();

                    float vehicleFuelAmount;
                    checkIfNumber(fuelAmount, out vehicleFuelAmount);
                    r_Garage.FuelingVehicle(vehicleLicense, (eFuelType)int.Parse(fuelType), vehicleFuelAmount);
                    notValid = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}. Please try again.", ex.Message);
                    Console.WriteLine();
                }
            }
        }


        /// 6

        public void ChargeYourVehicle()
        {
            bool notValid = true;

            while (notValid)
            {
                try
                {
                    string vehicleLicense = GetLicenseNumberFromUser();

                    Console.Write("Please enter the time you want to charge in minutes:");
                    string minuesAmount = Console.ReadLine();

                    float minutesToCharge;
                    checkIfNumber(minuesAmount, out minutesToCharge);
                    float hoursToCharge = ConvertMinutesToHours(minutesToCharge);

                    r_Garage.ChargeVehicle(vehicleLicense, hoursToCharge);
                    notValid = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}. Please try again.", ex.Message);
                    Console.WriteLine();
                }
            }
        }
        public float ConvertMinutesToHours(float i_Minutes)
        {
            return i_Minutes / 60;
        }

        /// 7


        public void PresentAllVehicleDetails()
        {
            bool notValid = true;

            while (notValid)
            {
                try
                {
                    string vehicleLicense = GetLicenseNumberFromUser();

                    if (r_Garage.CheckIfTheVehicleIsInGarage(vehicleLicense))
                    {
                        Vehicle vehicle = r_Garage.Vehicles[vehicleLicense].Vehicle;

                        Console.WriteLine("The vehicle you chose: {0}", vehicle.LicenseNumber);
                        Console.WriteLine("Here are all the details about this vehicle:");
                        Console.WriteLine("Owner name: {0}", r_Garage.Vehicles[vehicleLicense].OwnerName);
                        Console.WriteLine("Owner phone number: {0}", r_Garage.Vehicles[vehicleLicense].OwnerPhoneNumber);
                        Console.WriteLine("Vehicle status: {0}", r_Garage.Vehicles[vehicleLicense].VehicleStatus);
                        Console.WriteLine("Model name: {0}", vehicle.ModelName);
                        Console.WriteLine("Tires manufacture name: {0}", vehicle.Tires[0].ManufacturerName);
                        Console.WriteLine("Tire pressure for each tire");
                        foreach (var detail in vehicle.Tires)
                        {
                            Console.WriteLine(detail.TirePressure);
                        }

                        Console.WriteLine("Fuel type: {0}", vehicle.FuelType);
                        Console.WriteLine("Precent OfRemaining Energy: {0}", vehicle.PrecentOfRemainingEnergy);
                        Console.WriteLine("Speciefic details for the vehicle");
                        foreach (var detail in vehicle.SpecieficDetailsForEachKind)
                        {
                            Console.WriteLine(detail);
                        }
                        Console.WriteLine();
                        notValid = false;
                    }
                    else
                    {
                        throw new ArgumentException("There is no such vehicle in the garage");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}. Please try again.", ex.Message);
                    Console.WriteLine();
                }

            }
        }
    }
}
