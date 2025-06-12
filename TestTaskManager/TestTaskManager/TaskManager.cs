using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using task;
using category;

namespace taskManager
{
    public class TaskManager
    {
        public List<Category> Categories { get; private set; }
        public List<Task> Tasks { get; private set; }
        public TaskManager()
        {
            Tasks = new List<Task>();
            Categories = new List<Category>();

            LoadCategories();
            LoadTasks();
        }

        public void AddCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Название категории должно быть задано.");

            var existingCategory = Categories.FirstOrDefault(c => c.Name.ToLower() == categoryName.ToLower());
            if (existingCategory != null)
                throw new InvalidOperationException("Категория с таким именем уже существует.");

            int nextId = Categories.Any() ? Categories.Max(c => c.Id) + 1 : 1;
            Categories.Add(new Category(nextId, categoryName));
            SaveCategories();
        }

        public void AddTask(string description, int categoryId = -1)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException("Описание задачи не может быть пустым.");

            Tasks.Add(new Task(description) { CategoryId = categoryId });
            SaveTasks();
        }

        private void SaveCategories()
        {
            File.WriteAllLines("categories.txt", Categories.Select(c => $"{c.Id}|{c.Name}"));
        }

        private void LoadCategories()
        {
            if (!File.Exists("categories.txt")) return;

            var lines = File.ReadAllLines("categories.txt");
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 2 && int.TryParse(parts[0], out int id))
                    Categories.Add(new Category { Id = id, Name = parts[1] });
            }
        }

        public void RemoveTask(int index)
        {
            if (index < 0 || index >= Tasks.Count)
            {
                throw new IndexOutOfRangeException("Некорректный индекс задачи.");
            }
            Tasks.RemoveAt(index);
            SaveTasks();
        }
        public void ToggleTaskCompletion(int index)
        {
            if (index < 0 || index >= Tasks.Count)
            {
                throw new IndexOutOfRangeException("Некорректный индекс задачи.");
            }
            Tasks[index].IsCompleted = !Tasks[index].IsCompleted;
            SaveTasks();
        }
        public void SaveTasks()
        {
            File.WriteAllLines("tasks.txt", Tasks.Select(t =>$"{t.IsCompleted}|{t.Description}|{t.CategoryId}"));
        }
        private void LoadTasks()
        {
            if (!File.Exists("tasks.txt")) return;

            var lines = File.ReadAllLines("tasks.txt");
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 3 &&
                    bool.TryParse(parts[0], out bool isCompleted) &&
                    int.TryParse(parts[2], out int categoryId))
                {
                    Tasks.Add(new Task(parts[1]) { IsCompleted = isCompleted, CategoryId = categoryId });
                }
            }
        }

        public IEnumerable<Task> GetFilteredTasksByCategory(int categoryId)
        {
            return Tasks.Where(task => task.CategoryId == categoryId).ToList();
        }
    }
}
