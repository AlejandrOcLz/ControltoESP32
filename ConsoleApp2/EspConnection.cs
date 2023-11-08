using System;
using System.IO.Ports;

public class EspConnection
{
    private SerialPort serialPort;
    public EspConnection(string portName, int baudRate, int db, bool Rts)
    {
        serialPort = new SerialPort
        {
            PortName = portName,
            BaudRate = baudRate,
            DataBits = db,
            RtsEnable = Rts,
        };
    }

    public void OpenConnection()
    {
        if (!serialPort.IsOpen)
        {
            serialPort.Open();
            Console.WriteLine("Conexión Serial abierta.");
        }
        else
        {
            Console.WriteLine("La conexión serial ya está abierta.");
        }
    }

    public void CloseConnection()
    {
        if (serialPort.IsOpen)
        {
            serialPort.Close();
            Console.WriteLine("Conexión Serial cerrada.");
        }
    }

    public void SendMessage(Byte[] message)
    {
        Console.WriteLine($"{message.GetValue(0)} {message.GetValue(1)} {message.GetValue(2)}");

        if (serialPort.IsOpen)
        {
            serialPort.Write(message, 0, message.Length);
            Console.WriteLine($"Mensaje enviado");
        }
        else
        {
            Console.WriteLine("La conexión serial no está abierta.");
        }
    }

    public bool IsOpen()
    {
        return serialPort.IsOpen;
    }
}
