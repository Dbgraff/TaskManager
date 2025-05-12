using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using taskManager;

public class TaskManagerForm : Form
{
    private TaskManager taskManager;
    private ListBox tasksListBox;
    private TextBox descriptionTextBox;
    private Button addTaskButton;
    private Button removeTaskButton;
    private Button toggleCompletionButton;
    public TaskManagerForm()
    {
        this.Text = "Управление задачами";
        this.Width = 400;
        this.Height = 400;
        tasksListBox = new ListBox
        {
            Location = new System.Drawing.Point(10, 10),
            Width = 200,
            Height = 200
        };
        descriptionTextBox = new TextBox
        {
            Location = new System.Drawing.Point(220, 10),
            Width = 150
        };
        addTaskButton = new Button
        {
            Location = new System.Drawing.Point(220, 40),
            Text = "Добавить",
            Width = 70
        };
        addTaskButton.Click += AddTaskButton_Click;
        removeTaskButton = new Button
        {
            Location = new System.Drawing.Point(300, 40),
            Text = "Удалить",
            Width = 70
        };
        removeTaskButton.Click += RemoveTaskButton_Click;
        toggleCompletionButton = new Button
        {
            Location = new System.Drawing.Point(220, 70),
            Text = "Отметить",
            Width = 150
        };
        toggleCompletionButton.Click += ToggleCompletionButton_Click;
        this.Controls.Add(tasksListBox);
        this.Controls.Add(descriptionTextBox);
        this.Controls.Add(addTaskButton);
        this.Controls.Add(removeTaskButton);
        this.Controls.Add(toggleCompletionButton);
        taskManager = new TaskManager();
        UpdateTasksList();
    }
    private void UpdateTasksList()
    {
        tasksListBox.Items.Clear();
        foreach (var task in taskManager.Tasks)
        {
            tasksListBox.Items.Add($"{(task.IsCompleted ? "[X]" : "[ )")} {task.Description}");
        }
    }
    private void AddTaskButton_Click(object sender, EventArgs e)
    {
        try
        {
            taskManager.AddTask(descriptionTextBox.Text);
            descriptionTextBox.Clear();
            UpdateTasksList();
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
            UpdateTasksList();
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
            MessageBox.Show("Выберите задачу для изменения статуса!");
            return;
        }
        try
        {
            taskManager.ToggleTaskCompletion(tasksListBox.SelectedIndex);
            UpdateTasksList();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new TaskManagerForm());
    }
}