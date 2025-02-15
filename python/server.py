import socket

def send_message(text):
    addr = ("localhost", 8080)
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    sock.connect(addr)
    sock.sendall(text.encode("utf-8"))

    #data = sock.recv(1024)
    #print("Ответ:", data.decode("utf-8"))

    sock.close()





