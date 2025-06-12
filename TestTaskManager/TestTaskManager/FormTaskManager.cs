using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using taskManager;
using System.Drawing;
using task;

public class TaskManagerForm : Form
{
    private TaskManager taskManager;
    private Label nameTaskLabel;
    private ListBox tasksListBox;
    private TextBox descriptionTextBox;
    private Button addTaskButton;
    private Button removeTaskButton;
    private Button toggleCompletionButton;
    private ComboBox categoriesComboBox;
    private CheckBox filterCheckBox;
    private TextBox createCategoryTextBox;
    private Button createCategoryButton;
    public TaskManagerForm()
    {
        this.Text = "Управление задачами";
        this.Width = 400;
        this.Height = 400;

        nameTaskLabel = new Label
        {
            Location = new System.Drawing.Point(220, 10),
            Text = "Название задачи: "
        };

        createCategoryTextBox = new TextBox
        {
            Location = new Point(10, 280),
            Width = 200
        };

        createCategoryButton = new Button
        {
            Location = new Point(220, 280),
            Text = "Создать категорию",
            Width = 150
        };
        createCategoryButton.Click += CreateCategoryButton_Click;

        tasksListBox = new ListBox
        {
            Location = new Point(10, 10),
            Width = 200,
            Height = 200
        };

        descriptionTextBox = new TextBox
        {
            Location = new Point(220, 35),
            Width = 150
        };

        addTaskButton = new Button
        {
            Location = new Point(220, 65),
            Text = "Добавить",
            Width = 70
        };
        addTaskButton.Click += AddTaskButton_Click;

        removeTaskButton = new Button
        {
            Location = new Point(300, 65),
            Text = "Удалить",
            Width = 70
        };
        removeTaskButton.Click += RemoveTaskButton_Click;

        toggleCompletionButton = new Button
        {
            Location = new Point(220, 95),
            Text = "Отметить",
            Width = 150
        };
        toggleCompletionButton.Click += ToggleCompletionButton_Click;

        categoriesComboBox = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Location = new Point(10, 220),
            Width = 200
        };
        categoriesComboBox.SelectedIndexChanged += CategoriesComboBox_SelectedIndexChanged;

        filterCheckBox = new CheckBox
        {
            Text = "Фильтр по выбранной категории",
            Location = new Point(220, 220)
        };
        filterCheckBox.CheckedChanged += FilterCheckBox_CheckedChanged;

        Controls.Add(tasksListBox);
        Controls.Add(nameTaskLabel);
        Controls.Add(descriptionTextBox);
        Controls.Add(addTaskButton);
        Controls.Add(removeTaskButton);
        Controls.Add(toggleCompletionButton);
        Controls.Add(categoriesComboBox);
        Controls.Add(filterCheckBox);
        Controls.Add(createCategoryTextBox);
        Controls.Add(createCategoryButton);

        taskManager = new TaskManager();
        UpdateUIElements();
    }

    private void UpdateUIElements()
    {
        UpdateCategoriesList();
        UpdateTasksList();
    }

    private void UpdateCategoriesList()
    {
        categoriesComboBox.Items.Clear();
        categoriesComboBox.Items.Add("Все"); 
        foreach (var cat in taskManager.Categories)
        {
            categoriesComboBox.Items.Add(cat.Name); 
        }
        categoriesComboBox.SelectedIndex = 0; 
    }

    private void UpdateTasksList()
    {
        tasksListBox.Items.Clear();
        if (filterCheckBox.Checked && categoriesComboBox.SelectedIndex > 0)
        {
            var selectedCategory = taskManager.Categories[categoriesComboBox.SelectedIndex - 1];
            var filteredTasks = taskManager.GetFilteredTasksByCategory(selectedCategory.Id);
            foreach (var task in filteredTasks)
            {
                tasksListBox.Items.Add($"{(task.IsCompleted ? "[X]" : "[ ]")} {task.Description}");
            }
        }
        else
        {
            foreach (var task in taskManager.Tasks)
            {
                tasksListBox.Items.Add($"{(task.IsCompleted ? "[X]" : "[ ]")} {task.Description}");
            }
        }
    }

    private void AddTaskButton_Click(object sender, EventArgs e)
    {
        try
        {
            int categoryId = -1; 
            if (categoriesComboBox.SelectedIndex > 0)
            {
                categoryId = taskManager.Categories[categoriesComboBox.SelectedIndex - 1].Id;
            }
            taskManager.AddTask(descriptionTextBox.Text, categoryId); //
            descriptionTextBox.Clear();
            UpdateUIElements();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void RemoveTaskButton_Click(object sender, EventArgs e)
    {
        if (tasksListBox.SelectedIndex == -1)
        {
            MessageBox.Show("Выберите задачу для удаления!");
            return;
        }
        try
        {
            taskManager.RemoveTask(tasksListBox.SelectedIndex);
            UpdateUIElements();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ToggleCompletionButton_Click(object sender, EventArgs e)
    {
        if (tasksListBox.SelectedIndex == -1)
        {
            MessageBox.Show("Выберите задачу для изменения статуса!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            int visibleIndex = tasksListBox.SelectedIndex;

            if (filterCheckBox.Checked && categoriesComboBox.SelectedIndex > 0)
            {
                var selectedCategory = taskManager.Categories[categoriesComboBox.SelectedIndex - 1];
                var filteredTasks = taskManager.GetFilteredTasksByCategory(selectedCategory.Id);
                Task targetTask = filteredTasks.ElementAt(visibleIndex);
                int globalIndex = taskManager.Tasks.FindIndex(t => t.Equals(targetTask));

                taskManager.ToggleTaskCompletion(globalIndex);
            }
            else
            {
                int globalIndex = tasksListBox.SelectedIndex;
                taskManager.ToggleTaskCompletion(globalIndex);
            }

            UpdateTasksList();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CreateCategoryButton_Click(object sender, EventArgs e)
    {
        try
        {
            string categoryName = createCategoryTextBox.Text.Trim();
            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Укажите название категории.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            taskManager.AddCategory(categoryName);
            createCategoryTextBox.Clear();      
            UpdateUIElements();    
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }


    private void CategoriesComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateTasksList();
    }

    private void FilterCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        UpdateTasksList();
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new TaskManagerForm());
    }
}