import socket
import Voice
import time
def send_message(text):
    addr = ("localhost", 8080)
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    sock.connect(addr)
    sock.sendall(text.encode("utf-8"))

    #data = sock.recv(1024)
    #print("Ответ:", data.decode("utf-8"))
    #Voice.speak(data.decode("utf-8"))

    sock.close()


def Resive_msg():
    while True:
        time.sleep(0.1)
        addr = ("localhost", 8081)
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.bind(addr)
        sock.listen()

        con, addr = sock.accept()
        data = con.recv(1024)
        #print("Ответ:", data.decode("utf-8"))
        print(data.decode("utf-8"))
        Voice.speak(data.decode("utf-8"))

        sock.close()


