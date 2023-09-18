using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Store
{
    public string Name { get; set; }
    public string Address { get; set; }
    public List<string> Phones { get; set; } = new List<string>();
    public string Specialization { get; set; }
    public string WorkingHours { get; set; }

    public void AddPhone(string phone)
    {
        Phones.Add(phone);
    }

    public override string ToString()
    {
        return $"Store(Name='{Name}', Address='{Address}', Phones={string.Join(", ", Phones)}, Specialization='{Specialization}', WorkingHours='{WorkingHours}')";
    }
}

class Program
{
    private static List<Store> stores = new List<Store>();
    private const string FileName = "stores.json";

    static void Main(string[] args)
    {
        LoadStores();

        while (true)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Add a new store");
            Console.WriteLine("2. View list of stores");
            Console.WriteLine("3. Delete store by name");
            Console.WriteLine("4. Exit program");
            Console.Write("Select an option: ");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddStore();
                    break;

                case 2:
                    DisplayStores();
                    break;

                case 3:
                    Console.Write("Enter the store name to delete: ");
                    var storeNameToDelete = Console.ReadLine();
                    RemoveStoreByName(storeNameToDelete);
                    Console.WriteLine($"Store(s) with name {storeNameToDelete} deleted (if found).");
                    break;

                case 4:
                    SaveStores();
                    Console.WriteLine("Program terminated.");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    private static void LoadStores()
    {
        if (File.Exists(FileName))
        {
            try
            {
                var json = File.ReadAllText(FileName);
                stores = JsonSerializer.Deserialize<List<Store>>(json);
                Console.WriteLine("Data loaded successfully from stores.json");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading data from stores.json: {e.Message}. Starting with an empty list.");
            }
        }
        else
        {
            Console.WriteLine("stores.json not found. Starting with an empty list.");
        }
    }

    private static void AddStore()
    {
        Console.WriteLine("Enter store details:");

        Console.Write("Name: ");
        var name = Console.ReadLine();

        Console.Write("Address: ");
        var address = Console.ReadLine();

        Console.Write("Specialization: ");
        var specialization = Console.ReadLine();

        Console.Write("Working Hours: ");
        var workingHours = Console.ReadLine();

        var store = new Store
        {
            Name = name,
            Address = address,
            Specialization = specialization,
            WorkingHours = workingHours
        };

        while (true)
        {
            Console.Write("Add phone (Y/N): ");
            if (Console.ReadLine().Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write("Phone number: ");
                store.AddPhone(Console.ReadLine());
            }
            else
            {
                break;
            }
        }

        stores.Add(store);
        Console.WriteLine("Store added.");
    }

    private static void DisplayStores()
    {
        foreach (var store in stores)
        {
            Console.WriteLine(store);
        }
    }

    private static void RemoveStoreByName(string name)
    {
        stores.RemoveAll(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    private static void SaveStores()
    {
        var json = JsonSerializer.Serialize(stores, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FileName, json);
    }
}
