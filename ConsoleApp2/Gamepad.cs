using SharpDX.DirectInput;
using System;

public class Gamepad : IDisposable
{
    private Joystick joystick;

    public bool IsConnected { get; private set; }

        public string InstanceName => joystick.Information.InstanceName;


    public Gamepad(Guid instanceGuid)
    {
        var directInput = new DirectInput();
        joystick = new Joystick(directInput, instanceGuid);
        joystick.Properties.BufferSize = 128;
        joystick.Acquire();
        IsConnected = true;
    }

    public void PollEvents()
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
        // Puedes ajustar estos valores según tu control
        if (leftTrigger > 0)
        {
            Console.WriteLine($"Gatillo izquierdo presionado: {leftTrigger}");
        }
        if (rightTrigger > 0)
        {
            Console.WriteLine($"Gatillo derecho presionado: {rightTrigger}");
        }
    }

    public void Dispose()
    {
        joystick.Unacquire();
        joystick.Dispose();
    }
}