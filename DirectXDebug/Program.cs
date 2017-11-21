
//using Microsoft.DirectX.DirectInput;
using System;
using DirectX;

namespace DirectXDebug
{
    internal class controllHelper
    {
        private int speed = 50;
        private DirectX.Joystick joystick;

        public controllHelper(DirectX.Joystick joystick)
        {
            this.joystick = joystick;

            joystick.onAxisX += onAxisX;
            joystick.onAxisY += onAxisY;
            joystick.onAxisZ += onAxisZ;
            joystick.onAxisRz += onAxisRz;
            joystick.onAxisRx += onAxisRx;
            joystick.onAxisRy += onAxisRy;
            joystick.onButton += onButton;
            joystick.onPov += onPov;
            joystick.onAxisExtra += onAxisExtra;
        }

        public int axisSpeed(int status)
        {
            decimal percentage = (decimal)((decimal)speed / (decimal)100);

            return (int)Math.Round(percentage * status);
        }

        private void onButton(int button, bool pressed)
        {
            onCurrentState();
        }

        private void onPov(int pov, int status)
        {
            onCurrentState();
        }

        private void onAxisX(int status)
        {
            onCurrentState();
        }

        private void onAxisY(int status)
        {
            onCurrentState();
        }

        private void onAxisZ(int status)
        {
            onCurrentState();
        }

        private void onAxisRz(int status)
        {
            onCurrentState();
        }

        private void onAxisRx(int status)
        {
            onCurrentState();
        }

        private void onAxisRy(int status)
        {
            onCurrentState();
        }

        private void onAxisExtra(int axis, int status)
        {
            onCurrentState();
        }

        private void onCurrentState()
        {
            Console.Clear();

            SlimDX.DirectInput.JoystickState state = joystick.currentState;

            Console.WriteLine("Controller:" + joystick.JoystickName);

            bool[] jsButtons = state.GetButtons();
            int[] POVs = state.GetPointOfViewControllers();
            int[] Axis = state.GetSliders();

            Console.WriteLine("X: {0}, Y: {1},Z: {2}", state.X, state.Y, state.Z);
            Console.WriteLine("RX: {0}, RY: {1}, RZ: {2}", state.RotationX, state.RotationY, state.RotationZ);

            int i = 0;
            foreach (int Ax in Axis)
            {
                Console.WriteLine("Axel Nr {0}: {1}", i, Ax);
                i++;
            }

            foreach (int POV in POVs)
            {
                Console.WriteLine("POV Nr {0}: Aktiv", POV);
            }

            i = 0;
            foreach (bool button in jsButtons)
            {
                if (button)
                {
                    Console.WriteLine("Knapp Nr {0}: Aktiv", i);
                }
                i++;
            }
        }

    }

    internal class Program
    {
        private static DirectX.Joystick joystick;
        private static controllHelper controlHelper;


        private static void logMessages(string function, string messages)
        {
            Console.WriteLine("function=" + function + ",message=" + messages);
        }

        private static void Main(string[] args)
        {

            SlimDX.DirectInput.DirectInput dinput = new SlimDX.DirectInput.DirectInput();
            foreach (SlimDX.DirectInput.DeviceInstance device in dinput.GetDevices(SlimDX.DirectInput.DeviceClass.GameController, SlimDX.DirectInput.DeviceEnumerationFlags.AttachedOnly))
            {
                

                DirectX.Joystick.onLog = logMessages;
                DirectX.Joystick.debug = false;
                joystick = new DirectX.Joystick(dinput, device);

                
        
                joystick.axisX.min = 1;
                joystick.axisX.max = 99;
                joystick.axisX.deadZone = 1800;
                joystick.axisX.saturation = 10000;

                joystick.axisY.min = 101;
                joystick.axisY.max = 199;
                joystick.axisY.deadZone = 1800;
                joystick.axisY.saturation = 10000;

                joystick.axisZ.min = 201;
                joystick.axisZ.max = 299;
                joystick.axisZ.deadZone = 1800;
                joystick.axisZ.saturation = 10000;

                joystick.axisRx.min = 301;
                joystick.axisRx.max = 399;
                joystick.axisRx.deadZone = 2000;
                joystick.axisRx.saturation = 10000;

                joystick.axisRy.min = 401;
                joystick.axisRy.max = 499;
                joystick.axisRy.deadZone = 2000;
                joystick.axisRy.saturation = 10000;

                joystick.axisRz.min = 501;
                joystick.axisRz.max = 599;
                joystick.axisRz.deadZone = 2000;
                joystick.axisRz.saturation = 10000;

                joystick.axisExtra.min = 601;
                joystick.axisExtra.max = 699;
                joystick.axisExtra.deadZone = 2000;
                joystick.axisExtra.saturation = 10000;

                joystick.setJoystickValues();

                controlHelper = new controllHelper(joystick);
                joystick.start();


                break;
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}