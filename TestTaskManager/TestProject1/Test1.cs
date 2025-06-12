using System.IO;
using taskManager;
using task;

namespace TestProject1
{
    [TestClass]
    public class Test_TaskManager
    {

        [TestMethod]
        public void Test_Constructor_LoadsExistingTaskFromFile()
        {
            File.Delete("tasks.txt");
            File.WriteAllText("tasks.txt", "true|Task_1\nfalse|Task_2");
            var manager = new TaskManager();

            Assert.AreEqual(2, manager.Tasks.Count);
            Assert.AreEqual(true, manager.Tasks[0].IsCompleted);
            Assert.AreEqual("Task_1", manager.Tasks[0].Description);
            Assert.AreEqual(false, manager.Tasks[1].IsCompleted);
            Assert.AreEqual("Task_2", manager.Tasks[1].Description);

        }

        [TestMethod]
        public void Test_Constructor_CreateEmptyListIfNoFileExists()
        {
            File.Delete("tasks.txt");
            var manager = new TaskManager();

            Assert.AreEqual (0, manager.Tasks.Count);
        }



        [TestMethod]
        public void Test_AddTask_AddNewTaskToCollection()
        {
            File.Delete("tasks.txt");
            var manager = new TaskManager();
            manager.AddTask("Buy milk");

            Assert.AreEqual(1, manager.Tasks.Count);
            Assert.AreEqual("Buy milk", manager.Tasks[0].Description);
            Assert.AreEqual(false, manager.Tasks[0].IsCompleted);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_AddTask_ThrowsArgumentExceptionForEmptyDescription()
        {
            var manager = new TaskManager();
            manager.AddTask("");
        }

        [TestMethod]
        public void Test_RemoveTask_RemoveTaskByIndex()
        {
            File.Delete("tasks.txt");
            var manager = new TaskManager();
            manager.AddTask("Task_1");
            manager.AddTask("Task_2");

            manager.RemoveTask(0);

            Assert.AreEqual(1, manager.Tasks.Count);
            Assert.AreEqual("Task_2", manager.Tasks[0].Description);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_RemoveTask_IndexOutOfRangeExceptionForInvalidIndex()
        {
            File.Delete("tasks.txt");
            var manager = new TaskManager();
            manager.AddTask("Task_1");
            
            manager.RemoveTask(-1);
        }

        [TestMethod]
        public void Test_ToggleTaskCompletion_State()
        {
            File.Delete("tasks.txt");
            var manager = new TaskManager();
            manager.AddTask("Task_1");

            manager.ToggleTaskCompletion(0);

            Assert.AreEqual(true, manager.Tasks[0].IsCompleted);

            manager.ToggleTaskCompletion(0);

            Assert.AreEqual(false, manager.Tasks[0].IsCompleted);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_ToggleTaskCompletion_ThrowsIndexOutOfRangeExceptionForInvalidIndex()
        {
            File.Delete("tasks.txt");
            var manager = new TaskManager();
            manager.AddTask("Task_1");

            manager.ToggleTaskCompletion(-1);
        }

        [TestMethod]
        public void Tast_SaveTask_WritesCorrectDataToFile()
        {
            File.Delete("tasks.txt");
            var manager = new TaskManager();
            manager.AddTask("Task_1");
            manager.AddTask("Task_2");
            manager.Tasks[0].IsCompleted = true;

            manager.SaveTasks();

            var content = File.ReadAllText("tasks.txt");
            Assert.AreEqual("True|Task_1\r\nFalse|Task_2", content.Trim());
        }
    }

    [TestClass]
    public class Test_Task
    {
        [TestMethod]
        public void Test_Constructor_InitializesDescription()
        {
            string description = "test";
            var task = new task.Task(description);

            Assert.AreEqual(description, task.Description);
        }

        [TestMethod]
        public void Test_Constructor_SetsIsCompliteToFalseByDefault()
        {
            var task = new task.Task("random task");

            Assert.IsFalse(task.IsCompleted);
        }

        [TestMethod]
        public void Test_IsComplited_ChangesTaskStatus()
        {
            var task = new task.Task("task");
            task.IsCompleted = true;

            Assert.IsTrue(task.IsCompleted);
        }

        [TestMethod]
        public void Test_EmptyDescriptionShouldBeSettable()
        {
            string emptyDescription = "";
            var task = new task.Task(emptyDescription);

            Assert.AreEqual(emptyDescription, task.Description);
        }

        [TestMethod]
        public void Test_NullDescriptionShouldBeSettable()
        {
            var task = new task.Task(null);

            Assert.IsNull(task.Description);
        }
    }
}
