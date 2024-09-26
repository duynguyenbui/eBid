// Console.WriteLine("Hello, world!");
//
using OpenAI;

using var api = new OpenAIClient(new OpenAIAuthentication("lm-studio"),
    new OpenAIClientSettings(domain: "http://localhost:1234"));

var response =
    await api.EmbeddingsEndpoint.CreateEmbeddingAsync("The food was delicious and the waiter...",
        "gaianet/Nomic-embed-text-v1.5-Embedding-GGUF", dimensions: 768);

Console.WriteLine(response.Data[0].Embedding);

// using OpenAI;
// using OpenAI.Embeddings;
//
// try
// {
//     EmbeddingClient client = new EmbeddingClient(model: "second-state/Nomic-embed-text-v1.5-Embedding-GGUF", "lms-studio",
//         new OpenAIClientOptions() { Endpoint = new Uri("http://localhost:1234/v1") });
//
//     string description = "Best hotel in town if you like luxury hotels. They have an amazing infinity pool, a spa,"
//                          + " and a really helpful concierge. The location is perfect -- right downtown, close to all the tourist"
//                          + " attractions. We highly recommend this hotel.";
//
//     // Convert the description to a byte array
//     byte[] descriptionBytes = System.Text.Encoding.UTF8.GetBytes(description);
//
//     // Base64 encode the byte array
//     string base64Description = Convert.ToBase64String(descriptionBytes);
//
//     // Generate the embedding
//     Embedding embedding = client.GenerateEmbedding(base64Description, new EmbeddingGenerationOptions() { Dimensions = 768 });
//
//     ReadOnlyMemory<float> vector = embedding.Vector;
//
//     // Output the embedding vector
//     Console.WriteLine("Embedding Vector:");
//     foreach (float value in vector.Span)
//     {
//         Console.Write($"{value:F2} "); // Print each float with 2 decimal places
//     }
//     Console.WriteLine();
// }
// catch (Exception ex)
// {
//     Console.WriteLine($"An error occurred: {ex.Message}");
// }
// See https://aka.ms/new-console-template for more information
//
// using System.Collections;
//
// Zoo theZoo = new Zoo();
//
// theZoo.AddMammal("Whale");
// theZoo.AddMammal("Rhinoceros");
// theZoo.AddBird("Penguin");
// theZoo.AddBird("Warbler");
//
// foreach (string name in theZoo)
// {
//     Console.Write(name + " ");
// }
//
// Console.WriteLine();
// // Output: Whale Rhinoceros Penguin Warbler
//
// foreach (string name in theZoo.Birds)
// {
//     Console.Write(name + " ");
// }
//
// Console.WriteLine();
// // Output: Penguin Warbler
//
// foreach (string name in theZoo.Mammals)
// {
//     Console.Write(name + " ");
// }
//
// Console.WriteLine();
// // Output: Whale Rhinoceros
//
// Console.ReadKey();
//
//
// public class Zoo : IEnumerable
// {
//     // Private members.
//     private List<Animal> animals = new List<Animal>();
//
//     // Public methods.
//     public void AddMammal(string name)
//     {
//         animals.Add(new Animal { Name = name, Type = Animal.TypeEnum.Mammal });
//     }
//
//     public void AddBird(string name)
//     {
//         animals.Add(new Animal { Name = name, Type = Animal.TypeEnum.Bird });
//     }
//
//     public IEnumerator GetEnumerator()
//     {
//         foreach (Animal theAnimal in animals)
//         {
//             yield return theAnimal.Name;
//         }
//     }
//
//     // Public members.
//     public IEnumerable Mammals
//     {
//         get { return AnimalsForType(Animal.TypeEnum.Mammal); }
//     }
//
//     public IEnumerable Birds
//     {
//         get { return AnimalsForType(Animal.TypeEnum.Bird); }
//     }
//
//     // Private methods.
//     private IEnumerable AnimalsForType(Animal.TypeEnum type)
//     {
//         foreach (Animal theAnimal in animals)
//         {
//             if (theAnimal.Type == type)
//             {
//                 yield return theAnimal.Name;
//             }
//         }
//     }
//
//     // Private class.
//     private class Animal
//     {
//         public enum TypeEnum { Bird, Mammal }
//
//         public string Name { get; set; }
//         public TypeEnum Type { get; set; }
//     }
// }

// using System.Collections;
//
// DaysOfTheWeek days = new DaysOfTheWeek();
//
// foreach (string day in days)
// {
//     Console.Write(day + " ");
// }
//
// // Output: Sun Mon Tue Wed Thu Fri Sat
// Console.ReadKey();
//
// class DaysOfTheWeek : IEnumerable
// {
//     private string[] days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
//
//     public IEnumerator GetEnumerator()
//     {
//         for (int index = 0; index < days.Length; index++)
//         {
//             // Yield each day of the week.
//             yield return days[index];
//         }
//     }
// }

// using eBid.EventBus.Extensions;

// // The interface is covariant.
//
// ICovariant<Button> ibutton = new SampleImplementation<Button>();
// ICovariant<object> iobject = ibutton;
//
// // The class is invariant.
// SampleImplementation<Button> button = new();
//
// // Print Information about the interface.
// Console.WriteLine(ibutton.GetSomething());
// Console.WriteLine(iobject.GetSomething());
//
// class SampleImplementation<R> : ICovariant<R>
// {
//     public R GetSomething()
//     {
//         // Some code.
//         return default(R);
//     }
// }
//
// interface IContravariant<in A>
// {
//     void DoSomething(A sampleArgs);
//
//     void DoSomething<T>() where T : A;
//     // The following statement generates a compiler error.
//     // A GetSomething();
// }
//
// interface IVariant<out R, in A>
// {
//     R GetSomething();
//     void SetSomething(A sampleArg);
//     R GetSetSomethings(A sampleArg);
// }
//
// interface ICovariant<out R>
// {
//     R GetSomething();
// }
//
// class Button
// {
//     public int Id { get; set; }
//     public string Name { get; set; }
// }

// var person = new Person();
// Console.WriteLine(person.GetType().GetGenericTypeName());
//
// class Person
// {
//     public string Name { get; set; }
//     public int Age { get; set; }
// }