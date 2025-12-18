// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using Dapper;
using Npgsql;

namespace StudentAppPostgres
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Major { get; set; }
    }

    class Program
    {
       
        private static string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=;Database=StudentDB";

        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Показать всех студентов");
                Console.WriteLine("3. Обновить студента");
                Console.WriteLine("4. Удалить студента");
                Console.WriteLine("5. Выход");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddStudent(); break;
                    case "2": GetAllStudents(); break;
                    case "3": UpdateStudent(); break;
                    case "4": DeleteStudent(); break;
                    case "5": exit = true; break;
                    default: Console.WriteLine("Неверный выбор."); break;
                }
            }
        }

        private static void AddStudent()
        {
            try
            {
                Console.Write("Введите имя студента: ");
                string name = Console.ReadLine();
                Console.Write("Введите возраст: ");
                int age = int.Parse(Console.ReadLine());
                Console.Write("Введите специальность: ");
                string major = Console.ReadLine();

                using var connection = new NpgsqlConnection(connectionString);
                string sql = "INSERT INTO Students (Name, Age, Major) VALUES (@Name, @Age, @Major)";
                int rows = connection.Execute(sql, new { Name = name, Age = age, Major = major });
                Console.WriteLine($"{rows} запись(ей) добавлено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        private static void GetAllStudents()
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                string sql = "SELECT * FROM Students";
                IEnumerable<Student> students = connection.Query<Student>(sql);

                Console.WriteLine("\nСписок студентов:");
                foreach (var s in students)
                    Console.WriteLine($"{s.Id} | {s.Name} | {s.Age} | {s.Major}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        private static void UpdateStudent()
        {
            try
            {
                Console.Write("Введите Id студента для обновления: ");
                int id = int.Parse(Console.ReadLine());
                Console.Write("Введите новое имя: ");
                string name = Console.ReadLine();
                Console.Write("Введите новый возраст: ");
                int age = int.Parse(Console.ReadLine());
                Console.Write("Введите новую специальность: ");
                string major = Console.ReadLine();

                using var connection = new NpgsqlConnection(connectionString);
                string sql = "UPDATE Students SET Name=@Name, Age=@Age, Major=@Major WHERE Id=@Id";
                int rows = connection.Execute(sql, new { Id = id, Name = name, Age = age, Major = major });
                Console.WriteLine($"{rows} запись(ей) обновлено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        private static void DeleteStudent()
        {
            try
            {
                Console.Write("Введите Id студента для удаления: ");
                int id = int.Parse(Console.ReadLine());

                using var connection = new NpgsqlConnection(connectionString);
                string sql = "DELETE FROM Students WHERE Id=@Id";
                int rows = connection.Execute(sql, new { Id = id });
                Console.WriteLine($"{rows} запись(ей) удалено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }
    }
}

