using SlimDX.DirectInput;

//using Microsoft.DirectX.DirectInput;
using System;
using System.Threading;

namespace DirectX
{
    /// <summary>
    /// Contains properties for every axis.
    /// </summary>
    ///
    public class axis
    {
        /// <summary>
        /// Maximum value an axis can return
        /// </summary>
        public int max = +49;

        /// <summary>
        /// Minimum value an axis can return
        /// </summary>
        public int min = -49;

        /// <summary>
        /// reverses the scale so max value becomes min and min value becomes max value.
        /// </summary>
        public bool reverse = false;

        /// <summary>
        /// percentage of axis that should be deadzone, value is between 0-10000
        /// </summary>
        public int deadZone
        {
            set
            {
                if (value >= 0 || value <= 10000)
                {
                    deadZoneValue = value;
                }
            }
            get
            {
                return deadZoneValue;
            }
        }

        /// <summary>
        /// Sets where the maximum value of the axis will be, if set to 5000 the maximum value will be sent when you reached half of the axis., value is between 0-10000
        /// </summary>
        public int saturation
        {
            set
            {
                if (value >= 0 || value <= 10000)
                {
                    saturationValue = value;
                }
            }
            get
            {
                return saturationValue;
            }
        }

        private int deadZoneValue = 0;
        private int saturationValue = 10000;
    }

    public class Joystick
    {
        /// <summary>
        /// Set to true to recieve debug messages in console.
        /// </summary>
        static public bool debug = false;

        /// <summary>
        /// Capture all logmessages from object
        /// </summary>
        static public logDelegate onLog = delegate { };

        /// <summary>
        /// Holds an axis object with the maximum value, minumum value for the axis, and deadzone, saturation (outer deadzone)
        /// </summary>
        public axis axisExtra;

        /// <summary>
        /// Holds an axis object with the maximum value, minumum value for the axis, and deadzone, saturation (outer deadzone)
        /// </summary>
        public axis axisRx;

        /// <summary>
        /// Holds an axis object with the maximum value, minumum value for the axis, and deadzone, saturation (outer deadzone)
        /// </summary>
        public axis axisRy;

        /// <summary>
        /// Holds an axis object with the maximum value, minumum value for the axis, and deadzone, saturation (outer deadzone)
        /// </summary>
        public axis axisRz;

        /// <summary>
        /// Holds an axis object with the maximum value, minumum value for the axis, and deadzone, saturation (outer deadzone)
        /// </summary>
        public axis axisX;

        /// <summary>
        /// Holds an axis object with the maximum value, minumum value for the axis, and deadzone, saturation (outer deadzone)
        /// </summary>
        public axis axisY;

        /// <summary>
        /// Holds an axis object with the maximum value, minumum value for the axis, and deadzone, saturation (outer deadzone)
        /// </summary>
        public axis axisZ;

        /// <summary>
        /// Sets the minimum amount of milliseconds between two pollings on the joystick.
        /// Important, if you set this value to high, all values will not be registrered by the poll.
        /// </summary>
        public int minimumPollingTime = 0;

        /// <summary>
        /// Triggers when a axis-extra is changed.
        /// </summary>
        public JoystickAxisExtraDelegate onAxisExtra = delegate { };

        /// <summary>
        /// Triggers when a axis-Rx is changed.
        /// </summary>
        public JoystickAxisDelegate onAxisRx = delegate { };

        /// <summary>
        /// Triggers when a axis-Ryx is changed.
        /// </summary>
        public JoystickAxisDelegate onAxisRy = delegate { };

        /// <summary>
        /// Triggers when a axis-Rz is changed.
        /// </summary>
        public JoystickAxisDelegate onAxisRz = delegate { };

        /// <summary>
        /// Triggers when a axis-x is changed.
        /// </summary>
        public JoystickAxisDelegate onAxisX = delegate { };

        /// <summary>
        /// Triggers when a axis-y is changed.
        /// </summary>
        public JoystickAxisDelegate onAxisY = delegate { };

        /// <summary>
        /// Triggers when a axis-z is changed.
        /// </summary>
        public JoystickAxisDelegate onAxisZ = delegate { };

        /// <summary>
        /// Triggers when a button is pressed
        /// </summary>
        public JoystickButtonsDelegate onButton = delegate { };

        /// <summary>
        /// Triggers when a pov is changed.
        /// </summary>
        public JoystickPovDelegate onPov = delegate { };

        /// <summary>
        /// Joystickens Produktnamn.
        /// </summary>
        public string JoystickName = "";

        /// <summary>
        /// Constructor, do the following to loop through and select your joystick
        /// SlimDX.DirectInput.DirectInput dinput = new SlimDX.DirectInput.DirectInput();
        /// foreach (SlimDX.DirectInput.DeviceInstance device in dinput.GetDevices(SlimDX.DirectInput.DeviceClass.GameController, SlimDX.DirectInput.DeviceEnumerationFlags.AttachedOnly))
        /// {
        ///     Console.WriteLine("Controller:" + device.InstanceName);
        ///     
        ///     DirectX.Joystick.onLog = logMessages;
        ///     DirectX.Joystick.debug = false;
        ///     joystick = new DirectX.Joystick(dinput, device);
        ///     
        ///     joystick.axisX.min = 1;
        ///     joystick.axisX.max = 99;
        ///     joystick.axisX.deadZone = 0;
        ///     joystick.axisX.saturation = 10000;
        ///     
        ///     joystick.setJoystickValues();
        ///     
        ///     controlHelper = new controllHelper(joystick);
        ///     joystick.start();
        ///     
        ///     break;
        /// }
        /// </summary>
        /// <param name="dInput"></param>
        /// <param name="joystickInstance"></param>
        public Joystick(DirectInput dInput, DeviceInstance joystickInstance)
        {
            this.joystickInstance = joystickInstance;
            JoystickName = joystickInstance.ProductName;
            
            joystickDevice = new SlimDX.DirectInput.Joystick(dInput, joystickInstance.InstanceGuid);  // slimDX replacement
            joystickDevice.SetNotification(onJoystickEvent);
            joystickDevice.Acquire();

            axisY = new axis();
            axisX = new axis();
            axisZ = new axis();
            axisRy = new axis();
            axisRx = new axis();
            axisRz = new axis();
            axisExtra = new axis();

            /* initiate default joystick values.*/
            setJoystickValues();
        }

        /// <summary>
        /// Delegate for axis
        /// </summary>
        public delegate void JoystickAxisDelegate(int status);

        /// <summary>
        /// Delegate for axis
        /// </summary>
        public delegate void JoystickAxisExtraDelegate(int axis, int status);

        /// <summary>
        /// Delegate for buttons pressed
        /// </summary>
        public delegate void JoystickButtonsDelegate(int button, bool pressed);

        /// <summary>
        /// Delegate for pov pressed
        /// </summary>
        public delegate void JoystickPovDelegate(int pov, int status);

        public delegate void logDelegate(string function, string message);

        /// <summary>
        /// Holds the current state of the joystick.
        /// </summary>
        public JoystickState currentState
        {
            get
            {
                return joystickDevice.GetCurrentState();
            }
        }

        /// <summary>
        /// Sets ,min,max,saturation and deadzone values to joystick axes.
        /// </summary>
        public void setJoystickValues()
        {
            /* Set input range for axis */
            log("setJoystickValues", "Set properties to axis");

            /* Loopa igenom alla object och sätt axlarna via setAxisValue */
            foreach (DeviceObjectInstance joystickPart in joystickDevice.GetObjects())
            {
                if (joystickPart.ObjectTypeGuid == ObjectGuid.XAxis)
                {
                    setAxiskValue(joystickDevice.GetObjectPropertiesById((int)joystickPart.ObjectType), axisX);
                }
                else if (joystickPart.ObjectTypeGuid == ObjectGuid.YAxis)
                {
                    setAxiskValue(joystickDevice.GetObjectPropertiesById((int)joystickPart.ObjectType), axisY);
                }
                else if (joystickPart.ObjectTypeGuid == ObjectGuid.ZAxis)
                {
                    setAxiskValue(joystickDevice.GetObjectPropertiesById((int)joystickPart.ObjectType), axisZ);
                }
                /* Rotation */
                else if (joystickPart.ObjectTypeGuid == ObjectGuid.RotationalXAxis)
                {
                    setAxiskValue(joystickDevice.GetObjectPropertiesById((int)joystickPart.ObjectType), axisRx);
                }
                else if (joystickPart.ObjectTypeGuid == ObjectGuid.RotationalYAxis)
                {
                    setAxiskValue(joystickDevice.GetObjectPropertiesById((int)joystickPart.ObjectType), axisRy);
                }
                else if (joystickPart.ObjectTypeGuid == ObjectGuid.RotationalZAxis)
                {
                    setAxiskValue(joystickDevice.GetObjectPropertiesById((int)joystickPart.ObjectType), axisRz);
                }
                /* Sliders */
                else if (joystickPart.ObjectTypeGuid == ObjectGuid.Slider)
                {
                    setAxiskValue(joystickDevice.GetObjectPropertiesById((int)joystickPart.ObjectType), axisExtra);
                }
            }
        }

        /// <summary>
        /// Start polling joystick for values.
        /// </summary>
        public void start()
        {
            /* Starts separate thread to monitor joystick for changes. */
            this.pollingThread = new Thread(new ThreadStart(pollingServer));
            this.pollingThread.Name = "joystickServer";
            this.pollingThread.Start();
        }

        /// <summary>
        /// Stop all threads for the current class.
        /// </summary>
        public void stop()
        {
            /* Låter loopen i pollingserver endast köras en gång till.*/
            threadActive = false;

            /* Triggar att joysticken har ändrats för att få loopen att köras en gång till, se pollingServer*/
            onJoystickEvent.Set();

            /* Kör bara om tråden är skapad. */
            if (this.pollingThread != null)
            {
                /* Inväntar att pollingThread ska avslutas pga av föregående två commandon.
                 * 20141208 Behöver ej göras, tråden slutar av sig själv, finns ingen mening med att den här tråden ska vänta in den andra.
                 */
                // this.pollingThread.Join();

                /* Släpper kontrollen över joysticken. */
                joystickDevice.Unacquire();
            }
        }

        /// <summary>
        /// Holds the state before the previous state of the joystick, used then polling. to ensure joystick doesent jumpt between values.
        /// </summary>
        private JoystickState beforeBeforePreviousState;

        /// <summary>
        /// Holds the state before the previous state of the joystick, used then polling. to ensure joystick doesent jumpt between values.
        /// </summary>
        private JoystickState beforePreviousState;

        /// <summary>
        /// Holds the joystick device.
        /// </summary>
        private SlimDX.DirectInput.Joystick joystickDevice;

        /// <summary>
        /// Holds the joystick instance, create an joystickDevice of it.
        /// </summary>
        private DeviceInstance joystickInstance;

        /// <summary>
        /// AutoResetEvent that joystick triggers.
        /// </summary>
        private AutoResetEvent onJoystickEvent = new AutoResetEvent(true);

        /// <summary>
        /// This threads polls joystick for changes.
        /// </summary>
        private Thread pollingThread;

        /// <summary>
        ///Holds the previous state of the joystick, used then polling.
        /// </summary>
        private JoystickState previousState;

        /// <summary>
        /// Set to false to close all child threads.
        /// </summary>
        private bool threadActive = true;

        /// <summary>
        /// Kalkylerar status värde, beroende på hur inställningarna är satta.
        /// </summary>
        /// <param name="localAxis"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private int calculateStateValue(axis localAxis, int value)
        {
            if (localAxis.reverse)
            {
                return (localAxis.max - value) + localAxis.min;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// if debug=true this functions prints out debug messages in the console.
        /// </summary>
        private void log(string function, string message)
        {
            if (debug)
            {
                onLog(function, message);
            }
        }

        /// <summary>
        /// handles all events that are trigged from joystick.
        /// </summary>
        private void pollingClient()
        {
            joystickDevice.Poll();
            JoystickState state = joystickDevice.GetCurrentState();

            if (previousState.X != state.X && !(state.X == beforePreviousState.X && previousState.X == beforeBeforePreviousState.X))
            {
                int status = calculateStateValue(axisX, state.X);

                onAxisX(status);
                log("pollingClient", "onAxisX(" + status + ")");
            }

            if (previousState.Y != state.Y && !(state.Y == beforePreviousState.Y && previousState.Y == beforeBeforePreviousState.Y))
            {
                int status = calculateStateValue(axisY, state.Y);

                onAxisY(status);
                log("pollingClient", "onAxisY(" + status + ")");
            }

            if (previousState.Z != state.Z && !(state.Z == beforePreviousState.Z && previousState.Z == beforeBeforePreviousState.Z))
            {
                int status = calculateStateValue(axisZ, state.Z);

                onAxisZ(status);
                log("pollingClient", "onAxisZ(" + status + ")");
            }

            /* Rotation */
            if (previousState.RotationX != state.RotationX && !(state.RotationX == beforePreviousState.RotationX && previousState.RotationX == beforeBeforePreviousState.RotationX))
            {
                int status = calculateStateValue(axisRx, state.RotationX);

                onAxisRx(status);
                log("pollingClient", "onAxisRx(" + status + ")");
            }

            if (previousState.RotationY != state.RotationY && !(state.RotationY == beforePreviousState.RotationY && previousState.RotationY == beforeBeforePreviousState.RotationY))
            {
                int status = calculateStateValue(axisRy, state.RotationY);

                onAxisRy(status);
                log("pollingClient", "onAxisRy(" + status + ")");
            }

            if (previousState.RotationZ != state.RotationZ && !(state.RotationZ == beforePreviousState.RotationZ && previousState.RotationZ == beforeBeforePreviousState.RotationZ))
            {
                int status = calculateStateValue(axisRz, state.RotationZ);

                onAxisRz(status);
                log("pollingClient", "onAxisRz(" + status + ")");
            }

            /* Extra axis */
            int[] axis = state.GetSliders();
            int[] previousAxis = previousState.GetSliders();
            int[] beforePreviousAxis = beforePreviousState.GetSliders();
            int[] beforeBeforePreviousAxis = beforeBeforePreviousState.GetSliders();
            for (int i = 0, c = axis.Length; i < c; i++)
            {
                if (previousAxis[i] != axis[i] && !(axis[i] == beforePreviousAxis[i] && previousAxis[i] == beforeBeforePreviousAxis[i]))
                {
                    int status = calculateStateValue(axisExtra, axis[i]);

                    onAxisExtra(i, status);
                    log("pollingClient", "onAxisExtra(" + i + "," + status + ")");
                }
            }

            /* All Buttons */
            bool[] buttons = state.GetButtons();
            bool[] previousButtons = previousState.GetButtons();

            for (int i = 0, c = buttons.Length; i < c; i++)
            {
                if (previousButtons[i] != buttons[i])
                {
                    onButton(i, buttons[i]);
                    log("pollingClient", "onButton(" + i + "," + buttons[i] + ")");
                }
            }

            int[] pointOfViews = state.GetPointOfViewControllers();
            int[] previousPointOfViews = previousState.GetPointOfViewControllers();
            for (int i = 0, c = pointOfViews.Length; i < c; i++)
            {
                if (previousPointOfViews[i] != pointOfViews[i])
                {
                    onPov(i, pointOfViews[i]);
                    log("pollingClient", "onPov(" + i + "," + pointOfViews[i] + ")");
                }
            }

            beforeBeforePreviousState = beforePreviousState;
            beforePreviousState = previousState;
            previousState = state;
        }

        /// <summary>
        /// Listens for changes on the joystick and starts pollingClient
        /// </summary>
        private void pollingServer()
        {
            log("pollingServer", "Name: " + joystickInstance.InstanceName);
            log("pollingServer", "Product: " + joystickInstance.ProductName);
            try
            {
                joystickDevice.Poll();
                previousState = joystickDevice.GetCurrentState();
                beforePreviousState = joystickDevice.GetCurrentState();
                beforeBeforePreviousState = joystickDevice.GetCurrentState();

                while (threadActive)
                {
                    /* Så fort joysticken har triggats kör pollingClient() */
                    log("pollingServer", "onJoystickEvent.WaitOne();");
                    onJoystickEvent.WaitOne();
                    log("pollingServer", "pollingClient();");
                    pollingClient();
                    log("pollingServer", "minimumPollingTime > 0");
                    if (minimumPollingTime > 0)
                    {
                        System.Threading.Thread.Sleep(minimumPollingTime);
                    }
                }
            }
            catch (Exception e)
            {
                log("pollingServer", "Unhandled error, exiting pollingServer");
            }
            finally
            {
                log("pollingServer", "Thread has ended");
            }
        }

        /// <summary>
        /// Set min,max,saturation and deadzone to axis
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="propertieAxis"></param>
        private void setAxiskValue(ObjectProperties axis, axis propertieAxis)
        {
            try
            {
                /* Loopa igenom alla axlar */
                axis.SetRange(propertieAxis.min, propertieAxis.max);
                axis.DeadZone = propertieAxis.deadZone;
                axis.Saturation = propertieAxis.saturation;
            }
            catch
            {
                log("setAxiskValue", "Axis " + axis.ToString() + " does not exist");
            }
        }
    }
}