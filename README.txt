This is the visual studio directory for the Smart Tea Steeper.

Project Description:

The watch uses multiple sensors embedded within the smartwatch (such as HeartRateSensor, SleepState, Ambient temperature)
in order to suggest a type of tea to neutralize the off levels.
Once the tea has been suggested, the application will prompt the user to insert the suggested tea type leaves in the steeper.
Finally, users can connect their smart watch app to the smart tea brewer in order to steep the tea based on the right tea temperature.

Software: Tizen API 4, Visual Studio 2019, Tizen.Net Framework
Hardware: Smart TeaPot, Samsung Galaxy Active Smart Watch
--------------------------------------------------------------
Deployment instructions ( Follow only if the app is not on the watch/ A change has been made to the code and you need updated): 

1-Locate the Microsoft Visual Studio Solution File in the folder under name "TeaPot1.sln" and open it with visual studio
2- Make sure you have Tizen.net wearable framework installed from the NuGet Package manager in visual studio. 
3- Access the Tizen Device Manager from: Visual Studio-->Tools--> Tizen Device Manager
4- Connect the host machine and the wearable app to the same WiFi Network, then use the device manager to scan for available target devices.
5- Locate your IP address then toggle-on the switch to connect to the target device.
6- If your device is connected, the target device name in VS will change from Launch Tizen Emulator to the target wearable app IP address/WiFI
7- Go back to VS and run from: Visual Studio-->Debug-->"Start Without Debugging".
8- VS will load the application to the App and Launches it. 

--------------------------------------------------------------
Running Instructions:

1- Once the app has been loaded to the watch and installed, access it from the applications menu under TeaPot1
2- In order to use the applications main key feature, go to the Tea brewer tab in the Main Page Layout
3- The app will scan your vitals and update the screen.
4- Hit Suggest Tea

In order to brew suggested Tea, follow below instructions

1- Switch raspberry Pi 3+ on by connecting it to power or through USB of a running machine. 
2- Open the Bluetooth Low Energy server on the RPI through IDLE or your preferred python IDE/Editor under name TeaPotServer.py 
through: /home/pi/bluez-5.43/test/TeaPotServer.py
3- On your RPI Upper control bar, make bluetooth discoverable
4- Execute the Bluetooth GATT server python script in order to set Bluetooth low energy GATT characteristics
5- Use the RPI terminal in order to turn on Bluetooth low energy advertising mode by command "sudo hciconfig hci0 leadv 0"
6- Go back to the Smartwatch app, Wait 5 seconds after executing the terminal command and hit connect brewer.
7- The Wearable App will start scanning for ble Low Energy and once the server has been found, the user will be automatically 
redirected to a confirmation layout. 
8- Once the user hits the brew button, the app sends the trigger signal through a GATT characteristic
9- The RPI Receives the signal, in the form of the number of the trigger button, and sets it automatically leading to triggering the brewer
to brew using right time and temperature for the suggested tea.  
