from socket import *
from time import ctime

HOST=''
PORT=21567
BUFFSIZE=1024
ADDR=(HOST,PORT)

tcpSerSock=socket(AF_INET,SOCK_STREAM)
tcpSerSock.bind(ADDR)
tcpSerSock.listen(5)
while True:
    print ('waiting for connection...')
    tcpcliSock,addr=tcpSerSock._accept()
    print ('....connect from :',addr)

    while True:
        data=tcpcliSock.recv(BUFFSIZE)
        if not data:
            break
        tcpcliSock.send('[%s] %s' % (ctime(),data))

    tcpcliSock.close()
tcpSerSock.close()