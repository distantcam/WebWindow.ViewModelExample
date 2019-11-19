namespace WebWindows.ViewModelExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = new WebWindow(".NET Core + Vue.js file explorer");

            var view = new WindowView(window);
            view.SetViewModel(new MainViewModel());

            window.NavigateToLocalFile("wwwroot/index.html");
            window.WaitForExit();
        }
    }
}
