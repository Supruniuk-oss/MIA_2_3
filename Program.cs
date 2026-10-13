using MIA_2._3;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestApiClient
{
    internal class Program
    {
        // Створюємо екземпляр HttpClient із базовою адресою API
        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com/")
        };

        static async Task Main(string[] args)
        {
            // Налаштування кодування для коректного відображення кирилиці в консолі
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=============================================");
            Console.WriteLine("   Console Client for Public REST API (C#)   ");
            Console.WriteLine("=============================================\n");

            try
            {
                // 1. Отримання та вивід списку користувачів
                await GetAndDisplayUsersAsync();

                Console.WriteLine("\n---------------------------------------------\n");

                // 2. Отримання та вивід списку публікацій
                await GetAndDisplayPostsAsync(limit: 5);
            }
            catch (HttpRequestException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Помилка мережі/HTTP]: {ex.Message}");
                Console.ResetColor();
            }
            catch (JsonException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Помилка десеріалізації JSON]: {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Неочікувана помилка]: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }

        private static async Task GetAndDisplayUsersAsync()
        {
            Console.WriteLine("--> Надсилання асинхронного GET-запиту до /users...");

            // Асинхронний GET-запит
            HttpResponseMessage response = await _httpClient.GetAsync("users");
            response.EnsureSuccessStatusCode();

            // Зчитуємо JSON з тіла відповіді
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Десеріалізація JSON у список об'єктів UserDto
            List<UserDto>? users = JsonSerializer.Deserialize<List<UserDto>>(jsonResponse, options);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Успіх] Отримано користувачів: {users?.Count ?? 0}\n");
            Console.ResetColor();

            if (users != null)
            {
                Console.WriteLine("{0,-5} | {1,-20} | {2,-15} | {3,-25}", "ID", "Name", "Username", "Email");
                Console.WriteLine(new string('-', 70));

                foreach (var user in users)
                {
                    Console.WriteLine("{0,-5} | {1,-20} | {2,-15} | {3,-25}",
                        user.Id, user.Name, user.Username, user.Email);
                }
            }
        }

        private static async Task GetAndDisplayPostsAsync(int limit)
        {
            Console.WriteLine($"--> Надсилання GET-запиту до /posts (ліміт: {limit})...");

            HttpResponseMessage response = await _httpClient.GetAsync($"posts?_limit={limit}");
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // Десеріалізація JSON у список об'єктів PostDto
            List<PostDto>? posts = JsonSerializer.Deserialize<List<PostDto>>(jsonResponse, options);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Успіх] Отримано публікацій: {posts?.Count ?? 0}\n");
            Console.ResetColor();

            if (posts != null)
            {
                foreach (var post in posts)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[Post #{post.Id}] (User ID: {post.UserId})");
                    Console.ResetColor();
                    Console.WriteLine($"Title: {post.Title}");
                    Console.WriteLine($"Body : {post.Body.Replace("\n", " ")}");
                    Console.WriteLine(new string('.', 60));
                }
            }
        }
    }
}