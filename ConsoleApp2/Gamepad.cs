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

    public byte[] PollEvents()
    {
        joystick.Poll();
        var state = joystick.GetCurrentState();
        var buttons = state.Buttons;

        String seleccion = "";
        byte buttonPressed = 0;

        var data = joystick.GetBufferedData();


        // Revisar los botones
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i])
            {
                //Console.WriteLine($"Botón {i} presionado");
                buttonPressed = (byte)(i+1);
            }
            


        }

        // Revisar los gatillos
        int leftTrigger = state.X;
        int rightTrigger = state.RotationZ;

        rightTrigger = 65535 - rightTrigger;

        byte leftTriggerByte = (byte)(leftTrigger / 363);
        byte rightTriggerByte = (byte)(rightTrigger / 257);


  
        byte[] dataPacket = new byte[] { buttonPressed, leftTriggerByte, rightTriggerByte };

        return dataPacket;
    }

    public void Dispose()
    {
        joystick.Unacquire();
        joystick.Dispose();
    }
}