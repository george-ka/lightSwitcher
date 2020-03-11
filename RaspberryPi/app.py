from flask import Flask
from flask import jsonify
from flask import request
import serial
import time
import warnings
import struct
import serial.tools.list_ports

app = Flask(__name__)

arduino_ports = [
    p.device
    for p in serial.tools.list_ports.comports()
    if 'ttyACM' in p.description  # may need tweaking to match new arduinos
]

if not arduino_ports:
    raise IOError("No Arduino found")
if len(arduino_ports) > 1:
    warnings.warn('Multiple Arduinos found - using the first')

ser = serial.Serial(arduino_ports[0], 9600)
turn_on_offset = 48
turn_off_offset = 97

# state
switches = {}
for i in range(14):
	switches[i] = { 'name' : 'switch {}'.format(i), 'state': 0 }

@app.route('/')
def index():
	return 'This is my home API use /api/v1/switches/ to get switches state'

@app.route('/api/v1/switches/', methods=['GET'])
def get_switches():
	return jsonify(switches)

def send_arduino_command(switchId, isOn):
	if isOn:
		command = switchId + turn_on_offset
	else:
		command = switchId + turn_off_offset
	
	res = ser.write(struct.pack('>B', command))
	
	print("Hi you sent command %s result was %s" %(command, res))
	
	time.sleep(1)
	out = ""
	while ser.inWaiting() > 0:
		out += ser.read(1)

	if out != '':
		print("Raspberry response: " + out)
		print("End of response")
			

def send_turn_on(switchId):
	send_arduino_command(switchId, True)

def send_turn_off(switchId):
	send_arduino_command(switchId, False)
	

@app.route('/api/v1/switches/<switchId>', methods=['POST'])
def change_switch_state(switchId):
	switchId = int(switchId)
	switchModeIsOn = request.args.get('mode') == 'on'
	if switchId in switches:
		switches[switchId]['state'] = 1 if switchModeIsOn else 0
		if switchModeIsOn:
			send_turn_on(switchId) 
		else:
			send_turn_off(switchId)

		return jsonify(switches[switchId])
	else:
		return 'switch not found'

if __name__ == '__main__':
	app.run(debug=True, host='0.0.0.0')
