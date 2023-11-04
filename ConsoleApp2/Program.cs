using System;
using System.Linq;
using SharpDX.DirectInput;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Inicializar DirectInput
            var directInput = new DirectInput();

            // Lista todos los dispositivos de juego
            Console.WriteLine("Dispositivos de juego disponibles:");
            var gamepadDevices = directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices);
            var joystickDevices = directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices);

            var allDevices = gamepadDevices.Concat(joystickDevices).ToArray();

            if (allDevices.Length == 0)
            {
                Console.WriteLine("No se encontró ningún control de videojuegos.");
                return;
            }

            for (int i = 0; i < allDevices.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {allDevices[i].InstanceName}");
            }

            Console.WriteLine("Seleccione el número del control que desea utilizar:");
            int deviceNumber;
            if (!int.TryParse(Console.ReadLine(), out deviceNumber) || deviceNumber < 1 || deviceNumber > allDevices.Length)
            {
                Console.WriteLine("Selección inválida.");
                return;
            }

            var gamepadGuid = allDevices[deviceNumber - 1].InstanceGuid;

            // Crear la instancia del joystick/gamepad
            using (var joystick = new Joystick(directInput, gamepadGuid))
            {
                Console.WriteLine("Control de videojuegos seleccionado: " + joystick.Information.InstanceName);

                // Adquirir el dispositivo
                joystick.Properties.BufferSize = 128;
                joystick.Acquire();

                // Poll events from joystick
                while (true)
                {
                    joystick.Poll();
                    var state = joystick.GetCurrentState();
                    var buttons = state.Buttons;

                    // Revisar los botones
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        if (buttons[i])
                        {
                            Console.WriteLine($"Botón {i} presionado");
                        }
                    }

                    // Revisar los gatillos
                    int leftTrigger = state.Z; // Este es comúnmente el gatillo izquierdo
                    int rightTrigger = state.RotationZ; // Este es comúnmente el gatillo derecho

                    // Suponiendo que los gatillos están en reposo en 0 y completamente presionados en 65535
                    // puedes ajustar estos valores según tu control
                    if (leftTrigger > 0)
                    {
                        Console.WriteLine($"Gatillo izquierdo presionado: {leftTrigger}");
                    }
                    if (rightTrigger > 0)
                    {
                        Console.WriteLine($"Gatillo derecho presionado: {rightTrigger}");
                    }

                    System.Threading.Thread.Sleep(10);
                }
            }
        }
    }
}


