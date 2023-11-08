using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using SharpDX.DirectInput;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            EspConnection serialHandler = new EspConnection("COM8", 115200, 8, true);
            serialHandler.OpenConnection();
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

            byte[] dataToSend;

            using (var gamepad = new Gamepad(gamepadGuid))
            {
                Console.WriteLine("Control de videojuegos seleccionado: " + gamepad.InstanceName);

                // Poll events from gamepad
                while (true)
                {

                    dataToSend = gamepad.PollEvents();
                    
                    
                    try
                    {
                        serialHandler.SendMessage(dataToSend);
                    }
                    finally {
                        
                    }
                   

                    System.Threading.Thread.Sleep(500);
                }
            }

            serialHandler.CloseConnection();




        }
    }
}