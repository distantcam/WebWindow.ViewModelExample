using System.IO;
using System.Reflection;

namespace WebWindows.ViewModelExample
{
    class Program
    {
        static Assembly assembly = Assembly.GetAssembly(typeof(Program));

        static void Main(string[] args)
        {
            var window = new WebWindow("WebWindows ViewModel Demo", options =>
            {
                options.SchemeHandlers.Add("app", (string url, out string contentType) =>
                {
                    if (url.EndsWith(".js"))
                        contentType = "text/javascript";
                    else if (url.EndsWith(".css"))
                        contentType = "text/css";
                    else
                        contentType = "text/plain";

                    return assembly.GetManifestResourceStream("WebWindows.ViewModelExample.wwwroot." + url.Substring(6).Replace('/', '.'));
                });
            });

            var view = new WindowView(window);
            view.SetViewModel(new MainViewModel());

            using (Stream stream = assembly.GetManifestResourceStream("WebWindows.ViewModelExample.wwwroot.index.html"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                window.NavigateToString(result);
            }

            window.WaitForExit();
        }
    }
}
