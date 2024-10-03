
enum CompletionStatus
{
    NotDone,
    Done
}
class Program
{
    static void Main()
    {
        ToDoList todoList = new ToDoList();
        todoList.Insert("Get car fixed");
        todoList.Insert("Buy groceries");
        todoList.Insert("Paint house");
        new ToDoListApp(todoList).Run();
    }
}
class Task
{
    public string Title { get; set; }
    public CompletionStatus Status { get; private set; }
    public Task(string title)
    {
        Title = title;
        Status = CompletionStatus.NotDone;
    }
    public void ToggleStatus()
    {
        Status = Status == CompletionStatus.NotDone ? CompletionStatus.Done: CompletionStatus.NotDone;
    }
}
class ToDoList
{
    private List<Task> tasks = new List<Task>();
    public int CurrentIndex { get; private set; } = 0;
    public int Length => tasks.Count;
    public Task CurrentTask => tasks[CurrentIndex];
    public Task GetTask(int index)
    {
        return tasks[index];
    }
    public void Insert(string title)
    {
        if (tasks.Count == 0 || CurrentIndex == tasks.Count - 1)
        {
            tasks.Add(new Task(title));
        }
        else
        {
            tasks.Insert(CurrentIndex + 1, new Task(title));
        }
    }
    public void DeleteSelected()
    {
        if (tasks.Count > 0)
        {
            tasks.RemoveAt(CurrentIndex);
            if (CurrentIndex >= tasks.Count) CurrentIndex = tasks.Count - 1;
        }
    }
    public void SelectPrevious()
    {
        CurrentIndex = (CurrentIndex == 0) ? tasks.Count - 1 : CurrentIndex - 1;

    }
    public void SelectNext()
    {
        CurrentIndex = (CurrentIndex + 1) % tasks.Count;
    }
    public void SwapWithPrevious()
    {
        if (CurrentIndex > 0)
        {
            var temp = tasks[CurrentIndex];
            tasks[CurrentIndex] = tasks[CurrentIndex - 1];
            tasks[CurrentIndex - 1] = temp;
            CurrentIndex--;
        }
    }
    public void SwapWithNext()
    {
        if (CurrentIndex < tasks.Count - 1)
        {
            var temp = tasks[CurrentIndex];
            tasks[CurrentIndex] = tasks[CurrentIndex + 1];
            tasks[CurrentIndex + 1] = temp;
            CurrentIndex++;
        }
    }
}
class ToDoListApp
{
    private ToDoList _tasks;
    private bool _showHelp = true;
    private bool _insertMode = false;
    private bool _quit = false; 
    public ToDoListApp(ToDoList tasks) 
    {
        _tasks = tasks;
    }
    public void Run()
    {
        while (!_quit)
        {
            Console.Clear();
            Display();
            ProcessUserInput();
        }
    }
    public void Display()
    {
        DisplayTasks();
        if (_showHelp)
        {
            DisplayHelp();
        }
    }
    public void DisplayBar()
    {
        Console.WriteLine("----------------------------------");
    }
    public string MakeRow(int i)
    {
        Task task = _tasks.GetTask(i);
        string arrow = " ";
        if (task == _tasks.CurrentTask) arrow = "->";
        string check = " ";
        if (task.Status == CompletionStatus.Done) check = "X";
        return $"{arrow} [{check}] {task.Title}";
    }
    public void DisplayTasks()
    {
        DisplayBar();
        Console.WriteLine("Tasks:");
        for (int i = 0; i < _tasks.Length; i++)
        {
            Console.WriteLine(MakeRow(i));
        }
        DisplayBar();
    }
    public void DisplayHelp()
    {
        Console.WriteLine(
            @"Instuctions:
            h: show/hide instructionsh: show/hide instructions
            ↕: select previous or next task (wrapping around at the top and bottom)
            ↔: reorder task (swap selected task with previous or next task)
            space: toggle completion of selected task
            e: edit title
            i: insert new tasks
            delete/backspace: delete task");
            DisplayBar();
    }
    private string GetTitle()
    {
        Console.WriteLine("Please enter task title or (enter) for none:");
        return Console.ReadLine()!;
    }
    public void ProcessUserInput()
    {
        if (_insertMode)
        {
            string taskTitle = GetTitle();
            
            if (taskTitle.Length == 0)
                {
                    _insertMode = false;
                }
                else{
                    _tasks.Insert(taskTitle);
                }
            
        }
        else
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.Escape:
                    _quit = true;
                    break;
                case ConsoleKey.UpArrow:
                    _tasks.SelectPrevious();
                    break;
                case ConsoleKey.DownArrow:
                    _tasks.SelectNext();
                    break;
                case ConsoleKey.LeftArrow:
                    _tasks.SwapWithPrevious();
                    break;
                case ConsoleKey.RightArrow:
                    _tasks.SwapWithNext();
                    break;
                case ConsoleKey.I:
                    _insertMode = true;
                    break;
                case ConsoleKey.E:
                    _tasks.CurrentTask.Title = GetTitle();
                    break;
                case ConsoleKey.H:
                    _showHelp = !_showHelp;
                    break;
                case ConsoleKey.Spacebar:
                    _tasks.CurrentTask.ToggleStatus();
                    break;
                case ConsoleKey.Delete:
                case ConsoleKey.Backspace:
                    _tasks.DeleteSelected();
                    break;
                default:
                    break;
            }
        }
    }
}
