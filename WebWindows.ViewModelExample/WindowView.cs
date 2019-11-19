using System.Text.Json;
using Humanizer;

namespace WebWindows.ViewModelExample
{
    public class WindowView
    {
        readonly WebWindow window;
        ViewModel vm;

        public WindowView(WebWindow window)
        {
            this.window = window;
            window.OnWebMessageReceived += Window_OnWebMessageReceived;
        }

        private void Window_OnWebMessageReceived(object sender, string message)
        {
            var parsedMessage = JsonDocument.Parse(message).RootElement;

            var command = parsedMessage.GetProperty("command").GetString();
            if (command == "ready")
            {
                vm.ViewReady();
                return;
            }

            var method = vm.GetType().GetMethod(command.Pascalize());
            method.Invoke(vm, new object[] { parsedMessage });
        }

        public void SetViewModel(ViewModel vm)
        {
            this.vm = vm;
            vm.PropertyChanged += (sender, e) =>
            {
                var prop = sender.GetType().GetProperty(e.PropertyName);
                var value = prop.GetValue(sender);

                if (e.PropertyName == "Title")
                    window.Title = (string)value;
                else
                    window.SendMessage(JsonSerializer.Serialize(new { command = "propertyChanged", name = e.PropertyName.Camelize(), value }));
            };
        }
    }
}
