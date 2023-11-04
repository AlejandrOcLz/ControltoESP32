using System;
using System.IO.Ports;

public class EspConnection
{
    private SerialPort serialPort;
    public EspConnection(string portName, int baudRate)
    {
        serialPort = new SerialPort
        {
            PortName = portName,
            BaudRate = baudRate
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

    public void SendMessage(string message)
    {
        if (serialPort.IsOpen)
        {
            serialPort.WriteLine(message);
            Console.WriteLine($"Mensaje enviado: {message}");
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
